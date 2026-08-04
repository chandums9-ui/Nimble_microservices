using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{

    public class JournalResponse : StatusDTO
    {
        public string? ID { get; set; }
        public long? InfoID { get; set; }

        /// <summary>
        /// Bank feed Split transaction id and related journal transaction id & details 
        /// </summary>
        public FeedTransactionSplitDataDto FeedTransactionInfo { get; set; }

    }
    public class JournalEntryResponse : JournalEntryBaseDTO, IStatusDTO
    {
        public List<ApprovalCommentsDTO> Comments { get; set; }
        public List<AttachmentDTO> Attachments { get; set; }
        public List<ARToolTipDetails> ARToolTipDetails { get; set; }
        public bool IsAREntry { get; set; }
        public bool IsAPEntry { get; set; }
        public string AuditLink { get; set; }
        public string PrintEntryLink { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public bool IsUseTaxEnabled { get; set; }
        public bool HasLoginUserAccess { get; set; }
        public string COAOpeningBalId { get; set; }
        public bool IsDsCashCheckEntry { get; set; }
        public string CorpLegalName { get; set; }
        /// <summary>
        /// Recurring data while coming from recurring list edit
        /// </summary>
        public RecurringDTO RecurringData { get; set; }
        public bool IsDualBrand { get; set; }
        public bool IsMerchantReconciliation { get; set; }
        public bool IsFromDeposit { get; set; }
    }
    public class DsCashChecksCountDto
    {
        public int EntriesCount { get; set; }
    }

    public class ARToolTipDetails
    {
        public DateTime? EntryReceivedDate { get; set; }
        public decimal? AmountReceived { get; set; }
        public string JEID { get; set; }
    }
    public class JEListResponse : ModelBaseHeaderDTO, IStatusDTO
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public List<JournalModelDTO> Journals { get; set; } = new List<JournalModelDTO>();
        public List<EntrySummaryDTO> Summary { get; set; } = new List<EntrySummaryDTO>();
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public DateTime? MinEntryDate { get; set; }
        public DateTime? MaxEntryDate { get; set; }
    }
    public class EntrySummaryDTO
    {
        public int JournalType { get; set; }
        public string TypeName { get; set; }
        public long TypeCount { get; set; }
        public decimal Balance { get; set; }
    }
    public class JournalModelDTO
    {
        public string ID { get; set; }
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string EntryNumber { get; set; }
        public DateTime EntryDate { get; set; }
        public short EntryType { get; set; }
        public string EntryTypeDesc { get; set; }
        public decimal Amount { get; set; }
        /// <summary>
        /// JournalEntryStatus
        /// </summary>
        public int StatusID { get; set; }
        private string _status = string.Empty;
        public string Status //{ get; set; }
        {
            get
            {
                if (!ApprovalStatus.HasValue || (ApprovalStatus.HasValue && ApprovalStatus == (short)EntryApprovalStatus.Approved))
                {
                    if (this.StatusID == (short)JournalEntryStatus.Active)
                        return "Approved";
                    if (this.StatusID == (short)JournalEntryStatus.Void)
                        return JournalEntryStatus.Void.GetDisplayName();
                    else if (this.StatusID == (short)JournalEntryStatus.Delete)
                        return JournalEntryStatus.Delete.GetDisplayName();
                    if (this.StatusID == (short)JournalEntryStatus.InActive)
                        return JournalEntryStatus.InActive.GetDisplayName();
                    else
                        return this.ApprovalStatusDesc;
                }
                else
                {
                    return this.ApprovalStatusDesc;
                }
            }
            set { _status = value; }
        }
        /// <summary>
        /// To display Approve/Verification action buttons in ViewGrid
        /// </summary>
        public bool IsPayAccess { get; set; } = true;
        /// <summary>
        /// To display Approve/Verification action buttons in ViewGrid
        /// </summary>
        public string Pay { get; set; }
        public bool HasAttachments { get; set; }
        public bool IsLock { get; set; }
        public bool IsHold { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
        public bool IsVoid { get; set; }
        public bool IsReconciled { get; set; }
        //public string ReconciliationDate { get; set; }
        //public string ReconciliationToolTip { get; set; }
        public string? ClearedDate { get; set; }
        //public int ApprovalOrder { get; set; }
        public short? ApprovalType { get; set; }
        public short? ApprovalLevel { get; set; }
        public short? ApprovalStatus { get; set; }
        public string ApprovalStatusDesc
        {
            get
            {
                if (!ApprovalStatus.HasValue)
                    return string.Empty;
                else
                {
                    if (ApprovalStatus == (short)EntryApprovalStatus.Entry && !string.IsNullOrEmpty(AssignedToUserId))
                        return "In Verification";
                    else if (ApprovalStatus == (short)EntryApprovalStatus.Verification || (ApprovalStatus == (short)EntryApprovalStatus.Entry && string.IsNullOrEmpty(AssignedToUserId)))
                        return !string.IsNullOrEmpty(AssignedToUserId) ? "In Verification" : "In Approval";
                    else if (ApprovalStatus == (short)EntryApprovalStatus.InApproval)
                        return "In Approval";
                    else if (ApprovalStatus == (short)EntryApprovalStatus.Rejected)
                        return "Rejected";
                    else if (ApprovalStatus == (short)EntryApprovalStatus.UnApproved)
                        return "Un-Approved";
                    else if (ApprovalStatus == (short)EntryApprovalStatus.Approved)
                        return (IsVoid) ? "Voided" : "Approved";
                    else
                        return "Unknown Status";// string.Empty;
                }
            }
        }
        public string PreviousApprovalUserID { get; set; }
        public string PreviousApprovalUserName { get; set; }
        public bool IsEditAccess { get; set; } = true;
        public bool UserHasAccess { get; set; } = false;
        public int ChatNotificationCount { get; set; }
        [JsonIgnore]
        public DateTime MinEntryDate { get; set; }
        [JsonIgnore]
        public DateTime MaxEntryDate { get; set; }
        [JsonIgnore]
        public int TotalCount { get; set; }
        [JsonIgnore]
        public int PerPageCount { get; set; }
        public bool IsFromDeposit { get; set; }
    }

    public class JournalBillResponse : StatusDTO
    {
        public string? ID { get; set; }
        public long? InfoID { get; set; }
        public string BillNumber { get; set; }
    }
    public class JEQuickAddResponse : StatusDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }

    }
    public class EmployeeQuickAddLookupRespose
    {
        public List<KeyValuePairObject<string, string>> Frequencies { get; set; } = new List<KeyValuePairObject<string, string>>();
        public List<KeyValuePairObject<string, string>> JobInfo { get; set; } = new List<KeyValuePairObject<string, string>>();
        public List<KeyValuePairObject<string, string>> PayrollDepartments { get; set; } = new List<KeyValuePairObject<string, string>>();
    }

    public class LoanScheduleTransactionResp : StatusDTO
    {
        public decimal MonthlyAmount { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal SBAAmount { get; set; }
        public decimal CDCAmount { get; set; }
        public decimal CSAAmount { get; set; }
        public decimal CustomAmount { get; set; }
        public DateTime? EntryDate { get; set; }
    }

}
