using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{

    public class PublicTokenDTO
    {
        public string public_token { get; set; }
        public plaidInstitution institutionInfo { get; set; }
    }
    public class plaidInstitution
    {
        public string institution_id;
        public string name;
    }
    public class AccessTokenDTO
    {
        public string access_token { get; set; }
    }

    public class TransactionDTO : AccessTokenDTO
    {
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
    public class TransOptions
    {
        public string accountIDs { get; set; }
        public long count { get; set; }
        public int offset { get; set; }
    }
    public class PlaidRequestID
    {
        public string request_id { get; set; }
    }
    public class PlaidTokenRespInfo : PlaidRequestID
    {
        public string public_token { get; set; }
    }
    public class ExchangeToken : PlaidRequestID
    {
        public string access_token { get; set; }
        public string item_id { get; set; }
    }

    public class ExchangeTokenResponse
    {
        public ExchangeToken exchangeToken { get; set; }
        public plaidInstitution institutionInfo { get; set; }
    }
    public class BankAccountDetails
    {
        public string account_id { get; set; }
        public string name { get; set; }
        public string official_name { get; set; }
        public string subtype { get; set; }
        public string type { get; set; }
        public Balance balances { get; set; }
        public string mask { get; set; }
    }
    public class Balance
    {
        public decimal? available { get; set; }
        public decimal? current { get; set; }
        public string iso_currency_code { get; set; }
        public decimal? limit { get; set; }
    }
    public class PlaidAccounts : StatusDTO
    {
        public PlaidAccounts()
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
        public List<BankAccountDetails> accounts { get; set; }
        public PLaidAccountNumbers numbers { get; set; }
    }
    public class PLaidAccountNumbers
    {
        public List<PlaidAccountAchInfo> ach { get; set; }
    }
    public class PlaidAccountAchInfo
    {
        public string account { get; set; }
        public string account_id { get; set; }
        public string routing { get; set; }
        public string wire_routing { get; set; }
    }
    public class PlaidErrorMessages
    {
        public string display_message { get; set; }
        public string error_code { get; set; }
        public string error_message { get; set; }
        public string error_type { get; set; }

        public string request_id { get; set; }
    }
    public class PlaidTransactionsResponse : StatusDTO
    {
        public PlaidTransactionsResponse()
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
       public List<PlaidTransactionDetails> transactions { get; set; }
    }
    public class PlaidTransactionDetails : PlaidRequestID
    {
        public List<BankAccountDetails> accounts { get; set; }
        public long total_transactions { get; set; }
        public List<PlaidTransactions> transactions { get; set; }
    }
    public class PlaidTransactions
    {
        public string account_id { get; set; }
        public decimal amount { get; set; }
        public string category_id { get; set; }
        public string date { get; set; }
        public string name { get; set; }
        public string check_number { get; set; }
        public string merchant_name { get; set; }
        public string original_description { get; set; }
        public bool pending { get; set; }
        public string pending_transaction_id { get; set; }
        public string transaction_id { get; set; }
        public string transaction_type { get; set; }
    }
}
