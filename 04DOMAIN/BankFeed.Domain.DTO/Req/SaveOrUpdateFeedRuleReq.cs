using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
    public class SaveOrUpdateFeedRuleReq
    {
        public string CorpAccountcachekey { get; set; }
        public long? FeedRuleId { get; set; }
        public bool TransactionType { get; set; }
        public bool IsbankOrCreditAccount { get; set; }
        public int RuleCategoryId { get; set; }
        public string RuleId { get; set; }
        public string RuleName { get; set; }
        public short ApplyLines { get; set; }
        public List<string> CorporationIds { get; set; }
        public List<string> BankAccountNames { get; set; }
        public List<string> BankAccountIds { get; set; }
        public List<FeedRuleConditionsRequest> Conditions { get; set; }
        public AssignRuleRequest AssignRule { get; set; }
        public OppositeRuleRequest OppositeRule { get; set; }
    }

    public class FeedRuleConditionsRequest
    {
        public long? FeedRuleDetailId { get; set; }
        public int FieldName { get; set; }
        public int Condition { get; set; }
        public string Value { get; set; }
    }

    public class AssignRuleRequest
    {
        public int TransactionType { get; set; }
        public bool IsPostBill { get; set; }
        public string PayeeId { get; set; }
        public int? PayeeType { get; set; }
        public string PayeeName { get; set; }   // populated by Core via IdsCacheKey on load
        public List<RuleSplitRequest> Splits { get; set; }
        public string SplitAccountType { get; set; }
    }

    public class RuleSplitRequest
    {
        public long? FeedRuleMappingId { get; set; }
        public int SplitNo { get; set; }
        public bool SplitType { get; set; }
        public decimal SplitValue { get; set; }
        public string DepartmentId { get; set; }
        public string PurposeId { get; set; }
        public string Purposename { get; set; }   // populated by Core via IdsCacheKey on load
        public string AccountName { get; set; }   // populated by Core via IdsCacheKey on load
        public string AccountId { get; set; }
        public string SplitAccountType { get; set; }
    }

    public class OppositeRuleRequest
    {
        public string CorporationId { get; set; }
        public string AccountId { get; set; }
        public string PayeeId { get; set; }
        public int PayeeType { get; set; }
        public List<RuleSplitRequest> Splits { get; set; }
    }

    public class LoadFeedRuleDataReq
    {
        public long? FeedRuleId { get; set; }
        // CacheKey removed — names are now loaded via IdsCacheKey through Core,
        // not passed in as a pre-existing cache key.
    }

    public class LoadFeedRuleDataResp : StatusDTO
    {
        public SaveOrUpdateFeedRuleReq Data { get; set; }

        /// <summary>
        /// Cache key pointing to a Dictionary&lt;string, FeedRuleCacheData&gt; stored in Redis
        /// by GetFeedRuleDetails. The UI sends this to Core's GetNamesFromIdsCacheKey endpoint
        /// to get account names, purpose names, and payee names for all corporations.
        /// Key format: "BankFeed|RuleLoadIds|{Guid}" — hex corp ID is the dictionary key.
        /// </summary>
        public string IdsCacheKey { get; set; }
    }

    public class CategoryReq
    {
        public bool ParentCategoryType { get; set; }
        public bool IsBankOrCreditType { get; set; }
    }

    public class CategoryResp : StatusDTO
    {
        public List<string> CategoryNames { get; set; } = new List<string>();
    }

    public class UniqueRuleIdRes : StatusDTO
    {
        public string RuleID { get; set; }
    }

    /// <summary>
    /// Resolved IDs stored by Core in Redis (save direction: name → ID).
    /// ALSO used by BankFeed to store IDs in Redis (load direction: ID → name).
    /// The format is symmetric — Core's GetNamesFromIdsCacheKey reads this same
    /// structure regardless of which direction wrote it.
    ///
    /// Property names must match Core's CorporationMappings exactly for JSON
    /// round-trip to work (HeadAccountIds, Splits, PayeeId, PayeeType are the contract).
    /// </summary>
    public class FeedRuleCacheData
    {
        public List<byte[]> AccountIds { get; set; } = new();
        public List<FeedRuleSplitCacheData> Rows { get; set; } = new();
        public byte[] PayeeId { get; set; }

        /// <summary>
        /// Required for load direction so Core knows which table to look up payee name from.
        /// PayeeType 0/1 = Business table, 2/13 = Employee table.
        /// Stored by BankFeed's GetFeedRuleDetails from FeedRuleMapping.NameType.
        /// </summary>
        public short PayeeType { get; set; }
    }

    /// <summary>
    /// Resolved IDs for one split row.
    /// SplitLineNo is 1-based — set by Core when saving, read by BankFeed when loading.
    /// </summary>
    public class FeedRuleSplitCacheData
    {
        public byte[] SplitAccountId { get; set; }
        public byte[] PurposeId { get; set; }

        /// <summary>
        /// 1-based split position. Stored in FeedRuleMapping when saving.
        /// Preserved in this cache so splits always return in the correct configured order.
        /// </summary>
        public int SplitLineNo { get; set; }
    }

    /// <summary>
    /// Resolved names returned by Core's GetNamesFromIdsCacheKey.
    /// One entry per corporation (dictionary key = hex corp ID).
    /// </summary>
    public class FeedRuleNameCacheData
    {
        public List<string> HeadAccountNames { get; set; } = new();
        public List<FeedRuleSplitNameCacheData> Splits { get; set; } = new();

        /// <summary>
        /// Resolved payee name for this corporation.
        /// Null if no payee was configured or name could not be resolved.
        /// </summary>
        public string PayeeName { get; set; }
    }

    /// <summary>
    /// Resolved names for one split row.
    /// SplitLineNo mirrors the value in FeedRuleSplitCacheData so UI can correlate
    /// names to the correct split positions in the rule form.
    /// </summary>
    public class FeedRuleSplitNameCacheData
    {
        public string SplitAccountName { get; set; }
        public string PurposeName { get; set; }

        /// <summary>
        /// 1-based split position — matches FeedRuleSplitCacheData.SplitLineNo.
        /// </summary>
        public int SplitLineNo { get; set; }
    }

    public class NameAndTypeRes : StatusDTO
    {
        public List<NameAndType> Names { get; set; }
    }

    public class NameAndType
    {
        public string Name { get; set; }
        public short Type { get; set; }
    }
}