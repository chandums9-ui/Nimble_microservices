using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Resp
{
    public class FeedRuleViewGridRes:StatusDTO
    {
        public int TotalRecords { get; set; }
        public int RowsInPage { get; set; }
        public List<FeedDetails> Entries { get; set; } = new();
    }
    public class FeedDetails
    {
        public long FeedRuleID { get; set; }
        public string AccountType { get; set; }
        public string RuleCategory { get; set; }
        public string RuleID { get; set; }
        public string RuleName { get; set; }
        public int NoOfCorporation { get; set; }
        public int AccountsLinked { get; set; }
        public string RuleDescription { get; set; }
        public string RuleCreateType { get; set; }
        public int RulePostLogic { get; set; }
        public bool IsEditAccess { get; set; }
        public int TotalRecords { get; set; }
        public int RowsInPage { get; set; }
    }

}
