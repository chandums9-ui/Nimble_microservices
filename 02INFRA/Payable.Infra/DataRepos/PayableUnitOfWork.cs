using Common.App.Contracts;
using Common.Infra.DataRepos;
using Common.Infra.Extentions;
using Common.Infra.GenericRepos;
using DataModel.Domain.DataModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
 
using Microsoft.Extensions.Configuration;
using Payable.App.Contracts;
using Payable.Domain.DataModel;
using Payable.Infra.DBCon;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using static Payable.Domain.DTO.Resp.Setup1099Res;


namespace Payable.Infra.DataRepos
{
    public class PayableUnitOfWork : CommonUnitOfWork, IUnitOfWork, IDisposable
    {
        #region IRepository Fields

        public PayableDBContext context;
        public IRepository<CorpDiscountAccountConfiguration> corpDiscountAccountConfiguration;
        public IRepository<BillEntryInformation> billEntryInformation;
        public IRepository<ApprovalPloicy> approvalPloicy;
        public IRepository<ApprovalPloicyDetails> approvalPloicyDetails;
        public IRepository<AutoPostingInfo> autoPostingInfo;
        public IRepository<BatchPrint> batchPrint;
        public IRepository<BillEntryDebiitMemoPreference> billEntryDebitMemoPref;
        public IRepository<BillEntryPayments> billEntryPayments;
        public IRepository<BillPayOrInvoiceAdjust> billPayOrInvAdjust;
        public IRepository<BillPaymentsInformation> billPaymentsInfo;
        public IRepository<Business> business;
        public IRepository<BusinessDetails> businessDetails;
        public IRepository<BusinessInfo> businessInfo;
        public IRepository<BusinessPercentage> businessPercentage;
        public IRepository<BusinessPreferences> businessPreferences;
        public IRepository<BusinessRelatedExtension> businessRelatedExtension;
        public IRepository<BusinessTokenDetails> businessTokenDetails;
        public IRepository<Contact> contact;
        public IRepository<CorpAchsettings> corpAchsettings;
        public IRepository<CreditTerm> creditTerm;
        public IRepository<CorporationBankDetails> corporationBankDetails;
        public IRepository<DirectDepositBankDetails> directDepositBankDetails;
        public IRepository<DirectDepositExportDetails> directDepositExportDetails;
        public IRepository<FortePaymentTokens> fortePaymentTokens;
        public IRepository<InterCompanyReferences> interCompanyReferences;
        public IRepository<FortePaymentsSchedule> fortePaymentsSchedule;
        public IRepository<Frequency> frequencies;
        public IRepository<OcrcreditTerm> ocrcreditTerm;
        public IRepository<OcrjournalEntry> ocrjournalEntry;
        public IRepository<Ocrtransaction> ocrtransaction;
        public IRepository<OcrtransactionInvoice> ocrtransactionInvoice;
        public IRepository<UseTax> useTax;
        public IRepository<UseTaxDetails> useTaxDetails;
        public IRepository<UseTaxRates> useTaxRates;
        public IRepository<UserAchdetails> userAchdetails;
        public IRepository<UserCorporation> userCorporation;
        public IRepository<VendorDirectDepositDetails> vendorDirectDepositDetails;
        public IRepository<VendorTaxInfo> vendorTaxInfo;
        public IRepository<Country> country;
        public IRepository<State> state;
        public IRepository<Locking> locking;
        public IRepository<TransactionTaxInfo> transactionTaxInfo;
        public IRepository<CorporationTaxSettings> corporationTaxSettings;
        public IRepository<Pctransactions> pctransactions;
        public IRepository<BillEntryInformationDetails> billEntryInformationDetails;
        public IRepository<UserDirectDepositDetails> userDirectDepositDetails;
        public IRepository<User> users;
        public IRepository<ApfilterReq> apfilterReq;
        public IRepository<DirectDepositAuditInfo> directDepositAuditInfo;
        public IRepository<EftauditInfo> eftAuditinfo;
        public IRepository<Eftcolumns> eFTColumns;
        public IRepository<Eftconfig> eFTConfig;
        public IRepository<MasterBinAuditing> masterBinAuditing;
        public IRepository<MasterLongAuditing> masterLongAuditing;
        public IRepository<RepayConfig> repayConfig;
        public IRepository<RepayAuditInfo> repayAuditInfo;
        public IRepository<Eftformat> eFTFormat;
        public IRepository<EftformatDetails> eftformatDetails;
        public IRepository<JournalChatInfo> journalchatinfo;
        public IRepository<TemptableForBulkPrint> templateBulkPrint;
        public IRepository<OcrtransactionTaxInfo> ocrtransactionTaxInfo;
        public IRepository<Reconciliation> reconciliation;
        public IRepository<VendorAipurposeDetails> vendorAipurposeDetails;
        public IRepository<BillEntryAiinformationDetails> billEntryAiinformationDetails;
        public IRepository<BillPaymentImportDetails> billPaymentImportDetails; 
        public IRepository<BillPaymentImportMappingDetails> billPaymentImportMappingDetails;
        public IRepository<_1099miscExcludeSettings> excludeSettings;
        public IRepository<_1099miscThreshold> thresholds;
        public IRepository<_1099miscBoxLines> boxLines;
        public IRepository<_1099changeMappingTempTable> changeMappingsTemp;

