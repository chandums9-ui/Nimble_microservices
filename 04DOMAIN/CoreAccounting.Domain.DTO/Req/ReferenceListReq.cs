using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class ReferenceListReq
    {
        public List<ReferenceItem> References { get; set; } = new List<ReferenceItem>();
    }

    public class ReferenceItem
    {
        public string ID { get; set; }
        public ReferenceEnumType Type { get; set; }
    }

    public enum ReferenceEnumType
    {
        Corporation = 1,
        Business = 2,
        BankAccount = 3
    }
}
