using Common.Domain.DTO.Model.Base;
using Payable.Domain.DTO.Model;
using Payable.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class ReplaceReccuringEntryResponse:StatusDTO
    {
        public string RecurringID { get; set; }
    }
    public class LoadBillReccuringData : StatusDTO
    {
        public BillEntryDetails data {get;set;}
    }
    public class LoadBillsResponse : StatusDTO
    {
        public List<BillLookUPData> Lookup { get; set; } = new List<BillLookUPData>();
        public List<BillData> Bills { get; set; } = new List<BillData>();

        public List<VendorOutstandingAndDebitMemoAmount> VendorOutStandingAmounts { get; set; } = new List<VendorOutstandingAndDebitMemoAmount>();
    }
    public class BillLookUPData
    {
        public int BillType { get; set; }
        public string TypeName { get; set; }
        public long TypeCount { get; set; }
        public decimal Balance { get; set; }
    }
    public class BillPayLookUPData
    {
        public int Type { get; set; }
        public string TypeName { get; set; }
        public long TotalCount { get; set; }
        public decimal Balance { get; set; }
    }
    public class LoadBillPaysResponse : StatusDTO
    {
        public List<BillPayLookUPData> Lookup { get; set; } = new List<BillPayLookUPData>();
        public List<BillPayData> BillPays { get; set; } = new List<BillPayData>();
    }
    public class BillPayData
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string TypeID { get; set; }
        public string Date { get; set; }
        public string CorpName { get; set; }
        public string LegalName { get; set; }
        public string CorpID { get; set; }
        public string CheckNumber { get; set; }
        public string InvoideDueDate { get; set; }
        public string Number { get; set; }
        public string paymentDate { get; set; }
        public string VendorName { get; set; }
        public string VendorID { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountID { get; set; }
        public string PayMethodID { get; set; }
        public string PayMethodName { get; set; }
        public string PayMode { get; set; }
        public decimal Amount { get; set; }
        public bool UserHasAccess { get; set; }
        public string Status { get; set; }
        public int ChatNotificationCount { get; set; }
        public int StatusID { get; set; }
        public string Pay { get; set; }
        public long TotalCount { get; set; }
        public bool HasAttachments { get; set; }
        public bool IsVoid { get; set; }
        public bool IsLock { get; set; }
        public bool IsReconciliation { get; set; }
        [DefaultValue(true)]
        public bool IsEditAccess { get; set; } = true;
        public bool IsPayAccess { get; set; } = true;
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public int PageCount { get; set; }
        public bool IsHold { get; set; }
        public string PreviousApprovalUserID { get; set; }
        public string PreviousApprovalUserName { get; set; }

        public short PaymethodType { get; set; }
    }
    public class BillData
    {
        public string BID { get; set; }
        public long BillInfoID { get; set; }
        public string PreviousApprovalUserID {  get; set; }
        public string PreviousApprovalUserName { get; set; }
        public string EntryType { get; set; }
        public string TypeID { get; set; }
        public string BooksDate { get; set; }
        public string CorporationName { get; set; }

        public string LegalName { get; set; }
        public string CorporationID { get; set; }
        public string BillNum { get; set; }
        public string BillDate { get; set; }
        public string VendorName { get; set; }
        public string VendorID { get; set; }
        public string AccountNumber { get; set; }
        public string PayMethodID { get; set; }
        public string PayMethodName { get; set; }
        public int? PayMethodType { get; set; }
        public string Memo { get; set; }
        public bool UserHasAccess { get; set; }
        public string DueDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public int ChatNotificationCount { get; set; }
        public string ApprovalStatus { get; set; }
        public string StatusID { get; set; }
        public string RecievedVia { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByID { get; set; }
        public string CreatedDate { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToID { get; set; }
        public long TotalCount { get; set; }
        public string Pay { get; set; }
        public bool HasAttachments { get; set; }
        public bool IsVoid { get; set; }
        public bool IsLock { get; set; }
        public bool IsEditAccess { get; set; } = true;
        public bool IsPayAccess { get; set; } = true;
        public bool IsReconciled { get; set; }
        public bool IsHold { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public int PageCount { get; set; }
        public int ApprovalOrder {  get; set; }
        public int ApprovalType { get; set; }

        public bool IsScheduled { get; set; }

        public int RowIndex { get; set; }

        public bool BillPaymentUserAccess { get; set; }
    }

    //public class UserApprovalDetails
    //{
    //    public string CorporationID { get; set; }
    //    /// <summary>
    //    /// User First name & Last name
    //    /// </summary>
    //    public string Name { get; set; }
    //    public string UserName { get; set; }
    //    public string UserID { get; set; }
    //    public int ApprovalOrder { get; set; }
    //    public int ApprovalType { get; set; }
    //    public bool IsCurrentUser { get; set; }
    //}
    public class BillUploadResponse : StatusDTO
    {

    }
    public class UploadDetail
    {
        public string type { get; set; }
        public List<string> loc { get; set; }
        public string msg { get; set; }
        public byte[] Input { get; set; }
    }
    public class UploadErrorResponse
    {
        public List<UploadDetail> detail { get; set; } = new List<UploadDetail>();
    }

    public class BillsWidgetData:StatusDTO
    {
        public List<BillWidgetDataDbResponse> billsWidgetInfo { get; set; } =new List<BillWidgetDataDbResponse>();
    }
    public class BillWidgetDataDbResponse
    {
        public Int16 BillType { get; set; }
        public string TypeName {  get; set; }   
        public decimal Balance {  get; set; }   
    }
    public class BillEntryAIInfoDetails
    {
        public long id { get; set; }
        public string TransactionId { get; set; }
        public long BillEntryInformationId { get; set; }
        public string PurposeId { get; set; }
        public string AISuggestedPurposeName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string SelectedPurposeId { get; set; }
        public string SelectedPurposeName { get; set; }
        public string SelectedPurposeAccountId { get; set; }
        public string SelectedPurposeAccountName { get; set; }
        public string AIPurposeType { get; set; }
        public bool Status { get; set; }
        public bool IsPurposeUpdated { get; set; }
        public bool IsNewPurposeAdded { get; set; }

    }
    public class BillEntryAIInfoDetailsList : StatusDTO
    {
        public List<BillEntryAIInfoDetails> data { get; set; }
    }

    public class ViewBillsDbResponse:BillData
    {
        public string OpenDaysStatus { get; set; }  

        public string CheckNo { get; set; } 
    }
    public class VewBillsResponse:StatusDTO
    {
        public List<BillLookUPData> Lookup { get; set; } = new List<BillLookUPData>();
        public List<ViewBillsDbResponse> Bills { get; set; } = new List<ViewBillsDbResponse>();
    }
}
