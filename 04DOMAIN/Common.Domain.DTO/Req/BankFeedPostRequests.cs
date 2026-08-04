using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Common.Domain.DTO.Req
{
    public class SinglePostRequest : BankFeedPostRequestDTO
    {

    }

    //public class MultiplePostRequest : StatusDTO
    //{
    //    public List<BankFeedPostRequestDTO> PostRequests { get; set; }
    //}
    public class MultiplePostRequest
    {
        public List<SinglePostRequest> requests { get; set; }
    }

    public class MergeSettingsListRequest : LoadByIDRequest
    {
        public string AccountID { get; set; }
    }
    public class BillInfoRequest : LoadByIDRequest
    {
        public string JournalID { get; set; }
        public bool IsReconcile { get; set; } = false;
    }
    public class GroupBillInfoRequest : LoadByIDRequest
    {
        public List<string> JournalIDs { get; set; }
    }
    public class AuditLogReq 
    {
        public List<string> JournalIds { get; set; } = new List<string>() { "0x000000000000000000000000000000000000" };
    }

    public class CashAndCardDuesAsPerFeedsRequest
    {
        //public string ClientID { get; set; }
        public string CorpID { get; set; }
        public string CorpName { get; set; }
        public List<string> COAIds { get; set; }
    }
    public class TransactionListRequest
    {
        public string NimbleAccountID { get; set; }
        /// <summary>
        /// To get transactions within the given range from today
        /// </summary>
        public int NumberofDays { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        /// <summary>
        /// check -11, Journal -20,reciept - 32,billpay -15,customer reciept -33,Nofilter - 0
        /// </summary>
        public short SourceType { get; set; }
        /// <summary>
        /// To get specified transactions matched with amount
        /// </summary>
        [DefaultValue(null)]
        public decimal? AmountorCheckNO { get; set; }
        /// <summary>
        /// To hide reconciled transactions from list
        /// </summary>
        [DefaultValue(null)]
        public bool? HideReconcile { get; set; }
        /// <summary>
        /// To get required number of transactions for paging
        /// </summary>
        [DefaultValue(100)]
        [Range(1, 1000)]
        public int PageCount { get; set; }
        /// <summary>
        /// To skip transactions for paging
        /// </summary>
        [DefaultValue(0)]
        public int PageOffSet { get; set; }

        public string? NameID { get; set; }
        public bool IsFromManualMatch { get; set; }

        /// <summary>
        /// check -11, Journal -20,reciept - 32,billpay -15,customer reciept -33,PostAdjustmentType - 0
        /// </summary>
        public short PostAdjustmentType { get; set; }

        public long FeedAccID { get; set; }
        //public string ClientID { get; set; }
        //public List<long> FeedTranIDs { get; set; }
    }

    public class MultipleRequestsOfTransactionList
    {
        public List<TransactionListRequest> Request { get; set; }
    }
}