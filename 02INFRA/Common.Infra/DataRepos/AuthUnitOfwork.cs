using Amazon.Runtime.SharedInterfaces;
using Common.App.Contracts;
using Common.Domain.AuthDataModel;
using Common.Infra.AuthDBCon;
using Common.Infra.Extentions;
using Common.Infra.GenericRepos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Common.Infra.DataRepos
{
    public  class AuthUnitOfwork  : IAuthUnitOfWork, IDisposable 
    {

        public AuthDBContext context;
        public IConfiguration config;
        private readonly ILoggerService logger;

        public IRepository<ClientInfo> clientInfos;

        public IRepository<UserLoginActivity> userLoginActivities;

        public AuthUnitOfwork(AuthDBContext cont, IConfiguration _config, ILoggerService _logger)
        {
            this.context = cont;
            this.config = _config;
            this.logger = _logger;
        }

       
        public IRepository<ClientInfo> ClientInfos
        {
            get
            {
                if (this.clientInfos == null)
                {
                    this.clientInfos = new Repository<ClientInfo>(context);
                }
                return clientInfos;
            }
        }



       
        public IRepository<UserLoginActivity> UserLoginActivities
        {
            get
            {
                if (this.userLoginActivities == null)
                {
                    this.userLoginActivities = new Repository<UserLoginActivity>(context);
                }
                return userLoginActivities;
            }
        }
        public void ClearChangeTracker()
        {
            try
            {
                var undetachedEntriesCopy = context.ChangeTracker.Entries()
                    .Where(e => e.State != EntityState.Detached)
                    .ToList();

                foreach (var entry in undetachedEntriesCopy)
                    entry.State = EntityState.Detached;
            }
            catch { }
            try
            {
                context.ChangeTracker.Clear();
            }
            catch { }
        }
        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }

        public async Task<int> SaveWithTransactionAsync()
        {
            using var ctxtTransaction = context.Database.BeginTransaction();
            try
            {
                int rowsEffected = await context.SaveChangesAsync();
                await ctxtTransaction.CommitAsync();
                return rowsEffected;
            }
            catch (Exception ex) { await ctxtTransaction.RollbackAsync(); return 0; }
        }

        public int SaveWithTransaction()
        {
            using var ctxtTransaction = context.Database.BeginTransaction();
            try
            {
                int rowsEffected = context.SaveChanges();
                ctxtTransaction.Commit();
                return rowsEffected;
            }
            catch (Exception ex) { ctxtTransaction.Rollback(); return 0; }

        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<string> ExecuteSqlScalarCommand(List<SqlParameter> sqlParams, string command)
        {

            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = 360;
            cmd.CommandText = command;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] paramList = sqlParams.ToArray();
            cmd.Parameters.AddRange(paramList);
            // Open database connection  
            await context.Database.OpenConnectionAsync();


            // Create a DataReader  
            object val = await cmd.ExecuteScalarAsync();
            return val == null ? "" : val.ToString();
        }
        public async Task<int> ExecuteSqlNonQueryCommand(List<SqlParameter> sqlParams, string command)
        {

            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = 360;
            cmd.CommandText = command;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] paramList = sqlParams.ToArray();
            cmd.Parameters.AddRange(paramList);
            // Open database connection  
            await context.Database.OpenConnectionAsync();


            // Create a DataReader  
            int val = await cmd.ExecuteNonQueryAsync();
            return val;
        }
        private string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return "0x" + hex.ToString();
        }
        public async Task<List<T>> ExecuteSqlCommand<T>(List<SqlParameter> sqlParams, string command)
        {
            List<T> list = new List<T>();


            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = 360;
            cmd.CommandText = command;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] paramList = sqlParams.ToArray();
            cmd.Parameters.AddRange(paramList);
            // Open database connection  
            // OpenConnection(cmd);
            await context.Database.OpenConnectionAsync();
            T obj = default(T);
            // Create a DataReader  
            using (DbDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                try
                {
                    while (await rdr.ReadAsync())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            try
                            {
                                if (!object.Equals(rdr[prop.Name], DBNull.Value))
                                {
                                    if (rdr[prop.Name].GetType().Name.ToString() == "Byte[]")
                                    {
                                        if (prop.CanWrite) prop.SetValue(obj, ByteArrayToString((Byte[])rdr[prop.Name]), null);
                                        continue;
                                    }

                                    if (prop.CanWrite) prop.SetValue(obj, rdr[prop.Name], null);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.GetType() == typeof(IndexOutOfRangeException))
                                {
                                    // if the result set doesn't have this value, intercept the exception
                                    // and set the property value to null / 0
                                    if (prop.CanWrite) prop.SetValue(obj, null, null);
                                }
                                else
                                    throw new Exception(Environment.NewLine + "Data Type Mapping Failed" + ">>" + prop.Name + "<<" + Environment.NewLine + ex.Message, ex);
                            }
                        }
                        list.Add(obj);
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    rdr.Close();
                    throw;
                }
            }


            return list;
        }
        #region SqlADOCommand  

        public async Task BulkCopyAsync<T>(IEnumerable<T> list, string clientName, string tableName = null)
        {

            DataTable dt = list.ToDataTable<T>();
            if (dt != null && dt.Rows.Count > 0)
            {
                string tName = string.IsNullOrWhiteSpace(tableName) ? typeof(T).Name : tableName;
                using (SqlConnection con = new SqlConnection(getConnectionString(clientName)))
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        try
                        {
                            con.Open();
                            bulkCopy.DestinationTableName = tName;
                            await bulkCopy.WriteToServerAsync(dt);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(Environment.NewLine + $"Something went wrong: {ex}" + Environment.NewLine);
                        }
                    }
                    con.Close();
                }


            }

        }
        public async Task<string> ExecuteADOSqlScalarCommand(List<SqlParameter> sqlParams, string command, string clientName)
        {

            object val;
            using (SqlConnection con = new SqlConnection(getConnectionString(clientName)))
            {
                using (SqlCommand cmd = new SqlCommand(command, con))
                {
                    cmd.CommandTimeout = 360;
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] paramList = sqlParams.ToArray();
                    cmd.Parameters.AddRange(paramList);
                    con.Open();
                    val = await cmd.ExecuteScalarAsync();
                }
                con.Close();
            }

            return val.ToString() == null ? "" : val.ToString();
        }
        public async Task<int> ExecuteADOSqlNonQueryCommand(List<SqlParameter> sqlParams, string command, string clientName)
        {

            int val = 0;
            using (SqlConnection con = new SqlConnection(getConnectionString(clientName)))
            {
                using (SqlCommand cmd = new SqlCommand(command, con))
                {
                    cmd.CommandTimeout = 360;
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] paramList = sqlParams.ToArray();
                    cmd.Parameters.AddRange(paramList);
                    con.Open();
                    val = await cmd.ExecuteNonQueryAsync();
                }
                con.Close();


            }
            return val;
        }
        public async Task<List<T>> ExecuteADOSqlCommand<T>(List<SqlParameter> sqlParams, string command, string clientName)
        {
            List<T> list = null;
            //Try catch added temperorily due to some of DBs do not having some SPS while synching especially dash board sync
            try
            {
                using (SqlConnection con = new SqlConnection(getConnectionString(clientName)))
                {
                    using (SqlCommand cmd = new SqlCommand(command, con))
                    {
                        cmd.CommandTimeout = 360;
                        cmd.CommandText = command;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] paramList = sqlParams.ToArray();
                        cmd.Parameters.AddRange(paramList);

                        con.Open();
                        // Create a DataReader  
                        using (DbDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                        {
                            list = rdr.ToList<T>();
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                list = new List<T>();
                logger.LogError(Environment.NewLine + $"Something went wrong: {ex}" + Environment.NewLine);
            }

            return list;
        }

        public string getConnectionString(string clientName)
        {
            return config.GetConnectionString($"{clientName}Connection") ?? "";

        }

        #endregion

    }
}
