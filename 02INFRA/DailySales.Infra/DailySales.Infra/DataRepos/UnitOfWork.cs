using Common.App.Contracts;
using Common.Infra.GenericRepos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DailySales.App.Contracts;
using DailySales.Infra.DBCon;
using DailySales.Domain.DataModel;


using Microsoft.EntityFrameworkCore;
//using DataModel.Domain.DataModel;
//using ApprovalPloicy = DailySales.Domain.DataModel.ApprovalPloicy;


namespace DailySales.Infra.DataRepos
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public DailySalesContext context;
        #region Ctor
        public UnitOfWork(DailySalesContext cont)
        {
            this.context = cont;
        }

        #endregion Ctor

        #region IRepository Fields

        private IRepository<DailyConfigLine> dailyConfigLines;
        private IRepository<DailyConfigGroup> dailyConfigGroups;
        private IRepository<DailyConfigDepartment> dailyConfigDepartments;

        private IRepository<DailySaleDefaults> dailySaleDefaults;
        private IRepository<DailyConfigVerification> dailyConfigVerifications;
        private IRepository<DailyConfigInputEntry> dailyConfigInputEntries;

        private IRepository<DailyConfigInputEntryAr> dailyConfigInputEntryARs;

        private IRepository<DailyConfigInputEntryArdetails> dailyConfigInputEntryARDetails;
        private IRepository<DailyConfigInputEntryReceipt> dailyConfigInputEntryReceipts;
        private IRepository<DailyConfigInputEntryReceiptDetails> dailyConfigInputEntryReceiptDetails;

        private IRepository<DailyConfigInputEntryRevenue> dailyConfigInputEntryRevenues;
        private IRepository<DailyConfigInputEntryRevenueDetails> dailyConfigInputEntryRevenueDetails;
        private IRepository<DailyConfigInputEntryStatistics> dailyConfigInputEntryStatistics;

        private IRepository<DailyConfigInputEntryStatisticsDetails> dailyConfigInputEntryStatisticsDetails;

        

        private IRepository<PmscorporationMapping> pmsCorporationMappings;
        private IRepository<PmscorporationMappindDetails> pmsCorporationMappindDetails;

        private IRepository<Pmsinfo> pmsInfos;
        
        private IRepository<PmsclientInfo> pmsClientInfos;
         
        private IRepository<PmsfacilityMap> pmsFacilityMaps;
        private IRepository<PmsfacilityMapDetails> PmsfacilityMapDetails;
        private IRepository<UnMappedDailyConfigInputEntry> unMappedDailyConfigInputEntrys;
        private IRepository<DummyPmscorpMappingDetails> dummyPmscorpMappingDetails;

        //private IRepository<Corporation> corporations;
        private IRepository<Domain.DataModel.Store> stores;
        private IRepository<Domain.DataModel.JournalEntry> journalEntries;
        private IRepository<Domain.DataModel.Transaction> transactions;
        private IRepository<Lookup> lookups;
        private IRepository<DummyDailyConfigInputEntry> dummyDailyConfigInputEntries;
        private IRepository<DummyDailyConfigInputEntryArreceipt> dummyDailyConfigInputEntryArreceipts;
        private IRepository<DummyDailyConfigInputEntryRevStat> dummyDailyConfigInputEntryRevStats;
        private IRepository<Preferences> preferences;
        private IRepository<AdjustOpeningBalance> adjustOpeningBalances;
        private IRepository<ApprovalPloicy> approvalploicy;
        private IRepository<ApprovalPloicyDetails> approvalploicydetails;
        private IRepository<ApprovalComments> approvalcomments;
        private IRepository<Dsdeposits> dsdeposits;
        private IRepository<DsdepositInfo> dsdepositinfo;
        private IRepository<DsdepositInfoDetails> dsdepositinfodetails;
        private IRepository<DsCashCheckFilterReq> dscashcheckfilterreq;
        private IRepository<JournalEntryExt> journalentryext;
        private IRepository<MerchantCoapreference> merchantCoapreference;
        public IRepository<ReqMerchantRecon> reqMerchantRecon;
        private IRepository<MerchantReconciliation> merchantReconciliation;
        private IRepository<MerchantReconciliationDetail> merchantReconciliationDetail;

        private IRepository<OtbforeCastDetails> otbforeCastDetails;
        private IRepository<OtbforeCastIndividualDetails> otbforeCastIndividualDetails;
        private IRepository<OtbforeCastInfo> otbforeCastInfo;

        #endregion

        public IRepository<DailyConfigLine> DailyConfigLines
        {
            get
            {
                if (this.dailyConfigLines == null)
                {
                    this.dailyConfigLines = new Repository<DailyConfigLine>(context);
                }
                return dailyConfigLines;
            }
        }
        public IRepository<DailyConfigGroup> DailyConfigGroups
        {
            get
            {
                if (this.dailyConfigGroups == null)
                {
                    this.dailyConfigGroups = new Repository<DailyConfigGroup>(context);
                }
                return dailyConfigGroups;
            }
        }
        public IRepository<DailyConfigDepartment> DailyConfigDepartments
        {
            get
            {
                if (this.dailyConfigDepartments == null)
                {
                    this.dailyConfigDepartments = new Repository<DailyConfigDepartment>(context);
                }
                return dailyConfigDepartments;
            }
        }
        public IRepository<DailySaleDefaults> DailySaleDefaults
        {
            get
            {
                if (this.dailySaleDefaults == null)
                {
                    this.dailySaleDefaults = new Repository<DailySaleDefaults>(context);
                }
                return dailySaleDefaults;
            }
        }

        public IRepository<DailyConfigVerification> DailyConfigVerifications
        {
            get
            {
                if (this.dailyConfigVerifications == null)
                {
                    this.dailyConfigVerifications = new Repository<DailyConfigVerification>(context);
                }
                return dailyConfigVerifications;
            }
        }
        public IRepository<DailyConfigInputEntry> DailyConfigInputEntries
        {
            get
            {
                if (this.dailyConfigInputEntries == null)
                {
                    this.dailyConfigInputEntries = new Repository<DailyConfigInputEntry>(context);
                }
                return dailyConfigInputEntries;
            }
        }

        public IRepository<DailyConfigInputEntryAr> DailyConfigInputEntryARs
        {
            get
            {
                if (this.dailyConfigInputEntryARs == null)
                {
                    this.dailyConfigInputEntryARs = new Repository<DailyConfigInputEntryAr>(context);
                }
                return dailyConfigInputEntryARs;
            }
        }
        public IRepository<DailyConfigInputEntryArdetails> DailyConfigInputEntryARDetails
        {
            get
            {
                if (this.dailyConfigInputEntryARDetails == null)
                {
                    this.dailyConfigInputEntryARDetails = new Repository<DailyConfigInputEntryArdetails>(context);
                }
                return dailyConfigInputEntryARDetails;
            }
        }


        public IRepository<DailyConfigInputEntryReceipt> DailyConfigInputEntryReceipts
        {
            get
            {
                if (this.dailyConfigInputEntryReceipts == null)
                {
                    this.dailyConfigInputEntryReceipts = new Repository<DailyConfigInputEntryReceipt>(context);
                }
                return dailyConfigInputEntryReceipts;
            }
        }


        public IRepository<DailyConfigInputEntryReceiptDetails> DailyConfigInputEntryReceiptDetails
        {
            get
            {
                if (this.dailyConfigInputEntryReceiptDetails == null)
                {
                    this.dailyConfigInputEntryReceiptDetails = new Repository<DailyConfigInputEntryReceiptDetails>(context);
                }
                return dailyConfigInputEntryReceiptDetails;
            }
        }

        public IRepository<DailyConfigInputEntryRevenue> DailyConfigInputEntryRevenues
        {
            get
            {
                if (this.dailyConfigInputEntryRevenues == null)
                {
                    this.dailyConfigInputEntryRevenues = new Repository<DailyConfigInputEntryRevenue>(context);
                }
                return dailyConfigInputEntryRevenues;
            }
        }
        public IRepository<DailyConfigInputEntryRevenueDetails> DailyConfigInputEntryRevenueDetails
        {
            get
            {
                if (this.dailyConfigInputEntryRevenueDetails == null)
                {
                    this.dailyConfigInputEntryRevenueDetails = new Repository<DailyConfigInputEntryRevenueDetails>(context);
                }
                return dailyConfigInputEntryRevenueDetails;
            }
        }

        public IRepository<DailyConfigInputEntryStatistics> DailyConfigInputEntryStatistics
        {
            get
            {
                if (this.dailyConfigInputEntryStatistics == null)
                {
                    this.dailyConfigInputEntryStatistics = new Repository<DailyConfigInputEntryStatistics>(context);
                }
                return dailyConfigInputEntryStatistics;
            }
        }

        public IRepository<DailyConfigInputEntryStatisticsDetails> DailyConfigInputEntryStatisticsDetails
        {
            get
            {
                if (this.dailyConfigInputEntryStatisticsDetails == null)
                {
                    this.dailyConfigInputEntryStatisticsDetails = new Repository<DailyConfigInputEntryStatisticsDetails>(context);
                }
                return dailyConfigInputEntryStatisticsDetails;
            }
        }

        
        public IRepository<PmscorporationMapping> pmscorporationmappings
        {
            get
            {
                if (this.pmsCorporationMappings == null)
                {
                    this.pmsCorporationMappings = new Repository<PmscorporationMapping>(context);
                }
                return pmsCorporationMappings;
            }
        }

        public IRepository<PmscorporationMappindDetails> pmscorporationmappingdetails
        {
            get
            {
                if (this.pmsCorporationMappindDetails == null)
                {
                    this.pmsCorporationMappindDetails = new Repository<PmscorporationMappindDetails>(context);
                }
                return pmsCorporationMappindDetails;
            }
        }
        public IRepository<Pmsinfo> pmsInfo
        {
            get
            {
                if (this.pmsInfos == null)
                {
                    this.pmsInfos = new Repository<Pmsinfo>(context);
                }
                return pmsInfos;
            }
        }
        public IRepository<PmsclientInfo> pmsclientInfo {
            get
            {
                if (this.pmsClientInfos == null)
                {
                    this.pmsClientInfos = new Repository<PmsclientInfo>(context);
                }
                return pmsClientInfos;
            }
        }
        public IRepository<PmsfacilityMap> pmsfacilityMap
        {
            get
            {
                if (this.pmsFacilityMaps == null)
                {
                    this.pmsFacilityMaps = new Repository<PmsfacilityMap>(context);
                }
                return pmsFacilityMaps;
            }
        }
        public IRepository<PmsfacilityMapDetails> pmsfacilityMapDetails {
            get
            {
                if (this.PmsfacilityMapDetails == null)
                {
                    this.PmsfacilityMapDetails = new Repository<PmsfacilityMapDetails>(context);
                }
                return PmsfacilityMapDetails; 
            }
        }


        public IRepository<UnMappedDailyConfigInputEntry> unmappedDailyConfigInputEntrys
        {
            get
            {
                if (this.unMappedDailyConfigInputEntrys == null)
                {
                    this.unMappedDailyConfigInputEntrys = new Repository<UnMappedDailyConfigInputEntry>(context);
                }
                return unMappedDailyConfigInputEntrys;
            }
        }

        public IRepository<DummyPmscorpMappingDetails> dummypmscorpMappingDetails
        {
            get
            {
                if (this.dummyPmscorpMappingDetails == null)
                {
                    this.dummyPmscorpMappingDetails = new Repository<DummyPmscorpMappingDetails>(context);
                }
                return dummyPmscorpMappingDetails;
            }
        }

       

        public IRepository<Domain.DataModel.Store> Stores
        {
            get
            {
                if (this.stores == null)
                {
                    this.stores = new Repository<Domain.DataModel.Store>(context);
                }
                return stores;
            }
        }

        public IRepository<Domain.DataModel.JournalEntry> JournalEntries
        {
            get
            {
                if (this.journalEntries == null)
                {
                    this.journalEntries = new Repository<Domain.DataModel.JournalEntry>(context);
                }
                return journalEntries;
            }
        }

        public IRepository<Domain.DataModel.Transaction> Transactions
        {
            get
            {
                if (this.transactions == null)
                {
                    this.transactions = new Repository<Domain.DataModel.Transaction>(context);
                }
                return transactions;
            }
        }

        public IRepository<Lookup> Lookups
        {
            get
            {
                if (this.lookups == null)
                {
                    this.lookups = new Repository<Lookup>(context);
                }
                return lookups;
            }
        }

        public IRepository<DummyDailyConfigInputEntry> DummyDailyConfigInputEntries
        {
            get
            {
                if (this.dummyDailyConfigInputEntries == null)
                {
                    this.dummyDailyConfigInputEntries = new Repository<DummyDailyConfigInputEntry>(context);
                }
                return dummyDailyConfigInputEntries;
            }
        }

        public IRepository<DummyDailyConfigInputEntryArreceipt> DummyDailyConfigInputEntryArreceipts
        {
            get
            {
                if (this.dummyDailyConfigInputEntryArreceipts == null)
                {
                    this.dummyDailyConfigInputEntryArreceipts = new Repository<DummyDailyConfigInputEntryArreceipt>(context);
                }
                return dummyDailyConfigInputEntryArreceipts;
            }
        }

        public IRepository<DummyDailyConfigInputEntryRevStat> DummyDailyConfigInputEntryRevStats
        {
            get
            {
                if (this.dummyDailyConfigInputEntryRevStats == null)
                {
                    this.dummyDailyConfigInputEntryRevStats = new Repository<DummyDailyConfigInputEntryRevStat>(context);
                }
                return dummyDailyConfigInputEntryRevStats;
            }
        }

        public IRepository<Preferences> Preferences
        {
            get
            {
                if (this.preferences == null)
                {
                    this.preferences = new Repository<Preferences>(context);
                }
                return preferences;
            }
        }
        public IRepository<OtbforeCastDetails> OtbforeCastDetails
        {
            get
            {
                if (this.otbforeCastDetails == null)
                {
                    this.otbforeCastDetails = new Repository<OtbforeCastDetails>(context);
                }
                return otbforeCastDetails;
            }
        }
        public IRepository<OtbforeCastIndividualDetails> OtbforeCastIndividualDetails
        {
            get
            {
                if (this.otbforeCastIndividualDetails == null)
                {
                    this.otbforeCastIndividualDetails = new Repository<OtbforeCastIndividualDetails>(context);
                }
                return otbforeCastIndividualDetails;
            }
        }
        public IRepository<OtbforeCastInfo> OtbforeCastInfo
        {
            get
            {
                if (this.otbforeCastInfo == null)
                {
                    this.otbforeCastInfo = new Repository<OtbforeCastInfo>(context);
                }
                return otbforeCastInfo;
            }
        }

        public IRepository<AdjustOpeningBalance> AdjustOpeningBalances
        {
            get
            {
                if (this.adjustOpeningBalances == null)
                {
                    this.adjustOpeningBalances = new Repository<AdjustOpeningBalance>(context);
                }
                return adjustOpeningBalances;

            }
        }
        public IRepository<ApprovalPloicy> ApprovalPloicy
        {
            get
            {
                if (this.approvalploicy == null)
                {
                    this.approvalploicy = new Repository<ApprovalPloicy>(context);
                }
                return approvalploicy;

            }
        }

        public IRepository<ApprovalPloicyDetails> ApprovalPloicyDetails
        {
            get
            {
                if (this.approvalploicydetails == null)
                {
                    this.approvalploicydetails = new Repository<ApprovalPloicyDetails>(context);
                }
                return approvalploicydetails;

            }
        }

        public IRepository<ApprovalComments> ApprovalComments
        {
            get
            {
                if (this.approvalcomments == null)
                {
                    this.approvalcomments = new Repository<ApprovalComments>(context);
                }
                return approvalcomments;

            }
        }
        public IRepository<Dsdeposits> DsDeposits
        {
            get
            {
                if (this.dsdeposits == null)
                {
                    this.dsdeposits = new Repository<Dsdeposits>(context);
                }
                return dsdeposits;

            }
        }
        public IRepository<DsdepositInfo> DsDepositInfo
        {
            get
            {
                if (this.dsdepositinfo == null)
                {
                    this.dsdepositinfo = new Repository<DsdepositInfo>(context);
                }
                return dsdepositinfo;

            }
        }
        public IRepository<DsdepositInfoDetails> DsDepositInfoDetails
        {
            get
            {
                if (this.dsdepositinfodetails == null)
                {
                    this.dsdepositinfodetails = new Repository<DsdepositInfoDetails>(context);
                }
                return dsdepositinfodetails;

            }
        }

        public IRepository<DsCashCheckFilterReq> DsCashCheckFilterReq
        {
            get
            {
                if (this.dscashcheckfilterreq == null)
                {
                    this.dscashcheckfilterreq = new Repository<DsCashCheckFilterReq>(context);
                }
                return dscashcheckfilterreq;

            }
        }
        public IRepository<JournalEntryExt> JournalEntryExt
        {
            get
            {
                if (this.journalentryext == null)
                {
                    this.journalentryext = new Repository<JournalEntryExt>(context);
                }
                return journalentryext;

            }
        }
        public IRepository<MerchantCoapreference> MerchantCoapreferences
        {
            get
            {
                if (this.merchantCoapreference == null)
                {
                    this.merchantCoapreference = new Repository<MerchantCoapreference>(context);
                }
                return merchantCoapreference;
            }
        }


        public IRepository<MerchantReconciliation> MerchantReconciliation
        {
            get
            {
                if (this.merchantReconciliation == null)
                {
                    this.merchantReconciliation = new Repository<MerchantReconciliation>(context);
                }
                return merchantReconciliation;
            }
        }

        public IRepository<MerchantReconciliationDetail> MerchantReconciliationDetail
        {
            get
            {
                if (this.merchantReconciliationDetail == null)
                {
                    this.merchantReconciliationDetail = new Repository<MerchantReconciliationDetail>(context);
                }
                return merchantReconciliationDetail;
            }
        }
        public IRepository<ReqMerchantRecon> ReqMerchantRecon
        {
            get
            {
                if (this.reqMerchantRecon == null)
                {
                    this.reqMerchantRecon = new Repository<ReqMerchantRecon>(context);
                }
                return reqMerchantRecon;
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
                                    //string value = rdr[prop.Name]?.ToString();
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
