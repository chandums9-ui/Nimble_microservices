using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class LoadEPaymentsRequest
    {
        [Required(ErrorMessage = "Select atleast one corporation")]
        public string CorpIds { get; set; }
        public string VenId { get; set; }
      

        public short ActionType { get; set; }

        [Required(ErrorMessage = "PaymentMethod is required")]
        public short PaymentType { get; set; }
       // [Required(ErrorMessage = "Start date is required")]
        public DateTime? StartDate { get; set; }
       // [Required(ErrorMessage = "End date is required")]
        public DateTime? EndDate { get; set; }

        public int PageNo { get; set; }

        public int FilterType { get; set; }

        public long? BatchNo { get; set; }

        public short? Status { get; set; }// this used for repay

        public string SearchText { get; set; }
    }

    public class LoadEPaymentsResponse
    {

         public long BatchId { get; set; }
        public string JournalEntryId { get; set; }

        public string CorporationID { get; set; }
        public string TransactionId { get; set; }

        public long BatchNumber { get; set; }
        public int ExportCount { get; set; }

        public int MasterExportCount { get; set; }
        public string AccountId { get; set; }

        public string CorpName { get; set; }

        public string CorpLegalName { get; set; }

        public string CreatedUserName { get; set; }

        public string BankAccountName { get; set; }
        public DateTime CreatedDate { get; set; }

        public decimal Amount { get; set; }

        public int PaymentStatus { get; set; }

        public string BillPaymentStatus { get; set; }
        public string VendorName { get; set; }

        public string VendorId { get; set; }

        public string BillNumber { get; set; }

        public DateTime? BillDate { get; set; }
        public DateTime? BooksDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        public string PaymentName { get; set; }
        public  long RowNum { get; set; }

        public long BillsCount { get; set; }

        public DateTime? IntiationDate { get; set; }

        public string FormatName { get; set; }

        public string CheckNo { get; set; }

        public long TotalRecordsCount { get; set; }

        public int DefaultPageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public byte? DDType { get; set; }

        public string RefNumber { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string BillInfoJournalId { get; set; }

        public bool? IsBillDateEnable { get; set; }

        public string RepayPaymentStatus { get; set; }
    }

    public class LoadMasterEPaymentsResponse
    {
        public long BatchId { get; set; }
        public long BatchNumber { get; set; }
        public string CorporationID { get; set; }

        public string AccountId { get; set; }

        public int ExportCount { get; set; }
        public string AccountName { get; set; }
        public string CorpName { get; set; }

        public string CorpLegalName { get; set; }

        public decimal  TotalAmount { get; set; }

        public string CreatedUserName { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsVisible { get; set; } = false;

        public long TotalRecordsCount { get; set; }
        public int DefaultPageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<LoadEPaymentsResponse> LoadChildEPayments { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime? InitiateDate { get; set; }

        public bool? IsBillDateEnable { get; set; }

    }

    public class LoadEPaymentsInfo:StatusDTO
    {
        public List<LoadMasterEPaymentsResponse> MasterLoadEPayments { get; set; }
    }
}
