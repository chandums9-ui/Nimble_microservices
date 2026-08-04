using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public class NetAmountDTO
    {
        public byte[] ID { get; set; }      
        public decimal Amount { get; set; }
        public short? DeptType { get; set; }
        public bool DebitCredit { get; set; }
        public bool isGuestLedger { get; set; }
    }
}
