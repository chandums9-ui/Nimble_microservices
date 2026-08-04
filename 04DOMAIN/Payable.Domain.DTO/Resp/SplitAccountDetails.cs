using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class SplitAccountDetails
    {
        public string AccountID { get; set; }
        public decimal TaxPercentage { get; set; }
        public string AccountName { get; set; }
        public string PurposeID { get; set; }
        public string PurposeName { get;set; }
        public string TaxLineID { get; set; }
        public string TaxLineName { get; set; }
        public bool IsVendorTax { get;set; }

    }
}
