using Common.Domain.DTO.Model;
using Payable.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Payable.Domain.DTO.Req
{
    public class CorporationACHDTO
    {
        [DefaultValue(null)]
        public string LocationID { get; set; }
        [DefaultValue(null)]
        public string OrganizationID { get; set; }
        [DefaultValue(null)]
        public string PGAccountID { get; set; }
        [DefaultValue(null)]
        public string TransactionID { get; set; }
        [DefaultValue(null)]
        public string PaymentAccountID { get; set; }
        [DefaultValue(null)]
        public string DepositAccountID { get; set; }
        public short ProviderType { get; set; }
        public string PartnerAccID { get; set; }
        public string CustomerToken { get; set; }
        public string AddressToken { get; set; }

    }

    public class CreateACHVendorRequest : CorporationACHDTO
    {
        public string CorpID { get; set; }
        public string VendorName { get; set; }
        [DefaultValue(null)]
        public string? AddressName { get; set; }
        [DefaultValue(null)]
        public string? Adress { get; set; }
        [DefaultValue(null)]
        public string? City { get; set; }
        [DefaultValue(null)]
        public string StateCode { get; set; }
        [DefaultValue(null)]
        public string CountryCode { get; set; }
        [DefaultValue(null)]
        public string ZipCode { get; set; }
        [DefaultValue(null)]
        [EmailAddress]
        public string EmailID { get; set; }
        [DefaultValue(null)]
        public string WorkNum { get; set; }
    }
    public class CreateAccDetailsRequest : CorporationACHDTO
    {
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string RoutingNumber { get; set; }
        public string Title { get; set; }
        public bool IsDefault { get; set; }
    }
    public class CreateCCDetailsRequest : CorporationACHDTO
    {
        public string AccountNumber { get; set; }
        public string Title { get; set; }
        public string CVV { get; set; }
        public string NameOnCard { get; set; }
        public string EXPDate { get; set; }
        public string CardType { get; set; }
        public bool IsDefault { get; set; }
    }
    public class UpdateAddressDetailsRequest : CreateACHVendorRequest
    {

    }
    public class UpdateACHVendorRequest : CorporationACHDTO
    {
        public string CorpID { get; set; }
        public string VendorName { get; set; }
        public string VendorStatus { get; set; }
    }
    public class GetVendorAccDetailsRequest : CorporationACHDTO
    {
        public bool IsDefault { get; set; }
        [DefaultValue(false)]
        public bool IsCreditCard { get; set; }
        public string PayMethodToken { get; set; }

    }
    public class DeleteACHVendorRequest : CorporationACHDTO
    {

    }
}
