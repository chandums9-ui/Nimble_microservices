using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    public class BillPaymentRejectRequest
    {
        public string  AssignedTo { get; set; }
        public string JID { get; set; }
        public string Comment { get; set; }
        [DefaultValue(true)]
        public bool IsSave { get; set; }
    }
}
