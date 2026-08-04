using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillPaymentBalanceAndNewCheckNumber
    {
        public decimal Balance { get; set; }
        public string newChkNo { get; set; }
    }
}
