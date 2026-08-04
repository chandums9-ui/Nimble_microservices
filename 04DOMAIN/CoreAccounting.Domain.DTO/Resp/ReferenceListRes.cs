using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class ReferenceListRes : StatusDTO
    {
        public List<ReferenceItems> List { get; set; } = new List<ReferenceItems>();
    }

    public class ReferenceItems
    {
        public string ID { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
    }

    public class LoadDefaultCorporationAccounts : StatusDTO
    {
        public List<DefaultCorpAccountsList>? ListCorp { get; set; }
    }

    public class DefaultCorpAccountsList
    {
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public bool IsPrintBlankCheck { get; set; }
    }
}
