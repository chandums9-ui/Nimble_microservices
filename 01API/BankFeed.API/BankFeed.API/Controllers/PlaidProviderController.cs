using Microsoft.AspNetCore.Mvc;
using BankFeed.App.Contracts;
using BankFeed.Domain.DTO.Req;
using Common.API.ActionFilters;
using Common.API.Authorization;
using BankFeed.Domain.DTO.Model;
using BankFeed.Domain.DTO.Resp;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Resp;
using Microsoft.Extensions.Options;

namespace BankFeed.API.Controllers
{
    [Route("v1/Plaid")]
    [ApiController]
    [ValidateModel]
    //[Authorize]
    public class PlaidProviderController : ControllerBase
    {
        #region Fields
        private readonly IPlaidService PlaidService;
        private readonly PlaidKeyInfo plaidKeyInfo;

        #endregion
        #region Ctor
        public PlaidProviderController(IPlaidService _plaidService, IOptions<PlaidKeyInfo> _plaidKeyInfo)
        {
            this.PlaidService = _plaidService;
            this.plaidKeyInfo = _plaidKeyInfo.Value;
          
        }
        #endregion

        #region PlaidConnection
        /// <summary>
        /// Get Plaid Accounts and Balance
        /// </summary>
        /// <param name="connectionReq"></param>
        /// <returns>Http Status</returns>
        [Route("PlaidConnect")]
        [HttpPost]
        public async Task<IActionResult> Connect(ConnectionRequest connectionReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            //var ProviderRegID = unitOfWork.ProviderRegisters.GetAll(s => s.ClientId == new PFAID(clientID).UID && s.ProviderId == connReq.ProviderID).Result.Select(s => s.Id).FirstOrDefault();
            ConnectionStatus conResp =await PlaidService.PlaidConnect(connectionReq, clientID, clientName);
            if (conResp == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(conResp);
        }
        #endregion

        #region PlaidReconnection
        /// <summary>
        /// Get Plaid Accounts and Balance
        /// </summary>
        /// <param name="connectionReq"></param>
        /// <returns>Http Status</returns>
        [Route("ReConnect")]
        [HttpPost]
        public async Task<IActionResult> ReConnect(ConnectionRequest connectionReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            ConnectionStatus conResp = await PlaidService.PlaidReConnect(connectionReq);
            if (conResp == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(conResp);
        }
        #endregion

        #region PlaidSynchFeeds
        /// <summary>
        /// Get Plaid Accounts and Balance
        /// </summary>
        /// <param name="connectionReq"></param>
        /// <returns>Http Status</returns>
        [Route("SynchFeeds")]
        [HttpPost]
        public async Task<IActionResult> SynchFeeds(ConnectionRequest connectionReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            ConnectionStatus conResp = await PlaidService.PlaidSynchFeeds(connectionReq);
            if (conResp == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(conResp);
        }
        #endregion

        #region Plaid ProviderRegID
        /// <summary>
        /// Get ProviderRegisterID
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns>ProviderRegID</returns>
        [Route("ProviderID")]
        [HttpPost]
        public async Task<IActionResult> GetProviderRegID(GenericLongIDDTO reqDTO)
        {
            try
            {
                string clientID = HttpContext.Items["ClientId"].ToString();
                GenericLongResponse genResp = await PlaidService.GetProviderRegId(reqDTO.ID, clientID);
                if (genResp != null) { return Ok(genResp); }
                else
                    return NotFound(genResp);
            }
            catch { throw; }
        }
        #endregion

        [Route("CheckingBankConnection")]
        [HttpPost]
        public async Task<IActionResult> CheckingBankConnection(ConnectionCheckingReqDTO Request)
        {
            try
            {
                string clientID = HttpContext.Items["ClientId"].ToString();
                Request.ClientID = clientID;
                GenericLongResponse genResp = await PlaidService.CheckingProvConection(Request);
                if (genResp != null) { return Ok(genResp); }
                else
                    return NotFound(genResp);
            }
            catch { throw; }
        }



        #region Plaid ProviderToken
        /// <summary>
        /// Get ProviderToken
        /// </summary>
        /// <param name="FeedAccID"></param>
        /// <returns>ProviderAccID</returns>
        [Route("ProviderToken")]
        [HttpPost]
        public async Task<IActionResult> GetProviderToken(GenericLongIDDTO reqDTO)
        {
            try
            {
                //string clientID = HttpContext.Items["ClientId"].ToString();
                GenericLongStringResponse genResp = await PlaidService.GetProviderToken(reqDTO.ID);
                if (genResp != null) { return Ok(genResp); }
                else
                    return NotFound(genResp);
            }
            catch { throw; }
        }
        #endregion

        #region Plaid Environment Details
        /// <summary>
        /// Get Plaid Environment Details
        /// </summary>
        /// <param name=""></param>
        /// <returns>Environment Details</returns>
        [Route("EnvironmentDetails")]
        [HttpGet]
        public IActionResult GetEnvironmentDetails()
        {
            try
            {
                EnvironmentDetailsResponse envDetails = new EnvironmentDetailsResponse();
                envDetails.ClientName = plaidKeyInfo.ClientName.ToString();
                envDetails.Environment = plaidKeyInfo.Environment.ToString();
                envDetails.Key = plaidKeyInfo.clientSecret.ToString();
                return Ok(envDetails);
            }
            catch { throw; }
        }
        [Route("PublicToken")]
        [HttpPost]
        public async Task<IActionResult> GetPublicToken(AccessTokenDTO reqDTO)
        {
            try
            {
                //string clientID = HttpContext.Items["ClientId"].ToString();
                PlaidTokenRespInfo genResp = await PlaidService.PlaidCreatePublicToken(reqDTO);
                if (genResp != null) { return Ok(genResp); }
                else
                    return NotFound(genResp);
            }
            catch { throw; }
        }
        #endregion

    }
}
