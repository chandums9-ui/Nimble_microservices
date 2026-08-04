using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgmt.Domain.DTO.Req
{
   
    public class UrlInfoRequest
    {
        public string Name { get; set; }
        public string url { get; set; }
    }

    public class UserPwdReq
    {
        public string UserName { get; set; }

        public string url { get; set; }
    }

    public class UserInfoReq
    {
        public string UserID { get; set; }

        public string url { get; set; }
    }

    public class UserRequest
    {
        public string ClientID { get; set; }
        public string UserID { get; set;}
    }

    public class ClientDeletRequest
    {
        public string ClientID { get; set; }
        public string url { get; set; }
    }
}
                                                                                        