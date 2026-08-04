using BankFeed.Domain.Enums;
using Common.Domain.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class TransactionFilters : DateFilter
    {
        /// <summary
        /// FeedStatusFilterEnum
        /// 0/1/2- Confirmed/Pending/both
        /// </summary>
        public short FeedsStatus { get; set; } = (short)FeedStatusFilterEnum.BothFeeds;

        /// <summary>
        /// PostTypeFilterEnum
        /// 0/1/2-All/Payments/Reciepts
        /// </summary>
        public short PostType { get; set; } = Convert.ToInt16(PostTypeFilterEnum.All);

        /// <summary>
        /// StatusTypeFilterEnum
        /// 0-open transactions
        /// 1-all and posted transactions
        /// 2-possible matches
        /// 3-only Posted
        /// 4-Bills Matched
        /// 5-RuleApplied
        /// 6-Unassigned
        /// 7-Ignored
        /// </summary>
        public short StatusType { get; set; } = Convert.ToInt16(StatusTypeFilterEnum.openfeeds);
    }

    /// <summary>
    /// Has Date Filter settings
    /// </summary>
    public class DateFilter
    {
        /// <summary>
        /// DateFilterEnum
        /// 0/1/2-range/Today/Yesterday
        /// </summary>
        public string DateFilterType { get; set; }= Convert.ToString(DateFilterEnum.Range);

        /// <summary>
        /// Dates are only given when DateFilterType="Range"
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Dates are only given when DateFilterType="Range"
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Send short date string format, this will be converted in API to Date time format
        /// </summary>
        public string ShortFromDate { get; set; } = string.Empty;

        /// <summary>
        /// Send short date string format, this will be converted in API to Date time format
        /// </summary>
        public string ShortToDate { get; set; } = string.Empty;

    }


}
