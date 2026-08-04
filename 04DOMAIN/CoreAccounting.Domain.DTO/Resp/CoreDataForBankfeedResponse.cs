using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class CoreDataForBankfeedResponse : IStatusDTO
    {
        public string CacheKey { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
    }
    public class CorporationMappings
    {
        public List<byte[]> AccountIds { get; set; } = new();
        public byte[] PayeeId { get; set; }
        public short PayeeType { get; set; }
        public List<SplitPurposeResult> Rows { get; set; } = new();
    }
    public class SplitPurposeResult
    {
        public byte[] SplitAccountId { get; set; }
        public byte[] PurposeId { get; set; }
        public short SplitLineNo { get; set; }
    }

    public class CorporationNameMappings
    {
        public List<string> AccountNames { get; set; } = new();
        public string PayeeName { get; set; }

        public List<SplitPurposeNameResult> Rows { get; set; } = new();
    }

    public class SplitPurposeNameResult
    {
        public string SplitAccountName { get; set; }

        public string PurposeName { get; set; }
        public short SplitLineNo { get; set; }
    }
    public class TwoBinaryDataRes
    {
        public List<TwoBinaryData> res { get; set; }
    }

    public class TwoBinaryData
    {
        public byte[] Binary1 { get; set; }
        public byte[] Binary2 { get; set; }
    }

    public class EmployeePayeeNameRes
    {
        public byte[] CorporationId { get; set; }

        public byte[] PayeeId { get; set; }

        public string PayeeName { get; set; }
    }
    // Replace the existing GetNamesFromIdsCacheResponse in CoreDataForBankfeedResponse.cs

    public class GetNamesFromIdsCacheResponse : IStatusDTO
    {
        /// <summary>
        /// Head account names — flat list, same order as the original head account IDs.
        /// If 4 head accounts were selected: ["CASH", "BANK", "REVENUE", "EXPENSE"].
        /// Same name across all corps — resolved once from any single corp.
        /// </summary>
        public List<string> HeadAccountNames { get; set; } = new();

        /// <summary>
        /// Split account + purpose names, ordered by SplitLineNo.
        /// One entry per split row — same names across all corps.
        /// </summary>
        public List<SplitNameResult> Splits { get; set; } = new();

        /// <summary>
        /// Single payee name — same payee applies to all corps in this rule.
        /// Null if no payee was configured.
        /// </summary>
        public string PayeeName { get; set; }

        public string Status { get; set; }
        public int StatusCode { get; set; }
    }

    public class SplitNameResult
    {
        public string SplitAccountName { get; set; }
        public string PurposeName { get; set; }

        /// <summary>
        /// 1-based split position — matches the SplitLineNo stored in FeedRuleMapping.
        /// UI uses this to correlate names back to the correct split row.
        /// </summary>
        public int SplitLineNo { get; set; }
    }

    public class AccountList
    {
        public string AccountName { get; set; }
        public string AccountId { get; set; }
    }

    public class MapAccountsResponse : IStatusDTO
    {

        public List<AccountList> res { get; set; } = new();
        public string Status { get; set; }
        public int StatusCode { get; set; }
    }
}
