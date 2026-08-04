using Common.App.Contracts;
using Common.Domain.DTO.Resp;
using Common.Infra.DataRepos;
using Common.Infra.Extentions;
using Common.Infra.GenericRepos;
using CoreAccounting.App.Contracts;
using CoreAccounting.Infra.DBCon;
using Dashboard.Domain.DTO.Resp;
using DataModel.Domain.DataModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Ocsp;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;


namespace CoreAccounting.Infra.DataRepos
{
    public class CoreUnitOfWork : CommonUnitOfWork, IUnitOfWork, IDisposable
    {
        #region IRepository Fields

        public CoreDBContext context;
        public IConfiguration config;
        private readonly ILoggerService logger;
        #endregion IRepository Fields

        #region Ctor
        public CoreUnitOfWork(CoreDBContext cont, IConfiguration _config, ILoggerService _logger)
        {
            this.context = cont;
            this.config = _config;
            this.logger = _logger;
        }

        #endregion Ctor

        #region IRepository Common
        public IRepository<Account> Accounts
        {
            get
            {
                if (this.accounts == null)
                {
                    this.accounts = new Repository<Account>(context);
                }
                return accounts;
            }
        }
        public IRepository<Address> Addresses
        {
            get
            {
                if (this.addresses == null)
                {
                    this.addresses = new Repository<Address>(context);
                }
                return this.addresses;
            }
        }
        public IRepository<ApprovalComments> ApprovalComments
        {
            get
            {
                if (this.approvalComments == null)
                {
                    this.approvalComments = new Repository<ApprovalComments>(context);
                }
                return this.approvalComments;
            }
        }
        public IRepository<Business> Business
        {
            get
            {
                if (this.business == null)
                {
                    this.business = new Repository<Business>(context);
                }
                return business;
            }
        }
        public IRepository<Corporation> Corporations
        {
            get
            {
                if (this.corporations == null)
                {
                    this.corporations = new Repository<Corporation>(context);
                }
                return corporations;
            }
        }
        public IRepository<UserCorporation> UserCorporation
        {
            get
            {
                if (this.UserCorporations == null)
                {
                    this.UserCorporations = new Repository<UserCorporation>(context);
                }
                return UserCorporations;
            }
        }
        public IRepository<CorporationTaxSettings> CorporationTaxSettings
        {
            get
            {
                if (this.corporationTaxSettings == null)
                {
                    this.corporationTaxSettings = new Repository<CorporationTaxSettings>(context);
                }
                return corporationTaxSettings;
            }
        }
        public IRepository<UseTax> UseTax
        {
            get
            {
                if (this.useTax == null)
                {
                    this.useTax = new Repository<UseTax>(context);
                }
                return useTax;
            }
        }
        public IRepository<UseTaxDetails> UseTaxDetails
        {
            get
            {
                if (this.useTaxDetails == null)
                {
                    this.useTaxDetails = new Repository<UseTaxDetails>(context);
                }
                return useTaxDetails;
            }
        }
        public IRepository<UseTaxRates> UseTaxRates
        {
            get
            {
                if (this.useTaxRates == null)
                {
                    this.useTaxRates = new Repository<UseTaxRates>(context);
                }
                return useTaxRates;
            }
        }

