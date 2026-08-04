//using Azure;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.Domain.DTO.Model;
using UserMgmt.Domain.DTO.Req;

namespace UserMgmt.Domain.DTO.Resp
{

    public class PrivilegesResponse : UserPrivilegesBaseDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }

    public class UserPrivilegesResponse:StatusDTO
    {     

    }

    public class UserPrivilegesListResponse:StatusDTO
    {
        public List<UserPrivilegesBaseDTO> MenuItems { get; set; }      
    } 
    
}
