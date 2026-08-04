using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class PaymentGateWayDetails
    {
        public string MainURL { get; set; }
        public string CreateVendor { get; set; }
        public string CreateFundingSource { get; set; }
        public string CreateCCFundingSource { get; set; }
        public string UpdateAddress { get; set; }
        public string UpdateVendor { get; set; }
        public string GetFundingSource { get; set; }
        public string DeleteFundingSource { get; set; }
        public string DeleteVendor { get; set; }
        public string IsProduction { get; set; }
    }
    public class ForteGetFundingAccData
    {
        public string paymethod_token { set; get; }
        public string organization_id { set; get; }
        public string location_id { get; set; }
        public string customer_token { get; set; }
        public string customer_id { get; set; }
        public string label { get; set; }
        public string notes { get; set; }
        public ECheck echeck { set; get; }
        public Card card { set; get; }
        public string is_default { get; set; }
        public ForteResponse response { set; get; }
        public Links links { set; get; }
    }
    public class ForteVendUpdateData
    {
        public string customer_token { get; set; }
        public string location_id { get; set; }
        public string status { get; set; }
        public string first_name { get; set; }
        public string company_name { get; set; }
        public string display_name { get; set; }
        public ForteResponse response { get; set; }
        public Links links { get; set; }
    }
    public class ForteAddUpdateData
    {
        public string address_token { get; set; }
        public string location_id { get; }
        public string customer_token { get; set; }
        public string first_name { get; set; }
        public string company_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string label { get; set; }
        public string address_type { get; set; }
        public PhyAddress physical_address { get; set; }
        public ForteResponse response { get; set; }
        public Links links { get; set; }
    }
    public class ForteCCCreateData
    {
        public string paymethod_token { get; set; }
        public string location_id { get; }
        public string customer_token { get; set; }
        public Card card { get; set; }
        public ForteResponse response { get; set; }
        public Links links { get; set; }
    }
    public class Card
    {
        public string name_on_card { get; set; }
        public string last_4_account_number { get; set; }
        public string masked_account_number { get; set; }
        public string expire_month { get; set; }
        public string expire_year { get; set; }
        public string card_type { get; set; }
        public string suppress_account_updater { get; set; }
    }
    public class ForteAccCreateData
    {
        public string paymethod_token { get; set; }
        public string location_id { get; }
        public string customer_token { get; set; }
        public string notes { get; set; }
        public ECheck echeck { get; set; }
        public ForteResponse response { get; set; }
        public Links links { get; set; }
    }
    public class ECheck
    {
        public string account_holder { get; set; }
        public string masked_account_number { get; set; }
        public string last_4_account_number { get; set; }
        public string routing_number { get; set; }
        public string account_type { get; set; }
        public string bankName { get; set; }
    }
    public class ForteDeleteData
    {
        public string paymethod_token { get; set; }
        public string location_id { get; set; }
        public ForteResponse response { get; set; }
    }

    public class VendorCreateResult
    {
        public string customer_token { get; set; }
        public string location_id { get; set; }
        //public string default_shipping_address_token { get; set; }
        //public string default_billing_address_token { get; set; }
        public string first_name { get; set; }
        //public string last_name { get; set; }
        public string company_name { get; set; }
        public string display_name { get; set; }
        //public string customer_id { get; set; }
        public List<ACHAddress> addresses { get; set; }
        public ForteResponse response { get; set; }
        public Links links { get; set; }
    }
    public class ACHAddress
    {
        public string address_token { get; set; }
        public string location_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string label { get; set; }
        public string address_type { get; set; }
        public string shipping_address_type { get; set; }
        public PhyAddress physical_address { get; set; }
        public Links links { get; set; }
    }
    public class PhyAddress
    {
        public string street_line1 { get; set; }
        public string street_line2 { get; set; }
        public string locality { get; set; }
        public string region { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }

    }
    public class Links
    {
        public string addresses { get; set; }
        public string paymethods { get; set; }
        public string transactions { get; set; }
        public string settlements { get; set; }
        public string schedules { get; set; }
        public string self { get; set; }
    }
    public class ForteResponse
    {
        public string environment { get; set; }
        public string response_desc { get; set; }
    }
    public class SwirePayAccCreateData
    {
        public string message { get; set; }
        public SwirePayACCEntity entity { get; set; }
        public string responseCode { get; set; }
        public string status { get; set; }

    }
    public class SwirePayDeleteResponse
    {
        public string customer_token { get; set; }
        public string location_id { get; }
        public ForteResponse response { get; set; }
    }
    public class SwirePayACCEntity
    {
        public string gid { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string isVerified { get; set; }
        public string isDefault { get; set; }
        public string referenceId { get; set; }
        public SwirePayEntity contact { get; set; }
        public SwirePayIssuerBank issuerBank { get; set; }
        public string doNotDuplicate { get; set; }
        public string deleted { get; set; }
    }
    public class SwirePayIssuerBank
    {
        public string gid { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public SwirePayCountry country { get; set; }
        public string isLive { get; set; }
        public string isVerified { get; set; }
        public string referenceId { get; set; }
        public string routingNumber { get; set; }
        public string accountType { get; set; }
        public string bankName { get; set; }
        public string vpa { get; set; }
        public string alias { get; set; }
        public string lastFour { get; set; }
        public string live { get; set; }
        public string verified { get; set; }
        public string deleted { get; set; }
    }
    public class SwirePayAccResult
    {
        public string paymethod_token { get; set; }
        public string location_id { get; }
        public string customer_token { get; set; }
        public string notes { get; set; }
        public ECheck echeck { get; set; }
        public ForteResponse response { get; set; }
        public Links links { get; set; }
        public SwirePayACCEntity other { get; set; }
    }

    public class SwirePayData
    {
        public string message { get; set; }
        public SwirePayEntity entity { get; set; }
        public string responseCode { get; set; }
        public string status { get; set; }
    }
    public class SwirePayEntity
    {
        public string gid { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string paymentTermsDays { get; set; }
        public string referenceId { get; set; }
        public string contactType { get; set; }
        public SwirePayAddress address { get; set; }
        public Tax tax { get; set; }
        public string dwollaCustomer { get; set; }
        public string phoneNumber { get; set; }
        public string tds { get; set; }
        public string doNotDuplicate { get; set; }
        public string verified { get; set; }
        public string deleted { get; set; }
    }
    public class SwirePayAddress
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public SwirePayCountry country { get; set; }
        public string deleted { get; set; }
    }
    public class SwirePayCountry
    {
        public string id { get; set; }
        public string name { get; set; }
        public string alpha2 { get; set; }
        public string alpha3 { get; set; }
        public string unCode { get; set; }
    }
    public class Tax
    {
        public string id { get; set; }
        public string type { get; set; }
        public string country { get; set; }
    }
    public class SwirePayResult
    {
        public string customer_token { get; set; }
        public string location_id { get; set; }
        public string default_shipping_address_token { get; set; }
        public string default_billing_address_token { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company_name { get; set; }
        public string display_name { get; set; }
        public string customer_id { get; set; }
        public List<ACHAddress> addresses { get; set; }
        public ForteResponse response { get; set; }
        public Links links { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public SwirePayEntity other { get; set; }
    }
}
