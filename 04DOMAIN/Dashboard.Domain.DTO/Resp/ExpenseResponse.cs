using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Dashboard.Domain.DTO.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class ExpenseResponse : StatusDTO
    {
        public ExpenseResponse()
        {
            Expenses = new List<ExpenseAnalysis>();
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<ExpenseAnalysis> Expenses { get; set; }
    }
    public class ExpenseAnalysis : CorporationInfo
    {
        //public string CorpID { get; set; }
        //public string CorpName { get; set; }
        public Expense Payroll { get; set; }
        public Expense Utilities { get; set; }
        public Expense RoomExpense { get; set; }
        public Expense FandBExpense { get; set; }
        public Expense FranchiseFee { get; set; }
        public Expense ChargeBacks { get; set; }
    }
    public class Expense
    {
        public decimal  Current { get; set; }
        public decimal  Budget { get; set; }
        public decimal  LY { get; set; }
        public decimal ComparisonValue { get; set; }
    }
}
