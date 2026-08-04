using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class DepositAndBalanceSummaryResp : StatusDTO
    {
        public decimal AdjustmentOpeningBalance { get; set; }
        public decimal PendingExcessShortageTotal { get; set; }
        public decimal PESWithoutAdjBal { get; set; }
        public decimal PendingDepositTotal { get; set; }
        public string AsOfDate { get; set; }
    }
    public class DepositAndBalanceSummaryDbResp
    {
        public decimal AdjustmentOpeningBalance { get; set; }
        public decimal PendingExcessShortageTotal { get; set; }
        public decimal PESWithoutAdjBal {get; set;}
        public decimal PendingDepositTotal { get; set; }
        public string AsOfDate { get; set; }
    }


}
