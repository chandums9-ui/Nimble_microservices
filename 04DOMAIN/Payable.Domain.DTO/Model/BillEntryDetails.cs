using Common.Domain.DTO.Model.Base;
using Payable.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class ReplaceReccuringEntryRequest
    {
        public BillEntryDetails details { get; set; }
        public string DuplicateReccuringID { get; set; }

    }
    public class ReconToolTipDetails
    {
        public string AccountName { get; set; }
        public string ReconType { get; set; }
        public string StatementDate { get; set; }
    }
    public class BillEntryDetails
    {
        public string JEID { get; set; }
        [Required(ErrorMessage = "Corporation ID is required")]
        public string CorpID { get; set; }

        public string CorpName { get; set; }

        public string CorpLegalName { get; set; }
        public long BillInformationId { get; set; }

        [Required(ErrorMessage = "Vendor ID is required")]
        public string VenID { get; set; }
        //[Required(ErrorMessage = "Contract ID is required")]
        public string ContractID { get; set; }
        public string ContractAccNum { get; set; }
        [Required(ErrorMessage = "Books Date is required")]
        [DataType(DataType.Date)]
        public DateTime BooksDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BillDate { get; set; }
        //[Required(ErrorMessage = "Due Date is required")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
        [DefaultValue(null)]
        public string CreditDaysID { get; set; }
        //[Required(ErrorMessage = "Payment Method is required")]
        [DefaultValue(null)]
        public string PaymentMethodID { get; set; }
        [Required(ErrorMessage = "Bill Nmuber is required")]
        public string BillNumber { get; set; }
        [DefaultValue(null)]
        public string RefNumber { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        [DefaultValue(null)]
        public string Memo { get; set; }
        [DefaultValue(null)]
        public string CheckMemo { get; set; }
        [DefaultValue(false)]
        public bool HasAttachment { get; set; }
        [DefaultValue(false)]
        public bool IsHoldPayment { get; set; }
        [DefaultValue(false)]
        public bool IsVoid { get; set; }
        [DefaultValue(false)]
        public bool IsButtonVoid { get; set; }
        [DefaultValue(false)]
        public bool IsReconciled { get; set; }
        [DefaultValue(false)]
        public bool IsResumeReconciled { get; set; }
        [DataType(DataType.Date)]
        public DateTime VoidDate { get; set; }
        public string VoidReason { get; set; }

        public List<TransactionDivisionDetails> Divisions { get; set; } = new List<TransactionDivisionDetails>();
        public List<ReconToolTipDetails> ReconToolTipDetails { get; set; }
        public RecurringDTO RecurringInfo { get; set; }
        [DefaultValue(false)]
        public bool IsDebitMemo { get; set; }
        [DefaultValue(false)]
        public bool IsValidate { get; set; }
        [DefaultValue(true)]
        public bool IsSave { get; set; }
        [DefaultValue(null)]
        public string? SourceID { get; set; }
        [DefaultValue(null)]
        public short? ReferenceType { get; set; }
        [DefaultValue(null)]
        public string ParentAccountID { get; set; }
        public string ParentAccountName { get; set; }
        public short? BillStatus { get; set; }
        public string BillPaymentStatus { get; set; }
        public DateTime? LatestBillPaymentDate { get; set; }
        public List<BillEntryPaymentToolTipDetails> ToolTipDetails { get; set; }
        public bool IsEntryPass { get; set; }
        public bool IsUpdateEntryPass { get; set; }
        public string VenName { get; set; }
        public bool HasLoginUserAccess { get; set; }
        public  short BillEntryType {get;set;}
        public bool IsApproveAndUpdate { get; set; }
        public bool IsUpdateEntry {  get; set; }
        public string DuplicateReccuringID { get; set; }
        public string PayMethodName { get; set; }
        public int PaymentMethodType { get; set; }

        public string DocRefID { get; set; }
        public bool IsSaveVendorPurposeDetails { get; set; }
        // key - PurposeId Value - AccountId
        public Dictionary<string, string> NeedToUpdatePurposeList { get; set; } = new Dictionary<string, string>();

        public VendorBalanceResponse tempVendorOpeningBalance { get; set; } = null;
        public VendorDetailsResponse tempDefaultVendorInfo { get; set; } = null;
        public ContractDetailsResponse tempContractDetails { get; set; } = null;
    }
    public class QuickBillEntryDetails
    {
        public string ClientName { get; set; }
        public string JEID { get; set; }
        public string CorpID { get; set; }
        public string CorpName { get; set; }
        public string VenID { get; set; }
        public string VendorName { get; set; }
        public string? ContractID { get; set; } = null;
        public string? ContractAccNum { get; set; } = null;

        [Required(ErrorMessage = "Books Date is required")]
        [DataType(DataType.Date)]
        public DateTime BooksDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BillDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        public long? CreditDays { get; set; }

        [DefaultValue(null)]
        public string CreditDaysID { get; set; }

        [DefaultValue(null)]
        public string PaymentMethodID { get; set; }

        [DefaultValue(null)]
        public string PaymentMethodName { get; set; }

        [Required(ErrorMessage = "Bill Number is required")]
        public string BillNumber { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [DefaultValue(null)]
        public string AttachmentURL { get; set; }
        public int BillType { get; set; }
        public string FromClient { get; set; }

        [DefaultValue(null)]
        public string AddressDetails { get; set; }
        [DefaultValue(null)]
        public string AddressCity { get; set; }
        [DefaultValue(null)]
        public string AddressCountry { get; set; }
        [DefaultValue(null)]
        public string AddressState { get; set; }
        [DefaultValue(null)]
        public string AddressZipCode { get; set; }
        public List<TransactionDivisionInfo> Divisions { get; set; } = new List<TransactionDivisionInfo>();
        public byte[] ImageBase64Encoded { get; set; }
        public string FileName { get; set; }
    }

    public class TransactionDivisionInfo
    {
        public string TID { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string AccountID { get; set; }
        public string AccountCode { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
    }
    public class BillEntryValidationDetails : StatusDTO
    {
        public BillEntryDetails BillEntryDetails = new BillEntryDetails();
    }
}
