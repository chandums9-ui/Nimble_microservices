using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{
    public class GetUserNameRequest
    {
        public List<UserInformation> ListUserIDs { get; set; } = new List<UserInformation>();
    }

    public class UserInformation
    {
        public string UserID { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
