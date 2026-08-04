using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;

// TODO: Should Move this file to DailySale module.
namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [Authorize]
    public class DailySaleController : BaseController
    {
        #region Fields

        private readonly IDailySaleService dailysalesrv;

        #endregion

        #region Ctor
        public DailySaleController(IDailySaleService dailySaleService)
        {
            this.dailysalesrv=dailySaleService;
        }

        #endregion

        #region Daily Sale

        [Route("DailySale/Lines")]
        [HttpPost]

        /// <summary>
        /// This returns all the active lines in the corporation
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>It returns DailySaleLinesResponse</returns>
        public async Task<IActionResult> GetConfigLines(DailySaleRequest LineRequest)
        {
            DailySaleLinesResponse? response = null;
            try
            {
                if(!string.IsNullOrEmpty(LineRequest.CorpID))
                {
                    response = await dailysalesrv.GetConfigLines(LineRequest);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Constants.MSG_CORP_REQ);
                }
            }
            catch { throw; }
            finally { response = null; }
        }
        #endregion
    }
}
