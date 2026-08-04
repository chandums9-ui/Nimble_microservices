using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Model.Base.Contracts;
using System.Net;
using Common.Domain.DTO.Globalization;

namespace Common.Domain.DTO.Resp
{
    public class SinglePostResponse : BankFeedPostResponseDTO
    {

    }
    public class SplitPostResponse : SplitPostResponseDTO
    {

    }
    public class PaymentmethodResponse : StatusDTO
    {
        public List<PaymentMenthodListDTO> PaymentMethods { get; set; }
    }

    public class MultiplePostResponse : StatusDTO
    {
        public List<SinglePostResponse> responses { get; set; } = new List<SinglePostResponse>();
    }

    //this is for getting dailysalelines for mergesettings purpose
    public class MergeSettingsListResponse : StatusDTO
    {
        public List<MergeSettingsDTO> MergeSettingLines { get; set; }
    }
    public class BillInformationResponse : StatusDTO
    {
        public long BillInfoID { get;set; }
        public string PaymentMethodID { get; set; }
    }
    public class GroupBillInformationResponse : StatusDTO
    {


    }

    public class CashAndCardDuesAsPerFeedsResponse : StatusDTO
    {
        public List<CashAndCardDuesAsPerFeeds> DuesAsPerFeeds { get; set; }
    }
    public class CashAndCardDuesAsPerFeeds
    {
        public string NimbleAccountID { get; set; }
        public string NimbleAccountName { get; set; }
        public string NimbleAccountTypeName { get; set; }
        public decimal NimbleBalance { get; set; }
        public string NimbleBalanceToDisplay { get { return GlobalizationInfo.GetCultureSpecificAmountForDisplay(this.NimbleBalance); } }

        //public DateTime? LastSyncDate { get; set; }
        /// <summary>
        /// Meld last uodated date
        /// </summary>
        public DateTime? ProviderUpdatedOn { get; set; }

        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public decimal BankBalance { get; set; }
        public string BankBalanceToDisplay { get { return GlobalizationInfo.GetCultureSpecificAmountForDisplay(this.BankBalance); } }
        public long PaymentsCount { get; set; }
        public long RecieptsCount { get; set; }
        public string OpenFeeds { get; set; }
        public string NavigationLink { get; set; }
    }
    public class TransactionsShareResponse : StatusDTO
    {
        public string RequestID { get; set; }
        public int TotalCount { get; set; }
        public long FeedAccID { get; set; }
    }
    public class MultipleTransactionsShare : StatusDTO
    {
        public List<TransactionsShareResponse> responses { get; set; } = new List<TransactionsShareResponse>();
    }
}
