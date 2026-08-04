using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http.HttpResults;
using Payable.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class EFTExportFileDate
    {
        public string FileName { get; set; }
        public byte[] FileDate { get; set; }
    }
    public class EFTExportResponse:StatusDTO
    {
        public List<EFTExportFileDate> Files =new List<EFTExportFileDate>();
    }
    public class EFTExportRequest
    {
        public long BatchId { get; set; }

        public string JId { get; set; }

        public bool IsCreated { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
    public class ConfigAccountsResponse:StatusDTO
    {
        public List<AccountDTO> Accounts { get; set; }=new List<AccountDTO>();
    }
    public class ConfigResponse:StatusDTO
    {
        public long LongId { get; set; }
        public string StringId { get; set; }
    }
    public class LoadEPayConfigResponse:StatusDTO
    {
        public List<EPayData> EPayData { get; set; } =new List<EPayData>();
        public List<EPayCardData> EPayCards { get; set; } = new List<EPayCardData>();
    }
    public class LoadProviderConfigResponse:StatusDTO
    {
        public Repayconfig Repay {  get; set; } 
        public DDConfig DiretDeposit { get; set; }
        public EFTConfig EFTConfig { get; set; }
    }
    public class LoadProviderAuditResponse:StatusDTO
    {
        public string CorporationName { get; set; }
        public string CorporationLegalName { get; set; }
        public string ProviderName { get; set; }
        public int ProviderType { get; set; }
        public string PaymentAccount { get; set; }
        public string CreatedBy { get; set; }
        public List<RepayAuditData> RepayAudit { get; set; }=new List<RepayAuditData>();
        public List<DDAuditData> DDAudit { get; set; } =new List<DDAuditData>();
        public List<EFTAuditData>EFTAudit { get; set; }=new List<EFTAuditData>();
    }
    public class LoadProviderAuditData
    {
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
        public int ProviderType { get; set; }
        public string Provider { get; set; }
		public string AccountID { get; set; }
		public string Account { get; set; }
		public string CreatedByID { get; set; }
		public string CreateBy { get; set; }
		public string CreatedDate { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string CorporationLegalName { get; set; }
        public string BankName { get; set; }
        public string OriginatorNo { get; set; }
        public string DataCenterNo { get; set; }
        public int FormatID { get; set; }
        public string FormatName { get; set; }
        public int ActionType { get; set; }
        public string Action { get; set; }
        public string ActionByID { get; set; }
        public string ActionBy { get; set; }
        public string ActionDate { get; set; }
        public string UpdatedColumns { get; set; }
        public bool IsUpdated { get; set; }

        public long TotalCount { get; set; }
        public int MaxRows { get; set; }

        public string CorpLegalName { get; set; }
    }
    public class EFTAuditData
    {
        public string FormatName { get; set; }
        public ActionData Action { get; set; }
        public bool IsUpdated { get; set; }
        public long TotalCount { get; set; }
        public int MaxRows { get; set; }


    }
    public class RepayAuditData
    {
        public string ClientId { get; set; }
        public string ClientSecretId { get; set; }
        public ActionData Action { get; set; }
        public string UpdatedColumns { get; set; }
        public long TotalCount { get; set; }
        public int MaxRows { get; set; }
    }
    public class ActionData
    {
        public int ActionType { get; set; }
        public string Action { get; set; }
        public string ActionBy { get; set; }
        public string ActionDate { get; set; }
    }
    public class DDAuditData
    {
        public string CorporationLegalName { get; set; }
        public string BankName { get; set; }
        public string OriginatorNo { get; set; }
        public string DataCenterNo { get; set; }
        public ActionData Action {  get; set; }
        public string UpdatedColumns { get; set; }
        public long TotalCount { get; set; }
        public int MaxRows { get; set; }

    }
    public class EPayCardData
    {
        public string Provider { get; set; }
        public int ProviderType { get; set; }
        public long TotalCount { get; set; }
    }
    public class EPayData
    {
        public string EntryIDBIN {  get; set; }
        public long EntryIDLong { get; set; }
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
        public string ProviderName { get; set; }
        public int ProviderType { get; set; }
        public string PaymentAccountID { get; set; }
        public string PaymentAccount { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string LocationID { get; set; }
        public long TotalCount { get; set; }
        public long MaxRows { get; set; }
        public int Status  {  get; set; }
    }
    public class DDConfig
    {
        public string ID { get; set; } = string.Empty;
        public string CorporationID { get; set; } = string.Empty;
        public string CorporationName { get; set; } = string.Empty;
        public string AccoutnID { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string CorpLegalName { get; set; } = string.Empty;
        public string CorpShortName { get; set; } = string.Empty;
        [DefaultValue(1)]
        public int BankTemplate { get; set; } = 1;
        public string BankName { get;set; } = string.Empty;
        public string OriginatorNo { get; set; } = string.Empty;
        public string InstitutionNo { get; set; } = string.Empty;
        public string AccountNo { get; set; } = string.Empty;
        public string DataCentreNo { get; set; } = string.Empty;
        public string ReservedFields { get; set; } = string.Empty;
        public string SettlementCode { get; set; } = string.Empty;
        [DefaultValue("460")]
        public string PayableCode { get; set; } = "460";
        [DefaultValue(1)]
        public int BankPaymenntsIn { get; set; } = 1;
        public int? DDType { get; set; }
        public string BankPaymentsInName { get; set; } = string.Empty;
        [DefaultValue(false)]
        public bool HasAttachments { get; set; } = false;
        public string UpdatedColumns {  get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
    public class EPaymentsCountResponse : StatusDTO
    {
        public string EPaymentName { get; set; }
        public short? PaymentMethodType { get; set; }
        public int PaymentsCount { get; set; }
        public short? Status { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class EPaymentsPendingAmountResponse : StatusDTO
    {
        public long TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class EPaymentsBatchNoResponse 
    {
        public long Id { get; set; }
        public string CorpID { get; set; }
        public string AccountId { get; set; }
        public short PaymentMethod { get; set; }
        public string ClientId { get; set; }
        public short? IssueType { get; set; }
        public long BatchNo { get; set; }
        public short? Status { get; set; }

        public string StatusMsg { get; set; }
        public string? FormatName { get; set; }
        public long? FormatId { get; set; }

        public long? EpayBatchId { get; set; }
    }
    public class EPaymentsMultipleBatchNoResponse
    {
        public List<EPaymentsBatchNoResponse> EPaymentsBatchNoList { get; set; }
    }
    public class LoadEFTFormatList:StatusDTO
    {
       public List<EFTFormat> EFTFormats { get; set; } = new List<EFTFormat>();
    }
    public class EFTFormat
    {
        public long Id { get; set; }
        public string FormatName { get; set; }
        public short? Status { get; set; }
    }
    
}
