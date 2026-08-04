using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    //public class GraphicalRequest : PerformanceRequest
    //{
    //    /// <summary>
    //    /// to get selected category graphical detailed data 
    //    /// refer to DepartmentsFilterEnum
    //    /// </summary>
    //    public short Category { get; set; }
    //}

    public class TrendsGraphRequest:AnalyticsRequest
    {
        public Int32 Type { get; set; } 
        public Int32 Order {  get; set; }   
        public Int32 StatsType {  get; set; }
        public String CurrencySymbol { get; set; } = string.Empty;
    }


}
