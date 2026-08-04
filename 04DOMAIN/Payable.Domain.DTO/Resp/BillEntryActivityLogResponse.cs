using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class BillEntryActivityLogResponse
    {
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentedBy { get; set; }
        public short ApprovalTYpe { get; set; }
        public short ApprovalLevel { get; set; }
    }
    public class BillEntryActivityLogListResponse
    {
       public List<BillEntryActivityLogResponse> AcitivtyLog {  get; set; }
    }

}