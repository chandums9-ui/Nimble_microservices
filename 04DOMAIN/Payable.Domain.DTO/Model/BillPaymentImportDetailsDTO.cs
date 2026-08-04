using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillPaymentImportDetailsDTO
    {
        public Int64 ID { get; set; }
        public string CoporationID { get; set; }
        public DateTime PayDate { get; set; }
        public decimal Amount { get; set; }
        public string Number { get; set; }
        public string PaymethodID { get; set; }
        public string BankAccountID { get; set; }
        public string VendorID { get; set; }
        public int Status { get; set; }
        public string PaymentMemo { get; set; }
    }
}
