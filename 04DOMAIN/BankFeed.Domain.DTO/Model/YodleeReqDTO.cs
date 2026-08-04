using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class YodleeAccountListDTO :StatusDTO
    {
        public YodleeAccountDTO[] account { get; set; }
    }
    public class YodleeProviderAccountStatusDTO
    {
        public ProviderAccountDTO[] providerAccount { get; set; }
    }
    public class ProviderAccountDTO
    {
        public int id { get; set; }
        public string aggregationSource { get; set; }
        public string authType { get; set; }
        public DateTime createdDate { get; set; }
        public AccountDataset[] dataset { get; set; }

        public bool isManual { get; set; }

        public bool isRealTimeMFA { get; set; }

        public string lastUpdated { get; set; }

        public long providerId { get; set; }

        public string status { get; set; }
    }
    public class ProviderPreferenceDTO
    {
        public bool isDataExtractsEnabled { get; set; }
        public long linkedProviderAccountId { get; set; }
        public bool isAutoRefreshEnabled { get; set; }



    }
    public class YodleeAccountDTO
    {
        public long id { get; set; }
        public string BankAccId { get; set; }
        public YodleeBalance availableCash { get; set; }
        public YodleeBalance balance { get; set; }
        public string lastUpdated { get; set; }
        public string providerName { get; set; }
        public YodleeBalance currentBalance { get; set; }

        public string accountStatus { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public string CONTAINER { get; set; }
        public long providerAccountId { get; set; }
        public List<string> associatedProviderAccountId { get; set; }

        public AccountDataset[] dataset { get; set; }
        public string providerId { get; set; }
        public string accountType { get; set; }
        public YodleeBalance availableCredit { get; set; }
        public YodleeBalance originalLoanAmount { get; set; }

        public YodleeBalance availableBalance { get; set; }
    }

    public class AccountDataset
    {
        public string lastUpdated { get; set; }
        public string updateEligibility { get; set; }
        public string additionalStatus { get; set; }
        public int additionalStatusErrorCode { get; set; }
        public string nextUpdateScheduled { get; set; }

        public string name { get; set; }
        public string lastUpdateAttempt { get; set; }

    }
    public class YodleeBalance
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }
    public class YodleeTransactionlist
    {
        public List<YodleeTransaction> transaction { get; set; }
    }
    public class YodleeTransaction
    {
        public string date { get; set; }
        public string memo { get; set; }
        public YodleeTransDesc description { get; set; }
        public string settleDate { get; set; }
        public string type { get; set; }
        public string baseType { get; set; }
        public long id { get; set; }
        public YodleeAmount amount { get; set; }
        public string checkNumber { get; set; }
        public YodleeMerchant merchant { get; set; }
        public string transactionDate { get; set; }
        public string categoryType { get; set; }
        public long accountId { get; set; }
        public string CONTAINER { get; set; }
        public string postDate { get; set; }
        public string subType { get; set; }
        public string category { get; set; }
        public YodleeAmount runningBalance { get; set; }
        public string status { get; set; }


    }
    public class YodleeTransDesc
    {
        public string original { get; set; }
    }
    public class YodleeAmount
    {
        public double amount { get; set; }
    }
    public class YodleeMerchant
    {
        public string website { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

}
