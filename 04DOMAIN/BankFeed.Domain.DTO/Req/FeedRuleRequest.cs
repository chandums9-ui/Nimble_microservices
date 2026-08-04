using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Model;

namespace BankFeed.Domain.DTO.Req
{
    /// <summary>
    /// FeedRule Save and update request model
    /// </summary>
    public class FeedRuleRequest:FeedRuleDetailsDTO
    {
        public List<AccountDTO> CloneNimbleAccounts { get; set; }

    }
    public class FeedRuleRequest1
    {
        public FeedRuleDetailsDTO FeedRuleDTO { get; set; }
        public List<AccountDTO> CloneNimbleAccounts { get; set; }

    }
    public class MultipleFeedRuleRequest
    { 
        public List<FeedRuleRequest> Feedrule { get; set; }
    }

    /// <summary>
    ///For Feedrule clone SQl parameters for the Stored Procedures
    /// </summary>
    public class CloneRequest
    {
        public long FeedRuleID { get; set; }
        public string BankOrCreditAccountID { get; set; }
        public string CorpID { get; set; }
        public int Priority { get; set; }
    }

    public class RuleSpecificAccountReq
    {
        public string CorpID { get; set; }
    }
}