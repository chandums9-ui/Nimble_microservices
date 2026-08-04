using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class STRMonthDataRequest
    {
        public string corporation_id {  get; set; }
        public string profit_center_id { get; set; }
        public string year { get; set; }
        public string month { get; set; }   
    }
}
