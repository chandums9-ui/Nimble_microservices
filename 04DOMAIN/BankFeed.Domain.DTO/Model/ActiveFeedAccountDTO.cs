using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class ActiveFeedAccountDTO : FeedAccountMappingDTO, IModelBaseIDInt64, IModelBaseCorporationID
    {
        public string account_id { get; set; }
        public long ProviderRegID { get; set; }
        public long InstID { get; set; }
        public string ProvicerAccessID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
    }
}
