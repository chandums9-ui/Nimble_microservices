using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class ProfitandLossResponse : StatusDTO
    {
        public List<ProfitandLossDetails> profitandLossDetails { get; set; } = new List<ProfitandLossDetails>();

    }
    public class ProfitandLossDetails : CorpNames
    {
        public Int64 CorpKey { get; set; }
        public decimal CurrentIncome { get; set; }
        public decimal LYIncome { get; set; }
        public decimal BudgetIncome { get; set; }
        public decimal CurrentCOGS { get; set; }
        public decimal LYCOGS { get; set; }
        public decimal BudgetCOGS { get; set; }
        public decimal CurrentGrossIncome { get; set; }
        public decimal LYGrossIncome { get; set; }
        public decimal BudgetGrossIncome { get; set; }
        public decimal CurrentExpense { get; set; }
        public decimal LYExpense { get; set; }
        public decimal BudgetExpense { get; set; }
        public decimal CurrentOtherIncome { get; set; }
        public decimal LYOtherIncome { get; set; }
        public decimal BudgetOtherIncome { get; set; }
        public decimal CurrentNetIncome { get; set; }
        public decimal LYNetIncome { get; set; }
        public decimal BudgetNetIncome { get; set; }
        public decimal CurrentGOP { get; set; }
        public decimal LYGOP { get; set; }
        public decimal BudgetGOP { get; set; }
        public decimal CurrentGOPPERCENT { get; set; }
        public decimal LYGOPPERCENT { get; set; }
        public decimal BudgetGOPPERCENT { get; set; }

    }

}
