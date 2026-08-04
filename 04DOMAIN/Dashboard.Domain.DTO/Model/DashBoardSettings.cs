using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Model
{
    public class DashBoardSettings
    {
        public DefaultWidgetSettings DefaultWidgetSettings { get; set; }

        public string STRApiUrl { get; set; }
        public string GSSApiUrl { get; set; }
    }

    public class DefaultWidgetSettings
    {
        public Int16 DefaultFilterType { get; set; }
        public string DefaultComparisionTypeForTrends { get; set; }

        public string DefaultComparisionTypeForAnalysis { get; set; }
        public Int16 DefaultDateViewType { get; set; }
        public short Status {  get; set; }
        public Int16 GroupType { get; set; }
    }
  
}
