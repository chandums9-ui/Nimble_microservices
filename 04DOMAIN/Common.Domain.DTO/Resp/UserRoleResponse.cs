using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class UserRoleResponse:StatusDTO
    {
        public List<UserRoleID> RoleList { get; set; }
    }
    public class UserRoleID
    {
        public string RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
