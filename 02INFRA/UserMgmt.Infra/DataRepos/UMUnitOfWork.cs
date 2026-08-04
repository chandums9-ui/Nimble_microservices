using Common.App.Contracts;
using Common.Infra.GenericRepos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using UserMgmt.App.Contracts;
using UserMgmt.Domain.DataModel;
using UserMgmt.Infra.DBCon;

namespace UserMgmt.Infra.DataRepos
{
    public  class UMUnitOfWork  : IUnitOfWork,IDisposable
    {
        #region IRepository Fields
        public UMDBContext context;

        public IRepository<UserInfo> userInfos;
        public IRepository<Urlinfo> urlInfos;
        public IRepository<ClientInfo> clientInfos;

        public IRepository<ServerInfo> serverInfos;
        public IRepository<UrlserverInfo> urlserverInfos;
        public IRepository<UserClientLink> userClientLinks;
        
        public IRepository<UserLoginActivity> userLoginActivities;

        public IRepository<UserPrevileges> userPrevileges;
        public IRepository<UserCorpLink> userCorpLinks;
        public IRepository<Role> roles;
        public IRepository<RolePrevileges> rolePrevileges;

        public IRepository<UserPclink> userPclinks;

        public IRepository<DumpUserInformation> dumpUserInfo;
        #endregion

        #region CTOR
        public UMUnitOfWork(UMDBContext cont)
        {
            this.context = cont;
        }

        #endregion

        public IRepository<UserInfo> UserInfos
        {
            get
            {
                if (this.userInfos == null)
                {
                    this.userInfos = new Repository<UserInfo>(context);
                }
                return userInfos;
            }
        }
        public IRepository<Urlinfo> UrlInfos
        {
            get
            {   
                if (this.urlInfos == null)
                {
                    this.urlInfos = new Repository<Urlinfo>(context);
                }
                return urlInfos;
            }
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



        public IRepository<ServerInfo> ServerInfos
        {
            get
            {
                if (this.serverInfos == null)
                {
                    this.serverInfos = new Repository<ServerInfo>(context);
                }
                return serverInfos;
            }
        }


        public IRepository<UrlserverInfo> UrlserverInfos
        {
            get
            {
                if (this.urlserverInfos == null)
                {
                    this.urlserverInfos = new Repository<UrlserverInfo>(context);
                }
                return urlserverInfos;
            }
        }


        public IRepository<UserClientLink> UserClientLinks
        {
            get
            {
                if (this.userClientLinks == null)
                {
                    this.userClientLinks = new Repository<UserClientLink>(context);
                }
                return userClientLinks;
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
        public IRepository<UserPrevileges> UserPrevileges
        {
            get
            {
                if(this.userPrevileges == null)
                {
                    this.userPrevileges = new Repository<UserPrevileges>(context);
                }
                return userPrevileges;
            }
        }
        public IRepository<UserCorpLink> UserCorpLinks
        {
            get
            {
                if (this.userCorpLinks == null)
                {
                    this.userCorpLinks = new Repository<UserCorpLink>(context);
                }
                return userCorpLinks;
            }
        }

        public IRepository<Role> Role
        {
            get 
            {
                if (this.roles == null)
                {
                    this.roles = new Repository<Role>(context);
                }
                return roles;

            }
        }

        public IRepository<RolePrevileges> RolePrevileges
        {
            get
            {
                if(this.rolePrevileges == null)
                {
                    this.rolePrevileges=new Repository<RolePrevileges>(context);
                }
                return rolePrevileges;
                
            }
        }
        public IRepository<UserPclink> UserPclinks
        {
            get
            {
                if (this.userPclinks == null)
                {
                    this.userPclinks = new Repository<UserPclink>(context);
                }
                return userPclinks;

            }
        }

        public IRepository<DumpUserInformation> DumpUserInfo
        {
            get
            {
                if (this.dumpUserInfo == null)
                {
                    this.dumpUserInfo = new Repository<DumpUserInformation>(context);
                }
                return dumpUserInfo;
            }
        }

        #region Methods   

        #region CUD
        public void ClearChangeTracker()
        {
            context.ChangeTracker.Clear();
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

        #endregion

        #region SQLCommand
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

        #endregion

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
        #endregion

    }
}
