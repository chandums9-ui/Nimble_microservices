using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.Domain.DTO.Model;

namespace UserMgmt.Domain.DTO.Req
{
    public class RolePrivilegesRequest : RolePrivilegesDTO
    {

    }
    public class RolePrivilegesMultipleRequest
    {
        public long? RoleID { get; set; }
        public List<UserPrivilegesBaseDTO> RoleList { get; set; }
    }
    public class RoleSaveUpdateRequest : RoleDTO
    {

    }
    public class RoleDeleteRequest
    {
        public long RoleID { get; set; }
    }
    public class RoleFullAccessRequest
    {
        public long? RoleID { get; set; }
        public bool Full { get; set; }
        public long? ParentRoleID { get; set; }
    }


}
