using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public class MechantDailySalesRequest
    {

        public string CorporationID { get; set; }
        public string? PCID { get; set; } // Optional 
        public string AccountID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
    public class SaveTransactionRequest
    {
        public string CorporationID { get; set; }
        public string AccountID { get; set; }
        public string PCID { get; set; }

        public decimal Amount { get; set; }   // Common amount for both cases
        public int TransactionType { get; set; } // e.g., "ChargeBack" or "FeeAdjustment"

        public string UserID { get; set; }
        public DateTime TransactionDate { get; set; }

        public string TransactionUniqID { get; set; }
        public short ApprovalType {  get; set; }    
    }

    public class UndoValidationRequest
    {
        public List<string> MatchingIDs {  get; set; }  =new List<string>();
        public string UserID { get; set; }
    }

    public class DailysaleMultiCorpRequest
    {
        public string CorporationID { get; set; }
        public string PcID { get; set; }
        public string AccountID { get; set; }
    }

    public class MechantDailySalesMultiCorpRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<DailysaleMultiCorpRequest> dailysaleMultiCorpRequest { get; set; } = new List<DailysaleMultiCorpRequest>();
    }
    
}
