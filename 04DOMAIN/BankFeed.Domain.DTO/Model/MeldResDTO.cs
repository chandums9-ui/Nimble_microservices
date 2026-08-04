using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class MeldProcessResponse : StatusDTO
    {
        public long ID { get; set; }
        public bool IsGetHistoricalTransactions { get; set; } = false;
    }
    public class MeldConnectResponse : StatusDTO
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MeldConnectResponse()
        {
            StatusCode = StatusCodes.Status401Unauthorized; //Status = Constants.MSG_BADREQ;
        }

        /// <summary>
        /// Connection id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Connect token needed to go through the connect flow
        /// </summary>
        public string connectToken { get; set; }

        /// <summary>
        /// Meld will create a new customer or use the existing customer based on the external customer id provided in the request. Keep track of this customer id for use in other endpoints and webhook tracking.
        /// </summary>
        public string customerId { get; set; }

        /// <summary>
        /// External id for the customer as provided in the request
        /// </summary>
        public string externalCustomerId { get; set; }

        /// <summary>
        /// Widget url to go through the connect flow
        /// </summary>
        public string widgetUrl { get; set; }
    }

    public class MeldErrorMessages
    {
        public string code { get; set; }
        public string message { get; set; }
        public string requestId { get; set; }
        public DateTime? timestamp { get; set; }
    }

    public class MeldGetConnectionResponse : GetConnectionDTO, IStatusDTO
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MeldGetConnectionResponse()
        {
            StatusCode = StatusCodes.Status401Unauthorized; //Status = Constants.MSG_BADREQ;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class MeldSearchConnectionResponse : StatusDTO
    {
        public GetConnectionDTO[] connections { get; set; }
    }
    public class GetConnectionDTO
    {
        public string id { get; set; }
        public string accountId { get; set; }
        public string customerId { get; set; }
        public string institutionId { get; set; }
        public string institutionName { get; set; }
        public string status { get; set; }
        public string statusReason { get; set; }
        public string[] duplicateConnectionIds { get; set; }
        public string activeDuplicateConnectionId { get; set; }
        public string[] regions { get; set; }
        public string[] products { get; set; }
        public string[] optionalProducts { get; set; }
        public ServiceProviderInstitutionDetails serviceProviderDetails { get; set; }
        public DateTime createdAt { get; set; }
        public string externalCustomerId { get; set; }
        public DateTime? successfullyAggregatedAt { get; set; }
    }
    public class ServiceProviderInstitutionDetails
    {
        public string accessToken { get; set; }
        public string institutionId { get; set; }
        public string itemId { get; set; }
        public string latestCursor { get; set; }
        public string[] linkAdditionalConsentedProducts { get; set; }
        public string[] linkProducts { get; set; }
        public string linkSessionId { get; set; }
        public string requestId { get; set; }
        public string serviceProvider { get; set; }
    }

    public class FinancialAccountSearchResponse : StatusDTO
    {
        public List<FinancialAccountDTO> financialAccounts { get; set; } = new List<FinancialAccountDTO>();
        public int count { get; set; }
        public int remaining { get; set; }
    }
    public class FinancialAccountResponse : FinancialAccountDTO, IStatusDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class FinancialAccountDTO
    {
        public string key { get; set; }
        public string id { get; set; }
        public string accountId { get; set; }
        public string customerId { get; set; }
        public string externalCustomerId { get; set; }
        public string institutionId { get; set; }
        public string institutionName { get; set; }
        public string[] connectionIds { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public string name { get; set; }
        public string truncatedAccountNumber { get; set; }
        public AccIdentifiers identifiers { get; set; }
        public AccBalances balances { get; set; }
        public AccOwner[] owners { get; set; }
        public ServiceProviderFinancialAccountDetail[] serviceProviderDetails { get; set; }
    }

    public class AccIdentifiers
    {
        public object ach { get; set; } //accountNumber routingNumber
        public object eft { get; set; }
        public object international { get; set; }
        public object bacs { get; set; }
    }

    public class AccBalances
    {
        public string currency { get; set; }
        public object currentAmount { get; set; }
        public object availableAmount { get; set; }
        public DateTime? updatedAt { get; set; }
    }
    public class ServiceProviderBalances
    {
        public float? available { get; set; }
        public float? current { get; set; }
        public string iso_currency_code { get; set; }
        public int? limit { get; set; }
        public string unofficial_currency_code { get; set; }
    }

    public class AccOwner
    {
        public Address[] addresses { get; set; }
        public AccOwnerEmail[] emails { get; set; }
        public string[] names { get; set; }
        public AccOwnerPhonenumber[] phoneNumbers { get; set; }
    }

    public class Address
    {
        public Data data { get; set; }
        public string primary { get; set; }
    }

    public class Data
    {
        public string street { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public object full { get; set; }
    }

    public class ServiceProviderFinancialAccountDetail
    {
        public ServiceProviderAccount account { get; set; }
        public float? availableBalance { get; set; }
        public object auth { get; set; }
        public ServiceProviderBalances balances { get; set; }
        public string connectionId { get; set; }
        public Identity[] identities { get; set; }
        public string serviceProvider { get; set; }
        public DateTime? updatedAt { get; set; }
    }

    public class Detail
    {
        public float? availableCashBalance { get; set; }
        public float? vestedBalance { get; set; }
    }

    public class ServiceProviderAccount
    {
        public string account_id { get; set; }
        public ServiceProviderBalances balances { get; set; }
        public string mask { get; set; }
        public string name { get; set; }
        public string official_name { get; set; }
        public string persistent_account_id { get; set; }
        public string subtype { get; set; }
        public string type { get; set; }
        public string accountNickname { get; set; }
        public string accountNumberDisplay { get; set; }
        //public int aggregationAttemptDate { get; set; }
        //public int aggregationStatusCode { get; set; }
        //public int aggregationSuccessDate { get; set; }
        public float? balance { get; set; }
        ///// <summary>
        ///// MX
        ///// </summary>
        //public float? available_balance { get; set; }
        //public float? available_credit { get; set; }
        //public float? cash_balance { get; set; }
        //public float? cash_surrender_value { get; set; }
        //public float? credit_limit { get; set; }
        //public float? original_balance { get; set; }
        //public float? margin_balance { get; set; }        
        //public float? loan_amount { get; set; }
        //public float? interest_rate { get; set; }
        
        //public DateTime? started_on { get; set; }
        ///// <summary>
        ///// MX
        ///// </summary>
        //public float? statement_balance { get; set; }


        //public int balanceDate { get; set; }
        //public int createdDate { get; set; }
        public string currency { get; set; }
        public string customerId { get; set; }
        public Detail detail { get; set; }
        public int displayPosition { get; set; }
        public string id { get; set; }
        public string institutionId { get; set; }
        public long institutionLoginId { get; set; }
        //public int lastTransactionDate { get; set; }
        public string marketSegment { get; set; }
        public string number { get; set; }
        public string realAccountNumberLast4 { get; set; }
        public string status { get; set; }
    }

    public class Identity
    {
        public AccOwnerAddress[] addresses { get; set; }
        public AccOwnerEmail[] emails { get; set; }
        public string[] names { get; set; }
        public AccOwnerPhonenumber[] phone_numbers { get; set; }
    }

    public class AccOwnerAddress
    {
        public AddressData data { get; set; }
        public string primary { get; set; }
    }

    public class AddressData
    {
        public string city { get; set; }
        public string country { get; set; }
        public string postal_code { get; set; }
        public string region { get; set; }
        public string street { get; set; }
    }

    public class AccOwnerEmail
    {
        public string data { get; set; }
        public string primary { get; set; }
        public string type { get; set; }
    }

    public class AccOwnerPhonenumber
    {
        public string data { get; set; }
        public string primary { get; set; }
        public string type { get; set; }
    }


    public class MeldTransactionDTO
    {
        public string key { get; set; }
        public string id { get; set; }
        public string accountId { get; set; }
        public string customerId { get; set; }
        public string externalCustomerId { get; set; }
        public string financialAccountId { get; set; }
        public string[] connectionIds { get; set; }
        //public float? amount { get; set; }

        //[Column(TypeName = "decimal(18,2)")]
        // public decimal? amount { get; set; }
        public object amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime transactionDate { get; set; }
        public DateTime? postedDate { get; set; }
        public ServiceProviderTransactionDetail[] serviceProviderDetails { get; set; }
        public string category { get; set; }
        public object recurringStatus { get; set; }
    }

    public class ServiceProviderTransactionDetail : PLAIDServiceproviderdetail
    {
        public string id { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        // public decimal? amount { get; set; }
        public object amount { get; set; }

        /// <summary>
        /// serviceProvider:MX | FINICITY
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// serviceProvider:FINICITY
        /// </summary>
        public string memo { get; set; }
        ///// <summary>
        ///// serviceProvider:FINICITY
        ///// </summary>
        //public int transactionDate { get; set; }
        ///// <summary>
        ///// serviceProvider:FINICITY
        ///// </summary>
        //public int postedDate { get; set; }
        /// <summary>
        ///// serviceProvider:FINICITY
        ///// </summary>
        //public int createdDate { get; set; }
        public long accountId { get; set; }
        public long customerId { get; set; }
        public string connectionId { get; set; }
        public float? runningBalanceAmount { get; set; }
        public string status { get; set; }
        public DateTime? updatedAt { get; set; }
        public string serviceProvider { get; set; }
        public Categorization categorization { get; set; }
        /// <summary>
        /// serviceProvider:MX
        /// </summary>
        public string check_number_string { get; set; }
        /// <summary>
        /// serviceProvider:FINICITY
        /// </summary>
        public string checkNum { get; set; }
        /// <summary>
        /// serviceProvider:FINICITY
        /// </summary>
        public string ofxCheckNumber { get; set; }
        
        /// <summary>
        /// serviceProvider:MX | Plaid
        /// </summary>
        public string original_description { get; set; }
    }

    public class PLAIDServiceproviderdetail
    {
        /// <summary>
        /// serviceProvider:MX
        /// </summary>
        public DateTime date { get; set; }
        public string unofficial_currency_code { get; set; }
        //public Payment_Meta payment_meta { get; set; }
        public string merchant_entity_id { get; set; }
        public string logo_url { get; set; }
        public bool pending { get; set; }
        
        //public object account_owner { get; set; }
        public string personal_finance_category_icon_url { get; set; }
        //public object datetime { get; set; }
        /// <summary>
        /// serviceProvider:Plaid
        /// </summary>
        public string check_number { get; set; }        

        public string category_id { get; set; }
        public string iso_currency_code { get; set; }
        public string pending_transaction_id { get; set; }
        //public object[] counterparties { get; set; }
        //public DateTime updatedAt { get; set; }
        public string transaction_id { get; set; }
        public object transaction_code { get; set; }
        //public float? amount { get; set; }
        public string website { get; set; }
        //public object authorized_datetime { get; set; }
        public string merchant_name { get; set; }
        public string transaction_type { get; set; }
        public string account_id { get; set; }
        public string payment_channel { get; set; }
        public string name { get; set; }
        //public string serviceProvider { get; set; }
        public string connectionId { get; set; }
        //public Location location { get; set; }
        //public string[] category { get; set; }
        //public Personal_Finance_Category personal_finance_category { get; set; }
        public DateTime? authorized_date { get; set; }
    }

    public class Payment_Meta
    {
        public object payee { get; set; }
        public object payer { get; set; }
        public object ppd_id { get; set; }
        public object reason { get; set; }
        public object by_order_of { get; set; }
        public object payment_method { get; set; }
        public string reference_number { get; set; }
        public object payment_processor { get; set; }
    }

    public class Location
    {
        public object lat { get; set; }
        public object lon { get; set; }
        public object city { get; set; }
        public object region { get; set; }
        public object address { get; set; }
        public object country { get; set; }
        public object postal_code { get; set; }
        public object store_number { get; set; }
    }

    public class Personal_Finance_Category
    {
        public string primary { get; set; }
        public string detailed { get; set; }
        public string confidence_level { get; set; }
    }

    public class Categorization
    {
        public string country { get; set; }
        public string category { get; set; }
        public string bestRepresentation { get; set; }
        public string normalizedPayeeName { get; set; }
    }

    public class MeldTransactionResponse : MeldTransactionDTO, IStatusDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }

    public class MeldTransactionSearchResponse : StatusDTO
    {
        public List<MeldTransactionDTO> financialAccountTransactions { get; set; } = new List<MeldTransactionDTO>();
        public int count { get; set; }
        public int remaining { get; set; }
    }
}
