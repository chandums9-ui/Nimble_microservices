using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class CashCardRequest : AnalyticsRequest
    {
        public short? CashCardFilter { get; set; }
        public string? ProfitCenterId { get; set; }
        public short DeptType { get; set; }
        public short BasedOn { get; set; }
    }

}
