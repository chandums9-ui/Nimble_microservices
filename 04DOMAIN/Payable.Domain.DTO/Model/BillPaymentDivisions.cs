using Common.Domain.DTO.Enums;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillPaymentDivisions
    {
        public long Id { get; set; }
        public string JEID { get; set; }
        public string StoreID { get; set; }
        public long BillInfoId { get; set; }

        public string PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public int PaymentMethodType { get; set; }
        public string CorpID { get; set; }
        public string CorpName { get; set; }
        public string PCName { get; set; }

        public string PayeeID { get; set; }
        public string PayeeName { get; set; }
        public string ContractID { get; set; }
        public string AccountNumber { get; set; }
        public DateTime BooksDate { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string BillNumber { get; set; }
        public decimal Amount { get; set; } = 0.00M;
        public decimal AmountDue { get; set; } = 0.00M;
        public decimal DiscountAmount { get; set; } = 0.00M;
        public string DisplayDiscountAmt { get; set; }
        [DefaultValue(null)]
        public string DiscountAccount { get; set; }
        public decimal DiscountAmount1 { get; set; }
        public string DiscountAccountName { get; set; }
        public decimal AmountPaid { get; set; } = 0.00M;
        public string DisplayAmountPaid { get; set; }
        public string TID { get; set; }
        public string DTID { get; set; }
        public short BillPaymentStatus { get; set; }
        public bool IsDebitMemo { get; set; }
        public bool IsChecked { get; set; }

        public bool IsSubDivisionEnable { get; set; } = false;
        public decimal AmountPaid1 { get; set; } = 0.00M;
        public bool IsEditBill { get; set; } = false;
        public bool IsBillSelected { get; set; }
        public string DiscountMemo { get; set; }
        public bool IsDiscountDefaultAccount { get; set; } = false;
        public bool Pending { get; set; } = true;
        public List<PCDvisions> SubDivisions { get; set; } = new List<PCDvisions>();
        public bool HasAttachments { get; set; }
        public bool IsHoldBill { get; set; }

        public short RefType { get; set; }

        public int PageCount { get; set; }

        public bool IsDisable { get; set; }

        public decimal TotalAmountDue { get; set; }
        public decimal AllPageAmountDue { get; set; }
        public decimal AllPageBillAmount { get; set; }
        public bool IsDebitMemoAvailable { get; set; }
        //public string CheckMemo { get; set; }
        //public short SourceType { get; set; }

        public bool IsContractwisePay { get; set; }

        public int RowOrder { get; set;}

    }

    
    public class PCDvisions
    {
        public long Id { get; set; }

        public long BillInfoID { get; set; }
        public string PCName { get; set; }
        public string StoreID { get; set; }
        public decimal Amount { get; set; }
        public string DisplayAmountPaid { get; set; }

        public decimal AmountDue { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DisplayDiscountAmt { get; set; }

        public string DiscountAccount { get; set; }

        public decimal AmountPaid { get; set; }

        public string PCTID { get; set; }
    }
}
