using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    public class BillVoidRequest
    {
        public string JID { get; set; }
        public DateTime? VoidDate { get; set; }
        public string VoidRemarks { get; set; }
        public bool isVoid { get; set; }
        [DefaultValue(true)]
        public bool isSave { get; set; }
        [DefaultValue(false)]
        public bool isfromBE { get; set; }

        public bool IsValidate { get; set; } = false;
    }
}
