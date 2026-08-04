using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class QuickBillDTO
    {
        [Required]
        public string JId { get; set; }
        public long BillInfoID { get; set; } 
        public string BillDate { get; set; }
        [Required]
        public string Corporation { get; set; }
        

        public string CorporationName { get; set; }
        public string VendorId { get; set; }
        public string BooksDate { get; set; }
        public string VendorName { get; set; }
        public string PaymentMethod { get; set; }
        public int? PaymentType { get; set; }
        public string BillNumber { get; set; }
        public string Status { get; set; }
        public string BankAccountID { get; set; }
        public string AssignedUserID { get; set; }
        public string? AssignedTo { get; set; }
        public string PreviousApprovalUserID { get; set; }
        public string PreviousApprovalUserName { get; set; }
        public bool IsHold { get; set; }
        public bool IsVoid { get; set; }

        public bool IsSelectBill { get; set; }
        public bool UserHasAccess { get; set; }
        public bool BillPayUserHasAccess { get; set; }
        public bool IsLock { get; set;}
        public bool IsReconciled { get; set; }
        public int ApprovalOrder {  get; set; }
        public int ApprovalType { get; set; }
        public int Index { get; set; }
        public bool IsEditAccess { get; set; }
    }
}
