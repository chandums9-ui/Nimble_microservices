using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Common.Domain.DTO.Enums;

namespace Common.Domain.DTO.Req
{
    public class JournalSearchRequest : ModelBaseSearchRequest
    {
        public List<string> CorpIDs { get; set; } = new List<string>();
        /// <summary>
        /// If date filter -1, other wise send ViewFilterEnum value (i.e LatestEntried | LatestCreated | LatestModified)
        /// </summary>
        public int ViewFilter { get; set; }
        /// <summary>
        /// Selected Card type BillTypeEnum value (i.e Summary | TobeApproved)
        /// </summary>
        public int ViewType { get; set; }
        public List<SearchFilterDara> Filters { get; set; } = new List<SearchFilterDara>();
    }
    public class SearchFilterDara
    {
        /// <summary>
        /// BillSSFilterByEnum
        /// </summary>
        public short Filter { get; set; }

        [DefaultValue(7)] //Exact search
        public short OperationType { get; set; } = 7; 

        public string Value { get; set; }
    }
    public class JournalEntryRequest : JournalEntryDTO
    {       
        /// <summary>
        /// To update only the journal entry, set it to true. Also For Entry users while updating the rejected entry set it to true
        /// </summary>
        public bool IsUpdateEntry { get; set; } = false;
        /// <summary>
        /// To know the Statistics user reference is enabled while saving the data to do specific actions.
        /// </summary>
        public bool IsStatisticsEnabled { get; set; } = false;
        /// <summary>
        /// To delete the duplicate recurring data
        /// </summary>
        public string DuplicateReccuringID { get; set; }
        /// <summary>
        /// To Add new journal entry for the recurring data while comming from recurring screen.
        /// </summary>
        public bool IsEntryPass { get; set; } = false;

        /// <summary>
        /// To Update the recurring data & add new journal entry for the recurring data while comming from recurring screen.
        /// </summary>
        public bool IsUpdateEntryPass { get; set; } = false;

        /// <summary>
        /// To save the entry in DB default is true, when bulk operations | editing from recurring list it should be false, since we need to save all the entries at once.
        /// </summary>
        public bool IsSaveData { get; set; } = true;
        /// <summary>
        /// To save the entry in DB default is true, when bulk operations | editing from recurring list it should be false, since we need to save all the entries at once.
        /// </summary>
        public bool IsApproveAndUpdate { get; set; } = false;

        /// <summary>
        /// Bank feed Split transaction id and related journal transaction id & details when coming from Split entry
        /// </summary>
        public FeedTransactionSplitDataDto FeedTransactionInfo { get; set; }
        public string LoanScheduleTransId { get; set; } = string.Empty;
        public bool IsVoidDateEnable { get; set; } = false;

    }

    public class JournalEntryRejectRequest
    {
        public string JID { get; set; }
        public string AssignedUser { get; set; }
        public string Comment { get; set; }

        [DefaultValue(false)]
        public bool IsValidate { get; set; }//check for Corp Lock
        public string BillNumber { get; set; }
        public JournalSourceTypes SourceType { get; set; } = JournalSourceTypes.Journal;
    }

    public class UniqueNameCheckRequest
    {
        public string ReccuringName { get; set; }
        public int ReccuringEntryType { get; set; }
        public string CorporationID { get; set; }
        public string RecuringSourceID { get; set; }
    }
    public class JournalEntryGridRequest
    {
        public int PaymetType { get; set; }
        [DefaultValue(null)]
        public string CorpID { get; set; }
        [DefaultValue(null)]
        public string FromDate { get; set; }
        [DefaultValue(null)]
        public string ToDate { get; set; }
        /// <summary>
        /// ViewFilterEnum
        /// </summary>
        public int ViewFilter { get; set; }
        public int PageNumber { get; set; }
        /// <summary>
        /// Selected Card type BillTypeEnum value (i.e Summary | TobeApproved)
        /// </summary>
        public int ViewType { get; set; }
        public List<SearchFilterDara> Filters { get; set; } = new List<SearchFilterDara>();
    }
    public class FiltersRequest
    {
        public int SourceType { get; set; }
        public List<SearchFilterDara> Filters { get; set; } = new List<SearchFilterDara>();

    }
    public class QuickCustomerReq
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
    }
    public class QuickEmployeeReq
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PayFrequency { get; set; }
        public string PayRollDepartment { get; set; }
        public string JobTitle { get; set; }
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
        public bool IsEmployee { get; set; }
    }
    public class JobInfoReq
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class JournalEntryQuickRequest
    {
        public string ID { get; set; }  
        public string CorpID { get; set; }
        public DateTime EntryDate { get; set; }
        public short SourceType { get; set; }
        public List<DivisionEntry> Divisions { get; set; } = new List<DivisionEntry>();
    }

    public class DivisionEntry
    {
        public string ID { get; set; }
        public string AccountID { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string? TargetID { get; set; }
        public string? TargetValue { get; set; }
        public short? TargetType { get; set; }
        public string? Description { get; set; }    
    }

    public class JournalCommentRequest
    {
        public string JID { get; set; }
        public string Comment { get; set; }
        public short? ApprovalType { get; set; }
    }


    public enum LoanAccountType
    {
        Monthly = 1,
        Principal,
        Interest,
        SBA,
        CDC,
        CSA,
        Custom
    }
}
