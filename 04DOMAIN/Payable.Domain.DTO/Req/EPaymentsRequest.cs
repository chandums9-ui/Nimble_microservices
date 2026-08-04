using Common.Domain.DTO.Model.Base;
using Payable.Domain.DTO.Enums;
using Payable.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    public class LoadEPayConfigRequest
    {
        public string CorpID { get; set; }
        /// <summary>
        /// EpayProviderTypesEnum
        /// </summary>
        public short ProviderType { get; set; } = (short)EpayProviderTypesEnum.All;
        public bool IsActiveLog { get; set; } = true;
        public int PageNumber { get; set; } = 0;

    }
    public class LoadProviderRequest
    {
        public long ID { get; set; }
        public string BinId { get; set; }
        public int ProviderType {  get; set; }
        public bool IsFromGrid { get; set; } = false;
        public int PageNumber { get; set; } = 0;
    }
    public class SaveConfigRequest
    {
        public int ProviderType { get; set; }
        public Repayconfig RepayConfig { get; set; }
        public DDConfig DDConfig { get; set; }
        public EFTConfig EFTConfig { get; set; }
    }
    public class EFTFormatConfig : StatusDTO
    {
        public long ID { get; set; }
        public string FormatName { get; set; }
        public List<EFTFormatFields> PaymentDetails { get; set; }=new List<EFTFormatFields>();
        public List<EFTFormatFields> VendorDetails { get; set; } = new List<EFTFormatFields>();
        public List<EFTFormatFields> PayorDetails { get; set; } = new List<EFTFormatFields>();

    }
    public class EFTFormatFields 
    {
        public long ID { get; set; }
        public long ColumnID { get; set; }
        public string FieldName { get; set; }
        [DefaultValue(false)]
        public bool IsIncluded { get; set; } = false;
        public short ExcelCol { get; set; }
        public int Type { get; set; }
        public int order {  get; set; }
    }
    public class EFTExportData
    {
        public List<long?> BacthID { get; set;}=new List<long?>();
    }
    public class EFTExcelData
    {
        public long BatchNum { get; set; }
        public string PaymentNo { get; set; }
        public string VendorID { get; set; }
        public string PayeeName { get; set; }
        public decimal PaymentAmount { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal OriginalInvoiceAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetInvoiceAmount { get; set; }
        public string DetailsDescription { get; set; }
        public string AccountNumber { get; set; }
        public string AttentionLine { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PayorName { get; set; }
        public string PayorLegalName { get; set; }
        public string PayorAccountNumber { get; set; }
        public string PayorAddress1 { get; set; }
        public string PayorAddress2 { get; set; }
        public string PayorCity { get; set; }
        public string PayorState { get; set; }
        public string PayorZIP { get; set; }
        public string LocationID { get; set; }
        public string BankID { get; set; }
    }
    public class EFTConfig
    {
        public long EFTId { get; set; }
        public string CorporationId { get; set; }
        public string CorporationName { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public long? EFTFromatId { get; set; }
        public string FormatName { get; set; } = string.Empty;
        public bool ColumnsUpdated { get; set; } = false;
        public int isActive { get; set; }
    }
    public class Repayconfig
    {
        public long EntityId { get; set; }
        public string CorporationId { get; set; }
        public string PaymentAccountId { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecretId { get; set; } = string.Empty;
        public string UpdatedColumnIds { get; set; } = string.Empty;
        public string AccountName { get; set; }
        public bool Status { get; set; }
    }

    public class EPaymentCountRequest
    {
        public string CorpIDs { get; set; }
        public string VendorID { get; set; }
        public string PaymentIDs { get; set; }
        public DateTime?FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long? BatchNo { get; set; }


    }
    public class EPaymentBatchNoGenerationRequest
    {
        public string CorpID { get; set; }
        public string AccountID { get; set; }
        public short PayMethodType { get; set; }
        public string PaymentMethod { get; set; }
        public short? IssueType { get; set; }
        public string ClientId { get; set; }
        public short? Type { get; set; }
        public short? IsIncrement { get; set; } = 0;
        public long? BatchNo { get; set; }
        public long? FormatId { get; set; }
    }
    public class EPaymentMultipleBatchNoGenerationRequest:StatusDTO
    {
        public  List<EPaymentBatchNoGenerationRequest> BatchNoReqList  { get;set;}=new List<EPaymentBatchNoGenerationRequest>();
    }
    public class EFTBatchRequest
    {
        public string CorpId { get; set; }

        public string AccountId { get; set; }

        public long FormatId { get; set; }
    }

    public class EPayCheckNoRequest
    {
        public string CorpID { get; set;}

        public string AccountID { get; set; }

        public string PaymentMethodId { get; set; }

        public string UserId { get; set; }

        public string CheckNo { get; set; }

        public short Type { get; set; } = 0;

        public short PayMethodType { get; set; }
    }

    public class EPayCheckValidate
    {
        public string CorpID { get; set; }

        public string AccountID { get; set; }

        public string PaymentMethodId { get; set; }

        public string UserId { get; set; }

        public string CheckNo { get; set; }

        public short PayMethodType { get; set; }
    }
    public class EPayUpdateCheckNo
    {
        public string CorpID { get; set; }

        public string AccountID { get; set; }

        public string PaymentMethodId { get; set; }

        public string UserId { get; set; }

        public string JournalEntryId { get; set; }

        public string CheckNo { get; set; }

        public int? IsIncrement { get; set; }

  
    }
    public class EPayCheckValidateResp
    {
        public long? ID { get; set; }


        public long? CheckNo { get; set; }

        public string CorpID { get; set; }

        public string AccountID { get; set; }

        public short PaymentMethodId { get; set; }

    }

    public class EPayCheckNoResponse
    {
        public long  ID { get; set; }


        public string CheckNo { get; set; }

        public string CorpID { get; set; }

        public string AccountID { get; set; }

        public short PaymentMethodId { get; set; }

    }
    public class BatchIdsRequest
    {
        public List<long> BatchNo { get; set; }
        public Dictionary<string,long> SelectedBills { get; set; }
        public string clientId { get; set;}

        public string userId { get; set; }


    }
    public class BatchIdsList
    {

        public long BatchNumber { get; set; }

    }

    public class CheckDDConfig
    {

        public long ID { get; set; }

    }
    public class CheckRepayConfig
    {

        public long Id { get; set; }

    }
    public class EPayGroupAccounts
    {
        public string AccountId { get; set; }

        public string CorpId { get; set; }

        public string Jid { get; set; }
    }
}
