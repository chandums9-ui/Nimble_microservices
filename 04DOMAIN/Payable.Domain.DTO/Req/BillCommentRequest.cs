using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    public class BillCommentRequest
    {
        public string JID { get; set; }
        public string Comment { get; set; }
        public short? ApprovalType { get; set; }
    }

    public class CheckPrintingReq
    {
        public string CorpID { get; set;}
        public string AccountID { get; set; }
        public string JEID { get; set; }
    }
}
