using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class GetCoaRuleCorporationsReq
    {
        public string CorporationId { get; set; }
        public string AccountType { get; set; }
        public decimal? AccountCode { get; set; }

    }
}
