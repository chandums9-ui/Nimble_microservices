using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillEntryPaymentToolTipDetails
    {
        public DateTime? BillPaymentDate { get; set; }
        public decimal? AmountPaid { get; set; }
        public string BillPaymentJEID { get; set; }
        public string CheckNo { get; set; }
        public string PaymentType { get; set; }
        public short? PaymentMethodType { get; set; }
        public string BillApprovalStatus { get;set; }
        public int BillApprovalType { get; set; }
   
        public bool IsDDApi { get; set; } = false;
        public string MasterPaymentType { get; set; }
        public decimal? billOutStaingAmount { get; set; }
        public int EpaymentStatus { get; set; }
    }
}
