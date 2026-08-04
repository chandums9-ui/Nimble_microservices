using Common.Domain.DTO.Req;
using Dashboard.Domain.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class WidgetFormulaRequest : WidgetFormulaDTO
    {
        public WidgetSettingsFormulaDTO WidgetSettingsFormula { get; set; } = new WidgetSettingsFormulaDTO();
        public List<WidgetFormulaDetailsDTO> WidgetFormulaDetails { get; set; } = new List<WidgetFormulaDetailsDTO>();

    }

    public class CustFormulaRequest : LoadByIDRequest
    {
        /// <summary>
        /// refer to CustomTrendsEnum
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// refer to CustomAccountTypeEnum
        /// </summary>
        public short SubType { get; set; }

        public long UrlKey {  get; set; }   
    }
    public class CustStatsRequest : CustFormulaRequest
    {
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
        public Int16 IsMonthORDay { get; set; }
        public Int32 GroupID {  get; set; } 
        public Int32 DeptID {  get; set; }  
        //public Int32 UrlKey {  get; set; }
        public Int32 FromulaID {  get; set; }
        public Int32 LYyear { get; set; }

    }
}
