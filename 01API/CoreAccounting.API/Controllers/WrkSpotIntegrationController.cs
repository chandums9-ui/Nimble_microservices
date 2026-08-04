using Common.API.ActionFilters;
using CoreAccounting.App.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CoreAccounting.API.Controllers
{
    [Route("v1/WrkSpot")]
    [ApiController]
    [ValidateModel]
    public class WrkSpotIntegrationController : BaseController
    {
        #region Fields
        private readonly IWrkSpotIntegrationService wrkSpotService;
        #endregion
        #region Ctor
        public WrkSpotIntegrationController(IWrkSpotIntegrationService _wrkSpotService)
        {
            this.wrkSpotService = _wrkSpotService;

        }
        #endregion

        #region wrkSpot
        [Route("Summary")]
        [HttpPost]
        public async Task<IActionResult> SummaryDetails()
        {
            //string Resp=string.Empty;
            string clientID = (string)HttpContext.Items["ClientId"];
            string clientName = (string)HttpContext.Items["ClientName"];
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);

            await wrkSpotService.SummaryDetails(clientID,urlKey);


            return Ok();
        }

        #endregion
    }
}
