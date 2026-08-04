using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Model
{
    public class BillPaymentDTO : ModelBaseCorporationID
    {
        public string PayeeID { get; set; }
        public string PaymentMethodID { get; set; }
        public string BankAccountID { get; set; }
        public string PayeeAccountID { get; set; }
        public decimal Amount { get; set; }
        public string CheckNumber { get; set; }
        public string? Memo { get; set; }
        public string? ReferenceNumber { get; set; }
        public string EntryDate { get; set; }
        public string ClearedDate { get; set; } 
        public long BankTransRefID { get; set; }
        public bool HasAttachments { get; set; } = false;
        public string ClientID { get; set; }
        public long BillInfoID { get; set; }
        public bool? IsPossiblematch { get; set; }
    }
}
