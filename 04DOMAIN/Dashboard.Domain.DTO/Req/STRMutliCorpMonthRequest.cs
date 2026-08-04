using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class STRMutliCorpMonthRequest
    {
        public List<string> corporations { get; set; }
        public string sheet { get; set; }
        public string year { get; set; }
        public string month { get; set; }
    }
}
