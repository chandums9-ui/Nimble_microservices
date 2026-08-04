using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
     public class MerchantUnReconRespDTO
    {
        public string CorporationId { get; set; }       
        public string TransId { get; set; }
        public int? ReconStatus { get; set; }

    }
}
