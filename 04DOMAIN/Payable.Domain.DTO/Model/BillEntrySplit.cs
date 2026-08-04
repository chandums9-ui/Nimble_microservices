using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillEntrySplit
    {        
        public List<SplitBillEntry> SplitBillEntry { get; set; }

        public BillEntryDetails BillEntryDetails { get; set; }
    }
}
