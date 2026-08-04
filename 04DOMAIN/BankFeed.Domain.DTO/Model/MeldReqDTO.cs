using System.Security.Cryptography;
using System.Text;

namespace BankFeed.Domain.DTO.Model
{

    #region Webhook
    public class MeldWebhookInfo
    {
        public MeldWebhookInfo()
        {
            payload = new WebhookPayload();
        }
        /// <summary>
        /// Type of event
        /// </summary>
        public string eventType { get; set; }

        /// <summary>
        /// Meld's unique identifier for the event
        /// </summary>
        public string eventId { get; set; }

        /// <summary>
        /// The date and time the event was created
        /// </summary>
        public DateTime timestamp { get; set; }

        /// <summary>
        /// Account id
        /// </summary>
        public string accountId { get; set; }

        /// <summary>
        /// Id of the webhook profile responsible for this event to be sent
        /// </summary>
        public string profileId { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// An object containing additional information of the event depending on the event type
        /// </summary>
        public WebhookPayload payload { get; set; }
    }

    /// <summary>
    /// An object containing additional information of the event depending on the event type
    /// </summary>
    public class WebhookPayload
    {
        /// <summary>
        /// Unique identifier of the request to trigger the event
        /// </summary>
        public string requestId { get; set; }

        /// <summary>
        /// Meld's unique identifier for the customer
        /// </summary>
        public string customerId { get; set; }

        /// <summary>
        /// Your unique identifier for your customer. If maintaining your own customer management system this can also be used for tracking customer activity.
        /// </summary>
        public string externalCustomerId { get; set; }

        /// <summary>
        /// Meld's unique identifier for the connection that needs to be repaired.
        /// </summary>
        public string connectionId { get; set; }

        /// <summary>
        /// Meld's unique identifier for the financial institution
        /// </summary>
        public string institutionId { get; set; }

        /// <summary>
        /// Name of the institution connected to
        /// </summary>
        public string institutionName { get; set; }
        public string oldStatus { get; set; }
        public string oldStatusReason { get; set; }
        public string newStatus { get; set; }
        public string newStatusReason { get; set; }
        public string activeDuplicateConnectionId { get; set; }

        /// <summary>
        /// The financial accounts belonging to the connection. Fields vary per event type, and only id is guaranteed. Additional fields are detailed in the respective event types.
        /// </summary>
        public List<FinancialAccount> financialAccounts { get; set; }

        /// <summary>
        /// If a service provider webhook triggered this event, then this object will contain the raw service provider webhook payload. If there is no associated webhook, this will only contain the service provider name.
        /// </summary>
        public WebhookServiceproviderdetails serviceProviderDetails { get; set; }
    }

    /// <summary>
    /// If a service provider webhook triggered this event, then this object will contain the raw service provider webhook payload. If there is no associated webhook, this will only contain the service provider name.
    /// </summary>
    public class WebhookServiceproviderdetails
    {
        public string environment { get; set; }
        public WebhookError error { get; set; }
        public string item_id { get; set; }
        public string serviceProvider { get; set; }
        public string serviceProviderConnectionId { get; set; }
        public string webhook_code { get; set; }
        public string webhook_type { get; set; }
        public int new_transactions { get; set; }
    }

    public class WebhookError
    {
        public string error_type { get; set; }
        public string error_code { get; set; }
        public string error_message { get; set; }
        public string display_message { get; set; }
        public string request_id { get; set; }
        public string causes { get; set; }
        public int status { get; set; }
        public string documentation_url { get; set; }
        public string suggested_action { get; set; }
    }

    public static class WebhookUtil
    {
        //string secret = "42m4NMLS34WQ6BbMfo1KFKqMv4hy";
        //string url = "https://example.meld.io/webhooks";
        //string timestamp = "2022-05-26T20:25:17.682818Z";
        //string body = "{\"eventType\":\"WEBHOOK_TEST\",\"eventId\":\"GDtv8pQgwzc9HuFFBQFrww\",\"timestamp\":\"2022-05-26T20:23:45.908400Z\",\"accountId\":\"W9jpQc1HKvwscqMPcLmzUS\",\"profileId\":null,\"version\":\"2021-10-27\",\"payload\":{\"requestId\":\"7aWW1GXTjWCCtNubzvVX7V\"}}";
        //var MeldSignature = "O4bN5E0U9s88l2DFc0kjt-0w3LLA3Zkv8hXhafc22Hg=";

        //var result1 = CompareMeldSignature(MeldSignature, timestamp, url, body, secret);

