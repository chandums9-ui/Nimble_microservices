using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class SaveWidgetDateSettingsResponse : StatusDTO
    {
        public long WidgetID { get; set; }
        public Int16? DateFilterType { get; set; }
    }
}
