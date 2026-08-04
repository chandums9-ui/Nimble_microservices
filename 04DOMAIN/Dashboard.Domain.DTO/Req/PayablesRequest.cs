using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class PayablesRequest : AnalyticsRequest//, IModelBaseClientID
    {
        public string GroupID { get; set; }
        public string Client { get; set; }

    }
}
