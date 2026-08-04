using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Payable.Domain.DTO.Resp
{
    public class VendorTransactionListResponse
    {
        public List<VendorTransactionDetails> VendorTransactionDetails { get; set; } 
    }
}
