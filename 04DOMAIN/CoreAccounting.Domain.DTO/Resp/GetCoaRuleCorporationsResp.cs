using Common.Domain.DTO.Model.Base;
using CoreAccounting.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{

    public class GetCoaRuleCorporationsListResp : StatusDTO
    {
        public bool IsClone { get; set; }
        public List<GetCoaRuleCorporationsResp> Data { get; set; }
    }

    public class GetCoaRuleCorporationsResp
    {

        public string CorporationId { get; set; }
        public string CorporationName { get; set; }
    }

    public class GetCoaRuleCorporationsDbResp
    {
        public bool IsClone { get; set; }   
        public string CorporationId { get; set; }
        public string CorporationName { get; set; }
    }

}
