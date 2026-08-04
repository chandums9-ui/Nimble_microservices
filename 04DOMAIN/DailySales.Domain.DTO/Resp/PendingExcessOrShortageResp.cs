using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class PendingExcessOrShortageResp : StatusDTO
    {
        public decimal OpeningBalance { get; set; }
        public decimal? PendingExcessOrShortageATD { get; set; }
        public List<DepositsData> DepositData { get; set; }
    }

    public class DepositsData
    {
        public DateTime SalesDate { get; set; }
        public string Comments { get; set; }
        public List<Lines> Lines { get; set; }
    }
    public class Lines
    {
        public string LineName { get; set; }
        public long LineOrder { get; set; }
        public string LineId { get; set; }
        public decimal Amount { get; set; }
        public decimal DepositAmount { get; set; }

    }

    public class PendingExcessOrShortageDbResp
    {
        public DateTime SaleDate { get; set; }
        public string LineName { get; set; }
        public long LineOrder { get; set; }
        public string LineId { get; set; }
        public decimal Amount { get; set; }
        public decimal DepositAmount { get; set; }
        public string Comments { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? PendingExcessOrShortageATD { get; set; }

    }
}

