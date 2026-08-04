using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class IncomeStatementGroupReq
    {
        public class SubDepartmentsRequest
        {
            public string ClientID { get; set; }
        }

        public class IncomeStatementDepartmentWiseRequest
        {
            public string clientID { get; set; }
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
    }
}
