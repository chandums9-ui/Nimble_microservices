using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DataModel;
using BankFeed.Domain.DTO.Model;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;

namespace BankFeed.API.Controllers
{
    [Route("v1/Yodlee")]
    [ApiController]
    [ValidateModel]
    public class YodleeProviderController : ControllerBase
    {
        #region Fields
        private readonly IYodleeService YodleeService;
        private readonly IProviderInfo providerInfo;
        #endregion
        #region Ctor
        public YodleeProviderController(IYodleeService _yodleeService, IProviderInfo _providerInfo)
        {
            this.YodleeService = _yodleeService;
            this.providerInfo = _providerInfo;
        }
        #endregion

        #region PlaidConnectionORReconnection
        /// <summary>
        /// Get Yodlee Accounts and Balance
        /// </summary>
        /// <param name="connectionReq"></param>
        /// <returns>Http Status</returns>
        [Route("Connect")]
        [HttpPost]
        public async Task<IActionResult> Connect(YodleeConnectionReqDTO connectionReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            ConnectionStatus conResp = await YodleeService.YodleeConnect(connectionReq, clientID, clientName);
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
        /// Get Yodlee Accounts and Balance
        /// </summary>
        /// <param name="connectionReq"></param>
        /// <returns>Http Status</returns>
        [Route("ReConnect")]
        [HttpPost]
        public async Task<IActionResult> ReConnect(YodleeConnectionReqDTO connectionReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            ConnectionStatus conResp = await YodleeService.YodleeReConnect(connectionReq, clientID, clientName);
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
        /// Get Yodlee Accounts and Balance
        /// </summary>
        /// <param name="connectionReq"></param>
        /// <returns>Http Status</returns>
        [Route("SynchFeeds")]
        [HttpPost]
        public async Task<IActionResult> SynchFeeds(YodleeConnectionReqDTO connectionReq)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            ConnectionStatus conResp = await YodleeService.YodleeSynchFeeds(connectionReq, clientID, clientName);
            if (conResp == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(conResp);
        }
        #endregion
        #region Yodlee ProviderRegID
        /// <summary>
        /// Get ProviderRegisterID and User_Token
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns>ProviderRegID and User_Token</returns>
        [Route("ProviderID")]
        [HttpPost]
        public async Task<IActionResult> GetProviderRegIDToken(GenericLongIDDTO reqDTO)
        {
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                string clientName = (string)HttpContext.Items["ClientName"];
                GenericLongStringResponse genResp = await YodleeService.GetProviderRegIDToken(reqDTO.ID,clientID, clientName);
                if (genResp != null) { return Ok(genResp); }
                else
                    return NotFound(genResp);
            }
            catch { throw; }
        }
        #endregion
    }
}
