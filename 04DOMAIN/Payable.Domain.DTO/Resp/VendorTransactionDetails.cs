using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class VendorTransactionDetails
    {
        public string JEID { get; set; }
        public string BillNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string BillStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string BankAccount { get; set; }
        public string PaymentStatus { get; set; }

        public short? PaymentMethodType { get; set; }

        public int? DDType { get; set; }
    }
}