        public IRepository<CreditTerm> CreditTerms
        {
            get
            {
                if (this.creditTerms == null)
                {
                    this.creditTerms = new Repository<CreditTerm>(context);
                }
                return this.creditTerms;
            }
        }
        public IRepository<Frequency> Frequencies
        {
            get
            {
                if (this.frequencies == null)
                {
                    this.frequencies = new Repository<Frequency>(context);
                }
                return this.frequencies;
            }
        }
        public IRepository<ImportDocument> ImportDocuments
        {
            get
            {
                if (this.importDocuments == null)
                {
                    this.importDocuments = new Repository<ImportDocument>(context);
                }
                return importDocuments;
            }
        }
        public IRepository<ImportDocumentDetails> ImportDocumentDetails
        {
            get
            {
                if (this.importDocumentDetails == null)
                {
                    this.importDocumentDetails = new Repository<ImportDocumentDetails>(context);
                }
                return importDocumentDetails;
            }
        }
        public IRepository<JournalEntry> JournalEntries
        {
            get
            {
                if (this.journalEntries == null)
                {
                    this.journalEntries = new Repository<JournalEntry>(context);
                }
                return journalEntries;
            }
        }
        public IRepository<ApfilterReq> ApfilterReqs
        {
            get
            {
                if (this.apfilterReqs == null)
                {
                    this.apfilterReqs = new Repository<ApfilterReq>(context);
                }
                return apfilterReqs;
            }
        }
        public IRepository<BillPayOrInvoiceAdjust> BillPayOrInvoiceAdjusts
        {
            get
            {
                if (this.billPayOrInvoiceAdjusts == null)
                {
                    this.billPayOrInvoiceAdjusts = new Repository<BillPayOrInvoiceAdjust>(context);
                }
                return billPayOrInvoiceAdjusts;
            }
        }
        public IRepository<Employee> Employees
        {
            get
            {
                if (this.employees == null)
                {
                    this.employees = new Repository<Employee>(context);
                }
                return employees;
            }
        }
        public IRepository<PayrollJobInfo> PayrollJobInfos
        {
            get
            {
                if (this.payrollJobInfos == null)
                {
                    this.payrollJobInfos = new Repository<PayrollJobInfo>(context);
                }
                return payrollJobInfos;
            }
        }
        public IRepository<PayrollDepartmentInfo> PayrollDepartmentInfos
        {
            get
            {
                if (this.payrollDepartmentInfos == null)
                {
                    this.payrollDepartmentInfos = new Repository<PayrollDepartmentInfo>(context);
                }
                return payrollDepartmentInfos;
            }
        }
        public IRepository<LoanScheduleTransactions> LoanScheduleTransactions
        {
            get
            {
                if (this.loanScheduleTransactions == null)
                {
                    this.loanScheduleTransactions = new Repository<LoanScheduleTransactions>(context);
                }
                return loanScheduleTransactions;
            }
        }
        public IRepository<LoanScheduleTransactionDetails> LoanScheduleTransactionDetails
        {
            get
            {
                if (this.loanScheduleTransactionDetails == null)
                {
                    this.loanScheduleTransactionDetails = new Repository<LoanScheduleTransactionDetails>(context);
                }
                return loanScheduleTransactionDetails;
            }
        }
        public IRepository<DsdepositInfoDetails> DsdepositInfoDetails
        {
            get
            {
                if (this.dsdepositInfoDetails == null)
                {
                    this.dsdepositInfoDetails = new Repository<DsdepositInfoDetails>(context);
                }
                return dsdepositInfoDetails;
            }
        }
        public IRepository<Payroll> Payroll
        {
            get
            {
                if (this.payroll == null)
                {
                    this.payroll = new Repository<Payroll>(context);
                }
                return payroll;
            }
        }
        public IRepository<JournalEntryExt> JournalEntriesExt
        {
            get
            {
                if (this.journalEntriesExt == null)
                {
                    this.journalEntriesExt = new Repository<JournalEntryExt>(context);
                }
                return journalEntriesExt;
            }
        }
        public IRepository<Purpose> Purposes
        {
            get
            {
                if (this.purposes == null)
                {
                    this.purposes = new Repository<Purpose>(context);
                }
                return this.purposes;
            }
        }
        public IRepository<Reconciliation> Reconciliations
        {
            get
            {
                if (this.reconciliations == null)
                {
                    this.reconciliations = new Repository<Reconciliation>(context);
                }
                return reconciliations;
            }
        }
        public IRepository<Remind> Reminds
        {
            get
            {
                if (this.reminds == null)
                {
                    this.reminds = new Repository<Remind>(context);
                }
                return reminds;
            }
        }
        public IRepository<Repetitive> Repetitives
        {
            get
            {
                if (this.repetitives == null)
                {
                    this.repetitives = new Repository<Repetitive>(context);
                }
                return repetitives;
            }
        }
        public IRepository<RepetitiveCreditTerm> RepetitiveCreditTerms
        {
            get
            {
                if (this.repetitiveCreditTerms == null)
                {
                    this.repetitiveCreditTerms = new Repository<RepetitiveCreditTerm>(context);
                }
                return repetitiveCreditTerms;
            }
        }
        public IRepository<RepetitiveTransaction> RepetitiveTransactions
        {
            get
            {
                if (this.repetitiveTransactions == null)
                {
                    this.repetitiveTransactions = new Repository<RepetitiveTransaction>(context);
                }
                return repetitiveTransactions;
            }
        }
        public IRepository<RepetitiveTransactionInvoice> RepetitiveTransactionInvoices
        {
            get
            {
                if (this.repetitiveTransactionInvoices == null)
                {
                    this.repetitiveTransactionInvoices = new Repository<RepetitiveTransactionInvoice>(context);
                }
                return repetitiveTransactionInvoices;
            }
        }
        public IRepository<Store> ProfitCenters
        {
            get
            {
                if (this.profitCenters == null)
                {
                    this.profitCenters = new Repository<Store>(context);
                }
                return profitCenters;
            }
        }
        public IRepository<Transaction> Transactions
        {
            get
            {
                if (this.transactions == null)
                {
                    this.transactions = new Repository<Transaction>(context);
                }
                return transactions;
            }
        }
        public IRepository<TransactionInvoice> TransactionInvoices
        {
            get
            {
                if (this.transactionInvoices == null)
                {
                    this.transactionInvoices = new Repository<TransactionInvoice>(context);
                }
                return transactionInvoices;
            }
        }
        public IRepository<TransactionTaxInfo> TransactionTaxInfo
        {
            get
            {
                if (this.transactionTaxInfo == null)
                {
                    this.transactionTaxInfo = new Repository<TransactionTaxInfo>(context);
                }
                return transactionTaxInfo;
            }
        }
        public IRepository<UserPreferenceSettings> UserPreferencesSettings
        {
            get
            {
                if (this.userPreferencesSettings == null)
                {
                    this.userPreferencesSettings = new Repository<UserPreferenceSettings>(context);
                }
                return userPreferencesSettings;
            }
        }
        public IRepository<VendorAddress> VendorAddress
        {
            get
            {
                if (this.vendorAddress == null)
                {
                    this.vendorAddress = new Repository<VendorAddress>(context);
                }
                return this.vendorAddress;
            }
        }
        public IRepository<VendorContract> VendorContracts
        {
            get
            {
                if (this.vendorContracts == null)
                {
                    this.vendorContracts = new Repository<VendorContract>(context);
                }
                return this.vendorContracts;
            }
        }

