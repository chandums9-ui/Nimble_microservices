using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class BFeed_ImportRequest
    {
        public string AccountId { get; set; }
        public string CorporationID { get; set; }
    }
}
