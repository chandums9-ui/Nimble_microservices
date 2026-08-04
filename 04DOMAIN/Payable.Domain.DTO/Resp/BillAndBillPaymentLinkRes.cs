using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class BillAndBillPaymentLinkRes
    {

    }

    public class BillsPayments
    {
        public Int64 PaymentID { get; set; }
        public string CorporationID { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public DateTime? Date { get; set; }
        public string Number { get; set; }
        public string BankAccountID { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountType { get; set; }
        public string PaymentMethodID { get; set; }
        public int PaymentType { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMemo { get; set; }
        public decimal Amount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PageTotal { get; set; }
        public int TotalPageCount { get; set; }
        public bool IsSelected { get; set; } = false;
        public bool IsDisabled { get; set; } = false;
        public int TotalRecordsCount { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
    public class Bills
    {
        public string JEID { get; set; }
        public long BillInfoID { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryNumber { get; set; }
        public int BillType { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string ContractID { get; set; }  
        public bool IsPrintCheckEnabled { get; set; } = false;  
        public string RefType { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? BooksDate { get; set; }
        public int Number { get; set; }
        public string PaymentMethodID { get; set; }
        public string StoreID {  get; set; }    
        public string PaymentMethodName { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsSelected { get; set; } = false;
        public string BillStatus { get; set; }
        public int PaymentType { get; set; }
        public decimal BillAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OriginalPaidAmount { get; set; } 
        public string DisplayPaidAmount { get; set; }
        public bool IsHold {  get; set; }    
        public int RowOrder {  get; set; }  
        public decimal GrandTotal { get; set; }
        public decimal PageTotal { get; set; }
        public bool IsCreditOrDebitTran { get; set; } = false;
        public int TotalRecordsCount { get; set; }
        public int TotalPages { get; set; }

    }
    public class BillsAndPaymentsList : StatusDTO
    {
        public List<BillsPayments> PaymentsList { get; set; } = new List<BillsPayments>();
        public List<Bills> BillsList { get; set; } = new List<Bills>();
    }

    public class AutomatchedBills
    {
        public string JEID { get; set; }
        public long PaymentID { get; set; }
        public long BillInfoID { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? BooksDate { get; set; }
        public string Number { get; set; }
        public string PayeeName { get; set; }
        public string PayeeID {  get; set; }    
        public string BankAccountID { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountType { get; set; }
        public int PaymentType { get; set; }
        public string PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal PaymentAmount { get; set; }
        public string MatchedBill { get; set; }
        public bool IsSelected { get; set; } = false;
        public int Status { get; set; }
        public int TotalRecordsCount { get; set; }
        public int TotalPages { get; set; }
        public DateTime Date { get; set; }  // Bill PayDate
        public string MatchedBillNumber { get; set; }
        public DateTime? MinDate { get; set; }  
        public DateTime? MaxDate { get; set; }
        public bool IsHoldPayment { get; set; } = false; 
        public string PaymentMemo { get; set; } 

    }
    public class AutoMatchedList : StatusDTO
    {
        public List<AutomatchedBills> AutomatchedbillsList { get; set; } = new List<AutomatchedBills>();
    }

    public class SaveOrLinkPaymentReq
    {
        public List<BillsPayments> billsPayments { get; set; } = new List<BillsPayments>();
        public List<Bills> bills { get; set; } = new List<Bills>();
        public string UserID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }

    }

    

    public class AutomachtedBillsResponse
    {

    }

    public class MatchedBillDetails
    {
        public short BillType { get; set; }
        public string CorporationID {  get; set; } 
        public long BillInfoID {  get; set; } 
        public string JEID {  get; set; }   
        public string VendorName { get; set; }
        public string VendorID {  get; set; }   
        public DateTime? BillDate { get; set; }
        public DateTime? BooksDate { get; set; }
        public int PaymentType { get; set; }
        public string BillNumber { get; set; }
        public string PaymentMethodID {  get; set; }    
        public string PaymentMethodName { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public decimal BillAmount { get; set; }
        public decimal OutStandingAmount { get; set; }
        public string StoreID { get; set; } 

    }

    public class BillsToLinkDbResponse
    {
        public string VendorName { get; set; }
        public string RefType { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryNumber { get; set; }
        public string PayMethodID { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public decimal Outstanding { get; set; }
        public string PayMethodName { get; set; }
        public decimal PageTotalAmount { get; set; }
        public decimal GrandTotalAmount { get; set; }
    }
    public class BillsToLinkResponse : StatusDTO
    {
        public List<BillsToLinkDbResponse> billToLinkResponse { get; set; } = new List<BillsToLinkDbResponse> { };
    }

    public class MatchedBillsAndPaymentsResult
    {
        public Int64 PaymentID { get; set; }
        public int Status { get; set; }
        public string JounralID { get; set; }
    }

    public class AutoMatchedPaymentIDS : StatusDTO
    {
        public List<PaymentIDDBResponse> paymentids { get; set; } = new List<PaymentIDDBResponse>();

    }
    public class PaymentIDDBResponse
    {
        public long PaymentID {  get; set; } 
        public bool IsSelected { get; set; } = true;   
        public string CheckNumber { get; set; } = string.Empty; 
        public string BankAccountID { get; set; } = string.Empty;   
        public string PaymentMethodID { get; set; } = string.Empty; 
        public bool IsCheckNumberExist { get; set; }
        public string JEID { get; set; } = string.Empty;
    }

}
