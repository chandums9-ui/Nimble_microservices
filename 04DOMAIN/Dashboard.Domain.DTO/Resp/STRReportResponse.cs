    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{

    public class STRDayOrMonthOrWeekReport
    {
        public string corporation_name { get; set; }
        public string corporation_id { get; set; }
        public object profitcenter_id { get; set; }
        public object profitcenter_name { get; set; }
        public string str_id { get; set; }
        public int status_code { get; set; }
        public string detail { get; set; }
        public List<DayOrMonthOrWeekData> adr { get; set; }
        public List<DayOrMonthOrWeekData> occupancy { get; set; }
        public List<DayOrMonthOrWeekData> revpar { get; set; }
    }

  

    public class DayOrMonthOrWeekData
    {
        public string label { get; set; }
        public object change { get; set; }
        public DateTime timestamp { get; set; }
        public object change_rate { get; set; }
        public Metadata metadata { get; set; }
    }

   


  
}
