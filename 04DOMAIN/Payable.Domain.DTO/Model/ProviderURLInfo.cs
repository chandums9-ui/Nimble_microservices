using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class ProviderURLInfo
    {
        public string RePay { get; set; }

        public string repayAttachmentUrl { get; set; }
    }
    public class RePayKeyInfo
    {
        public string clientID { get; set; }
        public string clientSecret { get; set; }

        public string AdminKey { get; set; }
        public string WebhookURL { get; set; }
        public string WebhookSecret { get; set; }

        public string PrefixPayableDbConnection { get; set; }
    }

    public class WebhookInfo
    {
        public string WebhookURL { get; set; }
        public string ResourceEventPaid {  get; set; }
        public string ResourceEventFunded { get; set;}
        public string ResourceEventProcessed { get; set; }
        public string Account { get; set;}
    }
    public class ReceiveWebhook
    {

    }
    
    public class RePayEndPoints
    {

        public string AuthToken { get; set; }
        public string InitiatePayment { get; set; }
        public string ApprovePayment { get; set; }
        public string GetPaymentStatus { get; set; }
        public string Webhook { get; set; }

    }



    public class token
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
    public class RePayTokenInfo
    {
        public token? token { get; set; }
    }
    public class RepayTokenError
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public string referenceCode { get; set; }
        public string error_type { get; set; }

    }
    public class RePayPaymentInitiationResponse
    {
        public string id { get; set; }
        public PaymentGroupObject group { get; set; }
        public VendorObject vendor { get; set; }
        public string GroupID { get; set; }
        public string CustID { get; set; }
        public string PaymentNum { get; set; }
        public string paymentStatus { get; set; }

        public string PaymentType { get; set; }
        public string CheckNumber { get; set; }

        public string CheckImageUrl { get; set; }

        public string CardToken { get; set; }
    }
    public class VendorObject
    {
        public string id { get; set; }
        public string vendorName1 { get; set; }
        public string status { get; set; }
        public string paymentType { get; set; }
    }
    public class Invoices
    {
        public List<Invoice> invoices { get; set; } = new List<Invoice>();
    }
    public class Invoice
    {
        public string netAmount { get; set; }
        public string invoiceNumber { get; set; }
        public string invoiceDate
        {
            get; set;
        }
        public string totalAmount { get; set; }
        public string adjustAmount { get; set; }
        public string dueDate { get; set; }
        public string poNumber { get; set; }
        public string discount { get; set; }
    }
    public class RePayPaymentApproveResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public string totalAmount { get; set; }
        public string approved { get; set; }
        public string fundedDate { get; set; }

    }
    public class WebhookSubcriptionReq
    {
        public string ResourceEvent { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Account { get; set; }
    }
    public class WebhookEventResponse
    {
        //id
        public string id { get; set; }
        public string resourceEvent { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        //dateCreated,dateModified,securityKey1
        public string dateCreated { get; set; }
        public string dateModified { get; set; }
        public string account { get; set; }
        public string securityKey1 { get; set; }
        public string success { get; set; }
    }
    public class WebhookEvents
    {
        public List<WebhookEvent> Events { get; set; }
    }
    public class WebhookEvent
    {
        public string EventId { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }

        public string Url { get; set; }

        public string Account { get; set; }

        public string DateCreated { get; set; }
        public string DateModified { get; set; }

    }

    public class WebhookEventObject
    {
        public WebhookEventObject()
        {
            payload = new WebhookPayload();
        }

        public string ts { get; set; }
        public string s1 { get; set; }
        

        public string type { get; set; }

        public string DateCreated
        {
            get; set;
        }
        public WebhookPayload payload { get; set; }
    }
    public class WebhookPayload
    {
        public string Id { get; set; }

        //Payment Object as Body
        public PaymentObject body { get; set; }
        //public VendorObject vendor { get; set; }
    }
    public class PaymentObject
    {
        public string Id { get; set; }

        //paymentgroup Object
        public PaymentGroupObject Group { get; set; }
        public string CustID { get; set; }
        public string PaymentNumber { get; set; }
        public string PaymentStatus { get; set; }

        public string paymentType { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }

    }
    public class PaymentGroupObject
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Status { get; set; }
        public string Approved { get; set; }
        public decimal TotalAmount { get; set; }

    }


    public static class WebhookUtil
    {        

        public static byte[] CreateHMACSHA256Signature(string timestamp, string url, string body, string secret)
        {
            string stringToSign = string.Join(".", timestamp,body);
            using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var bytes = Encoding.UTF8.GetBytes(stringToSign);
            var hashedBytes = hmacsha256.ComputeHash(bytes);

            return hashedBytes;
        }

        public static string CreateHMACSHA256Base64Signature(string timestamp, string url, string body, string secret)
        {
            return Convert.ToBase64String(CreateHMACSHA256Signature(timestamp, url, body, secret));
        }

        public class GenericStringRequest
        {
            public string ClientId { get; set; }
        }
    }
}
