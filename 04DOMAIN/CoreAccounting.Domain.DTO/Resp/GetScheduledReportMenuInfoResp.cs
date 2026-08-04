using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class GetScheduledReportMenuInfoResp
    {
        public List<GetScheduledReportMenuInfoDboResp> schedules { get; set; } = new();
    }


    public class GetScheduledReportMenuInfoDbResp
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string DateRange { get; set; }
        public string ReportType { get; set; }
        public string Account { get; set; }
        public string FormateType { get; set; }
    }

    public class GetScheduledReportMenuInfoDboResp
    {
        public string report_id {  get; set; }
        public string report { get; set; }
        public string date_range { get; set; }
        public string report_type { get; set; }
        public string account { get; set; }
        public string formate_type { get; set; }

    }
}
