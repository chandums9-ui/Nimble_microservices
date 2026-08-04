using System;

namespace Dashboard.Domain.DTO.Req
{
    public class DepartmentIncomeRequest
    {
        public List<string> Corporations { get; set; }

        public List<string> AllCorporations { get; set; }

        public int ComparisonType { get; set; }

        public int OrderBy { get; set; }

        /// <summary>
        /// It is readonly, ShortFromDate will be converted to DateTime and given back other wise it returns DateTime.Today
        /// </summary>
        public DateTime FromDate
        {
            get
            {
                return (!string.IsNullOrEmpty(ShortFromDate) && DateTime.TryParse(ShortFromDate, out _)) ? Convert.ToDateTime(ShortFromDate) : DateTime.Today;
            }
        }

        /// <summary>
        /// It is readonly, ShortToDate will be converted to DateTime and given back other wise it returns DateTime.Now
        /// </summary>
        public DateTime ToDate
        {
            get
            {
                return (!string.IsNullOrEmpty(ShortToDate) && DateTime.TryParse(ShortToDate, out _)) ? Convert.ToDateTime(ShortToDate) : DateTime.Now;
            }
        }

        /// <summary>
        /// Send short date string format, this will be converted in API to Date time frmat(PerformanceRequest.FromDate)
        /// </summary>
        public string ShortFromDate { get; set; }

        /// <summary>
        /// Send short date string format, this will be converted in API to Date time frmat(PerformanceRequest.ToDate)
        /// </summary>
        public string ShortToDate { get; set; }
    }
}
