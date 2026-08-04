using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Payable.Domain.DTO.Req
{
    public class GetVendorNamesByUserOrCorpRequest
    {
        public string CorpID { get; set; }
        public bool GetBalance {  get; set; }
        [DefaultValue(false)]
        public bool GetInActiveVendors { get; set; }=false;
    }
    public class LoadPayMethodsReq
    {
        public string? CorpID { get; set; }
        public string? AccountID { get; set; }
        public bool IsRequiredCreditDays { get; set; }
        public bool isPaymentMethodsReq {  get; set; }  
    }
    public class CorporationLockReq()
    {
        public string CorpID { get; set; }
        public string BooksDate { get; set; }
        public short Type { get; set; }
    }
    public class GetVendorsContractRequest
    {
        public List<string> ReferenceID { get; set; } = new List<string>();
    }

    public class CommonDataDTO
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string ReferenceID { get; set; }
        public string ReferenceCode { get; set; }
        public short Type { get;set; }
    }
    public class FrequencyDTO : CommonDataDTO
    {
        public int? Interval { get; set; }
    }
    public class MiscInfoDTO : CommonDataDTO
    {
        public string Description { get; set; }
        public int SourceType { get; set; }
    }

    public class AdressDTO : CommonDataDTO
    {
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int StateID { get; set; } = 0;
        public string StateName { get; set; } = string.Empty;
        public int CountryID { get; set; } = 1;
        public string CountryName { get; set; }
        public string ZipCode { get; set; } = string.Empty;
    }

    public class ContactDTO : CommonDataDTO
    {
        public string MobileNum { get; set; } = string.Empty;
        public string AlternativeNum { get; set; } = string.Empty;
        public string WorkNum { get; set; } = string.Empty;
        public string FaxNum { get; set; } = string.Empty;
        public string EmailID { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;

    }

    public class VendorAddressDTO
    {
        public long ID { get; set; }
        public string? BusinessID { get; set; }
        [DefaultValue(null)]
        public string? AdressID { get; set; }
        [DefaultValue(null)]
        public string? ContactID { get; set; }
        [DefaultValue(null)]
        public string? BusinessTypeID { get; set; }
        public string? BusinessTypeName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }

    public class VendorAdress : VendorAddressDTO
    {
        //public AddressDTO Address {  get; set; }
        //public ContactDTO Contact { get; set; }
        [DefaultValue(null)]
        public string? AddressName { get; set; }
        [DefaultValue(null)]
        public string? Adress { get; set; }
        [DefaultValue(null)]
        public string? City { get; set; }
        [DefaultValue(null)]
        public string? StateID { get; set; }
        public string? StateName { get; set; }
        public string StateCode { get; set; }
        [DefaultValue(1)]
        [Range(1, long.MaxValue, ErrorMessage = "The country ID field is mandatory.")]
        public long CountryID { get; set; } = 1;
        public string? CountryName { get; set; }
        public string CountryCode { get; set; }
        [DefaultValue(null)]
        public string? ZipCode { get; set; }
        [DefaultValue(null)]
        public string? MobileNum { get; set; }
        [DefaultValue(null)]
        public string? AlternativeNum { get; set; }
        [DefaultValue(null)]
        public string? WorkNum { get; set; }
        [DefaultValue(null)]
        public string? FaxNum { get; set; }

        
        [DefaultValue(null)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please Enter a Valid Email")]
        public string? EmailID { get; set; }
        [DefaultValue(null)]
        public string? Website { get; set; }
        [DefaultValue(null)]
        public string AddressString { get; set; }
        [DefaultValue(false)]
        public bool IsUsedByContract { get; set; }
    }

    public class VendorContractDTO
    {
        [DefaultValue(null)]
        public string? ContractID { get; set; }
        [DefaultValue(null)]
        public string? BusinessID { get; set; }
        [DefaultValue(null)]
        public string? AccountNumber { get; set; }
        [DefaultValue(null)]
        public DateTime? ContractStartDate { get; set; } = null;
        [DefaultValue(null)]
        public DateTime? ContractExpiryDate { get; set; } = null;
        public string? VendorAdressID { get; set; }
        [DefaultValue(null)]
        public string ContractAddress { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        [DefaultValue(null)]
        public string? Notes { get; set; }
        [DefaultValue(null)]
        public string? AttachmentPath { get; set; }
        [DefaultValue(false)]
        public bool HasAttachments { get; set; }
        public List<VendorTaxInfoDTO> VendorTaxInfo { get; set; } = new List<VendorTaxInfoDTO>();
    }

    public class VendorTaxInfoDTO
    {
        public long? TaxInfoID { get; set; }
        [DefaultValue(null)]
        public string? VendorID { get; set; }
        [DefaultValue(null)]
        public string? NimbleAccountID { get; set; }

        [DefaultValue(null)]
        public string? NimbleAccountName { get; set; }
        public decimal? TaxRate { get; set; }
        public int LineOrder { get; set; } = 1;
        public short Status { get; set; }
        [DefaultValue(null)]
        public string? ContractID { get; set; }

        //Purpose Details
        public string PurposeID { get; set; }
        public string PurposeName { get; set; }
        public string PurposeAccountId { get; set; }
        public string PurposeAccountName { get; set; }
        public string PurposeAccountTypeID { get; set; }
        public string PurposeAccoutnTypeName { get; set; }
    }

    public class BusinessDTO : CommonDataDTO
    {
        public short? Type { get; set; }
        [DefaultValue(null)]
        public string? CompanyName { get; set; }
        public string? CorporationID { get; set; }
        public string? AccountID { get; set; }
        [DefaultValue(true)]
        public bool VendorStatus { get; set; }
        public BusinessInfoDTO BusinessInfo { get; set; } = new BusinessInfoDTO();
        public VendorDirectDepositDTO DirectDepositDetails { get; set; } = new VendorDirectDepositDTO();

    }
    public class BusinessInfoDTO
    {
        [DefaultValue(null)]
        public string? FederalID { get; set; }
        [DefaultValue(null)]
        public string? PrintCheckAs { get; set; }
        [DefaultValue(null)]
        public string? Print1099As { get; set; }
        [DefaultValue(null)]
        public string? DoingBusinessAs { get; set; }
        /// <summary>
        /// default is Mr. which equals 1
        /// </summary>
        public short Salutation { get; set; } = 1;
        [DefaultValue(null)]
        public string? FirstName { get; set; }
        [DefaultValue(null)]
        public string? MiddleName { get; set; }
        [DefaultValue(null)]
        public string? LastName { get; set; }
        [DefaultValue(null)]
        public string? CreditDaysID { get; set; }
        [DefaultValue(null)]
        public string? CreditDaysName { get; set; }
        [DefaultValue(null)]
        public string? SendMethodID { get; set; }
        [DefaultValue(null)]
        public string? SendMethodName { get; set; }
        [DefaultValue(null)]
        public string? PaymentMethodID { get; set; }
        [DefaultValue(null)]
        public string? PaymentMethodName { get; set; }
        [DefaultValue(null)]
        public string? CreditCarNo { get; set; }
        [DefaultValue(null)]
        public string? CreditCardExpiryDate { get; set; }
        [DefaultValue(null)]
        public string? NameonCredit { get; set; }
        [DefaultValue(null)]
        public string? Notes { get; set; }
        [DefaultValue(false)]
        public bool Is1099 { get; set; }
        [DefaultValue(false)]
        public bool IsInterCompany { get; set; }
        [DefaultValue(null)]
        public string? SSN { get; set; }
        [DefaultValue(null)]
        public string DefaultAccount { get; set; }

        [DefaultValue(null)]
        public string DefaultAccountName { get; set; }

        public short CheckTemplate { get; set; }
        [DefaultValue(false)]
        public bool PrintCheck { get; set; }
        [DefaultValue(false)]
        public bool ToBePrinted { get; set; }
        [DefaultValue(false)]
        public bool IsAutoBill { get; set; }
        public int? PostDays { get; set; }
        [DefaultValue(false)]
        public bool PrintCheckonContract { get; set; }
        public long? UseTaxID { get; set; }
    }

    public class VendorDirectDepositDTO
    {
        public string? VendorID { get; set; }
        /// <summary>
        /// defaultlt it is CAD which is 1
        /// </summary>
        public short? DDCurrencyLOC { get; set; } = 0;
        [DefaultValue(null)]
        public string? DDBranchNumber { get; set; }
        [DefaultValue(null)]
        public string? DDInstituteNo { get; set; }
        [DefaultValue(null)]
        public string? DDAccountNo { get; set; }
        [DefaultValue(null)]
        public string? CustomEFTRef { get; set; }
        [DefaultValue(null)]
        public string? PayerUniqueNo { get; set; }
    }

    public class ACHDetailsDTO
    {
        public long ID { get; set; }
        public string BusinessID { get; set; }
        public short SourceType { get; set; }
        public string CustomerToken { get; set; }
        public string AddressToken { get; set; }
        [DefaultValue(false)]
        public bool IsAutoPayment { get; set; }
        public long NumerofTimes { get; set; }
        public int RemindMeBefore { get; set; }
        public string? Email { get; set; }
        [DefaultValue(false)]
        public bool IsUnlimited { get; set; }
        public long? TransNumber { get; set; }
        public long? BillEntriesScheduled { get; set; }
        public string PayMethodToken { get; set; }
        [DefaultValue(false)]
        public bool IsDefault { get; set; }
        public List<ACHAccountDetails> AccountInfo { get; set; } = new List<ACHAccountDetails>();
    }
    public class ACHAccountDetails
    {
        public long ACHID { get; set; }
        public string PayMethodToken { get; set; }
        [DefaultValue(false)]
        public bool IsDefault { get; set; }
        public string AccountNumber { get; set; }
        public string AccountCardType { get; set; }
        public short Type { get; set; }
    }

    public class VendorImportDTO
    {
        [DefaultValue(null)]
        public string VendorName { get; set; }
        [DefaultValue(null)]
        public string CompanyName { get; set; }
        [DefaultValue(null)]
        public string VendorNotes { get; set; }
        [DefaultValue(null)]
        public string Address { get; set; }
        [DefaultValue(null)]
        public string CountryName { get; set; }
        [DefaultValue(null)]
        public string StateName { get; set; }
        [DefaultValue(null)]
        public string City { get; set; }
        [DefaultValue(null)]
        public string ZipCode { get; set; }
        [DefaultValue(null)]
        public string MobileNum { get; set; }
        [DefaultValue(null)]
        public string WorkNum { get; set; }
        [DefaultValue(null)]
        public string AlternativeNum { get; set; }
        [DefaultValue(null)]
        public string FaxNum { get; set; }
        [DefaultValue(null)]
        public string EmailID { get; set; }
        [DefaultValue(null)]
        public string BusinessType { get; set; }
        [DefaultValue(null)]
        public string CreditDays { get; set; }
        [DefaultValue(null)]
        public string FederalID { get; set; }
        [DefaultValue(null)]
        public string SSN { get; set; }
        public bool ELigiblefor1099 { get; set; }
        [DefaultValue(null)]
        public string AccountNumber { get; set; }
        [DefaultValue(null)]
        public string ContractStartDate { get; set; }
        [DefaultValue(null)]
        public string ContractAddress { get; set; }
        [DefaultValue(null)]
        public string ContractexpirationDate { get; set; }
        [DefaultValue(null)]
        public string CheckPrintByContract { get; set; }
        [DefaultValue(null)]
        public string PayMethod { get; set; }
        [DefaultValue(null)]
        public string PrintType { get; set; }
        [DefaultValue(null)]
        public string CheckTemplate { get; set; }
        [DefaultValue(null)]
        public string BankCardAccount { get; set; }
        [DefaultValue(null)]
        public string? BillEntrySPlit1 { get; set; }
        [DefaultValue(null)]
        public string BillEntrySPlit2 { get; set; }
        [DefaultValue(null)]
        public string BillEntrySPlit3 { get; set; }
        [DefaultValue(null)]
        public string BillEntrySPlit4 { get; set; }
        [DefaultValue(null)]
        public string BillEntrySPlit5 { get; set; }

    }

    public class UseTaxTransactionDTO
    {
        public string TransactionID { get; set; }
        public DateTime? EntryDate { get; set; }
    }

   public class UpdateDirectDepositBatch
    {
        public string Jid { get; set; }

        public long BatchId { get; set; }

        public string CorpIds { get; set; }
        public string CorpName { get; set; }

        public string AccountId { get; set; }

        public string VendorName { get; set; }
        public DateTime InitiateDate { get; set; }

        public bool isExportLog { get; set; }
    }

    public class UpdateDirectDepositBatchResponse:StatusDTO
    {

        public long ExportLogRefId { get; set; }
        public bool isExportLog { get; set; }
    }
}
