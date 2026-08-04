using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.Domain.DTO.Model;

namespace UserMgmt.Domain.DTO.Resp
{
    public class RolePrivilegesResponse
    {
        
    }
    public class RoleReposnse : StatusDTO
    {
        //public RoleReposnse()
        //{
        //    base.Status = Constants.MSG_RP_Exists;
        //    base.StatusCode = StatusCodes.Status404NotFound;

        //}
    }

    public class SubRoleResponse : StatusDTO
    {
        public List<SubRoleList> SubRoles { get; set; }
    }
    public class SubRoleList
    {
        public long? ID { get; set; }
        public string Name { get; set; }
    }

    public class SubRoleRequest : ModelBaseUserID
    {
    }
    public class RolePrivilageList : StatusDTO
    {
        public List<UserPrivilegesBaseDTO> RoleList { get; set; }
    }
}
