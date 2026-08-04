using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class STRDayWiseMonthData : StatusDTO
    {
        public string corporation_name { get; set; }
        public string corporation_id { get; set; }
        public string profitcenter_id { get; set; }
        public int status_code { get; set; }
        public string detail { get; set; }
        public Data data { get; set; }
    }

    public class AdrDailyByMonth
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
    }

    public class Data
    {
        public List<AdrDailyByMonth> adr_DailyByMonth { get; set; }
        public List<OccupancyDailyByMonth> occupancy_DailyByMonth { get; set; }
        public List<RevparDailyByMonth> revpar_DailyByMonth { get; set; }
    }


    public class OccupancyDailyByMonth
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
    }

    public class RevparDailyByMonth
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
    }


}
