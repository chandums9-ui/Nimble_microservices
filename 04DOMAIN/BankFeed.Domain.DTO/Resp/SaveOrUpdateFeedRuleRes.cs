using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Resp
{
    public class SaveOrUpdateFeedRuleRes:StatusDTO
    {
        public long FeedRuleId { get; set; }
    }

}
