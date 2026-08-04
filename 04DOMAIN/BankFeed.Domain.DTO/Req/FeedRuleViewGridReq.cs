using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
    public class FeedRuleViewGridReq
    {
        public string CorpId { get; set; }
        public string CorpIDKey { get; set; }
        public bool TransactionType { get; set; }
        public bool? isbankOrCreditAccount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
