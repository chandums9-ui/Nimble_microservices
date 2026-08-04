using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class STRRequest
    {

    }
    public class LatestAvailableDateRequest
    {
        public string UrlRequest { get; set; }
        public string type { get; set; }
        public string corporation_id { get; set; }
        public string profit_center_id { get; set; }
    }

    public class STRWeekORRangeRequest
    {
        public List<string> corporations { get; set; }
		public List<string> profit_centers { get; set; }

		public string sheet { get; set; }

        public string startdate { get; set; }
        public string enddate { get; set; }
        public string corporation_id { get; set; }
        public string profit_center_id { get;set; }
        public bool GetAllProfitCenterData{ get; set; }
        public string UrlRequest { get; set; }
    }

    public class STRWeeklyRequest
    {
        public List<string> corporations { get; set; }
        public string sheet { get; set; }

        public string week_start_date { get; set; }
        public string week_end_date { get; set; }
        public string corporation_id { get; set; }
        public string profit_center_id { get; set; }
        public string UrlRequest { get; set; }
    }

    public class STRMontlyRequest
    {
        public List<string> corporations { get; set; }
        public string sheet { get; set; }

        public string year { get; set; }
        public string corporation_id { get; set; }
        public string profit_center_id { get; set; }
        public string UrlRequest { get; set; }
    }

    public class STRYearlyRequest
    {
        public List<string> corporations { get; set; }
        public string sheet { get; set; }
        public string years_selected { get; set; }
        public string corporation_id { get; set; }
        public string profit_center_id { get; set; }
        public string UrlRequest { get; set; }
    }

    public class STRDayWiseMonthRequest 
    {
        public string corporation_id { get; set; }
        public string profit_center_id { get; set; }
        public string year { get; set; }
        public string month { get; set; }
    }

}
