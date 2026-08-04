using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class WidgetConfigLoadRequest
    {
        public string WidgetId { get; set; }    
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public short WidgetCompareFilter {  get; set; } 
        

    }
}
