using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class RecurringResponse
    {
        public bool IsRecurring { get; set; }
        public string RecurringID { get; set; }
        public string JEID { get; set; }
    }
}