        public static bool CompareMeldSignature(string MeldSignature, string timestamp, string url, string body, string secret)
        {
            //if (!string.IsNullOrEmpty(MeldSignature))
            //    return (MeldSignature == CreateHMACSHA256Base64UrlEncodedSignatureWithPadding(timestamp, url, body, secret));
            //else
                return false;
        }

        public static byte[] CreateHMACSHA256Signature(string timestamp, string url, string body, string secret)
        {
            string stringToSign = string.Join(".", timestamp.Trim(), url.Trim(), body.Trim()).Trim();
            using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var bytes = Encoding.UTF8.GetBytes(stringToSign);
            var hashedBytes = hmacsha256.ComputeHash(bytes);

            return hashedBytes;
        }

        public static string CreateHMACSHA256Base64Signature(string timestamp, string url, string body, string secret)
        {
            return Convert.ToBase64String(CreateHMACSHA256Signature(timestamp, url, body, secret));
        }
        //public static string CreateHMACSHA256Base64UrlEncodedSignature(string timestamp, string url, string body, string secret)
        //{
        //    return Base64UrlEncoder.Encode(CreateHMACSHA256Signature(timestamp, url, body, secret));
        //}
        public static string CreateHMACSHA256Base64UrlEncodedSignatureWithPadding(string timestamp, string url, string body, string secret)
        {
            try
            {
                return Convert.ToBase64String(CreateHMACSHA256Signature(timestamp, url, body, secret)) + "=";
            }
            catch { return string.Empty; }
        }

    }

    #endregion Webhook

    /// <summary>
    /// Request object to start a bank linking connection
    /// </summary>
    public class MeldConnectRequest
    {
        /// <summary>
        /// Your unique identifier for your customer. If maintaining your own customer management system this can also be used for tracking customer activity.
        /// </summary>
        public string externalCustomerId { get; set; }

        /// <summary>
        /// Institution id to bypass the institution selection pane.
        /// </summary>
        public string institutionId { get; set; }

        /// <summary>
        /// When searching financial institutions via the Meld bank picker, pre-populate the search string with this value
        /// </summary>
        public string institutionSearchString { get; set; }

        /// <summary>
        /// Set of products to load from the financial institution via the service provider after the connection is completed.
        /// Can be empty as long as there is at least one optional product specified.There must be at least one product specified somewhere for the connection to load some data.
        /// </summary>
        public List<string> products { get; set; } = new List<string>() { "BALANCES", "TRANSACTIONS" };

        /// <summary>
        /// Set of products to load from the financial institution via the service provider if supported. 
        /// Can be empty as long as there is at least one required product specified.There must be at least one product specified somewhere for the connection to load some data.
        /// Financial institutions do not need to support any of these products in order to be included in the Meld bank picker search results.
        /// </summary>
        public List<string> optionalProducts { get; set; } = new List<string>();// { "OWNERS", "IDENTIFIERS", "INVESTMENT_HOLDINGS", "INVESTMENT_TRANSACTIONS" };

        /// <summary>
        /// When searching financial institutions via the Meld bank picker, onlythose which service any of the regions provided will be included. A service region can be either an ISO 3166-1 alpha-2 country code or an ISO 3166-2 subdivision code. National financial institutions which service an entire country are included for the corresponding country when only subdivision regions are provided.
        /// </summary>
        public List<string> regions { get; set; } = new List<string>() { "US" };

        //public object accountPreferenceOverride { get; set; }
    }

    /// <summary>
    /// A Service Provider's connection to the institution may sometimes degrade or new financial account(s) may be discovered for the connection. In order to fix the connection or add these newly discovered account(s), the customer must reconnect.
    /// </summary>
    public class MeldReConnectRequest
    {
        /// <summary>
        /// The id of the connection in need of repair
        /// </summary>
        public string connectionId { get; set; }
    }
    public class MeldMergeConnectRequest
    {
       /// <summary>
       /// New active connection ID
       /// </summary>
        public string ActiveConnectionId { get; set; }

        /// <summary>
        /// existing connection id which went into  Partial Active state
        /// </summary>
        public string PartialActiveConnectionId { get; set; }

        /// <summary>
        /// existing connections Institution id which went into  Partial Active state
        /// </summary>
        public string PartialActiveInstitutionId { get; set; }
    }

    /// <summary>
    /// The financial accounts belonging to the connection. Fields vary per event type, and only id is guaranteed. Additional fields are detailed in the respective event types.
    /// </summary>
    public class FinancialAccount
    {
        /// <summary>
        /// Unique identifier for the financial account
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// If present, the name of the account as provided by the institution.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// If present, the last 4 digits of the account number, sometimes referred to as the account mask.
        /// </summary>
        public string truncatedAccountNumber { get; set; }

        /// <summary>
        /// Indicates if the account was newly added or is an existing account getting updated (i.e. during a refresh)
        /// </summary>
        public bool newAccount { get; set; }

