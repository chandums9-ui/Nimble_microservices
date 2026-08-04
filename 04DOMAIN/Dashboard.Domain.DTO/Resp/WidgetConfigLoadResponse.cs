using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public  class WidgetConfigLoadResponse
    {
        public string Department {  get; set; } 
        public decimal PercentageOfIncome {  get; set; } 
        public decimal ComparedPercentageOfIncome { get; set; } 
        public decimal Amount {  get; set; }
        public decimal? StatsOrHours { get; set; }
        public decimal ComparedStatsOrHours { get; set; }   
        public decimal ComparedAmount { get; set; }
        public decimal POR { get; set; }
        public decimal PAR { get; set; }
        public decimal ComparedPOR { get; set; }
        public decimal ComparedPAR { get; set; }
    }
}
