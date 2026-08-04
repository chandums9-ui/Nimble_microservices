using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Dashboard.Domain.DTO.Enums;
using Dashboard.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class AnalyticsRequest //: DateRangeDTO
    {
        public string CorporationId { get; set; }
        public long UrlKey { get; set; }
        /// <summary>
        /// refer to ComparisonFilterEnum
        /// </summary>
        public short ComparisonFilter { get; set; }
        public List<string> Corporations { get; set; }

        /// <summary>
        /// It is readonly, ShortFromDate will be converted to DateTime and given back other wise it returns DateTime.Today
        /// </summary>
        public DateTime? FromDate
        {
            get
            {
                DateTime fromDate;
                return (!string.IsNullOrEmpty(ShortFromDate) && DateTime.TryParse(ShortFromDate, out fromDate)) ? Convert.ToDateTime(ShortFromDate) : DateTime.Today;
            }
        }

        /// <summary>
        /// It is readonly, ShortToDate will be converted to DateTime and given back other wise it returns DateTime.Now
        /// </summary>
        public DateTime? ToDate
        {
            get
            {
                DateTime todate;
                return (!string.IsNullOrEmpty(ShortToDate) && DateTime.TryParse(ShortToDate, out todate)) ? Convert.ToDateTime(ShortToDate) : DateTime.Now;
            }
        }

        /// <summary>
        /// Send short date string format, this will be converted in API to Date time frmat(PerformanceRequest.FromDate)
        /// </summary>
        public string ShortFromDate { get; set; } = string.Empty;

        /// <summary>
        /// Send short date string format, this will be converted in API to Date time frmat(PerformanceRequest.ToDate)
        /// </summary>
        public string ShortToDate { get; set; } = string.Empty;

    }

    public class PayrollCostRequest : AnalyticsRequest
    {
        /// <summary>
        /// Represents the type of Payroll cost:
        /// 1. Available Rooms
        /// 2. Occupancy Rooms
        /// 3. Hours Per Month
        /// </summary>
        public short PayrollCostType { get; set; } =(short) PayrollDepartmentEnum.AvailableRooms;

        /// <summary>
        /// When a year is selected, the graph will display the data for each month of that year.
        /// </summary>
        public int FromYear { get; set; }
        public int ToYear { get; set; } 
    }

    /// <summary>
    /// Common request for the all widgets in labour 
    /// </summary>
    public class LabourRequest : PayrollCostRequest
    {
        public int LabourType { get; set; }
        public int Type {  get; set; }  

    }
    /// <summary>
    /// This request is common for  PandLgraph and BalancesheetGraph
    /// </summary>
    public class FinancialRequest : AnalyticsRequest
    {
        /// <summary>
        /// Based on the selected year, the graph data will be loaded on a month-by-month basis.
        /// </summary>
        public string FromYear { get; set; }
        public  string ToYear {  get; set; } 
       // public long Urlkey { get; set; }
    }

}
