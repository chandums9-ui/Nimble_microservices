using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Enums
{
    public enum DirectDepositUserAccess
    {
        Deactivate = 0,
        ManualPayments,
        AutoPayments
    }
    public enum BillsStatusTypesForGrid
    {
        [Display(Name = "Paid")]
        Paid,
        [Display(Name = "UnPaid")]
        Unpaid,
        [Display(Name = "Partial")]
        Partial,
        [Display(Name = "In verification")]
        InVerification,
        [Display(Name = "In approval")]
        InApproval,
        [Display(Name = "Rejected")]
        Rejected,
        [Display(Name = "Open credit")]
        Opencredit,
        [Display(Name = "Adjusted")]
        Adjusted,
        [Display(Name = "Part-adjusted")]
        PartlyAdjusted,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Voided")]
        Voided,

    }

    public enum PayableGridEnum
    {
        [Display(Name = "Summary")]
        Summary,
        [Display(Name = "Vendor Master")]
        VendorMaster,
        [Display(Name = "Bill Entry")]
        BillEntry,
        [Display(Name = "Bill Payment")]
        BillPayment,
        [Display(Name = "Vendor")]
        Vendor
    }



    public enum BillsStatusTypes
    {
        [Display(Name = "Paid")]
        paid,
        [Display(Name = "Unpaid")]
        unpaid,
        [Display(Name = "Partial")]
        partial,
        [Display(Name = "Entry")]
        entry,
        [Display(Name = "In verification")]
        Verified,
        [Display(Name = "Rejected")]
        rejected,
        [Display(Name = "Open credit")]
        opencredit,
        [Display(Name = "Adjusted")]
        adjusted,
        [Display(Name = "Part-adjusted")]
        partlyadjusted,
        [Display(Name = "Approved")]
        Approved,
    }

    public enum PaymentStatus
    {
        [Display(Name = "In approval")]
        InApproval = 1,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Rejected")]
        rejected,
    }



    public enum BillTypeEnum
    {
        [Display(Name = "Summary")]
        Summary,
        [Display(Name = "To be Approved")]
        TobeApproved,
        [Display(Name = "Pending Payments")]
        PendingPayments,
        [Display(Name = "Debit Memos")]
        DebitMemos,
        [Display(Name = "Scheduled Payments")]
        ScheduledPayments
    }

    public enum PaymentTypeEnum
    {
        [Display(Name = "Summary")]
        Summary = 3,
        [Display(Name = "To be Approved")]
        TobeApproved,
        [Display(Name = "Pending e Payments")]
        PendingEPayments,
        [Display(Name = "Pending Print Checks")]
        PendingPrintChecks
    }
    public enum SSSortByEnum
    {
        corporation,
        BooksDate,
        Number,
        VendorName,
        DueDate,
        Amount,
        Status,
        Memo,
        BillDate,
        Account,
        paymentmethod,
        Paymode,
        ACHStatus
    }
    public enum ApprovalStatusEnum
    {
        Entry = 1,
        Verification = 2,
        Approval = 3,
        Rejected = 4,
    }
    public enum BillSSFilterByEnum
    {
        [Display(Name = "Type")]
        Type = 0,
        [Display(Name = "Number")]
        Number = 1,
        [Display(Name = "Payee Name")]
        VendorName = 2,
        [Display(Name = "Account Number")]
        AccountNum = 3,
        [Display(Name = "Pay Method")]
        PayMethod = 4,
        [Display(Name = "Memo")]
        Memo = 5,
        [Display(Name = "Amount")]
        Amount = 6,
        [Display(Name = "Status")]
        Status = 7,
        [Display(Name = "Recieved Via")]
        RecievedVia = 8,
        [Display(Name = "Created by")]
        Createdby = 9,
        [Display(Name = "Assigned To")]
        AssignedTo = 10,
        [Display(Name = "Bank Account")]
        BankAccount = 11,
        [Display(Name = "Pay Mode")]
        PayMode = 12,
        [Display(Name = "Corporation")]
        Corporation = 13,
        [Display(Name = "Journal Type")]
        JournalType = 14,
        [Display(Name = "Description")]
        Description = 15,
        [Display(Name = "ToCorporation")]
        ToCorporation = 17,
        [Display(Name = "FromCorporation")]
        FromCorporation =18,
        [Display(Name = "TransferType")]
        TransferType =19,
        [Display(Name = "EntryNumber")]
        EntryNumber =20,
        [Display(Name = "Mode")]
        Mode =21,
    }
    public enum BillSortByEnum
    {
        [Display(Name = "Type")]
        Type = 1,
        [Display(Name = "Date")]
        BooksDate,
        [Display(Name = "Corporation")]
        CorpName,
        [Display(Name = "Number")]
        Number,
        [Display(Name = "Bill Date")]
        BillDate,
        [Display(Name = "Payee Name")]
        VendorName,
        [Display(Name = "Account Number")]
        AccountNum,
        [Display(Name = "Pay Method")]
        PayMethod,
        [Display(Name = "Memo")]
        Memo,
        [Display(Name = "Due Date")]
        DueDate,
        [Display(Name = "Amount")]
        Amount,
        [Display(Name = "Status")]
        Status,
        [Display(Name = "Recieved Via")]
        RecievedVia,
        [Display(Name = "Created by")]
        Createdby,
        [Display(Name = "Assigned To")]
        AssignedTo
    }
    public enum ViewFilterEnum
    {
        [Display(Name = "Latest Entries")]
        LatestEntried,
        [Display(Name = "Latest Created")]
        LatestCreated,
        [Display(Name = "Latest Modified")]
        LatestModified
    }

    public enum BillEntryLaunchPayActions
    {
        [Display(Name = "Pay")]
        Pay,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Preview")]
        Preview,
        [Display(Name = "Verify")]
        Verify,
        [Display(Name = "Approve")]
        Approve,
    }
    public enum EpayProviderTypesEnum
    {
        [Display(Name = "All")]
        All,
        [Display(Name = "Repay")]
        Repay,
        [Display(Name = "DirectDeposit")]
        DirectDeposit,
        [Display(Name = "EFT")]
        EFT
    }
    public enum EpayActionTypesEnum
    {
        [Display(Name = "Created")]
        Created = 0,
        [Display(Name = "Modified")]
        Modified,
        [Display(Name = "Deleted")]
        Deleted,
        [Display(Name = "Viewed")]
        Viewed
    }
    public enum ProviderEntityTypeEnum
    {
        [Display(Name = "Vendor")]
        vendor = 1,
        [Display(Name = "Repay")]
        Repay = 14,
        [Display(Name = "EFT")]
        EFT,
        [Display(Name = "DirectDeposit")]
        DirectDeposit
    }
    public enum BankPaymentsIn
    {
        [Display(Name = "CAD")]
        CAD
    }
    public enum BankTemplates
    {
        [Display(Name = "CPA005")]
        CPA005
    }
    public enum EpayDDType
    {
        [Display(Name = "Manual Export")]
        ManualEport,
        [Display(Name = "API Extraction")]
        APIExtraction
    }
    public enum EPaymentsIssueType
    {
        [Display(Name = "Repay")]
        Repay = 1,
        [Display(Name = "EFT")]
        EFT,
        [Display(Name = "DirectDeposit")]
        DirectDeposit,
        [Display(Name = "Check")]
        Check,
    }
    public enum EPaymentsBatchStatus
    {
        [Display(Name = "Entered")]
        Entered = 1,
        [Display(Name = "Verified")]
        Verified,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Processing")]
        Processing,
        [Display(Name = "PaidOrExportedOrUploaded")]
        PaidOrExportedOrUploaded,
        [Display(Name = "Rejected")]
        Rejected,
        [Display(Name = "Voided")]
        Voided,
        [Display(Name = "Deleted")]
        Deleted,
        [Display(Name = "Completed")]
        Completed
    }
    public enum EpayEFTFormatTypes
    {
        [Display(Name = "Payment Details")]
        PaymentDetails = 1,
        [Display(Name = "Vendor Details")]
        VendorDetails,
        [Display(Name = "Payor Details")]
        PayorDetails
    }

    public enum EPaymentActionType
    {
        [Display(Name = "Create")]
        Create = 1,
        [Display(Name = "Update")]
        Update,
        [Display(Name = "Delete")]
        Delete,
        [Display(Name = "Reject")]
        Reject,
        [Display(Name = "Void")]
        Void,
        [Display(Name = "Download")]
        Download
    }


    public enum EFTColumnsEnum
    {
        [Display(Name = "PaymentNo")]
        PaymentNo = 1,
        [Display(Name = "VendorID")]
        VendorID,
        [Display(Name = "PayeeName")]
        PayeeName,
        [Display(Name = "PaymentAmount")]
        PaymentAmount,
        [Display(Name = "InvoiceDate")]
        InvoiceDate,
        [Display(Name = "InvoiceNumber")]
        InvoiceNumber,
        [Display(Name = "OriginalInvoiceAmount")]
        OriginalInvoiceAmount,
        [Display(Name = "Discount")]
        Discount,
        [Display(Name = "NetInvoiceAmount")]
        NetInvoiceAmount,
        [Display(Name = "DetailsDescription")]
        DetailsDescription,
        [Display(Name = "AccountNumber")]
        AccountNumber,
        [Display(Name = "AttentionLine")]
        AttentionLine,
        [Display(Name = "Address1")]
        Address1,
        [Display(Name = "Address2")]
        Address2,
        [Display(Name = "City")]
        City,
        [Display(Name = "State")]
        State,
        [Display(Name = "ZIP")]
        ZIP,
        [Display(Name = "Email")]
        Email,
        [Display(Name = "Phone")]
        Phone,
        [Display(Name = "PayorName")]
        PayorName,
        [Display(Name = "PayorLegalName")]
        PayorLegalName,
        [Display(Name = "PayorAccountNumber")]
        PayorAccountNumber,
        [Display(Name = "PayorAddress1")]
        PayorAddress1,
        [Display(Name = "PayorAddress2")]
        PayorAddress2,
        [Display(Name = "PayorCity")]
        PayorCity,
        [Display(Name = "PayorState")]
        PayorState,
        [Display(Name = "PayorZIP")]
        PayorZIP,
        [Display(Name = "LocationID")]
        LocationID,
        [Display(Name = "BankID")]
        BankID
    }

    public enum RepayAuditColumnsEnum
    {
        [Display(Name = "ClientId")]
        ClientId = 1,
        [Display(Name = "ClientSecretId")]
        ClientSecretId,
        [Display(Name = "AccountId")]
        AccountId
    }
    public enum DDAuditColumnsEnum
    {
        [Display(Name = "CorporationLegalName")]
        CorporationLegalName = 1,
        [Display(Name = "BankName")]
        BankName,
        [Display(Name = "OriginatorNo")]
        OriginatorNo,
        [Display(Name = "DataCenterNo")]
        DataCenterNo,
        [Display(Name = "IsActive")]
        IsActive
    }
    public enum VendorAuditColumnsEnum
    {
        [Display(Name = "Name")]
        Name = 1,
        [Display(Name = "FederalID")]
        FederalID,
        [Display(Name = "SSN")]
        SSN,
        [Display(Name = "PrintCheckAs")]
        PrintCheckAs,
        [Display(Name = "IsAutoBill")]
        IsAutoBill,
        [Display(Name = "Terms")]
        Terms,
        [Display(Name = "PaymentMethodID")]
        PaymentMethodID,
        [Display(Name = "DDBranchNo")]
        DDBranchNo,
        [Display(Name = "DDInstitutionNo")]
        DDInstitutionNo,
        [Display(Name = "DDAccountNo")]
        DDAccountNo
    }
    public enum DepartmentTypeEnum
    {
        [Display(Name = "Not Applicable")]
        NotApplicable,
        [Display(Name = "Room Calculation")]
        RoomCalculation,
        [Display(Name = "Average Calculation")]
        AverageCalculation,
        [Display(Name = "Other Without Stats")]
        OtherWithoutStats
    }
    public enum IngnoreOCRPurposes
    {
        [Display(Name = "Previous Month Balance")]
        PreviousMonthBalance,
        [Display(Name = "Payments")]
        Payments,
        [Display(Name = "Balance Forward")]
        BalanceForward,
        [Display(Name = "Total Amount Due")]
        TotalAmountDue
    }
    public enum ActivityEnum
    {
        [Display(Name = "Entry")]
        Entry = 1,
        [Display(Name = "Verification")]
        Verification,
        [Display(Name = "Approval")]
        Approval,
        [Display(Name = "Rejected")]
        Rejected,
        [Display(Name = "Voided")]
        Voided,
        [Display(Name = "Updated")]
        Updated,
        [Display(Name = "Updated")]
        EntryUpdate
    }

    public enum BillOrBillPaymentEnum
    {
        [Display(Name = "To be Matched")]
        ToBeMatched = 1,
        [Display(Name = "Auto Matched ( To be Post )")]
        AutoMatched,

    }
    public enum BillOrBillPaymentOrderEnum
    {
        FIFO,
        LIFO
    }
    public enum BillPayImportStatusEnum
    {
        Pending = 1,
        AutoMapped,
        Delete,
        ManualMapped,
        Posted
    }
    public enum FormTypeOptions
    {
        [Display(Name = "MISC")]
        IsMisc = 1,
        [Display(Name = "NEC")]
        IsNEC

    }
}
