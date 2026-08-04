using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillPaymentLoad
    {
        public int TransNumber { get; set; }
        public string CorpName { get; set; }
        public int PrintCount { get; set; }
        public int EnableCheckNumPrint { get; set; }
        public DateTime LockDate { get; set; }
        public string TransactionID { get; set; }
        public string JournalEntryID { get; set; }
        public string EntryNumber { get; set; }
        public string BillEntryNumber { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal HeadAmount { get; set; }
        public string AdjustTransactionID { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal AdjustmentAmountHide { get; set; }
        public decimal AmountDue { get; set; }
        public string AppliedAccountID { get; set; }
        public string AppliedTransactionID { get; set; }
        public decimal BillpaymentOutStanding { get; set; }
        public string CheckNo { get; set; }
        public DateTime ClearedDate { get; set; }
        public string DepositID { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime BillDate { get; set; }

        public string JBillDate { get; set; }
        public short HoldPayment { get; set; }
        public decimal InvoiceAmountDue { get; set; }
        public short IsAttachment { get; set; }
        public string IsLocked { get; set; }
        public string IsPrintCheck { get; set; }
        public string IsReconciled { get; set; }
        public bool IsVoided { get; set; }
        public string Memo { get; set; }
        public short NewUpdateType { get; set; }
        public string PCName { get; set; }
        public decimal PaidAmount { get; set; }
        public string PayeeID { get; set; }
        public string PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaytokenID { get; set; }
        public short PrintType { get; set; }
        public short ReferenceMode { get; set; }
        public string SourceID { get; set; }
        public short SourceType { get; set; }
        public short Status { get; set; }
        public string VendorName { get; set; }
        public string AccountNum { get; set; }
        public short isSelected { get; set; }
        public bool DebitCredit { get; set; }
        public short ReconStatus { get; set; }
        public string CorpID { get; set; }
        public short PayMethodSourceType { get; set; }
        public DateTime ReconciledDate { get; set; }
        public string AdjustmentTransactionID { get; set; }
        public string DiscountAccountID { get; set; }
        public string DiscountAccountName { get; set; }
        public string DiscountTransactionID { get; set; }
        public string BillEntryID { get; set; }
        public DateTime VoidDate { get; set; }
        public string VoidRemarks { get; set; }
        public bool PrintChkCon { get; set; }
        public short BillPaymentStatus { get; set; }

        public string PCID { get; set;}
        public decimal PCAmount { get; set;}

        public string ContractID { get; set; }

        public string PaymentName { get; set; }
    }
}
