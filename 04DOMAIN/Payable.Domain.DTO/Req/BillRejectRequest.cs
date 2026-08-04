using Common.Domain.DTO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    public class BillRejectRequest
    {
        public  string JID { get; set; }
        public string AssignedUser { get; set; }
        public string Comment { get; set; }

        [DefaultValue(false)]
        public bool IsValidate { get; set; }//check for Corp Lock
        public JournalSourceTypes SourceType { get; set; } = JournalSourceTypes.Bill;
    }

    public class BillBulkRejectRequest
    {
        public string JID { get; set; }
        public string AssignedUser { get; set; }
        public string Comment { get; set; }

        public string BillNumber { get; set; }

        public JournalSourceTypes SourceType { get; set; } = JournalSourceTypes.Bill;
    }
}
