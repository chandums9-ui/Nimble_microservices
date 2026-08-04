using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.Domain.DTO.Model;

namespace UserMgmt.Domain.DTO.Req
{
     
    public class UserPrivilegesRequest : UserPrivilegesBaseDTO
    {
        public string UserID { get; set; }
    }

    public class UserFullAccessRequest
    {
        public string UserID { get; set;}
        public bool Full { get; set; } = false;
    }
     
    public class MenuPrivilegesRequest: ModelBaseUserID
    {
        public long? MenuId { get; set; }
    }
    

   
   
   

  

   
      

   

}