        #region EPay
        public IRepository<PaymentBatchIssue> ePaymentBatchIssues;
        public IRepository<EpaymentsBatch> ePaymentBatchs;
        public IRepository<EpaymentsBatchDetails> ePaymentBatchDetails;
        public IRepository<EpaymentsBatchLog> ePaymentBatchLogs;
        public IRepository<EpaymentsBatchVoidDeleteLog> ePaymentBatchVoidDeleteLogs;
        public IRepository<VendorAudit> vendorAudit;
        public IRepository<EpaymentLogInfo> epaymentLogInfo;
        #endregion
        #endregion IRepository Fields
        public IConfiguration config;
        private readonly ILoggerService logger;
        #region Ctor
        public PayableUnitOfWork(PayableDBContext cont, IConfiguration _config, ILoggerService _logger)
        {
            this.context = cont;
            this.config = _config;
            this.logger = _logger;
        }

        #endregion

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
        public IRepository<UserSettings> UserSettings
        {
            get
            {
                if (this.userSettings == null)
                {
                    this.userSettings = new Repository<UserSettings>(context);
                }
                return this.userSettings;
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
        public IRepository<CorporationTaxSettings> CorporationTaxSettings
        {
            get
            {
                if (this.corporationTaxSettings == null)
                {
                    this.corporationTaxSettings = new Repository<CorporationTaxSettings>(context);
                }
                return this.corporationTaxSettings;
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
        public IRepository<TransactionTaxInfo> TransactionTaxInfo
        {
            get
            {
                if (this.transactionTaxInfo == null)
                {
                    this.transactionTaxInfo = new Repository<TransactionTaxInfo>(context);
                }
                return this.transactionTaxInfo;
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
                return this.users;
            }
        }
        public IRepository<ApfilterReq> ApfilterReqs
        {
            get
            {
                if (this.apfilterReq == null)
                {
                    this.apfilterReq = new Repository<ApfilterReq>(context);
                }
                return this.apfilterReq;
            }
        }
        #endregion IRepository Common

        #region IRepository Payable
        public IRepository<Reconciliation> Reconciliation
        {
            get
            {
                if (this.reconciliation == null)
                {
                    this.reconciliation = new Repository<Reconciliation>(context);
                }
                return this.reconciliation;
            }
        }
        public IRepository<CorpDiscountAccountConfiguration> CorpDiscountAccountConfiguration
        {
            get
            {
                if (this.corpDiscountAccountConfiguration == null)
                {
                    this.corpDiscountAccountConfiguration = new Repository<CorpDiscountAccountConfiguration>(context);
                }
                return this.corpDiscountAccountConfiguration;
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
        public IRepository<AutoPostingInfo> AutoPostingInfo
        {
            get
            {
                if (this.autoPostingInfo == null)
                {
                    this.autoPostingInfo = new Repository<AutoPostingInfo>(context);
                }
                return this.autoPostingInfo;
            }
        }
        public IRepository<BatchPrint> BatchPrint
        {
            get
            {
                if (this.batchPrint == null)
                {
                    this.batchPrint = new Repository<BatchPrint>(context);
                }
                return this.batchPrint;
            }
        }
        public IRepository<BillEntryDebiitMemoPreference> BillEntryDebiitMemoPreference
        {
            get
            {
                if (this.billEntryDebitMemoPref == null)
                {
                    this.billEntryDebitMemoPref = new Repository<BillEntryDebiitMemoPreference>(context);
                }
                return this.billEntryDebitMemoPref;
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
                return (this.billEntryPayments);
            }
        }
        public IRepository<BillPayOrInvoiceAdjust> BillPayOrInvoiceAdjust
        {
            get
            {
                if (this.billPayOrInvAdjust == null)
                {
                    this.billPayOrInvAdjust = new Repository<BillPayOrInvoiceAdjust>(context);
                }
                return (this.billPayOrInvAdjust);
            }
        }
        public IRepository<BillPaymentsInformation> BillPaymentsInformation
        {
            get
            {
                if (this.billPaymentsInfo == null)
                {
                    this.billPaymentsInfo = new Repository<BillPaymentsInformation>(context);
                }
                return this.billPaymentsInfo;
            }
        }
        public IRepository<BusinessDetails> BusinessDetails
        {
            get
            {
                if (this.businessDetails == null)
                {
                    this.businessDetails = new Repository<BusinessDetails>(context);
                }
                return this.businessDetails;
            }
        }
        public IRepository<BusinessInfo> BusinessInfo
        {
            get
            {
                if (this.businessInfo == null)
                {
                    this.businessInfo = new Repository<BusinessInfo>(context);
                }
                return this.businessInfo;
            }
        }
        public IRepository<BusinessPercentage> BusinessPercentage
        {
            get
            {
                if (this.businessPercentage == null)
                {
                    this.businessPercentage = new Repository<BusinessPercentage>(context);
                }
                return this.businessPercentage;
            }
        }
        public IRepository<BusinessPreferences> BusinessPreferences
        {
            get
            {
                if (this.businessPreferences == null)
                {
                    this.businessPreferences = new Repository<BusinessPreferences>(context);
                }
                return this.businessPreferences;
            }
        }
        public IRepository<BusinessRelatedExtension> BusinessRelatedExtension
        {
            get
            {
                if (this.businessRelatedExtension == null)
                {
                    this.businessRelatedExtension = new Repository<BusinessRelatedExtension>(context);
                }
                return this.businessRelatedExtension;
            }
        }
        public IRepository<BusinessTokenDetails> BusinessTokenDetails
        {
            get
            {
                if (this.businessTokenDetails == null)
                {
                    this.businessTokenDetails = new Repository<BusinessTokenDetails>(context);
                }
                return this.businessTokenDetails;
            }
        }
        public IRepository<Contact> Contact
        {
            get
            {
                if (this.contact == null)
                {
                    this.contact = new Repository<Contact>(context);
                }
                return this.contact;
            }
        }
        public IRepository<CorpAchsettings> CorpAchsettings
        {
            get
            {
                if (this.corpAchsettings == null)
                {
                    this.corpAchsettings = new Repository<CorpAchsettings>(context);
                }
                return this.corpAchsettings;
            }
        }
        public IRepository<CorporationBankDetails> CorporationBankDetails
        {
            get
            {
                if (this.corporationBankDetails == null)
                {
                    this.corporationBankDetails = new Repository<CorporationBankDetails>(context);
                }
                return this.corporationBankDetails;
            }
        }
        public IRepository<CreditTerm> CreditTerm
        {
            get
            {
                if (this.creditTerm == null)
                {
                    this.creditTerm = new Repository<CreditTerm>(context);
                }
                return this.creditTerm;
            }
        }
        public IRepository<DirectDepositBankDetails> DirectDepositBankDetails
        {
            get
            {
                if (this.directDepositBankDetails == null)
                {
                    this.directDepositBankDetails = new Repository<DirectDepositBankDetails>(context);
                }
                return this.directDepositBankDetails;
            }
        }
        public IRepository<DirectDepositExportDetails> DirectDepositExportDetails
        {
            get
            {
                if (this.directDepositExportDetails == null)
                {
                    this.directDepositExportDetails = new Repository<DirectDepositExportDetails>(context);
                }
                return this.directDepositExportDetails;
            }
        }
        public IRepository<FortePaymentTokens> FortePaymentTokens
        {
            get
            {
                if (this.fortePaymentTokens == null)
                {
                    this.fortePaymentTokens = new Repository<FortePaymentTokens>(context);
                }
                return this.fortePaymentTokens;
            }
        }
        public IRepository<FortePaymentsSchedule> FortePaymentsSchedule
        {
            get
            {
                if (this.fortePaymentsSchedule == null)
                {
                    this.fortePaymentsSchedule = new Repository<FortePaymentsSchedule>(context);
                }
                return this.fortePaymentsSchedule;
            }
        }
        public IRepository<OcrcreditTerm> OcrcreditTerm
        {
            get
            {
                if (this.ocrcreditTerm == null)
                {
                    this.ocrcreditTerm = new Repository<OcrcreditTerm>(context);
                }
                return this.ocrcreditTerm;
            }
        }
        public IRepository<OcrjournalEntry> OcrjournalEntry
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
        public IRepository<Ocrtransaction> Ocrtransaction
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
        public IRepository<OcrtransactionInvoice> OcrtransactionInvoice
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
        public IRepository<UseTax> UseTax
        {
            get
            {
                if (this.useTax == null)
                {
                    this.useTax = new Repository<UseTax>(context);
                }
                return this.useTax;
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
                return this.useTaxDetails;
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
                return this.useTaxRates;
            }
        }
        public IRepository<UserAchdetails> UserAchdetails
        {
            get
            {
                if (this.userAchdetails == null)
                {
                    this.userAchdetails = new Repository<UserAchdetails>(context);
                }
                return this.userAchdetails;
            }
        }
        public IRepository<UserCorporation> UserCorporation
        {
            get
            {
                if (this.userCorporation == null)
                {
                    this.userCorporation = new Repository<UserCorporation>(context);
                }
                return this.userCorporation;
            }
        }
        public IRepository<VendorDirectDepositDetails> VendorDirectDepositDetails
        {
            get
            {
                if (this.vendorDirectDepositDetails == null)
                {
                    this.vendorDirectDepositDetails = new Repository<VendorDirectDepositDetails>(context);
                }
                return this.vendorDirectDepositDetails;
            }
        }
        public IRepository<VendorTaxInfo> VendorTaxInfo
        {
            get
            {
                if (this.vendorTaxInfo == null)
                {
                    this.vendorTaxInfo = new Repository<VendorTaxInfo>(context);
                }
                return this.vendorTaxInfo;
            }
        }
        public IRepository<Country> Country
        {
            get
            {
                if (this.country == null)
                {
                    this.country = new Repository<Country>(context);
                }
                return this.country;
            }
        }
        public IRepository<State> State
        {
            get
            {
                if (this.state == null)
                {
                    this.state = new Repository<State>(context);
                }
                return this.state;
            }
        }
        public IRepository<BillEntryInformationDetails> BillEntryInformationDetails
        {
            get
            {
                if (this.billEntryInformationDetails == null)
                {
                    this.billEntryInformationDetails = new Repository<BillEntryInformationDetails>(context);
                }
                return this.billEntryInformationDetails;
            }
        }

        public IRepository<UserDirectDepositDetails> UserDirectDepositDetails
        {
            get
            {
                if (this.userDirectDepositDetails == null)
                {
                    this.userDirectDepositDetails = new Repository<UserDirectDepositDetails>(context);
                }
                return this.userDirectDepositDetails;
            }
        }

        public IRepository<DirectDepositAuditInfo> DirectDepositAuditInfo
        {
            get
            {
                if (this.directDepositAuditInfo == null)
                {
                    this.directDepositAuditInfo = new Repository<DirectDepositAuditInfo>(context);
                }
                return this.directDepositAuditInfo;
            }
        }
        public IRepository<EftauditInfo> EftAuditinfo
        {
            get
            {
                if (this.eftAuditinfo == null)
                {
                    this.eftAuditinfo = new Repository<EftauditInfo>(context);
                }
                return this.eftAuditinfo;
            }
        }
        public IRepository<Eftcolumns> EFTColumns
        {
            get
            {
                if (this.eFTColumns == null)
                {
                    this.eFTColumns = new Repository<Eftcolumns>(context);
                }
                return this.eFTColumns;
            }
        }
        public IRepository<Eftconfig> EFTConfig
        {
            get
            {
                if (this.eFTConfig == null)
                {
                    this.eFTConfig = new Repository<Eftconfig>(context);
                }
                return this.eFTConfig;
            }
        }
        
        public IRepository<MasterBinAuditing> MasterBinAuditing
        {
            get
            {
                if (this.masterBinAuditing == null)
                {
                    this.masterBinAuditing = new Repository<MasterBinAuditing>(context);
                }
                return this.masterBinAuditing;
            }
        }
        public IRepository<MasterLongAuditing> MasterLongAuditing
        {
            get
            {
                if (this.masterLongAuditing == null)
                {
                    this.masterLongAuditing = new Repository<MasterLongAuditing>(context);
                }
                return this.masterLongAuditing;
            }
        }
        public IRepository<RepayConfig> RepayConfig
        {
            get
            {
                if (this.repayConfig == null)
                {
                    this.repayConfig = new Repository<RepayConfig>(context);
                }
                return this.repayConfig;
            }
        }
        public IRepository<RepayAuditInfo> RepayAuditInfo
        {
            get
            {
                if (this.repayAuditInfo == null)
                {
                    this.repayAuditInfo = new Repository<RepayAuditInfo>(context);
                }
                return this.repayAuditInfo;
            }
        }

        public IRepository<Eftformat> EFTFormat 
        {
            get
            {
                if (this.eFTFormat == null)
                {
                    this.eFTFormat = new Repository<Eftformat>(context);
                }
                return this.eFTFormat;
            }
        }
        public IRepository<EftformatDetails> EftformatDetails 
        { 
            get
            {
                if (this.eftformatDetails == null)
                {
                    this.eftformatDetails = new Repository<EftformatDetails>(context);
                }
                return this.eftformatDetails;
            }
        }

        public IRepository<JournalChatInfo> JournalChatInfo
        {
            get
            {
                if (this.journalchatinfo == null)
                {
                    this.journalchatinfo = new Repository<JournalChatInfo>(context);
                }
                return this.journalchatinfo;
            }
        }

        public IRepository<TemptableForBulkPrint> TemplateBulkPrint
        {
            get
            {
                if (this.templateBulkPrint == null)
                {
                    this.templateBulkPrint = new Repository<TemptableForBulkPrint>(context);
                }
                return this.templateBulkPrint;
            }
        }
        public IRepository<VendorAipurposeDetails> VendorAipurposeDetails
        {
            get
            {
                if (this.vendorAipurposeDetails == null)
                {
                    this.vendorAipurposeDetails = new Repository<VendorAipurposeDetails>(context);
                }
                return this.vendorAipurposeDetails;
            }
        }
        public IRepository<BillEntryAiinformationDetails> BillEntryAiinformationDetails
        {
            get
            {
                if (this.billEntryAiinformationDetails == null)
                {
                    this.billEntryAiinformationDetails = new Repository<BillEntryAiinformationDetails>(context);
                }
                return this.billEntryAiinformationDetails;
            }
        }
        public IRepository<BillPaymentImportDetails> BillPaymentImportDetails
        {
            get
            {
                if (this.billPaymentImportDetails == null)
                {
                    this.billPaymentImportDetails=new Repository<BillPaymentImportDetails>(context);    
                }
                return this.billPaymentImportDetails;
            }
        }

        public IRepository<BillPaymentImportMappingDetails> BillPaymentImportMappingDetails
        {
            get
            {
                if(this.billPaymentImportMappingDetails == null)
                {
                    this.billPaymentImportMappingDetails= new Repository<BillPaymentImportMappingDetails>(context); 
                }
                return this.billPaymentImportMappingDetails;    
            }
        }
        public IRepository<_1099miscExcludeSettings> ExcludeSettings
        {
            get
            {
                if (this.excludeSettings == null)
                {
                    this.excludeSettings = new Repository<_1099miscExcludeSettings>(context);
                }
                return excludeSettings;
            }
        }
        public IRepository<_1099miscThreshold> Thresholds
        {
            get
            {
                if (this.thresholds == null)
                {
                    this.thresholds = new Repository<_1099miscThreshold>(context);
                }
                return thresholds;
            }
        }
        public IRepository<_1099miscBoxLines> BoxLines
        {
            get
            {
                if (this.boxLines == null)
                {
                    this.boxLines = new Repository<_1099miscBoxLines>(context);
                }
                return boxLines;
            }
        }
        public IRepository<_1099changeMappingTempTable> ChangeMappingsTemp
        {
            get
            {
                if (this.changeMappingsTemp == null)
                {
                    this.changeMappingsTemp = new Repository<_1099changeMappingTempTable>(context);
                }
                return changeMappingsTemp;
            }
        }
        #endregion IRepository Payable
        #region Epay
        public IRepository<PaymentBatchIssue> EpaymentBatchIssues
        {
            get
            {
                if (this.ePaymentBatchIssues == null)
                {
                    this.ePaymentBatchIssues = new Repository<PaymentBatchIssue>(context);
                }
                return this.ePaymentBatchIssues;
            }
        }

        public IRepository<EpaymentsBatch> EpaymentBatches
        {
            get
            {
                if (this.ePaymentBatchs == null)
                {
                    this.ePaymentBatchs = new Repository<EpaymentsBatch>(context);
                }
                return this.ePaymentBatchs;
            }
        }

        public IRepository<EpaymentsBatchDetails> EpaymentBatchDetails
        {
            get
            {
                if (this.ePaymentBatchDetails == null)
                {
                    this.ePaymentBatchDetails = new Repository<EpaymentsBatchDetails>(context);
                }
                return this.ePaymentBatchDetails;
            }
        }
        public IRepository<EpaymentsBatchLog> EpaymentBatchLogs
        {
            get
            {
                if (this.ePaymentBatchLogs == null)
                {
                    this.ePaymentBatchLogs = new Repository<EpaymentsBatchLog>(context);
                }
                return this.ePaymentBatchLogs;
            }
        }

        public IRepository<EpaymentsBatchVoidDeleteLog> EpaymentBatchVoidDeleteLogs
        {
            get
            {
                if (this.ePaymentBatchVoidDeleteLogs == null)
                {
                    this.ePaymentBatchVoidDeleteLogs = new Repository<EpaymentsBatchVoidDeleteLog>(context);
                }
                return this.ePaymentBatchVoidDeleteLogs;
            }
        }
        public IRepository<VendorAudit> VendorAudit
        {
            get
            {
                if (this.vendorAudit == null)
                {
                    this.vendorAudit = new Repository<VendorAudit>(context);
                }
                return this.vendorAudit;
            }
        }
        public IRepository<EpaymentLogInfo> EpaymentLogInfo
        {
            get
            {
                if (this.epaymentLogInfo == null)
                {
                    this.epaymentLogInfo = new Repository<EpaymentLogInfo>(context);
                }
                return this.epaymentLogInfo;
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
                return ocrtransactionTaxInfo;
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
