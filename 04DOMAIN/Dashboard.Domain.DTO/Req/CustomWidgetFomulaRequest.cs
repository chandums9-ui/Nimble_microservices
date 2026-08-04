using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class CustomWidgetFomulaRequest
    {
        public long WidgetID { get; set; } 
        public short WidgetType { get; set; }
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
        public long VsLY { get; set; }
        public string CorpID {  get; set; }
        public int IsMonthOrDayOrYear { get; set; }
        public long FormulaID { get;set; }
        public Int32 IncomeGroupID {  get; set; }   
        public Int32 IncomeDeptID {  get; set; }  
        public Int32 Urlkey { get; set; }
    }
    public class IncomeGroupAccountsRequest
    {
        public string CorporationId { get; set; }
        public long GroupID { get; set; }
        public long DepartmentId {  get; set; } 
        public long UrlKey {  get; set; }   
    }
}
