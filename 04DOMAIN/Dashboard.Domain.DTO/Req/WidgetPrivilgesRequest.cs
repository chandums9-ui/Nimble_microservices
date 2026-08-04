using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class WidgetPrivilgesRequest
    {
        public string sourceID {  get; set; }
        public bool sourceType {  get; set; }
        public bool? IsAddWidget { get; set; }
        public bool? IsExport { get; set; }
        public bool? IsSchedule { get; set; }
    }
    public class widgetscreateRequest
    {
        public long WidgetPrivId { get; set; }

        public long WidgetId { get; set; }

        public bool? IsView { get; set; }

        public bool? IsNavigate { get; set; }

        public bool? IsFormula { get; set; }

        public bool? IsDelete { get; set; }
    }
    

}
