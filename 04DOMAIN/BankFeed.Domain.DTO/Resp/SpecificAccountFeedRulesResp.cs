using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Resp
{
    public class SpecificAccountFeedRulesResp : StatusDTO
    {
        public List<FeedRuleResponses> Rules { get; set; }
    }

    public class FeedRuleResponses
    {
        public long FeedRuleId { get; set; }
        public string FeedTransactionType { get; set; }   // Payments / Receipts

        public string QueryMatchType { get; set; }
        public string RuleID { get; set; }
        public string RuleCategory { get; set; }
        public int TransactionType { get; set; }
        public string NameID { get; set; }
        public List<RuleConditionResponse> Conditions { get; set; }
    }

    public class RuleConditionResponse
    {
        public string FieldName { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
    }

    public class SpecificAccountFeedRulesDbResp
    {
        public long FeedRuleId { get; set; } 
        public string FeedTransactionType { get; set; }
        public string QueryMatchType { get; set; }
        public string RuleID { get; set; }
        public string RuleCategory { get; set; }
        public string FieldName { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
        public int TransactionType { get; set; }
        public string NameID { get; set; }

    }
}
