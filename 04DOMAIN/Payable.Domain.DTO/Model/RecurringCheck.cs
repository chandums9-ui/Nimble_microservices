using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class RecurringCheck
    {
        public string RecurringName { get; set; }
        public bool IsDebitMemo { get; set; }
        public bool IsOCREntry { get; set; }
        public string CorpID { get; set; }
    }
}
