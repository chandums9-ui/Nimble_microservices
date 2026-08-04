using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BulkBillPayment
    {
        [Required(ErrorMessage = "Payment method is required")]
        public string PaymentMethodID { get; set; }
        [Required(ErrorMessage = "Payment date is required")]
        public DateTime? BillPaymentDate { get; set; }
        [Required(ErrorMessage = "Bank account is required")]
        public string BankAccount { get; set; }
        public int CheckPrintingType { get; set; }

        public string CheckNumber { get; set; }
        public bool IsCheckNumebValidationReq { get; set; } = false;
        public string OldCheckNumber { get; set; }

        public bool ToBePrinted { get; set; }
        public bool isApprovedBillPayment { get; set; } = false;
        public bool PrintCheckNow { get; set; }
        public List<QuickBillDTO> JIDsList { get; set; }
        public int BillPayType { get; set; }
        public long JournalPaymentId { get; set; }
        public decimal PaymentAmount { get; set; }  
        public string PaymentMemo { get; set; } 
    }
}
