
using Payable.Domain.DTO.Model.RepayModel.Common;
using System;
using System.Text.Json.Serialization;

namespace Payable.Domain.DTO.Model.RepayModel.v1
{
    public class RepayCreateOrderResponse : RepayBaseResponseModel
    {

        public string id { get; set; }
        public GroupResponse group { get; set; }
        public string custId { get; set; }

        public string paymentNumber { get; set; }

        public RepayVendor vendor { get; set; }

        public List<GroupLink> links { get; set; }

        public List<VenorInvoices> invoices { get; set; }

        public string paymentStatus { get; set; } //order status

        public string paymentType { get; set; }

        public DateTime dateCreated { get; set; }
        public bool approved { get; set; } = false;

        public string status { get; set; } //approval status
        public string dateModified { get; set; }

        public RepayErrorResponse error { get; set; }
    }
    public class RepayErrorResponse
    {
        public long? code { get; set; }

        public string cause { get; set; }

        public string message { get; set; }
    }
    public class GroupResponse
    {
        public string id { get; set; }

        public string name { get; set; }

        public string status { get; set; }

        public string approved { get; set; }

        public string custId { get; set; }

        public string totalAmount { get; set; }

        public DateTime dateCreated { get; set; }

        public DateTime dateModified { get; set; }

       


    }
    public class GroupLink
    {
        public string rel { get; set; }

        public string href { get; set; }

        public string type { get; set; }
    }

    public class RepayWebhookRequest 
    {
        public string Id { get; set; }

        public RepayCreateOrderResponse body { get; set; }

        public string type { get; set; }

        public string dateCreated { get; set; }
    }
    public class RepayWebhookRequest1 : RepayBaseRequestModel
    {
        public string Id { get; set; }

        public string resourceEvent { get; set; }

        public string description { get; set; }

        public string url { get; set; }

        public string account { get; set; }

        [JsonIgnore]
        public string ts { get; set; }

        [JsonIgnore]
        public string s1 { get; set; }

    }
    public class reapytest :RepayBaseRequestModel
    {
        public string ts { get; set; }

        public string s1 { get; set; }

    }
    public class RepayPaidWebhookResponse 
    {
        public string Id { get; set; }

        public RepayPaidWebhookBody body { get; set; }

        public string type { get; set; }

        public string dateCreated { get; set; }
    }

    public class RepayPaidWebhookBody
    {
        public string id { get; set; }
        public RepayPaidWebhookGroupResponse group { get; set; }
        public string custId { get; set; }

        public string paymentNumber { get; set; }

        public RepayPaidVendor vendor { get; set; }

        public List<GroupLink> links { get; set; }

        public List<VenorPaidInvoices> invoices { get; set; }

        public string paymentStatus { get; set; } //order status

        public string paymentType { get; set; }

        public string dateCreated { get; set; }
        public string dateModified { get; set; }

        public string voucherLink { get; set; }

        public string checkNumber { get; set; }
    }

    public class RepayPaidWebhookGroupResponse
    {
        public string id { get; set; }

        public string name { get; set; }

        public string status { get; set; }

        public bool approved { get; set; }

      

        public string totalAmount { get; set; }

        public string dateCreated { get; set; }

        public string dateModified { get; set; }

        public List<GroupLink> links { get; set; }

    }
    /// <summary>
    /// This response body used for Payment Processed/ Funded
    /// </summary>
    public class RepayProcesedWebhookResponse
    {
        public string id { get; set; }

        public RepayProcesedWebhookBody body { get; set; }

        public string type { get; set; }

        public string dateCreated { get; set; }
    }
    public class RepayProcesedGroupResponse
    {

            public string id { get; set; }

            public string name { get; set; }

            public string status { get; set; }

            public bool approved { get; set; }

            public string custId { get; set; }

            public string totalAmount { get; set; }

            public DateTime dateCreated { get; set; }

            public DateTime dateModified { get; set; }

            public List<GroupLink> links { get; set; }

        
    }
    public class RepayProcesedWebhookBody
    {
        public string id { get; set; }
        public string name { get; set; }

        public string status { get; set; }

        public bool approved { get; set; }
        public string totalAmount { get; set; }

