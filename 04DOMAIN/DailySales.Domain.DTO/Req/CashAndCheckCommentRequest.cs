using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public class CashAndCheckCommentRequest
    {
        public string JID { get; set; }
        public string Comment { get; set; }
        public short? ApprovalType { get; set; }
        public short? ApprovalLevel { get; set; }
    }
}
