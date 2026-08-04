using Common.Domain.DTO.Model.Base;
using Payable.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class EpaymentModels
    {
        public class PostPendingEPaymentList
        {


            [Required(ErrorMessage = "JournalId is required")]
            public string JournalEntryId { get; set; }
            public string VendorId { get; set; }
            public string VendorName { get; set; }

            [Required(ErrorMessage = "AccountId is required")]
            public string AccountId { get; set; }

            [Required(ErrorMessage = "CorporationId is required")]
            public string CorporationId { get; set; }

            public string CorporationName { get; set; }

            public string    AccountName { get; set; }

            public long ? FormatID { get; set; }

            public string FormatName { get; set; }
            public long? BatchNo { get; set; }

            public bool isChecked { get; set; }

            public decimal Amount { get; set; }

            public string CheckNo { get; set; }

            public string BillNumber { get; set; }

            public DateTime? paymentDate { get; set; }

            public int BillsCount { get; set; }
        }






        public class PostPendingEPaymentInfo
        {
            [Required(ErrorMessage = "IntiationDate is required")]
            public DateTime IntiationDate { get; set; }

            [Required(ErrorMessage = "PaymentType is required")]
            public short PaymentType { get; set; }

            public bool isExportLog { get; set; }
            public bool IsValidate { get; set; }
            public List<PostPendingEPaymentList> PendingEPayments { get; set; }
        }

        public class EPaymentInfo
        {
            public DateTime IntiationDate { get; set; }

            [Required(ErrorMessage = "PaymentType is required")]
            public short PaymentType { get; set; }
            public short ActionType { get; set; }

            public bool isExportLog { get; set; }
            public bool IsValidate { get; set; }
            public List<EPaymentDetails> EPaymentDetails { get; set; }

        }

        public class DDExportRequest
        {
            public DateTime IntiationDate { get; set; }
            public short PaymentType { get; set; }
            public short ActionType { get; set; }

            public bool isExportLog { get; set; }
            public string CreationNum { get; set; }
            public DateTime CreationDate { get; set; }
            public List<DDExportDetails> ExportDetails { get; set; }

            public bool IsCreated { get; set; }

        }

        public class DDExportDetails
        {
            [Required]
            public string JournalEntryId { get; set; }
            [Required]
            public long BatchId { get; set; }
          
            public string AccountId { get; set; }

            public string CorpId { get; set; }
            public string VendorId { get; set; }
            public decimal Amount { get; set; }
            //[Required]
            public string CheckNo { get; set; }

            public string BatchNo { get; set; }

        }
        public class PaymentInfoList
        {
            public decimal Amount { get; set; }

            public string JournalEntryId { get; set; }

            public string CorporationID { get; set; }

            public string TransactionId { get; set; }

            public string VendorId { get; set; }

            public string AccountId { get; set; }

            public string PaymentMethodID { get; set; }

            public string BillNumber { get; set; }

            public short PaymentStatus { get; set; }

            public DateTime EntryDate { get; set; }
        }
        public class EPaymentResponse : StatusDTO
        {
            public short PaymentType { get; set; }
            public DateTime InitiationDate { get; set; }

            public bool isExportLog { get; set; }
            public bool isExportReport { get; set; }
            public long ExportLogRefId { get; set; } = 0;
            public EFTExportResponse eftResponse { get; set; }
            public List<EPaymentDetails> EPaymentDetails { get; set; }
            public List<DirectDepositExportResponse> DirectDepositExportResponse { get; set; }

            public EPayEmailRequest EPayEmailReq { get; set; }


        }
        

        public class EPaymentDetails
        {
            [Required]
            public string JournalEntryId { get; set; }
            [Required]
            public string CorpId { get; set; }
            [Required]
            public long BatchId { get; set; }
            [Required]
            public bool isChecked { get; set; }

            public short PaymentStatus { get; set; }
            public string VendorId { get; set; }
            public string VendorName { get; set; }
            public string AccountId { get; set; }

            public string CorpName { get; set; }

            public string AccountName { get; set; }

            public long BatchNo { get; set; }

            public DateTime? InitiationDate { get; set; }

            public decimal Amount { get; set; }

            public string CheckNo { get; set; }

            public int NumberOfPayments { get; set; }

           // public DateTime IntiationDate { get; set; }

            public string FormatName { get; set; }

            public bool isCreated { get; set; }

            public long Id { get; set; }

            public string BillNumber { get; set; }
        }


        public class DirectDepositExportRequest
        {

            public DateTime? FundAvailableDate { get; set; }

            public string CreationNum { get; set; }

            public DateTime CreationDate { get; set; }

            public bool  IsExportLog { get; set; }

            public List<EPaymentDetails> exportList { get; set; }

        }

        public class DirectDepositExportResponse:StatusDTO
        {
            public string FileText { get; set; }

            public string FileName { get; set; }

        }

        public class EPayLogRequest
        {
            public string CorpIds { get; set; }

            public string VenorId { get; set; }

            public string BankAccount { get; set; }
            /// <summary>
            /// 1=Repay,2=DD-Manual,3=DD-API,4=EFT
            /// </summary>
            public short EPaymentType { get; set; }

            public short PaymentMethod { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }
        }

        public class EPayLogResponse
        {

            public long LogId { get; set; }

            public long  BatchNo { get; set; }

            public string PaymentType { get; set; }

           // public byte[] CorporationId { get; set; }

            public string CorporationName { get; set; }

            public string LegalName { get; set; }

           // public byte[] AccountId { get; set; }

            public string  AccountName { get; set; }

            public DateTime PaymentDate { get; set; }

           // public byte[] PaymentId { get; set; }

            public string PaymentMethod { get; set; }

           // public byte[] VendorId { get; set; }

            public string VendorName { get; set; }

           

            public string Clr { get; set; }

            public decimal Amount { get; set; }

           // public byte[] CreatedUser { get; set; }

            public string CreatedBy { get; set; }

           // public byte[] ModifiedUser { get; set; }

            public string ModifiedBy { get; set; }

           // public byte[] InitiatedUser { get; set; }

            public string InitiatedBy { get; set; }

            public DateTime InitiationDate { get; set; }

           // public short Status { get; set; }

            public short StatusMsg { get; set; }

            public string PaymentStatus { get; set; }

            public int? ExportCount { get; set; }
        }

        public class EPayEmailPreference
        {
            public bool userEmailAccess { get; set; } = false;
            public bool clientEmailAccess { get; set; } = false;

            public string UserId { get; set; }

            public string ClientId { get; set; }
        }

        public class EPayEmailRequest 
        {
            public string EmailBody { get; set; }

            public string clientEmail { get; set; }

            public string userEmail { get; set; }

            public string Subject { get; set; }
            public bool IsEmailEnable { get; set; }

        }

        public class DDLogDetails
        {
            public long CreationNum { get; set; }

            public DateTime CreatedDate { get; set; }

            public string ExportedBy { get; set; }

            public decimal Amount { get; set; }

            public string Vendor { get; set; }

        }
        public class EPayTempRef
        {
            public long Id { get; set; }
        }
        public class EPayExportDetails
        {
           
            public string JournalEntryId { get; set; }
         
            public long BatchId { get; set; }

            public string AccountId { get; set; }

            public string CorpId { get; set; }

            public string CorpName { get; set; }

            public string LegalName { get; set; }
            public string VendorId { get; set; }
            public decimal Amount { get; set; }
          
            public string CheckNo { get; set; }

            public long BatchNumber { get; set; }

            public DateTime IntiationDate { get; set; }

            public string VendorName { get; set; }

            public string BankAccount { get; set; }

        }

        public class EPayCheckProviderPreference
        {
            public int EPayType { get; set; }

            public long CorporationsCount { get; set; }
        }
        public class EPayLogTempDetails
        {
            public string CorporationId { get; set; }

            public string BatchId { get; set; }
            public int? PaymentType { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
        public class EPayCheckProviderModel
        {
            public List<EPayCheckProviderPreference> ProviderList { get; set; }
        }

        public class EpayReexportReq
        {
            public long? Id { get; set; }

            public long? MemberId { get; set; }

            public long? BatchNo { get; set; }

            public string PaymethodId { get; set; }

            public string CorpIds { get; set; }

            public int? PayType { get; set; }
        }
        public class EPayCorpList
        {
            public string CorpList { get; set; }
        }

        public class GetRepayProcessedList
        {
            public DateTime StDate { get; set; }

            public DateTime EdDate { get; set; }


        }

        public class GetRepayProcessedListResponse
        {
            public string    PaymentId { get; set; }

            public string PaymentGroupId { get; set; }

            public string PaymentGroupName { get; set; }

            public string PaymentMethodType { get; set; }

            public string PaymentStatus { get; set; }
        }
    }
}
