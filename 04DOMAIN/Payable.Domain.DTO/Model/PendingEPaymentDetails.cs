using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class EPaymentPendingDetails
    {
        public string JournalEntryId { get; set; }
        public short PaymentType { get; set; }
    }

    public class PendingEPaymentsRequest
    {
        [Required (ErrorMessage ="Select atleast one corporation")]
        public string CorpIds { get; set; }
        public string VenId { get; set; }
        [Required(ErrorMessage ="PaymentMethod is required")]
        public short PaymentType { get; set;}
       
        public DateTime? StartDate { get; set; }
       
        public DateTime? EndDate { get; set; }

        public int PageNo { get; set; } = 0;

        public int FilterType { get; set; }
    }
    public class PendingEPaymentsResponse
    {
        public long Id { get; set; }
        public string JournalEntryId { get; set; }

        public string TransactionId { get; set; }
        public string CorporationID { get; set; }

        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string CorpName { get; set; }

        public string CorpLegalName { get; set; }

        public string VendorName { get; set; }
        public string VendorId { get; set; }
        public string BankAccountName { get; set; }

        public string BankAccountId { get; set; }
        public decimal Amount { get; set; }

        public DateTime? DueDate { get; set; }

        public string BillNumber { get; set; }

        public long RowNum { get; set; }

        public long BillsCount { get; set; }

        public string PaymentStatus { get; set; }

        public long TotalRecordsCount { get; set; }

        public int DefaultPageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long FormatID { get; set; }
        public string FormatName { get; set; } = string.Empty;

        public string BillInfoJournalId { get; set; }
    }

    public class PendingEPaymentsInfo : StatusDTO
    {
       public List<PendingEPaymentsResponse> PendingEPayments { get; set; } 
    }

    public class EPayCorporationList
    {
        public string CorporationID { get; set; }

        public string CorporationName { get; set; }

        public string LegalName { get; set; }

        public string PropertyType { get; set; }

        public string Service { get; set; }

        public string Brand { get; set; }

        public string PMS { get; set; }
    }

    public class CorpLegalNameCheck
    {
        public bool? IsCorpLegalNameShow { get; set; }

   
    }
}
