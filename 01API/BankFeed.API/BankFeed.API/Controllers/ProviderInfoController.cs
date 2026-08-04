using BankFeed.App.Contracts;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Resp;
using BankFeed.Domain.DTO.Model;
using Microsoft.AspNetCore.Mvc;
using BankFeed.App.Services;
using BankFeed.Domain.DTO.Resp;

namespace BankFeed.API.Controllers
{
    [Route("v1/Provider")]
    [ApiController]
    [Authorize]
    [ValidateModel]

    public class ProviderInfoController : ControllerBase
    {
        #region Fields

        private readonly IProviderInfo providerInfo;
        private readonly IYodleeService yodleeService;

        #endregion

        #region Ctor
        public ProviderInfoController(IProviderInfo _providerInfo, IYodleeService _yodleeService, IPlaidService _plaidService)
        {
            this.providerInfo = _providerInfo;
            this.yodleeService = _yodleeService;

        }

        #endregion

        #region Provider

        /// <summary>
        /// To get list of providers in Nimble
        /// </summary>
        /// <returns>providers list</returns>
        [Route("List")]
        [HttpGet]
        public async Task<IActionResult> ProviderList()
        {
            try
            {
                return Ok(await providerInfo.GetProvidersList());
            }
            catch { throw; }



        }

        /// <summary>
        /// To register with a provider in Nimble
        /// </summary>
        /// <param name="reqDTO"></param>
        /// <returns>registration ID and Status</returns>
		[Route("Register")]
        [HttpPost]
        public async Task<IActionResult> ProviderRegister(GenericLongIDDTO reqDTO)
        {
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                string clientName = (string)HttpContext.Items["ClientName"];
                string clientInfoID = (string)HttpContext.Items["ClientInfoID"];
                GenericLongStringResponse genericLongRespDTO = await providerInfo.RegisterProviderByClientID(reqDTO, clientID, clientName, clientInfoID);
                if (genericLongRespDTO != null) { return Ok(genericLongRespDTO); }
                else
                    return NotFound(genericLongRespDTO);


            }
            catch { throw; }
        }

        /// <summary>
        /// To register with a provider in Nimble
        /// </summary>
        /// <param name="reqDTO"></param>
        /// <returns>registration ID and Status</returns>
        [Route("SynchFeeds")]
        [HttpPost]
        public async Task<IActionResult> SynchFeeds(string? accountId, short providerType, DateTime fromDate, DateTime toDate)
        {
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            ConnectionStatus conResp = await providerInfo.SynchFeeds(providerType, fromDate, toDate, null, accountId, null, clientID, clientName);

            if (conResp == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(conResp);
        }

        

        #endregion



    }
}
