using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillEntryTableCols
    {
        public string? Type { get; set; }
        public DateTime? Date { get; set; }
        public string? Corporation { get; set; }
        public long? Number { get; set; }
        public string? PayeeName { get; set; }
        public string? AccountNumber { get; set; }
        public string? PayMethod { get; set; }
        public string? Memo { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Status { get; set; }
        public string? Pay { get; set; }
        public string? Action { get; set; }
        public DateTime? BillDate { get; set; }
        public string? ReceivedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AssignedTo { get; set; }
        public string? Aprove { get; set; }
        public DateTime? CreditDate { get; set; }
        public string? View { get; set; }
    }
}
