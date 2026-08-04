using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class BillPaymentResponse :StatusDTO
    {
        public List<BillPaymentInfoResp> BillPaymentList {  get; set; }=new List<BillPaymentInfoResp>();

        public string BatchID { get; set; }
        public string FailedJournalPaymentIds{ get; set; }
    }
    public class HoldBillResponse : StatusDTO
    {
        public List<HoldBillRes> res { get; set; } = new List<HoldBillRes>();
    } 
    public class HoldBillRes
    {
        public string JEID { get; set; }    
        public bool IsHold { get; set; }   
        public string BillNumber { get; set; }  
    }

    public class BillPaymentInfoResp:StatusDTO
    {
        public long BpInfoID { get; set; }

        public string JournalEntryID { get; set; }

        public string TransactionID { get; set; }

        public string VendorID { get; set; }

        public string ContractID { get; set; }

        public string Number { get; set; }

        public bool PrintCheckNow { get; set; }
        public string BatchID { get; set; }

        public string Memo { get; set; }
    }
}
