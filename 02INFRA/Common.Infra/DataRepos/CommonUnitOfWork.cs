using Common.App.Contracts;
using Common.Infra.GenericRepos;
using DataModel.Domain.DataModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.DataRepos
{
    public class CommonUnitOfWork
    {
        #region IRepository Fields
        public IRepository<Account> accounts;
        public IRepository<Address> addresses;
        public IRepository<ApprovalComments> approvalComments;
        public IRepository<Business> business;
        public IRepository<Corporation> corporations;
        public IRepository<UserCorporation> UserCorporations;
        public IRepository<CorporationTaxSettings> corporationTaxSettings; 
        public IRepository<CreditTerm> creditTerms;
        public IRepository<Frequency> frequencies;
        public IRepository<ImportDocument> importDocuments;
        public IRepository<ImportDocumentDetails> importDocumentDetails;
        public IRepository<JournalEntry> journalEntries;
        public IRepository<ApfilterReq> apfilterReqs;
        public IRepository<BillPayOrInvoiceAdjust> billPayOrInvoiceAdjusts;
        public IRepository<Employee> employees;
        public IRepository<PayrollDepartmentInfo> payrollDepartmentInfos;
        public IRepository<LoanScheduleTransactions> loanScheduleTransactions;
        public IRepository<LoanScheduleTransactionDetails> loanScheduleTransactionDetails;
        public IRepository<DsdepositInfoDetails> dsdepositInfoDetails;
        public IRepository<Payroll> payroll;
        public IRepository<PayrollJobInfo> payrollJobInfos;
        public IRepository<JournalEntryExt> journalEntriesExt;
        public IRepository<Purpose> purposes;
        public IRepository<Reconciliation> reconciliations;
        public IRepository<Remind> reminds;
        public IRepository<Repetitive> repetitives;
        public IRepository<RepetitiveCreditTerm> repetitiveCreditTerms;
        public IRepository<RepetitiveTransaction> repetitiveTransactions;
        public IRepository<RepetitiveTransactionInvoice> repetitiveTransactionInvoices;
        public IRepository<Store> profitCenters;
        public IRepository<Transaction> transactions;
        public IRepository<TransactionInvoice> transactionInvoices;
        public IRepository<TransactionTaxInfo> transactionTaxInfo;
        public IRepository<UserPreferenceSettings> userPreferencesSettings;
        public IRepository<VendorAddress> vendorAddress;
        public IRepository<VendorContract> vendorContracts;
        public IRepository<InterCompanyReferences> interCompanyReferences;
        public IRepository<UserSettings> userSettings;
        public IRepository<MiscInfo> miscInfo;
        public IRepository<User> users;
        public IRepository<CorporationYearEndProcess> corporationYearEndProcess;
        public IRepository<Locking> locking;
        public IRepository<ApprovalPloicy> approvalPloicy;
        public IRepository<ApprovalPloicyDetails> approvalPloicyDetails;
        public IRepository<OcrjournalEntry> ocrjournalEntry;
        public IRepository<Ocrtransaction> ocrtransaction;
        public IRepository<OcrtransactionInvoice> ocrtransactionInvoice;
        public IRepository<OcrtransactionTaxInfo> ocrtransactionTaxInfo;
        public IRepository<BillEntryInformation> billEntryInformation;
        public IRepository<BillPaymentsInformation> billPaymentsInformation;
        public IRepository<Pctransactions> pctransactions;
        public IRepository<UseTax> useTax;
        public IRepository<UseTaxDetails> useTaxDetails;
        public IRepository<UseTaxRates> useTaxRates;
        public IRepository<FundtransferFilterReq> fundtransferFilterReq;
        public IRepository<Employee> employee;
        public IRepository<BillEntryPayments> billEntryPayments;
        public IRepository<DepartmentConfig> departmentConfig;


        #endregion



    }

}
