using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using CoreAccounting.Domain.DTO.Model;
using CoreAccounting.Domain.DTO.Req;
using Common.Domain;
using Common.Domain.DTO.Globalization;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class TransactionListResponse : TransactionListRequest, IStatusDTO
    {
        public int TotalCount { get; set; }
        public List<TransList> transList { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
    }


    public class TransactionUpdateResponse : StatusDTO
    {
        public List<string> TransactionIDs { get; set; }
        public List<string> JeIds { get; set; } 
        public short ActionStaus { get; set; }
    }

    public class TransList
    {
        public DateTime Date { get; set; }
        public string CheckNO { get; set; }
        public string Description { get; set; }
        public string PayeeName { get; set; }
        public string PayeeID { get; set; }

        public decimal Amount { get; set; }
        //public string DisplayAmount { get { return GlobalizationInfo.GetCultureSpecificAmountForDisplay(this.Amount); } }

        public short TransactionType { get; set; }

        public string TransactionTypeName { get; set; }
        public byte DebitCredit { get; set; }//for payment/Reciept -0/1
        public bool IsDailySale { get; set; }
        public string TransactionID { get; set; }
        public string JournalEntryID { get; set; }
        public string BillNum { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public byte IsAdjEntry { get; set; }
        public string AdjustmentLinkID { get; set; }
        public string MergeRefID { get; set; }
        public string MergeLineId { get; set; }
        public string MerchantReconRefID { get; set; }
        /// <summary>
        /// Reconciled / Resumed reconciled will be true else false
        /// </summary>
        public bool IsSelected { get; set; }
        public string ReconId { get; set; }

        public long FeedAccID { get; set; }
        public short Status { get; set; }

    }
    public class TransactionVerificationResp : StatusDTO
    {
        public List<VerificationTransactionResp> verificationTransactionResp { get; set; }
    }
    public class VerificationTransactionResp
    {
        public string TransactionID { get; set; }
        public short TransactionStatus { get; set; }
    }
    public class VerificationStatusResp : StatusDTO
    {
        public string AccountID { get; set; }
        public short ActStatus { get; set; }
    }
    public class CheckNumRes : StatusDTO
    {
        public string CheckNO { get; set; }
    }
}
