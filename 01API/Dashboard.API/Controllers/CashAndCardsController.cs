using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Microsoft.AspNetCore.Mvc;
using Dashboard.App.Contracts;
using Dashboard.App.Services;
using Dashboard.Domain.DTO.Req;
using Azure;
using Dashboard.Domain.DTO.Resp;
using Common.Domain.DTO.Model.Base;
using Azure.Core;
using System.Globalization;
using Amazon.Runtime.Internal;
using System.Diagnostics;
using Common.Domain.DTO.Resp;
using Common.Domain.DTO.Req;

namespace Dashboard.API.Controllers
{
    [Route("v1/CashAndCards")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class CashAndCardsController : ControllerBase
    {
        #region Fields
        private readonly ICashAndCardsService CashAndCardsServices;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public CashAndCardsController(ICashAndCardsService _CashAndCardsServices, IConfiguration configuration)
        {
            this.CashAndCardsServices = _CashAndCardsServices;
            _configuration = configuration;
        }
        #endregion

        #region 
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        [Route("CashAndCardSummary")]
        [HttpPost]
        public async Task<IActionResult> GetCashAndCardSummary(CashandCardSummaryReq Request)
        {


            List<CashAndCardSummaryRespo> response = new List<CashAndCardSummaryRespo>();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await CashAndCardsServices.GetCashAndCardSummary(Request, urlInfoID,getClientName());
                return Ok(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }
        [Route("CashAndCardDetails")]
        [HttpPost]
        public async Task<IActionResult> GetCashAndCardDetails(CashandCardDetailReq Request)
        {
            List<CashAndCardDetailsRespo> response = new List<CashAndCardDetailsRespo>();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await CashAndCardsServices.GetCashAndCardDetails(Request, urlInfoID,getClientName());
                return Ok(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }
        #endregion
    }
}


