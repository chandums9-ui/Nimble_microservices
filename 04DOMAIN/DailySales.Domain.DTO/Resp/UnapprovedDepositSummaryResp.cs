using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class UnapprovedDepositSummaryResp : StatusDTO
    {
        public int UnapprovedCount { get; set; }
        public decimal UnapprovedTotal { get; set; }
    }
}
