using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
     public class MerchanatReconCOAReq
    {
        public string CorporationId { get; set; }
        public string? Pcid { get; set; }
        public string AccountId { get; set; }
        public string AdjustmentAccountId { get; set; }
        public string ExcessMerchantAccountId {  get; set; }    
        public string ChargeBackAccountId { get; set; }

    }
    
    public class MerchantCOApreferenceResponse: MerchanatReconCOAReq, IStatusDTO
    {
        public int StatusCode { get; set ; }
        public string Status { get; set; }

       
    }




}
