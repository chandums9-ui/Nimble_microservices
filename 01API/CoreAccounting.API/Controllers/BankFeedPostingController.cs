using Common.API.ActionFilters;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;
//using Common.API.ActionFilters;
using CoreAccounting.App.Contracts;
using Common.Domain.DTO.App;
using CoreAccounting.Domain.DTO.Resp;
using BankFeed.App.Services;
using BankFeed.Domain.DTO.Resp;
using BankFeed.Domain.DTO.Req;
using Microsoft.Extensions.Options;
//using Azure;

namespace CoreAccounting.API.Controllers
{
    [Route("v1/BankFeedPosting")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class BankFeedPostingController : BaseController
    {

        #region Fields
        private readonly IBankFeedPostingService bankFeedPostingService;
        private IOptions<List<ServerAnlyticsGroup>> serverGroup;
        #endregion

        #region Constructor
        public BankFeedPostingController(IBankFeedPostingService _bankFeedPostingService, IOptions<List<ServerAnlyticsGroup>> _serverGroup)
        {
            this.bankFeedPostingService = _bankFeedPostingService;
            this.serverGroup = _serverGroup;
        }
        #endregion

        private long getUrlID()
        {
            return Convert.ToInt64((string)HttpContext.Items["UrlID"]);
        }
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        private string getNPConnection()
        {
            string client = getClientName();
            string clientServer = "";
            if (serverGroup != null & serverGroup.Value.Count() > 0)
                clientServer = serverGroup.Value.Where(s => s.clients.Contains(client)).Select(s => s.ServerName).FirstOrDefault();
            return (string.IsNullOrEmpty(clientServer) ? "" : clientServer);

        }

        /// <summary>
        /// this posts the transaction and returns the posted details
        /// </summary>
        /// <param name="PostReq"></param>
        /// <returns>It returns SinglePostResponse </returns>
        [Route("SinglePost")]
        [HttpPost]
        public async Task<IActionResult> SaveSinglePost(SinglePostRequest PostReq)
        {
            try
            {
                SinglePostResponse respose = await bankFeedPostingService.PostSingleTransaction(PostReq,getUrlID(), getClientName(),getNPConnection());
                if (respose != null && respose.StatusCode == StatusCodes.Status200OK)
                    return Ok(respose);
                else if (respose != null && !string.IsNullOrEmpty(respose.Status))
                    return Ok(respose);

                else
                    return NotFound(respose);
            }
            catch { throw; }
        }

        /// <summary>
        /// It will post the multiple transactions based on multiple posts requests
        /// </summary>
        /// <param name="PostReq"></param>
        /// <returns>it returns multiple response </returns>
        [Route("MultiplePost")]
        [HttpPost]
        public async Task<IActionResult> SaveMultiplePosts(MultiplePostRequest PostReq)
        {
            try
            {
                MultiplePostResponse response = await bankFeedPostingService.PostMultipleTransactions(PostReq, getUrlID(), getClientName(), getNPConnection());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response.Status);
                else
                    return NotFound(response);

            }
            catch { throw; }
        }

        /// <summary>
        /// Based on request  it will load PaymentMethods
        /// </summary>
        /// <returns>It will return Id,Name,Status and StatusCodes</returns>
        [Route("PaymentMethods")]
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethodList()
        {
            PaymentmethodResponse response = null;
            try
            {
                response = await bankFeedPostingService.GetpaymentMethodList();
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
        }

        /// <summary>
        /// it Gets list of dailysale lines of departmenttype=8,7 for mergesettings purpose
        /// </summary>
        /// <param name="Request">It contains CorporationID,AccID</param>
        /// <returns>It returns List of DailySaleLines Data as MergeSettingsListResponse</returns>
        [Route("MergeSettingsList")]
        [HttpPost]
        public async Task<IActionResult> GetMergeSettingLines(MergeSettingsListRequest Request)
        {
            MergeSettingsListResponse response = null;
            try
            {
                response = await bankFeedPostingService.GetMergeSettingLines(Request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
        }


        [Route("PostingAutoRuleTrans")]
        [HttpPost]
        public async Task<IActionResult> PostingAutoRuleTrans(MultiplePostRequest Request)
        {
            FeedTranResponse response = null;
            try
            {
                response = await bankFeedPostingService.PostingAutoRuleTrans(Request, getUrlID(), getClientName(), getNPConnection());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
        }


        [Route("GetBillInformation")]
        [HttpPost]
        public async Task<IActionResult> GetBillInformation(BillInfoRequest req)
        {
            BillInformationResponse response = null;
            try
            {
                response = await bankFeedPostingService.GetBillInformation(req);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
        }

        [Route("UpdatingBillInformation")]
        [HttpPost]
        public async Task<IActionResult> UpdatingBillInformation(BillInfoRequest req)
        {
            BillInformationResponse response = null;
            try
            {
                response = await bankFeedPostingService.UpdatingBillInformation(req, getUrlID(), getClientName(), getNPConnection());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
        }
        //GroupUpdatingBillInformation
        [Route("GroupUpdatingBillInformation")]
        [HttpPost]
        public async Task<IActionResult> GroupUpdatingBillInformation(GroupBillInfoRequest req)
        {
            GroupBillInformationResponse response = null;
            try
            {
                response = await bankFeedPostingService.GroupUpdatingBillInformation(req, getUrlID(), getClientName(), getNPConnection());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
        }

        [Route("SaveMultiTranAuditLog")]
        [HttpPost]
        public async Task<IActionResult> SaveMultiTranAuditLog(AuditLogReq req)
        {
            int response ;
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];          

                response = await bankFeedPostingService.SaveMultiTranAuditLog(req);
                if (response != 0)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
        }
    }
}
