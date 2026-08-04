using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepayPaidConsole
{
    public class RepayDTO
    {
        public class AuthToken
        {
            public string Token { get; set; }
            public string authID { get; set; }
            public string statusCode { get; set; }

        }

        public class ClientIdRes : StatusDTO
        {
            public List<ClientIdList> ClientIds { get; set; } = new List<ClientIdList>();
        }
        public class ClientIdList
        {
            public string ClientId { get; set; }
        }
    }
}
