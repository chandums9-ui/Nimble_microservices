using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class ProviderBaseURLInfo
    {
        public string Yodlee { get; set; }
        public string Plaid { get; set; }
        public string Meld { get; set; }
    }
    public class YodleeKeyInfo
    {
        public string clientID { get; set; }
        public string clientSecret { get; set; }
        public string AdminKey { get; set; }
    }
    public class PlaidKeyInfo
    {
        public string clientID { get; set; }
        public string clientSecret { get; set; }
        public string ClientName { get; set; }
        public string Environment { get; set; }
    }
    public class MeldKeyInfo
    {
        public string APIKey { get; set; }
        public string WebhookURL { get; set; }
        public string WebhookSecret { get; set; }
        public string Products { get; set; } = "BALANCES, TRANSACTIONS";
        public string OptionalProducts { get; set; } //= "OWNERS,IDENTIFIERS,INVESTMENT_HOLDINGS, INVESTMENT_TRANSACTIONS";
    }
    public class PlaidEndPoints
    {
        public string Accounts { get; set; }
        public string Auth { get; set; }
        public string Transactions { get; set; }
        public string publicToken { get; set; }
        public string exchangeToken { get; set; }
        public string Unregister { get; set; }
    }

    public class YodleeEndPoints
    {
        public string authToken { get; set; }
        public string account { get; set; }
        public string providerAccounts { get; set; }
        public string providerAccountsbyProviderId { get; set; }
        public string transactions { get; set; }
        public string transactionsCount { get; set; }
        public string userRegister { get; set; }
        public string getUser { get; set; }
        public string userUnregister { get; set; }
        public string yodleeAdminKey { get; set; }
    }
    public class MeldEndPoints
    {
        public string StartConnection { get; set; }
        public string RepairConnection { get; set; }
        public string UpdateConnections { get; set; }
        public string RefreshConnections { get; set; }
        public string ImportConnections { get; set; }
        public string SearchConnections { get; set; }
        public string DeleteConnection { get; set; }
        public string GetInstitutionConnection { get; set; }
        public string GetFinancialAccount { get; set; }
        public string SearchFinancialAccount { get; set; }
        public string GetTransaction { get; set; }
        public string SearchTransactions { get; set; }
        public string CreateProcessorsToken { get; set; }
    }
    public class token
    {
        public string accessToken { get; set; }
        public string issuedAt { get; set; }
        public int expiresIn { get; set; }
    }
    public class YodleeTokenInfo
    {
        public token? token { get; set; }
    }
    public class YodleeTokenError
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public string referenceCode { get; set; }

    }




}
