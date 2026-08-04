using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class VendorDuesRequest : AnalyticsRequest
    {
        public short DueStatus { get; set; }
        public long UrlKey { get; set; }
        public string Client { get; set; }
    }
}