        public IRepository<InterCompanyReferences> InterCompanyReferences
        {
            get
            {
                if (this.interCompanyReferences == null)
                {
                    this.interCompanyReferences = new Repository<InterCompanyReferences>(context);
                }
                return this.interCompanyReferences;
            }

        }

        public IRepository<User> Users
        {
            get
            {
                if (this.users == null)
                {
                    this.users = new Repository<User>(context);
                }
                return users;
            }
        }
        public IRepository<CorporationYearEndProcess> CorporationYearEndProcess
        {
            get
            {
                if (this.corporationYearEndProcess == null)
                {
                    this.corporationYearEndProcess = new Repository<CorporationYearEndProcess>(context);
                }
                return corporationYearEndProcess;
            }
        }
        public IRepository<Locking> Locking
        {
            get
            {
                if (this.locking == null)
                {
                    this.locking = new Repository<Locking>(context);
                }
                return this.locking;
            }
        }
        public IRepository<ApprovalPloicy> ApprovalPloicy
        {
            get
            {
                if (this.approvalPloicy == null)
                {
                    this.approvalPloicy = new Repository<ApprovalPloicy>(context);
                }
                return this.approvalPloicy;
            }
        }
        public IRepository<ApprovalPloicyDetails> ApprovalPloicyDetails
        {
            get
            {
                if (this.approvalPloicyDetails == null)
                {
                    this.approvalPloicyDetails = new Repository<ApprovalPloicyDetails>(context);
                }
                return this.approvalPloicyDetails;
            }
        }
        public IRepository<OcrjournalEntry> OcrJournalEntry
        {
            get
            {
                if (this.ocrjournalEntry == null)
                {
                    this.ocrjournalEntry = new Repository<OcrjournalEntry>(context);
                }
                return this.ocrjournalEntry;
            }
        }
        public IRepository<Ocrtransaction> OcrTransactions
        {
            get
            {
                if (this.ocrtransaction == null)
                {
                    this.ocrtransaction = new Repository<Ocrtransaction>(context);
                }
                return this.ocrtransaction;
            }
        }
        public IRepository<OcrtransactionInvoice> OcrTransactionInvoice
        {
            get
            {
                if (this.ocrtransactionInvoice == null)
                {
                    this.ocrtransactionInvoice = new Repository<OcrtransactionInvoice>(context);
                }
                return this.ocrtransactionInvoice;
            }
        }
        public IRepository<OcrtransactionTaxInfo> OcrtransactionTaxInfo
        {
            get
            {
                if (this.ocrtransactionTaxInfo == null)
                {
                    this.ocrtransactionTaxInfo = new Repository<OcrtransactionTaxInfo>(context);
                }
                return this.ocrtransactionTaxInfo;
            }
        }
        public IRepository<BillEntryInformation> BillEntryInformation
        {
            get
            {
                if (this.billEntryInformation == null)
                {
                    this.billEntryInformation = new Repository<BillEntryInformation>(context);
                }
                return this.billEntryInformation;
            }
        }
        public IRepository<BillPaymentsInformation> BillPaymentsInformation
        {
            get
            {
                if (this.billPaymentsInformation == null)
                {
                    this.billPaymentsInformation = new Repository<BillPaymentsInformation>(context);
                }
                return this.billPaymentsInformation;
            }
        }

