using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{

    public class GetUserCorporationsResp
    {
        public string UserId { get; set; }
        public Dictionary<string, UserCorporationInfo> UserCorporations { get; set; } = new();
    }

    public class UserCorporationInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string User_Id { get; set; }
        public string Role { get; set; }
        public List<CorpInfo> Corporations { get; set; } = new();
        
    }
    public class CorpInfo
    {
        public string CorporationName { get; set; }
        public string CorporationID { get; set; }
    }
    public class GetCorporationsDbResp
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string CorporationName { get; set; }
        public string CorporationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
    }
}