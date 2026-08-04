using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class IncomeStatementResponse
    {
        public string GroupId {  get; set; }    
        public string GroupName {  get; set; }  
        public string DepartmentName {  get; set; } 
        public IncomeChartOfAccounts ChartOfAccounts { get; set; }
        
    }
    public class IncomeChartOfAccounts
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; } 
    }
}
