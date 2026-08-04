using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class ApprovalPolicyUserDetails
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int ApprovalOrder { get; set; }
        public int ApprovalType { get; set; }
        public bool IsCureentUser { get; set; }
    }
}
