using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class CardDataDto
    {
        public  string CardName { get; set; }
        public int CardDataType { get; set; }
        public string TypeName { get; set; }
        public long TypeCount { get; set; }
        public decimal Balance { get; set; }
    }
}
