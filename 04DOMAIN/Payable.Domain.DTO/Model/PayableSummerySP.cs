using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class PayableSummerySP
    {
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
        public string LegalName { get; set; }
        public string VendorName { get; set; }
        public string PaymethodName { get; set; }
        public decimal BillToBeApproved { get; set; }
        public int BillToBeApprovedCount { get; set; }
        public decimal PendingPayments { get; set; }
        public int PendingPaymentsCount { get; set; }
        public decimal PaymentsToBeapproved { get; set; }
        public int PaymentsToBeapprovedCount { get; set; }
        public int ExpectedCount { get; set; }
        public string Status { get; set; }
    }
}
