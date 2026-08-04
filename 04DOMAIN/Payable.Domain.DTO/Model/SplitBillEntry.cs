using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class SplitBillEntry
    {
        public string BillNumber { get; set; }
        public List<SplitTransactionDivisionDetails> SplitTransactionDivisions { get; set; }

        public decimal Amount { get; set; }
    }
}
