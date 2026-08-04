using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgmt.Domain.DTO.Req
{   
    public class UserCorpLinkRequest: ModelBaseCorporationID
    {
       // public string CorporationID { get; set; }
    }

    //public class UserCorpPCLinkRequest
    //{     
    //    public List<UserCoporations> Corporations { get;set; }
    //}

    public class UserCoporationsPCLinkRequest
    {
        public string CorpID { get; set; }
        public List<UserProfitcenters> UserPc { get; set; }
    }
    public class UserProfitcenters
    {
        public string ProfitCenterID { get; set; } 
    }

    public class UserPCDeleteRequest
    {
       // public string CorporationID { get; set; }
        public string ProfitCenterID { get;set; } 
    }
    public class UserPCUpdateRequest
    {       
        public string ProfitCenterID { get; set; }
        public short Status { get; set; }
    }
    
}
