using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
    public class FeedSynchRequest: FeedSyncDTO, IModelBaseClientID
    {
        public string ClientID { get; set; }
        public long ClientInfoID { get; set; }
    }
}
