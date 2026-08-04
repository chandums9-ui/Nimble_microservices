using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedRuleViewDTO : FeedRuleDTO
    {
        public string ApplicableAccountID { get; set; }
        public string MatchedQueryName
        {
            get
            {
                if (QueryMatchType == 1)
                {
                    return "AllLines";
                }
                else if (QueryMatchType == 2)
                {
                    return "Single Line";
                }
                else
                {
                    return null;
                }
            }
        }
        public string NimAccName { get; set; } = string.Empty;
        public string RuleQuery { get; set; }
        public string QueryStatusMsg
        {
            get
            {
                switch (AutoApplyEnable)
                {
                    case true:
                        return "Automatic";
                    case false:
                        return "Manual";

                }
            }
        }
        public string RuleCreatedStatus
        {
            get
            {
                switch (RuleStatus)
                {
                    case 1:
                        return "Direct Master";
                    case 2:
                        return "Bank Feeds";
                    default:
                        return string.Empty;
                }
            }
        }

    }
}
