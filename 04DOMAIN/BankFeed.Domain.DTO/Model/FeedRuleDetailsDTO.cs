using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedRuleDetailsDTO : FeedRuleDTO
    {
        public FeedRuleDetailsDTO() { QueryMatchType = 1; AmountType = 0; }

        /// <summary>
        /// Feedrule query conditions
        /// </summary>
        public List<FeedRuleDivisinDTO> Details { get; set; } = new List<FeedRuleDivisinDTO>(); // query details
        /// <summary>
        /// Feedrule applied, posted details
        /// </summary>
        public FeedRuleMappingDTO MapDetails { get; set; } = new FeedRuleMappingDTO();
        public int FeedTransactionsCount { get; set; }
    }
}
