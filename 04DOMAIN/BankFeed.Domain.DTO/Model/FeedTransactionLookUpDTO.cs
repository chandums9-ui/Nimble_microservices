using Common.Domain.DTO.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedTransactionLookUpDTO
    {
        /// <summary>
        /// StatusTypeFilterEnum
        /// </summary>
        public List<KeyValuePairObject<int, string>> StatusTypes { get; set; }

        /// <summary> 
        /// PostTypeFilterEnum
        /// </summary>
        public List<KeyValuePairObject<int, string>> PostTypes { get; set; }

        public AccountSummaryDTO AccountSummary { get; set; }
    }
}
