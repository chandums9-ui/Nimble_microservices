using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
    public  class BFeed_AttentionRequiredRequest
    {
        public string CorpIDKey { get; set; }
        public string CorporationId { get; set; }
    }
    public class BFeed_MapAccountRequest
    {
        public string CorporationId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountID { get; set; }
        public long FeedAccountID { get; set; }
    }
    public class BFeed_RemoveMapAccountRequest
    {
        public List<FeedAccountIDlist> res { get; set; }

        public bool IsfromMapped { get; set; }
    }
    public class FeedAccountIDlist
    {
        public long FeedAccountID { get; set; }
    }

}
