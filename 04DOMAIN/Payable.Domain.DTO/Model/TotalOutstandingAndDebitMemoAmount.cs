using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class TotalOutstandingAndDebitMemoAmount
    {
        public decimal TotalOutstanding { get; set; }
        public decimal DebitMemoAmount { get; set;}

    }

    public class VendorOutstandingAndDebitMemoAmount
    {
        public int RefType { get; set; }
        public decimal TotalAmount { get; set; }


        public long BillsCount { get; set; }

        public string BillType { get; set; }
    }

    public class VendorOustandingInfo
    {

       public string VendorID { get; set; }
        public decimal TotalOutStandingAmount { get; set; }

        public decimal TotalDebitMemoAmount { get; set; }

    }
}
