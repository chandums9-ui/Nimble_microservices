using Payable.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    public class LoadBillsRequest
    {
        /// <summary>
        /// BillTypeEnum
        /// </summary>
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
        public List<FilterData> Filters { get; set; } = new List<FilterData>();
        public int PageNumber { get; set; }
    }
    public class LoadBillPaysRequest
    {
        /// <summary>
        /// 0-Summary,1-TobeApproved
        /// </summary>
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
        public List<FilterData> Filters { get; set; } = new List<FilterData>();
        public int PageNumber { get; set; }
    }
    public class SchedulePaymentsListResponse
    {
        public List<SchedulePaymentData> data { get; set; } = new List<SchedulePaymentData>();
    }
    public class SchedulePaymentData
    {
        public string VendorID { get; set; }
        public string CorpID { get; set; }

        public string AccountID { get; set; }

        public string PaymethodID { get; set; }

        public string JID { get; set; }
        public string ContractID { get; set; }
    }

    public class BillPaySSFilter
    {
        public string CorporationID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PageCount { get; set; }
        /// <summary>
        /// SSSortByEnum
        /// </summary>
        public int SortBy { get; set; }
        public int SortDirection { get; set; }
        public FilterDetails CheckNumber { get; set; }
        public FilterDetails VendorName { get; set; }
        public string AccountID { get; set; }
        public string PayMethodID { get; set; }
        public FilterDetails Amount { get; set; }
        public string PayMode { get; set; }
        public int Status { get; set; }
        public string ACHStatus { get; set; }
        public bool IsShowPaymentsWithoutBills { get; set; }
    }
    public class ShowSearchFilter
    {
        public int PageCount { get; set; }
        /// <summary>
        /// SSSortByEnum
        /// </summary>
        public int SortBy { get; set; }
        [DefaultValue(0)]
        public int SortDirection { get; set; }
        public FilterDetails Number { get; set; }
        public FilterDetails VendorName { get; set; }
        public FilterDetails Amount { get; set; }
        public FilterDetails Memo { get; set; }
        public FilterDetails AccountNum { get; set; } = new FilterDetails();
    }
    public class FilterDetails
    {
        public int ID { get; set; }
        public int FilterOption { get; set; }
        [DefaultValue(null)]
        public string text { get; set; }
        public int Amount { get; set; }
    }

    public class BillEntryIORequest
    {
        public string JEID { get; set; }
        public string CorporationID { get; set; }
        public string VendorID { get; set; }
        public string ContractID { get; set; }
        public string Path1 { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public string BillDate { get; set; }
        public string OptionalDueDate { get; set;}

        public string InvoiceType { get; set; }
        public string profitCenterId  { get; set; }
        public List<BillTransactionIORequest> TransactionDetails { get; set; }= new List<BillTransactionIORequest>();
    }
    public class BillTransactionIORequest
    {
        public string purpose { get; set;}
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string type { get; set;}
        public string NimblePurposeId { get; set;}
        public string NimbleAccountId { get; set; }
    }

    public class UpdateUnapprovedBillsAIRequest
    {
        public long CurrentBillInfoId { get; set; }
        [DefaultValue(false)]
        public bool IsApplyVendorPrefToUnapprovedBills { get; set; } = false;
        public string VendorId { get; set; } = string.Empty;
        [DefaultValue(false)]
        public bool IsupdatePurposeAccInUnapprovedBills { get; set; } = false; 
        public List<string> UpdatedPurposeIds { get; set; } = new List<string>();
        public List<long> listBillInfoIDs { get; set; } = new List<long>();
        public bool IsUpdateAll { get; set; }
    }

    public class UpdateUnapprovedBillsAIMessage
    {
        public UpdateUnapprovedBillsAIRequest UpdateUnapprovedBillsAIRequest { get; set; }
        public string MessageID { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty ;
        public string ClientID { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
    }
    public class GetBillsRequest
    {
        public string CoporationId { get; set; }
        public string UserID {  get; set; }
        public Int32 BillType {  get; set; }
    }
    public class BillApproveRequest
    {
        public string JID { get; set; }
        [DefaultValue(false)]
        public bool IsValidate { get; set; } //To Check Corporation Lock
        [DefaultValue(false)]
        public bool IsSaveVendorAiPurposeDetails { get; set; }
        public bool IsHoldPayment { get; set; } 
        public List<TransactionDivisionDetails> RemovedTransactionsSplitines { get; set; } = new List<TransactionDivisionDetails>();

    }

}
