using Common.API.ActionFilters;
using Common.App.Contracts;
using Common.Domain.DTO.Model.Base;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;
using Payable.App.Contracts;
using Payable.App.Service;
using Payable.Domain.DTO.Model;
using Payable.Domain.DTO.Model.RepayModel.v1;
using RestSharp;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using static Payable.Domain.DTO.Model.WebhookUtil;

namespace Payable.API.Controllers
{
    [Route("v1/Repay")]
    [ApiController]
    [ValidateModel]
    public class RePayProviderController : Controller
    {
       
        #region Fields
        private readonly IRePayService rePayService;
        private readonly ILoggerService logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion
        #region Ctor
        public RePayProviderController(IRePayService _rePayService, IConfiguration config)
        {
            this.rePayService = _rePayService;


        }
        #endregion

        #region Repay

        

        

        [Route("WebhookPaymentFunded")]
        [HttpPost]
        public async Task<IActionResult> RepayFundResponse([FromBody] RepayProcesedWebhookResponse Req)//List<long> batchDetailIds
        {

            //string headers = Request.Headers.ToString();
            string signature = "";
            string headers = String.Empty;
            foreach (var key in Request.Headers.Keys)
            {
                if(key== "X-Webhook-Verification")
                {
                    signature = Request.Headers[key];
                }
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            }
               


            var obj = rePayService.InsertWebhookFundedResponse(Req, headers,signature);
            return Ok();
        }
        [Route("WebhookPaymentProcess")]
        [HttpPost]
        public async Task<IActionResult> RepayProcessedResponse([FromBody] RepayProcesedWebhookResponse Req)//List<long> batchDetailIds
        {

            //string headers = Request.Headers.ToString();

            string headers = String.Empty;
            string signature = "";
           
            foreach (var key in Request.Headers.Keys)
            {
                if (key == "X-Webhook-Verification")
                {
                    signature = Request.Headers[key];
                }
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            }
            //const string HeaderKeyName = "X-Webhook-Verfication";
            // Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);




            var obj = rePayService.InsertWebhookProcessedResponse(Req, headers,signature);
            return Ok();
        }

        [Route("WebhookPaymentOpen")]
        [HttpPost]
        public async Task<IActionResult> RepayOpenPaymentResponse([FromBody] RepayPaymentReturned Req)//List<long> batchDetailIds
        {

            //string headers = Request.Headers.ToString();

            string headers = String.Empty;
            string signature = "";

            foreach (var key in Request.Headers.Keys)
            {
                if (key == "X-Webhook-Verification")
                {
                    signature = Request.Headers[key];
                }
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            }
            //const string HeaderKeyName = "X-Webhook-Verfication";
            // Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);




            var obj = rePayService.InsertWebhookPaymentOpenResponse(Req, headers, signature);
            return Ok();
        }

        [Route("WebhookPaymentCancel")]
        [HttpPost]
        public async Task<IActionResult> RepayPaymentCancelResponse([FromBody] RepayPaidWebhookResponse Req)//List<long> batchDetailIds
        {

            string headers = String.Empty;
            string signature = "";
            foreach (var key in Request.Headers.Keys)
            {
                if (key == "X-Webhook-Verification")
                {
                    signature = Request.Headers[key];
                }
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            }

            var obj = rePayService.InsertWebhookPaymentCancelResponse(Req, headers,signature);
            return Ok();
        }

        [Route("WebhookPaymentPaid")]
        [HttpPost]
        public async Task<IActionResult> RepayPaidResponse([FromBody] RepayPaidWebhookResponse Req)//List<long> batchDetailIds
        {

            string headers = String.Empty;
            string signature = "";

            foreach (var key in Request.Headers.Keys)
            {
                if (key == "X-Webhook-Verification")
                {
                    signature = Request.Headers[key];
                }
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            }
            var obj = rePayService.InsertWebhookPaidResponse(Req, headers,signature);
            return Ok();
        }
        [Route("WebhookRejectCardTransaction")]
        [HttpPost]
        public async Task<IActionResult> WebhookRejectCardTransaction([FromBody] RepayRejectCardTranResponse Req)//List<long> batchDetailIds
        {

            string headers = String.Empty;
            string signature = "";

            foreach (var key in Request.Headers.Keys)
            {
                if (key == "X-Webhook-Verification")
                {
                    signature = Request.Headers[key];
                }
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            }
            var obj = rePayService.InsertWebhookCardRejectTransactionResponse(Req, headers, signature);
            return Ok();
        }


        [Route("WebhookCanelPaymentGroup")]
        [HttpPost]
        public async Task<IActionResult> WebhookCanelPaymentGroup([FromBody] RepayPaymentGroupCancelResponse Req)//List<long> batchDetailIds
        {

            string headers = String.Empty;
            string signature = "";

            foreach (var key in Request.Headers.Keys)
            {
                if (key == "X-Webhook-Verification")
                {
                    signature = Request.Headers[key];
                }
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            }
            var obj = rePayService.InsertWebhookPaymentGroupCancelResponse(Req, headers, signature);
            return Ok();
        }
        [Route("WebhookPaymentReturned")]
        [HttpPost]
        public async Task<IActionResult> WebhookPaymentReturned([FromBody] RepayPaymentReturned Req)//List<long> batchDetailIds
        {

            string headers = String.Empty;
            string signature = "";

            foreach (var key in Request.Headers.Keys)
            {
                if (key == "X-Webhook-Verification")
                {
                    signature = Request.Headers[key];
                }
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            }
            var obj = rePayService.InsertWebhookPaymentReturned(Req, headers, signature);
            return Ok();
        }


