using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class DailySalesMatricesReq
    {        
        public List<string> CorporationIds {  get; set; }
        public DateTime Date { get; set; }
        
    }

    public class OTBOverViewDetReq
    {
        public string CorporationId { get; set; }
        public DateTime? FromDate
        {
            get
            {
                DateTime fromDate;
                return (!string.IsNullOrEmpty(ShortFromDate) && DateTime.TryParse(ShortFromDate, out fromDate)) ? Convert.ToDateTime(ShortFromDate) : DateTime.Today;
            }
        }

        public DateTime? ToDate
        {
            get
            {
                DateTime todate;
                return (!string.IsNullOrEmpty(ShortToDate) && DateTime.TryParse(ShortToDate, out todate)) ? Convert.ToDateTime(ShortToDate) : DateTime.Now;
            }
        }
        public int OTBViewPeriod { get; set; }
        public int OTBGraphMetrics { get; set; }
        public bool BudgetChk { get; set; }
        public bool LYChk { get; set; }
        public string ShortFromDate { get; set; } = string.Empty;
        public string ShortToDate { get; set; } = string.Empty;
    }
    public class OTBPickUpOverViewDetReq
    {
        public string CorporationId { get; set; }
        public DateTime? FromDate
        {
            get
            {
                DateTime fromDate;
                return (!string.IsNullOrEmpty(ShortFromDate) && DateTime.TryParse(ShortFromDate, out fromDate)) ? Convert.ToDateTime(ShortFromDate) : DateTime.Today;
            }
        }

        public DateTime? ToDate
        {
            get
            {
                DateTime todate;
                return (!string.IsNullOrEmpty(ShortToDate) && DateTime.TryParse(ShortToDate, out todate)) ? Convert.ToDateTime(ShortToDate) : DateTime.Now;
            }
        }
        public int OTBViewPeriod { get; set; }
        public int OTBGraphMetrics { get; set; }
        public bool PickUp1DayChk { get; set; }
        public bool PickUp3DaysChk { get; set; }
        public bool PickUp7DaysChk { get; set; }
        public bool PickUp14DaysChk { get; set; }
        public bool PickUp30DaysChk { get; set; }
        public string ShortFromDate { get; set; } = string.Empty;
        public string ShortToDate { get; set; } = string.Empty;
    }

    public class OTBPerformanceReq
    {
        public string CorporationIDLst { get; set; }
        public DateTime? FromDate
        {
            get
            {
                DateTime fromDate;
                return (!string.IsNullOrEmpty(ShortFromDate) && DateTime.TryParse(ShortFromDate, out fromDate)) ? Convert.ToDateTime(ShortFromDate) : DateTime.Today;
            }
        }

        public DateTime? ToDate
        {
            get
            {
                DateTime todate;
                return (!string.IsNullOrEmpty(ShortToDate) && DateTime.TryParse(ShortToDate, out todate)) ? Convert.ToDateTime(ShortToDate) : DateTime.Now;
            }
        }
        public int SortBy { get; set; }
        public int PerformanceVarTyp { get; set; }
        public string ShortFromDate { get; set; } = string.Empty;
        public string ShortToDate { get; set; } = string.Empty;
    }

    public class OTBPickUpReq
    {
        public string CorporationIDLst { get; set; }
        public DateTime? FromDate
        {
            get
            {
                DateTime fromDate;
                return (!string.IsNullOrEmpty(ShortFromDate) && DateTime.TryParse(ShortFromDate, out fromDate)) ? Convert.ToDateTime(ShortFromDate) : DateTime.Today;
            }
        }

        public DateTime? ToDate
        {
            get
            {
                DateTime todate;
                return (!string.IsNullOrEmpty(ShortToDate) && DateTime.TryParse(ShortToDate, out todate)) ? Convert.ToDateTime(ShortToDate) : DateTime.Now;
            }
        }
        public int PickUpOf { get; set; }
        public int ViewBy { get; set; }
        public int PickUpdaysSel { get; set; }
        public string ShortFromDate { get; set; } = string.Empty;
        public string ShortToDate { get; set; } = string.Empty;
    }
}



