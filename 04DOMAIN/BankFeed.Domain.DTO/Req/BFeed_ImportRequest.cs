using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
  
    public class BFeed_importRequest
    {
        public List<TransactionList> Transactions { get; set; }

    }
    public class TransactionList
    {
        public long FeedRuleID { get; set; }
        public DateTime Date { get; set; }
        public string CheckNO { get; set; }
        public string Description { get; set; }
        public string PayeeName { get; set; }
        public string PayeeID { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
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
        public bool IsSelected { get; set; }
        public string ReconId { get; set; }
        public long FeedAccID { get; set; }
        public short Status { get; set; }
        public short TransPostType { get; set; }
        public bool IsAutoRuleApplied { get; set; }
        public string LogSeqID { get; set; }
        public int PageNo { get; set; }
    }

    public class BFeed_IOImportRequest
    {
        public long ImportId { get; set; }
        public string CorporationID { get; set; }
        public string BankName { get; set; }
        public string AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
    public class BFeed_NimbleImportRequest
    {
        public BFeed_ImportReq UploadFileReq { get; set; }
        public string CorporationID { get; set; }
        public string BankName { get; set; }
        public string AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string FileType { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime? StatementEndDate { get; set; }
        public decimal EndingBalance { get; set; }
    }

    public class BFeed_ImportFile
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
    public class BFeed_ImportReq
    {
        public List<BFeed_ImportFile> files { get; set; }
        public string CorpId { get; set; }
        public string Memo { get; set; }
        public string FileName { get; set; }
        public string clientURL { get; set; }
    }
}

