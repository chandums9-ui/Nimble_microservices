using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public  class MerchantUnReconReq
    {
        public string CorporationId { get; set; }
        public string? PCID { get; set; }
        public string AccountId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public List<int> BatchTypes {  get; set; } 

        public List<int> CardTyeps { get; set; }
    }
}


