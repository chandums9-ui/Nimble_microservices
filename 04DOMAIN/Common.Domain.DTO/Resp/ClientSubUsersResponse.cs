using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class ClientSubUsersResponse
    {
        public List<ClientSubUsers> ClientSubUsers { get; set; } = new List<ClientSubUsers>();
    }

    public class ClientSubUsers
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserRoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}
