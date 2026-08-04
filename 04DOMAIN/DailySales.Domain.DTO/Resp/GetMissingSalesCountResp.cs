using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class GetMissingSalesCountResp : StatusDTO
    {
        public List<DateTime> SalesDates { get; set; }
        public int SalesCount { get; set; }
    }

    public class MissingSaleDbResp
    {
        public DateTime MissingDate { get; set; }
    }
}
