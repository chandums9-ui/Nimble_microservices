using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class SplitTransactionDivisionDetails
    {
        public string TID { get; set; }
        public string AccountID { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
