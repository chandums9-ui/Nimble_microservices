using Common.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FluentValidation;
using System.ComponentModel;
//using AutoMapper.Configuration.Annotations;
using System.Text.Json.Serialization;
using Common.Domain.DTO.Req;

namespace Common.Domain.DTO.Model.Base
{

    public abstract class JournalEntryBaseDTO : ModelBaseCorporation
    {
        public string? ID { get; set; }
        public string? ParentID { get; set; }
        public string? SourceID { get; set; }
        public string EntryNumber { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Comment { get; set; }
        public bool HasAttachments { get; set; } = false;
        public bool IsAccrual { get; set; } = false;
        public bool IsPrintEntry { get; set; } = false;
        public bool IsRecurring { get; set; } = false;
        public bool IsAutomatic { get; set; } = false;
        public bool IsPreVoid { get; set; } = false;
        public bool IsVoid { get; set; } = false;
        public DateTime? VoidDate { get; set; }
        public string? ClearedDate { get; set; }
        public string? VoidReason { get; set; }
        public bool IsReconciliation { get; set; } = false;
        public string? ReconciliationDate { get; set; }
        public string? ReconciliationToolTip { get; set; }
        public List<JournalDivisionDTO> Divisions { get; set; } = new List<JournalDivisionDTO>();
        public short JEApprovalStatus { get; set; }
        public short? SourceType { get; set; }
        public short? ParentSourceType { get; set; }
        public string BillPaymentStatus { get; set; }
    }
    public class JournalEntryDTO : JournalEntryBaseDTO
    {
        public bool? IsMultiplePost { get; set; } = false;
        public RecurringDTO? RecurringData { get; set; } = new RecurringDTO();

        public string ClientID { get; set; }
    }
    public class RecurringDTO
    {
        public string? ID { get; set; }
        public DateTime NextDate { get; set; }
        public string RecurringName { get; set; }
        public string FrequencyID { get; set; }
        public string FrequencyName { get; set; }
        public bool IsMonthEndDate { get; set; } = false;
        public string? SourceID { get; set; }
        public string? SourceType { get; set; }
        [DefaultValue(false)]
        public bool IsRemindmeBefore { get; set; }
        public string RemaindID { get; set; }
        public string RemindName { get; set; }
        public DateTime? EndDate { get; set; }
        public string RecurringMail { get; set; }
        public string? Description { get; set; }
        [DefaultValue(false)]
        public bool IsAutomatic { get; set; }
        [DefaultValue(false)]
        public bool IsRecurring { get; set; }
        //public List<JournalEntry> JournalEntry { get; set; }
    }
    public class JournalDivisionDTO
    {
        public string? ID { get; set; }
        public string? ParentID { get; set; }
        public string AccountID { get; set; }
        public string AccountValue { get; set; }
        public string SourceValue { get; set; }
        public string StoreValue { get; set; }
        public string TargetValue { get; set; }
        public string AccountType { get; set; } = string.Empty;
        /// <summary>
        /// PC Id
        /// </summary>
        public string? StoreID { get; set; } //PC
        public string Description { get; set; }
        /// <summary>
        /// Name (Vendor | Employee | Customer) ID
        /// </summary>
        public string? TargetID { get; set; } //NameID
        /// <summary>
        /// purpose ID
        /// </summary>
        public string? SourceID { get; set; } //purposeID
        /// <summary>
        /// TransactionSourceTypes Enum
        /// </summary>
        public short? SourceType { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public short? TargetType { get; set; } //Name TypeiD
        public DateTime TransactionDate { get; set; }
        public short? Order { get; set; }
        public decimal? Statistics { get; set; }
        public string? ReconciliationID { get; set; }
        public short? ReconciliationStatus { get; set; }
        public bool? IsBankTransactionRefID { get; set; }
        public string? BankTransactionRefID { get; set; }
        public short? UseTaxID { get; set; }
        public string UseTaxName { get; set; }
        public LoanAccountType LoanAccountType { get; set; }

    }
    public class JournalAccountDTO : JournalEntryDTO
    {
        public string AccountID { get; set; }
        public string PaymentMethodID { get; set; }
        public string PayeeID { get; set; }
        /// <summary>
        /// TransactionSourceTypes Enum
        /// </summary>
        public short? PayeeType { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }
        public bool? ToBePrinted { get; set; }
        public bool? IsBankTransactionRefID { get; set; }
        public string? BankTransactionRefID { get; set; }
        //public string ClientID {  get; set; }
    }

    /// <summary>
    /// Bank feed Split transaction Info
    /// </summary>
    public class FeedTransactionSplitDataDto
    {

        /// <summary>
        /// Bank feed Split transaction id when coming from Split entry
        /// </summary>
        public string FeedTransactionID { get; set; }
        /// <summary>
        /// Bank feed Split transaction Account id when coming from Split entry
        /// </summary>
        public string FeedTransactionAccID { get; set; }

        /// <summary>
        /// Split account related journal transaction id
        /// </summary>
        public string TransactionID { get; set; }
    }
}
