using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDumps
{
    public class WarehouseDumpDTO
    {
        public class AuthToken
        {
            public string Token { get; set; }
            public string authID { get; set; }
            public string statusCode { get; set; }

        }
    }
}
