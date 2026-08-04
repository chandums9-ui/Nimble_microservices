using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DTO.Model;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using BankFeed.Domain.Enums;
using Common.API.ActionFilters;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Resp;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json;

namespace BankFeed.API.Controllers
{
    [Route("v1/Meld")]
    [ApiController]
    [ValidateModel]
    public class MeldProviderController : ControllerBase
    {
        #region Fields
        private readonly IMeldService meldService;
        private readonly IProviderInfo providerInfo;
        private readonly ILoggerService logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFeedAccountService feedaccountService;
        #endregion

        #region Ctor
        public MeldProviderController(IMeldService _MeldService, IProviderInfo _providerInfo, ILoggerService _logger, IHttpContextAccessor httpContextAccessor, IFeedAccountService _feedaccountService)
        {
            this.meldService = _MeldService;
            this.providerInfo = _providerInfo;
            this.logger = _logger;
            _httpContextAccessor = httpContextAccessor;
            this.feedaccountService = _feedaccountService;
        }
        #endregion

        #region Connect
        [Route("Connect")]
        [HttpPost]
        public async Task<IActionResult> Connect(MeldConnectRequest ConnReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            MeldConnectResponse conResp = await meldService.Connect(ConnReq, clientID, clientName);
            if (conResp == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(conResp);
        }

        [Route("ReConnect")]
        [HttpPost]
        public async Task<IActionResult> ReConnect(MeldReConnectRequest ConnReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            if (ConnReq != null && !string.IsNullOrEmpty(ConnReq.connectionId))
            {
                MeldConnectResponse conResp = await meldService.ReConnect(ConnReq);
                if (conResp != null)
                    return Ok(conResp);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        [Route("ConnectionComplete")]
        [HttpPost]
        public async Task<IActionResult> ConnectionComplete(ConnectionCompleteParamString ConnReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            long clientInfoID = 0;
            Int64.TryParse((string)HttpContext.Items["ClientInfoID"], out clientInfoID);
            var extCust = (clientID + "_" + clientName).Trim('_');

            Task.Delay(TimeSpan.FromSeconds(10)).Wait();
            var res = await meldService.ConnectionComplete(ConnReq, extCust, clientInfoID);
            if (res != null)
                return Ok(res);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        [Route("DeleteConnect")]
        [HttpPost]
        public async Task<IActionResult> DeleteConnect(MeldReConnectRequest ConnReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            if (ConnReq != null && !string.IsNullOrEmpty(ConnReq.connectionId))
            {
                this.logger.LogTrace("@DeleteConnect API >> calling Delete connection from DeleteConnect API for connectionId: " + ConnReq.connectionId);
                MeldConnectResponse conResp = await meldService.DeleteConnection(ConnReq);
                if (conResp != null)
                    return Ok(conResp);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        [Route("MergeConnect")]
        [HttpPost]
        public async Task<IActionResult> MergePartiallyActiveConnectionToNewConnection(MeldMergeConnectRequest MergeConnReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            if (MergeConnReq != null)
            {
                StatusDTO conResp = await meldService.MergePartiallyActiveConnectionToNewConnection(MergeConnReq.ActiveConnectionId, MergeConnReq.PartialActiveConnectionId, MergeConnReq.PartialActiveInstitutionId);
                if (conResp != null)
                    return Ok(conResp);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        /// <summary>
        /// Trigger a refresh of all financial accounts for a given institution connection with the latest information from the service provider. This is a premium service for most service providers and should be used sparingly for requesting up-to-date data as of the time of the call to this endpoint. Transaction and balance data is updated daily by service providers, and Meld's webhooks will notify of these updates so that product data does not become stale.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("RefreshConnection")]
        [HttpPost]
        public async Task<IActionResult> RefreshConnectionAndAccounts(MeldReConnectRequest ConnReq)
        {
            if (ConnReq != null && !string.IsNullOrEmpty(ConnReq.connectionId))
            {
                var res = await meldService.RefreshConnectionAndAccounts(ConnReq);
                if (res != null)
                    return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        #endregion Connect

        #region SYNC

        [Route("SynchFeeds")]
        [HttpPost]
        public async Task<IActionResult> SynchFeeds(ConnectionRequest connectionReq)
        {
            //string authnToken = (_httpContextAccessor.HttpContext.Request.Headers.Keys.Contains("Authorization")) ? _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "") : string.Empty;
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            ConnectionStatus conResp = await meldService.MeldSynchFeeds(connectionReq, clientID, clientName);//, authnToken);
            if (conResp == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(conResp);
        }

        /// <summary>
        /// To sync login user 
        /// </summary>
        /// <param name="connectionReq"></param>
        /// <returns></returns>
        [Route("SynchUserConnections")]
        [HttpPost]
        public async Task<IActionResult> SynchUserConnections(FeedAccountRequest feedAccReq)
        {
            //string authnToken = (_httpContextAccessor.HttpContext.Request.Headers.Keys.Contains("Authorization")) ? _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "") : string.Empty;
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            ConnectionStatus conResp = new ConnectionStatus() { StatusCode = StatusCodes.Status404NotFound, Status = Constants.MSG_NO_DATA_FOUND };
            List<ConnectionRequest> connReq = null;
            DateTime currentDate = DateTime.Now;
            try
            {
                if (!(feedAccReq.SyncInterval > 0 && feedAccReq.CorpIDs != null && feedAccReq.CorpIDs.Any()))
                    return Ok(conResp); 

                this.logger.LogTrace(string.Concat(Environment.NewLine + "---- @GetConnectedFeeds - SynchUserConnections -----"));

                var activeFeedResponse = await feedaccountService.GetConnectedFeeds(clientID, feedAccReq);
                if (activeFeedResponse != null && activeFeedResponse.StatusCode == StatusCodes.Status200OK)
                {
                    if (activeFeedResponse.Feeds != null && activeFeedResponse.Feeds.Any() && activeFeedResponse.Feeds.Where(t => t.ProviderID == Convert.ToInt64(NimbleProvidersEnum.Meld) && t.BankAccount != null && t.BankAccount.Status == (short)BankAccountStatusEnum.Active && !string.IsNullOrEmpty(t.CorpID)).Any())
                    {
                        var list = activeFeedResponse.Feeds.Where(t => t.ProviderID == Convert.ToInt64(NimbleProvidersEnum.Meld) && t.BankAccount != null && t.BankAccount.Status == (short)BankAccountStatusEnum.Active && !string.IsNullOrEmpty(t.CorpID)).ToList();
                        if (list != null)
                        {
                            connReq = new List<ConnectionRequest>();
                            list.Select(t => t.InsID).Distinct().ForEach(instId =>
                            {
                                if (list.Where(fAcc => fAcc.InsID == instId && !string.IsNullOrEmpty(fAcc.CorpID)).Any())
                                {
                                    var instConn = list.Where(fAcc => fAcc.InsID == instId && !string.IsNullOrEmpty(fAcc.CorpID)).OrderBy(t => t.BankAccount.LastUpdateDate).FirstOrDefault();
                                    if (instConn != null && (currentDate - instConn.BankAccount.LastUpdateDate.Value).Hours >= feedAccReq.SyncInterval)
                                        connReq.Add(new ConnectionRequest()
                                        {
                                            CanMoveToActive = false,
                                            institution_id = instId.ToString(),
                                            ConnectType = ProvidersConnectTypeEnum.Sync.ToString(),
                                            ProviderRegID = instConn.ProviderRegID,
                                            ProviderID = Convert.ToInt64(NimbleProvidersEnum.Meld),
                                        });
                                }
                            });
                            if (connReq != null && connReq.Any())
                                conResp = await meldService.SynchUserConnections(connReq, clientID, clientName);//, authnToken);
                        }
                    }
                }
                if (conResp == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
                }
                else
                    return Ok(conResp);

            }
            catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!"); }
        }
        #endregion SYNC

        #region Webhook
        
        [Route("MeldWebhookInfo")]
        [HttpPost]
        public async Task<IActionResult> GetMeldWebhook(MeldWebhookInfo webhookJson)
        {
            string MeldSignatureTimestamp = string.Empty;
            string MeldSignature = string.Empty;
            ICollection<string> keys = new List<string>();

            if (_httpContextAccessor.HttpContext != null)
            {
                keys = _httpContextAccessor.HttpContext?.Request.Headers.Keys;
                if (_httpContextAccessor.HttpContext.Request.Headers.Keys.Count > 0)
                {
                    if (_httpContextAccessor.HttpContext.Request.Headers.Keys.Contains("meld-signature-timestamp".ToLower()))
                        MeldSignatureTimestamp = _httpContextAccessor.HttpContext.Request.Headers["meld-signature-timestamp"].ToString();
                    if (_httpContextAccessor.HttpContext.Request.Headers.Keys.Contains("meld-signature"))
                        MeldSignature = _httpContextAccessor.HttpContext.Request.Headers["meld-signature"].ToString();
                }
            }

            string clientName = (string)HttpContext.Items["ClientName"];
            await meldService.GetMeldWebhook(webhookJson, MeldSignatureTimestamp, MeldSignature, clientName);
            return Ok();
        }
        #endregion Webhook

        /*
        #region Connection

        

        /// <summary>
        /// Retrieve details of a specific institution connection.
        /// </summary>
        /// <param name="req">Connection id as Path param</param>
        /// <returns></returns>
        [Route("GetConnection")]
        [HttpPost]
        public async Task<IActionResult> GetInstitutionConnection(MeldReConnectRequest ConnReq)
        {
            if (ConnReq != null && !string.IsNullOrEmpty(ConnReq.connectionId))
            {
                var res = await meldService.GetInstitutionConnection(ConnReq);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        /// <summary>
        /// Retrieve institution connections filtered by various parameters.
        /// </summary>
        /// <param name="AccountSearchRequest">Search filters</param>
        /// <returns>Returns institution connections</returns>
        [Route("SearchConnections")]
        [HttpPost]
        public async Task<IActionResult> SearchInstitutionConnections(MeldSearchConnectionsRequest ConnSearchRequest)
        {
            if (ConnSearchRequest != null)
            {
                var res = await meldService.SearchInstitutionConnections(ConnSearchRequest);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        /// <summary>
        /// Update a connection and its products. Adds new products retroactively to an existing connection and triggers a refresh with this updated set of products. The products provided must not already be present on the connection and be supported by the underlying service provider and institution in order to be successful.
        /// </summary>
        /// <param name="connReq"></param>
        /// <returns></returns>
        [Route("UpdateConnection")]
        [HttpPost]
        public async Task<IActionResult> UpdateConnectionProducts(MeldReConnectRequest ConnReq)
        {
            if (ConnReq != null && !string.IsNullOrEmpty(ConnReq.connectionId))
            {
                var res = await meldService.UpdateConnectionProducts(ConnReq);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        
        #endregion Connection

        #region Accounts

        /// <summary>
        /// Use this endpoint to retrieve account information of a specific financial account.
        /// </summary>
        /// <param name="financialAccountId"></param>
        /// <returns></returns>
        [Route("GetFinancialAccount")]
        [HttpPost]
        public async Task<IActionResult> GetFinancialAccount(string financialAccountId)
        {
            if (!string.IsNullOrEmpty(financialAccountId))
            {
                var res = await meldService.GetFinancialAccount(financialAccountId);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        /// <summary>
        /// Use this endpoint to filter financial accounts by various parameters.
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>Returns Financial accounts</returns>
        [Route("SearchFinancialAccount")]
        [HttpPost]
        public async Task<IActionResult> SearchFinancialAccount(MeldFinancialAccountSearchRequest AccSearchRequest)
        {
            if (AccSearchRequest != null)
            {
                var res = await meldService.SearchFinancialAccount(AccSearchRequest);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        #endregion Accounts

        #region Transactions
        /// <summary>
        /// Use this endpoint to retrieve transaction information of a specific financial account transaction.
        /// </summary>
        /// <param name="transactionId">Transaction id</param>
        /// <returns>Returns transaction information of a specific financial account transaction.</returns>
        [Route("GetTransaction")]
        [HttpPost]
        public async Task<IActionResult> GetFinancialAccountTransaction(string transactionId)
        {
            if (!string.IsNullOrEmpty(transactionId))
            {
                var res = await meldService.GetFinancialAccountTransactions(transactionId);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        /// <summary>
        /// Use this endpoint to filter financial account transactions by various parameters.
        /// </summary>
        /// <param name="financialAccountId">Search filters</param>
        /// <returns></returns>
        [Route("SearchTransaction")]
        [HttpPost]
        public async Task<IActionResult> SearchFinancialAccountTransactions(MeldTransactionSearchRequest TransSearchRequest)
        {
            if (TransSearchRequest != null)
            {
                var res = await meldService.SearchFinancialAccountTransactions(TransSearchRequest);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }

        #endregion Transactions
        */

        #region Webhook Calls for testing
        //[Route("SaveInstitution")]
        //[HttpPost]
        //public async Task<IActionResult> RegisterMeldProviderForClient(MeldWebhookInfo webhookInfo)
        //{
        //    string clientID = (string)HttpContext.Items["ClientId"];
        //    string clientName = (string)HttpContext.Items["ClientName"];
        //    long clientInfoID = 0;
        //    Int64.TryParse((string)HttpContext.Items["ClientInfoID"],out clientInfoID);

        //    var res = await meldService.RegisterMeldProviderForClient(webhookInfo, clientInfoID);
        //    if (res != null && res.StatusCode == StatusCodes.Status200OK)
        //        return Ok(res);
        //    else
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        //}

        //[Route("DeleteInstitution")]
        //[HttpPost]
        //public async Task<IActionResult> ProcessDeleteFinancialAccounts(MeldWebhookInfo webhookInfo)
        //{
        //    var res = await meldService.ProcessConnectionStatusChange(webhookInfo);
        //    if (res != null && res.StatusCode == StatusCodes.Status200OK)
        //        return Ok(res);
        //    else
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        //}

        //[Route("SaveInstitutionAccounts")]
        //[HttpPost]
        //public async Task<IActionResult> ProcessSearchFinancialAccounts(MeldWebhookInfo webhookInfo)
        //{
        //    var res = await meldService.ProcessUpdateFinancialAccounts(webhookInfo);
        //    if (res != null && res.StatusCode == StatusCodes.Status200OK)
        //        return Ok(res);
        //    else
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        //}

        //[Route("UpdateInstitution")]
        //[HttpPost]
        //public async Task<IActionResult> ProcessConnectionStatusChange(MeldWebhookInfo webhookInfo)
        //{
        //    var res = await meldService.ProcessConnectionStatusChange(webhookInfo);
        //    if (res != null && res.StatusCode == StatusCodes.Status200OK)
        //        return Ok(res);
        //    else
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        //}

        //[Route("SaveTransactions")]
        //[HttpPost]
        //public async Task<IActionResult> ProcessTransactions(MeldWebhookInfo webhookInfo)
        //{
        //    var res = await meldService.ProcessTransactions(webhookInfo);
        //    if (res != null && res.StatusCode == StatusCodes.Status200OK)
        //        return Ok(res);
        //    else
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        //}
        #endregion Webhook Calls for testing
    }
}
