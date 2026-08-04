using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DTO.Req;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;

namespace BankFeed.API.Controllers
{
    [Route("v1/BFeed/")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class BFeed_importController : Controller
    {
        #region Fields
        private readonly IBFeed_ImportService BFeed_ImportService;
        #endregion

        #region Ctor
        public BFeed_importController(IBFeed_ImportService _bFeed_ImportService)
        {
            this.BFeed_ImportService = _bFeed_ImportService;
        }
        #endregion


        [Route("ImportFeedTransactions")]
        [HttpPost]
        public async Task<IActionResult> ImportFeedTransactions(BFeed_importRequest Request)
        {
            StatusDTO response = null;
            try
            {
                response = await BFeed_ImportService.ImportFeedTransactions(Request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }
    }
}
