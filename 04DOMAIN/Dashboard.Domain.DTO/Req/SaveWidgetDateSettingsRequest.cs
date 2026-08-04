using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class SaveWidgetDateSettingsRequest
    {
        public long WidgetID { get; set; }
        public Int16 DateFilterType { get; set; }
    }
}
