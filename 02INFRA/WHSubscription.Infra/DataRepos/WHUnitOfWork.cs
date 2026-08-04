using Common.App.Contracts;
using Common.Infra.DataRepos;
using Common.Infra.Extentions;
using Common.Infra.GenericRepos;
using DataModel.Domain.DataModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHSubscription.App.Contract;
using WHSubscription.Domain.DataModel;
using WHSubscription.Infra.DBCon;

namespace WHSubscription.Infra.DataRepos
{
    public class WHUnitOfWork :IUnitOfWork
    {
        #region IRepository Fields

 
        public IRepository<Whsubscription> whsubscription;
        public IRepository<Whlog> whlog;
        #endregion
        #region variables
        public WHSubscriptionContext context;
        public IConfiguration config;
        private readonly ILoggerService logger;
        #endregion
        #region Ctor
        public WHUnitOfWork(WHSubscriptionContext cont, IConfiguration _config, ILoggerService _logger)
        {
            this.context = cont;
            this.config = _config;
            this.logger = _logger;
        }

        #endregion
        #region IRepository WHSubscriptions
        public IRepository<Whsubscription> WHSubscriptions
        {
            get
            {
                if (this.whsubscription == null)
                {
                    this.whsubscription = new Repository<Whsubscription>(context);
                }
                return this.whsubscription;
            }
        }
        public IRepository<Whlog> WHLogs
        {
            get
            {
                if (this.whlog == null)
                {
                    this.whlog = new Repository<Whlog>(context);
                }
                return this.whlog;
            }
        }
        #endregion
        #region CUD       

        public int Save()
        {
            return context.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
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

        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }
        #endregion
        #region SqlCommand  
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
        public async Task<List<T>> ExecuteSqlCommand<T>(List<SqlParameter> sqlParams, string command)
        {
            List<T> list;
            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = 360;
            cmd.CommandText = command;
            cmd.CommandType = CommandType.StoredProcedure;
            // Only add parameters if provided
            if (sqlParams != null && sqlParams.Count > 0)
            {
                SqlParameter[] paramList = sqlParams.ToArray();
                cmd.Parameters.AddRange(paramList);
            }
            await context.Database.OpenConnectionAsync();
            // Create a DataReader  
            using (DbDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {

                list = rdr.ToList<T>();

            }
            return list;
        }



        #endregion

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

        #region Dispose
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

        #endregion Dispose
    }
}
