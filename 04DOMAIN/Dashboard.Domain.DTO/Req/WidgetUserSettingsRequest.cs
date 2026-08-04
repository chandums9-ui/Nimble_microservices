using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class WidgetUserSettingsRequest : ValidationRequest
    {
        public string WidgetName { get; set; }
        public long WidgetID { get; set; }
        public Int16 DateFilterType { get; set; }
        public Int16? DateViewType { get; set; }
        public string CompValue { get; set; }
        public string ColumnValue { get; set; }
        public int WidgetType { get; set; }
    }
    public class ValidationRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int VsYear { get; set; }
        public int ViewBy { get; set; }
    }
}
