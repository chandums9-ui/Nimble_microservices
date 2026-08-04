using Common.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{
    public class FundTransferRequest: FundTransferDTO
    {
        public bool? IsMultiplePost { get; set; } = false;
        public bool HasAttachments { get; set; } = false;
        public string ClientID {  get; set; }
    }
    
}