        public IRepository<Pctransactions> Pctransactions
        {
            get
            {
                if (this.pctransactions == null)
                {
                    this.pctransactions = new Repository<Pctransactions>(context);
                }
                return this.pctransactions;
            }
        }
        public IRepository<FundtransferFilterReq> FundtransferFilterReq
        {
            get
            {
                if (this.fundtransferFilterReq == null)
                {
                    this.fundtransferFilterReq = new Repository<FundtransferFilterReq>(context);
                }
                return this.fundtransferFilterReq;
            }
        }
        public IRepository<Employee> Employee
        {
            get
            {
                if (this.employee == null)
                {
                    this.employee = new Repository<Employee>(context);
                }
                return this.employee;
            }
        }
        public IRepository<BillEntryPayments> BillEntryPayments
        {
            get
            {
                if (this.billEntryPayments == null)
                {
                    this.billEntryPayments = new Repository<BillEntryPayments>(context);
                }
                return this.billEntryPayments;
            }
        }
        public IRepository<MiscInfo> MiscInfo
        {
            get
            {
                if (this.miscInfo == null)
                {
                    this.miscInfo = new Repository<MiscInfo>(context);
                }
                return this.miscInfo;
            }
        }
        public IRepository<DepartmentConfig> DepartmentConfig
        {
            get
            {
                if (this.departmentConfig == null)
                {
                    this.departmentConfig = new Repository<DepartmentConfig>(context);
                }
                return this.departmentConfig;
            }
        }

        #endregion IRepository Common

        #region IRepository Core

        #endregion IRepository Core

        #region Methods   
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
            int value = (await cmd.ExecuteNonQueryAsync());
            return value;
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
                            logger.LogError(Environment.NewLine + $"Something went wrong {clientName} : {ex}" + Environment.NewLine);
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
            List<T> list=null;
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
                logger.LogError(Environment.NewLine + $"Something went wrong from {clientName}: {ex}" + Environment.NewLine);
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