        public string dateCreated { get; set; }

        public string dateModified { get; set; }

        public string custId { get; set; }
        public List<GroupLink> links { get; set; }
    }


    public class RepayGroupOrderResponse : RepayBaseResponseModel
    {
        public List<RepayCreateOrderResponse> data { get; set; }

        public string    offset { get; set; }
        public string count { get; set; }

        public string MyProperty { get; set; }

    }

    public class RepayRejectCardTranResponse
    {
        public string id { get; set; }

        public RepayRejectCardTranBody body { get; set; }

        public string type { get; set; }

        public string dateCreated { get; set; }
    }
    public class RepayRejectCardTranBody
    {
        public string id { get; set; }


        public string cardToken { get; set; }

        public string cardLast4 { get; set; }

        public string mcc { get; set; }

        public string tcc { get; set; }

        public long exchangeRate { get; set; }

        public string Override { get; set; }

        public RepayRejectCardMerchantInfo merchant { get; set; }

        public string transactionDate { get; set; }

        public decimal transactionAmount { get; set; }

        public string status { get; set; }

        public List<GroupLink> links { get; set; }

    }

    public class RepayRejectCardMerchantInfo
    {
        public string name { get; set; }


        public string city { get; set; }

        public string state { get; set; }

        public string zip { get; set; }



    }

    public class RepayPaymentGroupCancelResponse
    {
        public string id { get; set; }

        public RepayPaymentGroupBody body { get; set; }

        public string type { get; set; }

        public string dateCreated { get; set; }
    }

    public class RepayPaymentGroupBody
    {
        public string id { get; set; }

        public string name { get; set; }

        public string status { get; set; }

        public string totalAmount { get; set; }

        public bool approved { get; set; }

        public string dateCreated { get; set; }

        public string dateModified { get; set; }

        public List<GroupLink> links { get; set; }

    }

    public class RepayPaymentReturned
    {
        public string id { get; set; }

        public RepayPaymentReturnedBody body { get; set; }

        public string type { get; set; }

        public string dateCreated { get; set; }
    }

    public class RepayPaymentReturnedBody
    {
        public string id { get; set; }

        public RepayPaymentReturnedBodyGroup group { get; set; }

        public string custId { get; set; }
        public string paymentNumber { get; set; }

         public RepayPaidVendor vendor { get; set; }

        public List<GroupLink> links { get; set; }

        public List<VenorInvoices> invoices { get; set; }

        public string paymentStatus { get; set; }

        public string paymentType { get; set; }

        public string dateCreated { get; set; }

        public string dateModified { get; set; }
        public string checkNumber { get; set; }
    }
    public class RepayPaymentReturnedBodyGroup
    {
        public string id { get; set; }

        public string name { get; set; }

        public string status { get; set; }

        public bool approved { get; set; }

        public string totalAmount { get; set; }

        public string dateCreated { get; set; }

        public string dateModified { get; set; }

        public List<GroupLink> links { get; set; }
    }

    public class RepayPaymentCheckImage
    {
        public string id { get; set; }
        public RepayPaymentCheckImageBody body { get; set; }

        public string type { get; set; }

        public string dateCreated { get; set; }

    }
    public class RepayPaymentCheckImageBody
    {
        public string id { get; set; }

        public RepayPaymentCheckImageBodyGroup group { get; set; }

        public string custId { get; set; }

        public string paymentNumber { get; set; }

        public RepayPaidVendor vendor { get; set; }

        public List<GroupLink> links { get; set; }

        public List<VenorInvoices> invoices { get; set; }

        public string paymentStatus { get; set; }

        public string paymentType { get; set; }

        public string checkNumber { get; set; }

        public string checkImageUrl { get; set; }
        public string dateCreated { get; set; }

        public string dateModified { get; set; }



    }
    public class RepayPaymentCheckImageBodyGroup
    {
        public string id { get; set; }
        public string name { get; set; }

        public string status { get; set; }

        public bool approved { get; set; }

        public string totalAmount { get; set; }
        public string dateCreated { get; set; }

        public string dateModified { get; set; }
        public List<GroupLink> links { get; set; }
    }


}

