using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.App.Contracts;
using Common.Infra.GenericRepos;
using BankFeed.App.Contracts;
using BankFeed.Domain.DataModel;
using BankFeed.Infra.DBCon;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using BankFeed.Domain.DTO.Model;

namespace BankFeed.Infra.DataRepos
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public BankFeedsContext context;
        #region Ctor
        public UnitOfWork(BankFeedsContext cont)
        {
            this.context = cont;
        }

        #endregion Ctor

        #region Fields
        public IRepository<FeedAccount> accountInfos;
        public IRepository<Providers> providerInfos;
        public IRepository<ProviderRegister> providerRegisters;


        public IRepository<FeedInstitute> instituteInfos;
        public IRepository<AttentionRequiredBankAccounts> attentionRequiredBankAccounts;
        public IRepository<FeedAccountMapping> accountMappings;
        public IRepository<FeedConnectionLog> connectionLogs;

        public IRepository<FeedTransactions> feedTransactions;
        public IRepository<FeedTransactionsDeleted> FeedTransactionsDelete;


        public IRepository<FeedTransactionMapping> feedTransactionMappings;
        public IRepository<FeedTransactionMappingDeleted> FeedTransactionMappingDelete;

        public IRepository<FeedTransSharing> feedTranscationSharing { get; set; }

        public IRepository<FeedPossibleTransSharing> feedPossibleTransSharings { get; set; }
        public IRepository<FeedTransactionPossibleMatches> feedTransactionPossibleMatches { get; set; }
        public IRepository<FeedRule> feedRules;

        public IRepository<FeedRuleMapping> feedRuleMappings;
        public IRepository<FeedAccountMergeSettings> feedAccountMergeSettings;
        public IRepository<FeedRuleDetails> feedRuleDetails;

        public IRepository<FeedSynchSettings> feedSynchSettings;

        public IRepository<FeedTimeZone> timeZones;

        public IRepository<ImportFormatSettings> importFormatSettings;
        public IRepository<FeedSettings> feedSettings;
        public IRepository<ProviderInsChangeLog> providerInsChangeLogs;
        public IRepository<NimbleAccountMergeSettings> nimbleAccountMergeSettings;
        public IRepository<FeedCategory> feedCategory;
        public IRepository<FeedBankOrCreditAccounts> feedBankOrCreditAccounts;
        #endregion

        public IRepository<FeedAccount> FeedAccounts
        {
            get
            {
                if (this.accountInfos == null)
                {
                    this.accountInfos = new Repository<FeedAccount>(context);
                }
                return accountInfos;
            }
        }
        public IRepository<Providers> Providers
        {
            get
            {
                if (this.providerInfos == null)
                {
                    this.providerInfos = new Repository<Providers>(context);
                }
                return providerInfos;
            }
        }
        public IRepository<ProviderRegister> ProviderRegisters
        {
            get
            {
                if (this.providerRegisters == null)
                {
                    this.providerRegisters = new Repository<ProviderRegister>(context);
                }
                return providerRegisters;
            }
        }

        public IRepository<FeedInstitute> FeedInstitutes
        {
            get
            {
                if (this.instituteInfos == null)
                {
                    this.instituteInfos = new Repository<FeedInstitute>(context);
                }
                return instituteInfos;
            }
        }
        public IRepository<AttentionRequiredBankAccounts> AttentionRequiredBankAccounts
        {
            get
            {
                if (this.attentionRequiredBankAccounts == null)
                {
                    this.attentionRequiredBankAccounts = new Repository<AttentionRequiredBankAccounts>(context);
                }
                return attentionRequiredBankAccounts;
            }
        }
        public IRepository<FeedAccountMapping> FeedAccountMappings
        {
            get
            {
                if (this.accountMappings == null)
                {
                    this.accountMappings = new Repository<FeedAccountMapping>(context);
                }
                return accountMappings;
            }
        }

        public IRepository<FeedAccountMergeSettings> FeedAccountMergeSettings
        {
            get
            {
                if (this.feedAccountMergeSettings == null)
                {
                    this.feedAccountMergeSettings = new Repository<FeedAccountMergeSettings>(context);
                }
                return feedAccountMergeSettings;
            }
        }
        public IRepository<FeedConnectionLog> FeedConnectionLogs
        {
            get
            {
                if (this.connectionLogs == null)
                {
                    this.connectionLogs = new Repository<FeedConnectionLog>(context);
                }
                return connectionLogs;
            }
        }
        public IRepository<FeedTransactions> FeedTransactions
        {
            get
            {
                if (this.feedTransactions == null)
                {
                    this.feedTransactions = new Repository<FeedTransactions>(context);
                }
                return feedTransactions;
            }
        }
        public IRepository<FeedTransactionsDeleted> FeedTransactionsDeleted
        {
            get
            {
                if (this.FeedTransactionsDelete == null)
                {
                    this.FeedTransactionsDelete = new Repository<FeedTransactionsDeleted>(context);
                }
                return FeedTransactionsDelete;
            }
        }

        public IRepository<FeedTransactionMapping> FeedTransactionMappings
        {
            get
            {
                if (this.feedTransactionMappings == null)
                {
                    this.feedTransactionMappings = new Repository<FeedTransactionMapping>(context);
                }
                return feedTransactionMappings;
            }
        }
        public IRepository<FeedTransactionMappingDeleted> FeedTransactionMappingDeleted
        {
            get
            {
                if (this.FeedTransactionMappingDelete == null)
                {
                    this.FeedTransactionMappingDelete = new Repository<FeedTransactionMappingDeleted>(context);
                }
                return FeedTransactionMappingDelete;
            }
        }

        public IRepository<FeedRule> FeedRules
        {
            get
            {
                if (this.feedRules == null)
                {
                    this.feedRules = new Repository<FeedRule>(context);
                }
                return feedRules;
            }
        }

        public IRepository<FeedRuleDetails> FeedRuleDetails
        {
            get
            {
                if (this.feedRuleDetails == null)
                {
                    this.feedRuleDetails = new Repository<FeedRuleDetails>(context);
                }
                return feedRuleDetails;
            }
        }
        public IRepository<FeedRuleMapping> FeedRuleMappings
        {
            get
            {
                if (this.feedRuleMappings == null)
                {
                    this.feedRuleMappings = new Repository<FeedRuleMapping>(context);
                }
                return feedRuleMappings;
            }
        }

        public IRepository<FeedSynchSettings> FeedSynchSettings
        {
            get
            {
                if (this.feedSynchSettings == null)
                {
                    this.feedSynchSettings = new Repository<FeedSynchSettings>(context);
                }
                return feedSynchSettings;
            }
        }

        public IRepository<FeedTimeZone> FeedTimeZones
        {
            get
            {
                if (this.timeZones == null)
                {
                    this.timeZones = new Repository<FeedTimeZone>(context);
                }
                return timeZones;
            }
        }

        public IRepository<ImportFormatSettings> ImportFormatSettings
        {
            get
            {
                if (this.importFormatSettings == null)
                {
                    this.importFormatSettings = new Repository<ImportFormatSettings>(context);
                }
                return importFormatSettings;
            }
        }
        public IRepository<FeedSettings> FeedSettings
        {
            get
            {
                if (this.feedSettings == null)
                {
                    this.feedSettings = new Repository<FeedSettings>(context);
                }
                return feedSettings;
            }
        }

        public IRepository<ProviderInsChangeLog> ProviderInsChangeLogs
        {
            get
            {
                if (this.providerInsChangeLogs == null)
                {
                    this.providerInsChangeLogs = new Repository<ProviderInsChangeLog>(context);
                }
                return providerInsChangeLogs;
            }
        }
        public IRepository<FeedTransSharing> FeedTranscationSharing
        {
            get
            {
                if (this.feedTranscationSharing == null)
                {
                    this.feedTranscationSharing = new Repository<FeedTransSharing>(context);
                }
                return feedTranscationSharing;
            }
        }
        public IRepository<FeedPossibleTransSharing> FeedPossibleTransSharings
        {
            get
            {
                if (this.feedPossibleTransSharings == null)
                {
                    this.feedPossibleTransSharings = new Repository<FeedPossibleTransSharing>(context);
                }
                return feedPossibleTransSharings;
            }
        }

        public IRepository<FeedTransactionPossibleMatches> FeedTransactionPossibleMatches
        {
            get
            {
                if (this.feedTransactionPossibleMatches == null)
                {
                    this.feedTransactionPossibleMatches = new Repository<FeedTransactionPossibleMatches>(context);
                }
                return feedTransactionPossibleMatches;
            }
        }

        public IRepository<NimbleAccountMergeSettings> NimbleAccountMergeSettings
        {
            get
            {
                if (this.nimbleAccountMergeSettings == null)
                {
                    this.nimbleAccountMergeSettings = new Repository<NimbleAccountMergeSettings>(context);
                }
                return nimbleAccountMergeSettings;
            }
        }
        public IRepository<FeedCategory> FeedCategory
        {
            get
            {
                if (this.feedCategory == null)
                {
                    this.feedCategory = new Repository<FeedCategory>(context);
                }
                return feedCategory;
            }
        }
        public IRepository<FeedBankOrCreditAccounts> FeedBankOrCreditAccounts
        {
            get
            {
                if (this.feedBankOrCreditAccounts == null)
                {
                    this.feedBankOrCreditAccounts = new Repository<FeedBankOrCreditAccounts>(context);
                }
                return feedBankOrCreditAccounts;
            }
        }
        #region CUD
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
            //if (res > 0)
            //    context.ChangeTracker.Clear();

            //return res;
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

        #region SqlCommands

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
