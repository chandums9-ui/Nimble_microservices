using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillPaymentApproveRequest
    {
        public string JEID { get; set; }
        [DefaultValue(false)]
        public bool isCheckNumberAutoGenerate { get; set; }
        public string StartingCheckNumber { get; set; }

        public bool ToBePrinted { get; set; }
        public bool PrintCheckNow { get; set; }
        [DefaultValue(true)]
        public bool IsSave { get; set; }
    }
    public class BulkBillPaymentApproveRequest
    {
        public List<string> JEID { get; set; }
        [DefaultValue(false)]

        public string StartingCheckNumber { get; set; }
        public bool ToBePrinted { get; set; }

        public bool PrintCheckNow { get; set; }

        [DefaultValue(true)]
        public bool IsSave { get; set; }
    }

    public class BillPaymentApproveInfo
    {
        public string JEID { get; set; }
        [DefaultValue(false)]
        public bool isCheckNumberAutoGenerate { get; set; }

        public bool ToBePrinted { get; set; }

        public string CheckNumber { get; set; }
        [DefaultValue(true)]
        public bool IsSave { get; set; }

    }
    public class BillInfoOutStandingAmounts
    {
        public long BillInfoID { get; set; }

        public decimal UsedAmount { get; set; }
    }

    public class ContractWiseBills
    {
        public List<long> BillInfoID { get; set; }

        public string VendorID { get; set; }

        public string ContractID { get; set; }

        public string AccountNumber { get; set; }

        public int Index { get; set; }

    }
    public class BillPaymentDetails
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Corporation ID is required")]
        public string CorpID { get; set; }
        public string CorpName { get; set; }

        public string CorpLegalName { get; set; }
        public string JEID { get; set; }
        public string VendorID { get; set; }

        public string ContractID { get; set; }
        public string AccountNumber { get; set; }
        public DateTime Date { get; set; }
        //public decimal Amount { get; set; }
        public string PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public int PaymentMethodType { get; set; }
        public string BankAccountID { get; set; }
        public string BankAccountName { get; set; }
        public decimal BankAccountBalance { get; set; }
        public string Memo { get; set; }
        public string CheckNumber { get; set; }
        public string RefNumber { get; set; }
        public string OldCheckNumber { get; set; }
        public int PaymentID { get; set; }
        public bool ToBePrinted { get; set; }
        public bool PrintCheckNow { get; set; }

        [DefaultValue(true)]
        public bool IsSave { get; set; }
        public short? BillPaymentStatus { get; set; }
        public DateTime? VoidDate { get; set; }
        public bool? IsVoid { get; set; }
        public string VoidRemarks { get; set; }
        [DefaultValue(false)]
        public bool IsValidate { get; set; }
        
        public string PayeeName { get; set; }
        public List<BillPaymentDivisions> BillPaymentDivisions { get; set; }=new List<BillPaymentDivisions>();
       // public BillPaymentDetails Clone() => (BillPaymentDetails)this.MemberwiseClone();
        public BillPaymentReconcileDetails ReconcileDetails { get; set; }
        public decimal? ExcessAmount { get; set;}
        public decimal Amount { get; set; }
        public bool IsApproveAndUpdate { get; set; }
        public bool IsUserHasAccess {  get; set; }

        public int EpaymentStatus { get; set; }

        public bool IsCheckAlreadyPrinted { get; set; }
        public decimal TotalAMountDue { get; set; }
        public decimal TotalBillAmount { get; set; }
        public string FeedTransInfo { get; set; }

        public string ClearDate { get; set; }

    }

    public class BillPaymentSaveReqProcessResponse :StatusDTO
    {
       public  List<BillPaymentDetails> BillPaymentDetails { get; set; } = new List<BillPaymentDetails>();
    }

    public class BillPaymentSaveRequest 
    {
        public List<BillPaymentDetails> BillPaymentDetails { get; set; }=new List<BillPaymentDetails>();
        public short BillPayType { get; set; }

    }
    public class DBListOFEntries<T> 
    {
        public List<T> ItemsTobeInserted { get; set; } = new List<T>();
        public List<T> ItemsTobeUpdated { get; set; }=new List<T>();

        public List<T> ItemsTobeDeleted { get; set; } = new List<T>();
    }
    public class Debitmemos
    {
        public long BillInfoId { get; set; }
        public string ContractID { get; set; }
        public string VendorID { get; set; }
        public string AccountNumber { get; set; }
        public DateTime BooksDate { get; set; }

        public string TID { get; set; }
        public decimal AmountDue { get; set; }
        public decimal Applied { get; set; }

        public decimal Available { get; set; }
    }
    public class BillPayCheckInfo
    {
        public string ContractID { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string AccountNumber { get; set; }

        public string CheckNo { get; set; } 

    }

    public class QuickBillPaymentDetails
    {
        public string ClientName { get; set; }
        public string JEID { get; set; }
        public string CorpID { get; set; }
        public string CorpName { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string ParentAccountID { get; set; }
        public string ParentAccountName { get; set; }
        public string PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        [Required(ErrorMessage = "Bill PaymentDate is required")]
        [DataType(DataType.Date)]
        public DateTime BillPaymentDate { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "CheckNumber is required")]
        public string CheckNumber { get; set; }
        public string Memo { get; set; }
        public string BillPaymentStatus { get; set; }
        public string TransactionStatus { get; set; }
        public string AttachmentURL { get; set; }
        [DefaultValue(true)]
        public bool IsSave { get; set; }
        public string FromClient { get; set; }
        public List<long> BillUsedIDs { get; set; } = new List<long>();
    }

    public class BillPaymentValidationDetails : StatusDTO
    {
        public BillPaymentSaveRequest BillPaySaveRequest { get; set; } = new BillPaymentSaveRequest();

    }
}
