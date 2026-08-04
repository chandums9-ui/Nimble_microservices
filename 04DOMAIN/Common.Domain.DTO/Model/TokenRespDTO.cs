using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{

    public class TokenMinRespDTO : StatusDTO
    {
        public string Token { get; set; }
    }

        public class TokenRespDTO : StatusDTO
    {
        public string Token { get; set; }
        public string AuthID { get; set; }
        public string ClientUrl { get; set; }
        public string ClientFullUrl { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public long UrlID { get; set; }
        public string UserFullName { get; set; }
        public string UrlInfo { get; set; } 

        public string CurServerDateTime { get; set; }
    }
    public class ValidateTokenRespDTO : StatusDTO
    {
        public string UserID { get; set; }
        public string ClientID { get; set; }

        public string ClientName { get; set; }
        public string UserInfoID { get; set; }
        public string ClientInfoID { get; set; }

        public string UrlID { get; set; }

        public string ClientUrl { get; set; }
    }
    public class AuthenticationDetails
    {
        public bool IsValidate { get; set; }
        public string ClientFullURL { get; set; }
        public DateTime CurServerDateTime { get; set; }
        public string UrlInfo { get; set; }
    }

}
