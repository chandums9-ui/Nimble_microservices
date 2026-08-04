using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Resp
{
    public class ConnectionStatus : StatusDTO
    {
        public int ConnStatus { get; set; }
        public long ConnID { get; set; }
        public short ProviderStatus { get; set; }
        public long FeedAccId { get; set; }
        public long transCount { get; set; }
        public DateTime? LastSynchedOn { get; set; }
        public DateTime? TransMinDate { get; set; }
        public decimal AccountBal { get; set; }

        public decimal AccountAvailBal { get; set; }
        public short ProviderType { get; set; }

        public long ProviderRegID { get; set; }

    }
}
