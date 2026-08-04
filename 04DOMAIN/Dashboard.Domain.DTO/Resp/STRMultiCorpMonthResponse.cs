using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{


    public class AdrMultiCorpData
    {
        public List<AdrData> data { get; set; }
        public string detail { get; set; }
        public int status_code { get; set; }
    }

    public class OccMultiCorpData
    {
        public List<OccData> data { get; set; }
        public string detail { get; set; }
        public int status_code { get; set; }
    }

    public class RevparMultiCorpData
    {
        public List<RevparData> data { get; set; }
        public string detail { get; set; }
        public int status_code { get; set; }
    }
    public class AdrMonthAvgs
    {
        public string sheet { get; set; }
        public List<AdrData> data { get; set; }
    }

    public class AdrMonthAvgsGlance
    {
        public string sheet { get; set; }
        public List<OccData> data { get; set; }
    }

    public class OccupancyMonthAvgs
    {
        public string sheet { get; set; }
        public List<OccData> data { get; set; }
    }

    public class OccupancyMonthAvgsGlance
    {
        public string sheet { get; set; }
        public List<OccData> data { get; set; }
    }

    public class RevparMonthAvgs
    {
        public string sheet { get; set; }
        public List<RevparData> data { get; set; }
    }

    public class RevparMonthAvgsGlance
    {
        public string sheet { get; set; }
        public List<RevparData> data { get; set; }
    }
    public class AdrData
    {
        public object corporation_name { get; set; }
        public object corporation_id { get; set; }
        public object profitcenter_id { get; set; }
        public object str_id { get; set; }
        public object profitcenter_name { get; set; }
        public AdrMonthAvgs adr_monthlyAvgs { get; set; }
        public AdrMonthAvgsGlance adr_monthlyAvgs_glance { get; set; }
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
        public object tag_year { get; set; }
        public object tag_type { get; set; }
    }

    public class OccData
    {
        public object corporation_name { get; set; }
        public object corporation_id { get; set; }
        public object profitcenter_id { get; set; }
        public object str_id { get; set; }
        public object profitcenter_name { get; set; }
        public OccupancyMonthAvgs occupancy_monthlyAvgs { get; set; }
        public OccupancyMonthAvgsGlance occupancy_monthlyAvgs_glance { get; set; }
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
        public object tag_year { get; set; }
        public object tag_type { get; set; }
    }

    public class RevparData
    {
        public object corporation_name { get; set; }
        public object corporation_id { get; set; }
        public object profitcenter_id { get; set; }
        public object str_id { get; set; }
        public object profitcenter_name { get; set; }
        public RevparMonthAvgs revpar_monthlyAvgs { get; set; }
        public RevparMonthAvgsGlance revpar_monthlyAvgs_glance { get; set; }
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public object change { get; set; }
        public object tag_type { get; set; }
        public object tag_year { get; set; }
    }




}
