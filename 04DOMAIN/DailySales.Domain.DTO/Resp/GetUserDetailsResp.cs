using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class GetUsersByCorpResp : StatusDTO
    {
        public List<CorporationUsersDto> Corporations { get; set; }
    }

    public class CorporationUsersDto
    {
        public string CorporationID { get; set; }
        public string Name { get; set; }
        public List<UserDetailsDto> Users { get; set; }
    }

    public class UserDetailsDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
    }


    public class GetUserDetailsDBResp
    {
        public string CorporationID { get; set; }
        public string Name { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }

    }
}
