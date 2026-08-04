using Common.Domain.DTO.Model;
using Payable.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class ContractListResponse
    {
        public List<ContractDTO> Contracts { get; set; }
    }
}
