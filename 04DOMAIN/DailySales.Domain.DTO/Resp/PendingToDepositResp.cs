using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class PendingToDepositResp:StatusDTO
    {
        public decimal Amount { get; set; }
    }
    public class PendingToDepositDbResp
    {
        public decimal TotalAmount { get; set; }
    }

}

