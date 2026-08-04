using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Model
{
    public class WidgetForExportDTO
    {
        /// <summary>
        ///  Unique identifier for the WidgetForExport
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Name of the widget
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the widget (WidgetTypeEnum)
        /// </summary>
        public short Type { get; set; }
        public DateTime? FromDate
        {
            get
            {
                DateTime fromDate;
                return (!string.IsNullOrEmpty(ShortFromDate) && DateTime.TryParse(ShortFromDate, out fromDate)) ? fromDate : (DateTime?)null;
            }
        }
        public DateTime? ToDate
        {
            get
            {
                DateTime toDate;
                return (!string.IsNullOrEmpty(ShortToDate) && DateTime.TryParse(ShortToDate, out toDate)) ? toDate : (DateTime?)null;
            }
        }
        public string ShortFromDate { get; set; } = string.Empty;
        public string ShortToDate { get; set; } = string.Empty;
        public int SubType { get; set; }
    }
}
