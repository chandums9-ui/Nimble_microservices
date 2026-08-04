using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dashboard.Domain.DTO.Resp
{
    public class ReportsDashboardResponse : StatusDTO
    {
        public class FinancialDashboardResponse
        {
            public List<MetricResponse>? Metrics { get; set; }
            public List<CashPositionResponse>? CashPositions { get; set; }
            public List<DepartmentFinancialResponse>? Departments { get; set; }
        }

        public class MetricResponse
        {
            public int Order { get; set; }
            public string MetricName { get; set; }
            public int CorpKey { get; set; }
            public decimal ActualValue { get; set; }
            public decimal Budget { get; set; }
            public decimal LY { get; set; }
        }

        public class CashPositionResponse
        {
            public int Month { get; set; }
            public string MonthName { get; set; }
            public decimal CashPositionInBank { get; set; }
        }

        public class DepartmentFinancialResponse
        {
            public long DeptID { get; set; }
            public string DepartmentName { get; set; }
            public int DeptType { get; set; }
            public int Type { get; set; }
            public decimal ActualRevenue { get; set; }
            public decimal ActualExpense { get; set; }
            public decimal LYRevenue { get; set; }
            public decimal LYExpense { get; set; }
            public decimal BudgetRevenue { get; set; }
            public decimal BudgetExpense { get; set; }
        }

    }
}
