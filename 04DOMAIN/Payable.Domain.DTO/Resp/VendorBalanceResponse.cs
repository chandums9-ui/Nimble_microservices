using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class VendorBalanceResponse : StatusDTO
    {
        public decimal Balance { get; set; }
    }
}
