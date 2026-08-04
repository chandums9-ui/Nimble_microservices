using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgmt.Domain.DTO.Model
{
    public class UserPrivilegesBaseDTO
    {
        public long MenuID { get; set; }
        public bool Create { get; set; } = false;
        public bool View { get; set; } = false;
        public bool Update { get; set; } = false;
        public bool Delete { get; set; } = false;
    }
}
