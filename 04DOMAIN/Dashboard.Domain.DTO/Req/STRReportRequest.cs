using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class STRReportRequest
    {
      
            public string corporation_id { get; set; }
            public string profit_center_id { get; set; }
            public string startdate { get; set; }
            public string enddate { get; set; }
            public string viewBy { get; set; }
        

    }
}