        /// <summary>
        /// The products that were updated for the account. This list will never include TRANSACTIONS because their updates are handled separately within the transactions webhooks. Products are considered updated even if nothing changed, and it is solely meant to indicate that the updated products were fetched from the service provider.
        /// </summary>
        public List<string> products { get; set; }

        /// <summary>
        /// Financial account id
        /// </summary>
        public string financialAccountId { get; set; }
        public string oldestTransactionUpdatedSearchKey { get; set; }

        /// <summary>
        /// If present, number of new transactions available
        /// </summary>
        public int numTransactionsAdded { get; set; }

        /// <summary>
        /// If present, number of updated transactions available for the financial account
        /// </summary>
        public int numInvestmentTransactionsAdded { get; set; }
        public int numTransactionsUpdated { get; set; }
        public int numInvestmentTransactionsUpdated { get; set; }
        public List<string> transactionIdsRemoved { get; set; }
        public List<string> investmentTransactionIdsRemoved { get; set; }
    }

    public class MeldSearchDTO
    {
        /// <summary>
        /// Meld generated unique identifier for your customer. This should be used to track customer activity. Should not be provided in conjunction with externalCustomerId
        /// </summary>
        public string customerId { get; set; }

        /// <summary>
        /// Your unique identifier for your customer. If maintaining your own customer management system this can also be used for tracking customer activity.
        /// </summary>
        public string externalCustomerId { get; set; }

        /// <summary>
        /// The id of the connection in need of repair
        /// </summary>
        public string connectionId { get; set; }

        /// <summary>
        /// Institution id
        /// </summary>
        public string institutionId { get; set; }

        /// <summary>
        /// Institution name. If present, only institution connections with this institution will be included.If institutionId is present then institutionId will take precedence.
        /// </summary>
        public string institutionName { get; set; }

        /// <summary>
        /// Unique key corresponding to a transaction's position in the paginated list of results -- used to indicate that only transactions residing before this key will be retrieved.
        /// The before and after fields cannot be used in the same request.
        /// </summary>
        public string before { get; set; }

        /// <summary>
        /// Unique key corresponding to a transaction's position in the paginated list of results -- used to indicate that only transactions residing after this key will be retrieved.
        /// The before and after fields cannot be used in the same request.
        /// </summary>
        public string after { get; set; }

    }
    public class MeldSearchConnectionsRequest : MeldFinancialAccountSearchRequest
    {
        /// <summary>
        /// Ref: MeldConnectionStatusEnum. Comma separated list of institution connection statuses. If present, only institution connections with these statuses will be included.
        /// </summary>
        public string statuses { get; set; }

        /// <summary>
        /// Comma separated list of service providers. If present, only institution connections with these service providers will be included.
        /// </summary>
        public string serviceProviders { get; set; }

        /// <summary>
        /// Start date to begin search from of format yyyy-mm-dd
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// End date stop search on of format yyyy-mm-dd
        /// </summary>
        public string endDate { get; set; }

    }
    public class MeldFinancialAccountSearchRequest : MeldSearchDTO
    {        
        /// <summary>
        /// Limits number of returned financial account transactions. Default value: 10 (Minimum value: 1  Maximum value: 100)
        /// </summary>
        public Int32 limit { get; set; } = 10;
    }
    public class MeldTransactionSearchRequest : MeldSearchDTO
    {

        /// <summary>
        /// Financial account id
        /// </summary>
        public string financialAccountId { get; set; }

        /// <summary>
        /// Financial account transaction status
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Start date to begin search from of format yyyy-mm-dd
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// End date stop search on of format yyyy-mm-dd
        /// </summary>
        public string endDate { get; set; }

        /// <summary>
        /// Transaction category
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// Limits number of returned financial account transactions. Default value: 100 (Minimum value: 1  Maximum value: 1000)
        /// </summary>
        public Int32 limit { get; set; } = 100;
    }


    public class ConnectionCompleteParamString
    {
        public string institutionId { get; set; }
        public string institutionName { get; set; }
        public string connectionId { get; set; }
        public int accountCount { get; set; }
        public Account[] accounts { get; set; }
        /// <summary>
        /// if ImportAccountDetailsToConvert exists, delete this account after successfull connetion on Meld and transfer of the posted feeds to connection
        /// </summary>
        public FeedAccountInfoDTO ImportAccountDetailsToConvert { get; set; }
        public short ConnectType { get; set; }//ChangeProvider|Re-Connect
    }

    public class Account
    {
        public string name { get; set; }
        public string mask { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public string verification_status { get; set; }
        public string class_type { get; set; }
    }


}
