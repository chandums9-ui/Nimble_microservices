using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Payable.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    internal class BillAndBillPaymentReq
    {
    }
    public class BillsAndPaymentsReq
    {
        public string CorpID { get; set; }
        public string? VendorID { get; set; }   // By default All
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string UserID { get; set; }
        public int BillPageNumber { get; set; }
        public int Type { get; set; }
        public int BillPaymentPageNumber { get; set; }
        public int TransactionRange { get; set; }
        public short OrderType { get; set; }    //0-FIFO,1-LIFO
        public bool? IsFromBill { get; set; }
        public bool? IsPaginationRequired { get; set; }
        public List<string> paymentIDS { get; set; } = new List<string>();
    }

    public class SaveOrLinkPaymentResp : StatusDTO
    {
        public string JournalEntryID { get; set; }

    }

    public class PostBillPayRequest : BillsPayments
    {
        public List<Bills> Bills { get; set; } = new List<Bills>();
    }



    public class SaveAutolinkOrMatch : BillsAndPaymentsReq
    {

    }
    public class AutomatchedBillsPostReq
    {
        public List<AutomatchedBills> automatchedBills { get; set; } = new List<AutomatchedBills>();
    }
    public class BulkPostBillPayRequest
    {
        public string PaymentMethodID { get; set; }
        
        public DateTime? BillPaymentDate { get; set; }
        public string BankAccount { get; set; }
        public List<string> JIDsList { get; set; }
        public long JournalPaymentId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentMemo { get; set; }
        public string CheckNumber { get; set; } 
    }
}
