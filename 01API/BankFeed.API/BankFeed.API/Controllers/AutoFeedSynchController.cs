using BankFeed.App.Contracts;
using Common.API.ActionFilters;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;
namespace BankFeed.API.Controllers
{
    [Route("v1/BankFeedAuto")]
    [ApiController]
    [ValidateModel]
    public class AutoFeedSynchController : ControllerBase
    {
        #region Fields

        private readonly IProviderInfo providerInfo;
        private readonly IAutoSynchFeed autoSynchFeed;

        #endregion

        #region Ctor
        public AutoFeedSynchController(IAutoSynchFeed _autoSynchFeed)
        {
            this.autoSynchFeed = _autoSynchFeed;


        }
        #endregion
        [Route("Synch")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AutoSynchFeeds()
        {
            GenericLongResponse response = await autoSynchFeed.AutoSynchBankFeeds();
            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(response);
        }
        [Route("InActiveAccSynch")]
        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> InActiveAccountAutoSynch()
        {
            GenericLongResponse response = await autoSynchFeed.InActiveAccountsSynchBankFeeds();
            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(response);
        }

        [Route("ForcedRefresh")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForcedConnectionRefresh()
        {
            GenericLongResponse response = await autoSynchFeed.ForcedRefreshForActiveConnections();
            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            else
                return Ok(response);
        }
    }
}
