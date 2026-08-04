using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class STRGridDataResponse : StatusDTO
    {
        public string str_id { get; set; }

        public Monthly Monthly { get; set; }
        
        public Weekly Weekly { get; set; }
    }

    public class Weekly
    {
        public List<Report> data { get; set; }
        public int status_code { get; set; }
        public string detail { get; set; }
        public string corporation_name { get; set; }
    }

    public class Monthly
    {
        public List<Report> data { get; set; }
        public int status_code { get; set; }
        public string corporation_name { get; set; }
        public string detail { get; set; }
    }
    public class Report
    {
        public string file_name { get; set; }
        public string s3_key { get; set; }
        public DateTime? upload_date { get; set; }
        public string corporation_name { get; set; }
        public string str_id { get; set; }
        public string corporation_id { get; set; }
        public string profit_center_id { get; set; }
        public string user_id { get; set; }
        public string client_id { get; set; }
        public string url { get; set; }
        public string report_type { get; set; }
        public List<DateTime> date_range { get; set; }
        public string objId { get; set; }
    }

}