        [Route("WebhookPaymentCheckImage")]
        [HttpPost]
        public async Task<IActionResult> WebhookPaymentCheckImage([FromBody] RepayPaymentCheckImage Req)//List<long> batchDetailIds
        {

            string headers = String.Empty;
            string signature = "";

            foreach (var key in Request.Headers.Keys)
            {
                if (key == "X-Webhook-Verification")
                {
                    signature = Request.Headers[key];
                }
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            }
            var obj = rePayService.InsertWebhookPaymentCheckImage(Req, headers, signature);
            return Ok();
        }
        //[Route("WebhookPaymentCheckPaymentStatus")]
        //[HttpPost]
        //public async Task<IActionResult> WebhookPaymentCheckPaymentStatus(string id)//List<long> batchDetailIds
        //{


        //    var obj = rePayService.GetPaymentStatus(id);
        //    return Ok();
        //}

        [Route("TestPayload/{timestamp}/{key}")]
        [HttpPost]
        public async Task<IActionResult> TestPayload(string timestamp,string key, [FromBody] dynamic Req)//List<long> batchDetailIds
        {
            var data = JsonConvert.DeserializeObject<dynamic>(Req.ToString());
          
            string jsoninfo = JsonConvert.SerializeObject(data);
            string obj = timestamp + "." + jsoninfo;
           var resp= Method4(obj,key);
            var resp1 = Method3(obj, key);
            var resp2 = Method2(obj, key);
            var resp3 = Method1(obj, key);
            return Ok(resp1);
        }
        
        private static string Method4(string message, string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        private  string Method3(string message, string key)
        {
            Encoding encoding = Encoding.UTF8;

            //Reference http://en.wikipedia.org/wiki/Secure_Hash_Algorithm
            //SHA256 block size is 512 bits => 64 bytes.
            const int HashBlockSize = 64;

            var keyBytes = encoding.GetBytes(key);
            var opadKeySet = new byte[HashBlockSize];
            var ipadKeySet = new byte[HashBlockSize];


            if (keyBytes.Length > HashBlockSize)
            {
                keyBytes = GetHash(keyBytes);
            }

            // This condition is independent of previous
            // condition. If previous was true
            // we still need to execute this to make keyBytes same length
            // as blocksize with 0 padded if its less than block size
            if (keyBytes.Length < HashBlockSize)
            {
                var newKeyBytes = new byte[HashBlockSize];
                keyBytes.CopyTo(newKeyBytes, 0);
                keyBytes = newKeyBytes;
            }


            for (int i = 0; i < keyBytes.Length; i++)
            {
                opadKeySet[i] = (byte)(keyBytes[i] ^ 0x5C);
                ipadKeySet[i] = (byte)(keyBytes[i] ^ 0x36);
            }

            var hash = GetHash(ByteConcat(opadKeySet,
                GetHash(ByteConcat(ipadKeySet, encoding.GetBytes(message)))));

            // Convert to standard hex string 
            return hash.Select<byte, string>(a => a.ToString("x2"))
                        .Aggregate<string>((a, b) => string.Format("{0}{1}", a, b));
        }

        private static byte[] GetHash(byte[] bytes)
        {
            using (var hash = new SHA256Managed())
            {
                return hash.ComputeHash(bytes);
            }
        }

        private static byte[] ByteConcat(byte[] left, byte[] right)
        {
            if (null == left)
            {
                return right;
            }

            if (null == right)
            {
                return left;
            }

            byte[] newBytes = new byte[left.Length + right.Length];
            left.CopyTo(newBytes, 0);
            right.CopyTo(newBytes, left.Length);

            return newBytes;
        }

        private  String Method2(String text, String key)
        {
            // change according to your needs, an UTF8Encoding
            // could be more suitable in certain situations
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
       
        private string Method1(string text, string key)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(text);

            HMACSHA256 hashString = new HMACSHA256(keybyt);
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
        #endregion

        #region Webhook
        [Route("WebhookEvents/{key}/{timestamp}/{originalkey}")]
        [HttpPost]
        public async Task<IActionResult> WebhookEventForPayments(string key,string timestamp,string originalkey, [FromBody] RepayPaidWebhookResponse Req)
        {
            WebhookEventObject webhookEventObj=new WebhookEventObject();

            string jsoninfo = JsonConvert.SerializeObject(Req);
            var verificationKey = "1E44386908F094C540D233FC46551E1BE8DF8D88407B472DC50D82B66B5FC6D1";
            string ts =timestamp;
            string payload = timestamp+"." + jsoninfo;

            var sadsa = GetHMACSHA256BillDesk(jsoninfo, originalkey);

            return Ok();
        }

        private string GetHMACSHA256BillDesk(string text, string key)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(text);

            HMACSHA256 hashString = new HMACSHA256(keybyt);
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
        #endregion
    }
}
