using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class DepositMenuInfoResp : StatusDTO
    {
        public List<DateTime> SaleDates { get; set; }
    }

    public class DepositSaleDatesDbResp
    {
        public DateTime SaleDate { get; set; }
    }
}
