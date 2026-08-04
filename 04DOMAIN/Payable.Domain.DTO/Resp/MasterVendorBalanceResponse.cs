using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class MasterVendorBalanceResponse
    {
        public string VendorID { get; set; }
        public  decimal Balance { get; set; }
    }
}
