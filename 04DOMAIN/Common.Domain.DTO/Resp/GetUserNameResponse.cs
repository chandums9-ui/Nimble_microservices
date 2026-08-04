using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class GetUserNameResponse : StatusDTO
    {
        public List<UserInformation> ListUserInfo { get; set; } = new List<UserInformation>();
    }
}
