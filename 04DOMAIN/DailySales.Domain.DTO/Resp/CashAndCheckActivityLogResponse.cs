using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class CashAndCheckActivityLogResponse
    {
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentedBy { get; set; }
        public short ApprovalTYpe { get; set; }
        public short ApprovalLevel { get; set; }
    }
    public class CashAndCheckActivityLogListResponse
    {
        public List<CashAndCheckActivityLogResponse> AcitivtyLog { get; set; }
    }
}
