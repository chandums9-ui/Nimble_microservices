using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgmt.Domain.DTO.Model
{
    public class RoleDTO
    {
        public string Name { get; set; }
        public long? ParentID { get; set; }
        public long? RoleID { get; set; }
        public short Status { get; set; }
    } 
}
