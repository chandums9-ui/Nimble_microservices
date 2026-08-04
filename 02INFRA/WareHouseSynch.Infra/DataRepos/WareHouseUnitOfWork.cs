using WareHouseSynch.App.Contracts;
using Common.App.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
                                                                                                                                                                                          
using WareHouseSynch.Domain.DataModel;
using Common.Infra.GenericRepos;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using System.Reflection;
using Common.Domain.DTO.Enums;
using System.Security.Cryptography.Xml;
using Microsoft.Extensions.Configuration;
using Common.Infra.DataRepos;
using WareHouseSynch.Infra.DBCon;
using Common.Infra.Extentions;

namespace WareHouseSynch.Infra.DataRepos
{
    public class WareHouseUnitOfWork : IWareHouseUnitOfWork
    {

        #region Fields
        public NPAnalyticsWareHouseContext context;
        public IConfiguration config;
        public IRepository<FactAccrual> _factAccruals;
        public IRepository<FactAccrualStaging> _factAccrualStaging;

        public IRepository<FactBillStaging> _factBillStaging;
        public IRepository<FactBillPaymentStaging> _factBillPaymentStaging;
        public IRepository<FactBill> _factBills;
        public IRepository<FactBillPayment> _factBillPayments;
        public IRepository<FactRevenSalesAndStat> _factRevenSalesAndStats;
        #endregion
        #region Ctor
        public WareHouseUnitOfWork(NPAnalyticsWareHouseContext cont, IConfiguration _config)
        {
            this.context = cont;
            this.config = _config;
        }
        #endregion

        #region Properties
        public IRepository<FactAccrual> FactAccruals
        {
            get
            {
                if (this._factAccruals == null)
                {
                    this._factAccruals = new Repository<FactAccrual>(context);
                }
                return _factAccruals;
            }
        }

        public IRepository<FactRevenSalesAndStat> FactRevenSalesAndStats
        {
            get
            {
                if (this._factRevenSalesAndStats == null)
                {
                    this._factRevenSalesAndStats = new Repository<FactRevenSalesAndStat>(context);
                }
                return _factRevenSalesAndStats;
            }
        }
        public IRepository<FactAccrualStaging> FactAccrualStaging
        {
            get
            {
                if (this._factAccrualStaging == null)
                {
                    this._factAccrualStaging = new Repository<FactAccrualStaging>(context);
                }
                return _factAccrualStaging;
            }
        }

        public IRepository<FactBill> FactBills
        {
            get
            {
                if (this._factBills == null)
                {
                    this._factBills = new Repository<FactBill>(context);
                }
                return _factBills;
            }
        }
        public IRepository<FactBillPayment> FactBillPayments
        {
            get
            {
                if (this._factBillPayments == null)
                {
                    this._factBillPayments = new Repository<FactBillPayment>(context);
                }
                return _factBillPayments;
            }
        }
        public IRepository<FactBillPaymentStaging> FactBillPaymentStaging
        {
            get
            {
                if (this._factBillPaymentStaging == null)
                {
                    this._factBillPaymentStaging = new Repository<FactBillPaymentStaging>(context);
                }
                return _factBillPaymentStaging;
            }
        }
        public IRepository<FactBillStaging> FactBillStaging
        {
            get
            {
                if (this._factBillStaging == null)
                {
                    this._factBillStaging = new Repository<FactBillStaging>(context);
                }
                return _factBillStaging;
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
        public string getConnectionString(string clientName)
        {
            return config.GetConnectionString($"{clientName}Connection") ?? "";

        }
        #endregion
      
        #region SqlADOCommand  

        public async Task BulkCopyAsync<T>(IEnumerable<T> list, string clientName,string tableName=null)
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
                        catch(Exception ex)
                        {
                            throw;
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
            List<T> list ;

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




            return list;
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
            SqlParameter[] paramList = sqlParams.ToArray();
            cmd.Parameters.AddRange(paramList);
            // Open database connection  
            // OpenConnection(cmd);
            await context.Database.OpenConnectionAsync();
            // Create a DataReader  
            using (DbDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                list = rdr.ToList<T>();

            }


            return list;
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

