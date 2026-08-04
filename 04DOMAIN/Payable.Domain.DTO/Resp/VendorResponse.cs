using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Payable.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class VendorUserPreferncesResponse : StatusDTO
    {
        [DefaultValue(false)]
        public bool IsSSNFederalReq { get; set; }
        [DefaultValue(false)]
        public bool IsContractTaxLineEnabled { get; set; }
        [DefaultValue(false)]
        public bool IsUseTaxEnabled { get; set; }
        [DefaultValue(false)]
        public bool IsACHEnabled { get; set; }
        [DefaultValue(false)]
        public bool IsDDEnabled { get; set; }

    }

    public class GetVendorPullDownRes : StatusDTO
    {
        public List<PullDownData> Data { get; set; } = new List<PullDownData>();
    }
    public class PullDownData
    {
        public string ID { get; set; }
    }

    public class CreditDaysLoadResponse : StatusDTO
    {
        public CreditDaysLoadResponse()
        {
            CreditDaysList = new List<FrequencyDTO>();
        }
        public List<FrequencyDTO> CreditDaysList { get; set; }
    }
    public class MiscInfoLoadResponse : StatusDTO
    {
        public MiscInfoLoadResponse()
        {
            MiscInfoList = new List<MiscInfoDTO>();
        }
        public List<MiscInfoDTO> MiscInfoList { get; set; }
    }
    public class MiscInfoSaveResponse : CommonDataDTO, IStatusDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class CountryLoadResponse : StatusDTO
    {
        public CountryLoadResponse()
        {
            CountryList = new List<CommonDataDTO>();
        }
        public List<CommonDataDTO> CountryList { get; set; }
    }
    public class StatesLoadResponse : StatusDTO
    {
        public StatesLoadResponse()
        {
            StatesList = new List<CommonDataDTO>();
        }
        public List<CommonDataDTO> StatesList { get; set; }
    }
    public class CreditDaysSaveResponse : CommonDataDTO, IStatusDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class VendorAddressesListResponse : StatusDTO
    {
        public VendorAddressesListResponse()
        {
            VendorAddressList = new List<VendorAdress>();
        }
        public List<VendorAdress> VendorAddressList { get; set; }
    }
    public class VendorAdressesListForContractsResponse : StatusDTO
    {
        public VendorAdressesListForContractsResponse()
        {
            Adresses = new List<CommonDataDTO>();
        }
        public List<CommonDataDTO> Adresses { get; set; }
    }
    public class VendorAdressSaveResponse : CommonDataDTO, IStatusDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }

    public class VendorContractLoadResponse : StatusDTO
    {
        public VendorContractLoadResponse()
        {
            vendorContracts = new List<VendorContractDTO>();

        }
        public List<VendorContractDTO> vendorContracts { get; set; }

    }
    public class VendorContractSaveResponse : CommonDataDTO, IStatusDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class VendorLoadResponse : BusinessDTO, IStatusDTO
    {
        public VendorLoadResponse()
        {
            VendorContract = new List<VendorContractDTO>();
            VendorAddressList = new List<VendorAdress>();
        }
        public List<VendorAdress> VendorAddressList { get; set; } = new List<VendorAdress>();
        public List<VendorContractDTO> VendorContract { get; set; } = new List<VendorContractDTO>();
        public ACHDetailsDTO ACHDetails { get; set; } = new ACHDetailsDTO();
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class VendorAddressStringsResponse : StatusDTO
    {
       public List<VendorAdressStringData> Adresses { get; set; }=new List<VendorAdressStringData>();
    }
    public class VendorAdressStringData
    {
        public string VendorID { get; set; }
        public long VendorAddressID { get; set; }
        public string VendorAddressString { get; set; }
    }
    public class VendorCloningSaveResponse : StatusDTO
    {
        public int TotalVendors { get; set; }
        public int FailedVendorsCount { get; set; }
        public string FailedVendors { get; set; }
    }
    public class VendorSaveResponse : CommonDataDTO, IStatusDTO
    {
        public string CorpID { get; set; }
        public string AccountID { get; set; }
        public decimal OpeningBalance { get; set; }
        public string PayMethodID { get; set; }
        public DateTime AsOfDate { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public List<ContractIDs> ContractDetails { get; set; } = new List<ContractIDs>();
        public Dictionary<string,List<string>> InvalidDataResponse {  get; set; }= new Dictionary<string,List<string>>();

        public string VendorIDs { get; set; }

        public string JournalIDs { get; set; }

    }
    public class ContractIDs
    {
        public string ContractID { get; set; }
        public string AccountNum { get; set; }
    }

    public class UseTaxLoadResponse : StatusDTO
    {
        public List<CommonDataDTO> taxs { get; set; } = new List<CommonDataDTO>();
    }

    public class LoadACHDetailsResponse : StatusDTO
    {
        //public LoadACHDetailsResponse()
        //{
        //    ACHDetails.AccountInfo=new List<ACHAccountDetails>();
        //}

        public ACHDetailsDTO ACHDetails { get; set; }
    }
    public class VendorAuditLog
    {
        public long? EntityId { get; set; }
        public long VendorAuditId { get; set; }
        public short? EntityType { get; set; }
        public string EntityTypeName { get; set; }
        public string ActionDate { get; set; }
        public string ActionBy { get; set; }
        public string ActionByName { get; set; }
        public short? ActionType { get; set; }
        public string ActionName { get; set; }
        public string VendorName { get; set; }
        public string FederalId { get; set; }
        public string SSN { get; set; }
        public string PrintCheckAs { get; set; }
        public bool? IsAutoBill { get; set; }
        public string TermsId { get; set; }
        public string TermsName { get; set; }
        public string PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public string DDBranchNo { get; set; }
        public string DDInsitutuionNo { get; set; }
        public string DDAccountNo { get; set; }
        public string Status { get; set; }
        public string UpdatedColumns { get; set; } = string.Empty;

    }
    public class VendorAuditLogResponse:StatusDTO
    {
        public string CorporationName { get; set; }
        public string VendorName { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<VendorAuditLog> vendorAuditResponse { get; set; } = new List<VendorAuditLog>();
    }
    public class VendorInfoResponse : StatusDTO
    {
        public List<VendorDetailsForSubscription> vendorDetails { get; set; }
    }
    public class VendorDetailsForSubscription
    {
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string CorporationID { get; set; }
        public string DefaultAddress { get; set; }
        public int Status { get; set; }

    }
    public class VendorContractMasterResponse : StatusDTO
    {
        public List<VendorContractsEventPayload> VendorContractMasterList { get; set; } = new List<VendorContractsEventPayload>();
    }

    public class VendorContractsEventPayload
    {
        public string ClientName { get; set; }
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string FederalID { get; set; }
        public long? CreditDays { get; set; }
        public string? CreditDaysID { get; set; }
        public string? PaymentMethodID { get; set; }
        public string? PaymentMethodName { get; set; }
        public DefaultAddressDetails DefaultAddressDetails { get; set; } = new DefaultAddressDetails();
        public List<ContractDetails> ContractDetails { get; set; } = new List<ContractDetails>();

    }

    public class DefaultAddressDetails
    {
        [DefaultValue(null)]
        public string? AddressName { get; set; }
        [DefaultValue(null)]
        public string? Address { get; set; }
        [DefaultValue(null)]
        public string? City { get; set; }
        public long? StateID { get; set; }
        public string? StateName { get; set; }
        public long? CountryID { get; set; }
        public string? CountryName { get; set; }
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
        public string? EmailID { get; set; }
    }

    public class ContractDetails
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
        [DefaultValue(null)]
        public string? ContractAddress { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
        public List<VendorTaxInformationDTO> VendorTaxInfo { get; set; } = new List<VendorTaxInformationDTO>();
    }

    public class VendorTaxInformationDTO
    {
        public string PurposeAccountId { get; set; }
        public string PurposeAccountName { get; set; }
    }
    public class VendorWithDefaultAddress
    {
        public string ClientName { get; set; }
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string FederalID { get; set; }
        public long? CreditDays { get; set; }
        public string? CreditDaysID { get; set; }
        public string? PaymentMethodID { get; set; }
        public string? PaymentMethodName { get; set; }
        [DefaultValue(null)]
        public string? AddressName { get; set; }
        [DefaultValue(null)]
        public string? Address { get; set; }
        [DefaultValue(null)]
        public string? City { get; set; }
        public long? StateID { get; set; }
        public string? StateName { get; set; }
        public long? CountryID { get; set; }
        public string? CountryName { get; set; }
        [DefaultValue(null)]
        public string? ZipCode { get; set; }
        [DefaultValue(null)]
        public string? MobileNum { get; set; }
        [DefaultValue(null)]
        public string? AlternativeNum { get; set; }
        [DefaultValue(null)]
        public string? WorkNum { get; set; }
        [DefaultValue(null)]
        public string? FaxNumber { get; set; }
        public string? Email { get; set; }
    }

    public class ContractWithVendorTaxInfo
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
        [DefaultValue(null)]
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string? ContractAddress { get; set; }
        public string PurposeAccountId { get; set; }
        public string PurposeAccountName { get; set; }
    }
}
