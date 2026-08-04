using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillPaymentImportMappingDetailsDTO
    {
        public Int64 ID { get; set; }
        public Int64 BillPayImportID { get; set; }
        public string BillJournalID { get; set; }
        public string Status { get; set; }
    }
}
