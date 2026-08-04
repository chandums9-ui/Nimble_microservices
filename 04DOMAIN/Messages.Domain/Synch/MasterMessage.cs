using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messages.Domain.Synch
{
    public class MasterMessage
    {
        public string? MessageID { get; set; }
        public required short EventType { get; set; }
    }

    #region Corporation
    public class CorporationEventPayload
    {
        [JsonPropertyName("corporationID")]
        public string CorporationID { get; set; }

        [JsonPropertyName("BookKeepingDiretory")]
        public string BookKeepingDiretory { get; set; } = string.Empty;

        [JsonPropertyName("ClientID")]
        public string ClientID { get; set; } = string.Empty;

        [JsonPropertyName("ClientUserName")]
        public string ClientUserName { get; set; } = string.Empty;

        [JsonPropertyName("LegalName")]
        public string LegalName { get; set; } = string.Empty;

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Status")]
        public short Status { get; set; }

        [JsonPropertyName("client_url")]
        public string ClientURL { get; set; } = string.Empty;

    }

    public class CorporationMessageEvent : MasterMessage
    {
        public CorporationEventPayload CorporationEventPayload { get; set; }
    }

    public class CorporationReattemptEvent : MasterMessage
    {
        public CorporationEventPayload CorporationEventPayload { get; set; }
        public string? UrlName { get; set; }
        public long? SubscriptionID { get; set; }
    }
    #endregion

    #region PC

    public class PCEventPayload
    {

        [JsonPropertyName("ID")]
        public string ID { get; set; } = string.Empty;

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("BookKeepingDiretory")]
        public string BookKeepingDiretory { get; set; } = string.Empty;

        [JsonPropertyName("Status")]
        public short Status { get; set; }

        [JsonPropertyName("corporationID")]
        public string CorporationID { get; set; }
    }

    public class PCMessageEvent : MasterMessage
    { 
        public PCEventPayload PCEventPayload { get; set; }
    }
    public class PCReattemptEvent : MasterMessage
    {
        public PCEventPayload PCEventPayload { get; set; }
        public string? UrlName { get; set; }
        public long? SubscriptionID { get; set; }
    }
    #endregion

    #region Vendor 
    public class VendorEventPayload
    {
        [JsonPropertyName("id")]
        public string ID { get; set; } = string.Empty;

        [JsonPropertyName("defaultAddress")]
        public string? DefaultAddress { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public short Status { get; set; }

        [JsonPropertyName("corp_id")]
        public string? CorporationID { get; set; }
    }

    public class VendorMessageEvent : MasterMessage
    {
        public VendorEventPayload VendorEventPayload { get; set; }
    }

    public class VendorReattemptEvent : MasterMessage
    {
        public VendorEventPayload VendorEventPayload { get; set; }
        public string? UrlName { get; set; } 
        public long? SubscriptionID { get; set; }
    }

    #endregion

    #region Contract
    public class ContractEventPayload
    {
        [JsonPropertyName("id")]
        public string ID { get; set; } = string.Empty;

        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("adress")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("status")]
        public short Status { get; set; }

        [JsonPropertyName("vendor_id")]
        public string VendorID { get; set; } = string.Empty;
    }

    public class ContractMessageEvent : MasterMessage
    {
        public ContractEventPayload ContractEventPayload { get; set; }
    }

    public class ContractReattemptEvent : MasterMessage
    {
        public ContractEventPayload ContractEventPayload { get; set; }
        public string? UrlName { get; set; }
        public long? SubscriptionID { get; set; }
    }
    #endregion

    #region Account
    public class AccountEventPayload
    {
        public string ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountTypeID { get; set; }
        public int AccountTypeOrder { get; set; }
        public string ParentAccountName { get; set; }
        public string ParentAccountNumber { get; set; }
        /// <summary>
        /// Status =17 & AccountTypeID= 0x0FA700000000000000000000000000000024 => default account for UI
        /// </summary>
        public short AccountStatus { get; set; }
        public int IsAllow { get; set; }
        public bool IsAllowAccount { get; set; }
        public string ClientName { get; set; }

    }

    public class AccountMessageEvent : MasterMessage
    {
        public AccountEventPayload AccountEventPayload { get; set; }

    }
    public class AccountReattemptEvent : MasterMessage
    {
        public AccountEventPayload AccountEventPayload { get; set; }
        public string? UrlName { get; set; }
        public long? SubscriptionID { get; set; }
    }
    #endregion
}
