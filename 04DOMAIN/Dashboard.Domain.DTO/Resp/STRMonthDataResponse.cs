using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Dashboard.Domain.DTO.Resp
{

    public class AdrMonthlyAvg
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
    }

    public class AdrMonthlyAvgsGlance
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object tag_type { get; set; }
        public object change { get; set; }
        public object tag_year { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }

    

    public class OccupancyMonthlyAvg
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
    }

    public class OccupancyMonthlyAvgsGlance
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object tag_year { get; set; }
        public object change { get; set; }
        public object tag_type { get; set; }
    }

    public class RevparMonthlyAvg
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
    }

    public class RevparMonthlyAvgsGlance
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
        public object tag_year { get; set; }
        public object tag_type { get; set; }
    }

    public class STRMonthDataResponse
    {
        public string corporation_name { get; set; }
        public string corporation_id { get; set; }
        public string profitcneter_id { get; set; }
        public string profitcenter_name { get; set; } 
        public string str_id { get; set; }
        public int status_code { get; set; }
        public string detail { get; set; }
        public List<AdrMonthlyAvg> adr_monthlyAvgs { get; set; }
        public List<AdrMonthlyAvgsGlance> adr_monthlyAvgs_glance { get; set; }
        public List<OccupancyMonthlyAvg> occupancy_monthlyAvgs { get; set; }
        public List<OccupancyMonthlyAvgsGlance> occupancy_monthlyAvgs_glance { get; set; }
        public List<RevparMonthlyAvg> revpar_monthlyAvgs { get; set; }
        public List<RevparMonthlyAvgsGlance> revpar_monthlyAvgs_glance { get; set; }
    }




}
