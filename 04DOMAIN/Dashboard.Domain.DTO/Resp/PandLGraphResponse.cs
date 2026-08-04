using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dashboard.Domain.DTO.Resp.PandLGraphResponse;

namespace Dashboard.Domain.DTO.Resp
{
    public class PandLGraphResponse : StatusDTO
    {
        public decimal TotalIncome { get; set; }        
        public decimal TotalNetIncome { get; set; }       
        public decimal TotalExpense { get; set; }

        public List<KeyValuePairObject<string, PandlStatistics>> PandLReports {  get; set; } = new List<KeyValuePairObject<string, PandlStatistics>>();

        public class PandlStatistics
        {
            public decimal Income { get; set; }
            //public decimal COGS { get; set; }
            public decimal NetIncome { get; set; }
            //public decimal GrossProfit { get; set; }
            public decimal Expense { get; set; }
           // public decimal otherIncomeExpense { get; set; }
        }
    }

    public class PandldbResponse 
    {
        public string MonthName {  get; set; }
        public decimal Expense {  get; set; }
        public decimal NetIncome {  get; set; }
        public decimal Income { get; set; }
        public int Month { get; set; }
    }
    
    public class pandlMobiledbResponse : PandldbResponse
    {
        public decimal COGS { get; set; }
    }
   
    public class PandlMobileResponse: StatusDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalNetIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalCogs { get; set; }
        public List<KeyValuePairObject<string, PandlMobileStats>> PandLReportsData { get; set; } = new List<KeyValuePairObject<string, PandlMobileStats>>();
        public class PandlMobileStats : PandlStatistics
        {
            public decimal Cogs { get; set; }

        }
    }

}
