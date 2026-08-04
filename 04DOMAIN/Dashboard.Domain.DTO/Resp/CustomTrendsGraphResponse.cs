using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class CustomTrendsGraphResponse
    {
        public string MonthName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalLYAmount { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal TotalForecast { get; set; }
        public int SubType { get; set; }
        public decimal Current { get; set; } = 0.0M;
        public decimal Budget { get; set; } = 0.0M;
        public decimal LY { get; set; } = 0.0M;
        public decimal Forecast { get; set; } = 0.0M;
        public Int32 Type { get; set; }

        public long FromulaID { get; set; }
    }
}
