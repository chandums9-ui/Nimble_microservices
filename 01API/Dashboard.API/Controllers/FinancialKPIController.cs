using Amazon.Runtime.Internal;
using Azure.Core;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Dashboard.App.Contracts;
using Dashboard.App.Services;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace Dashboard.API.Controllers
{
    [Route("v1/Financial")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class FinancialKPIController : ControllerBase
    {
        #region Fields
        private readonly IFinancialKPIService financialServices;
        #endregion

        #region Ctor
        public FinancialKPIController(IFinancialKPIService _financialServices)
        {
            this.financialServices = _financialServices;
        }
        #endregion

        #region Endpoints
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        /// <summary>
        /// To get balance sheet data monthly for corporations based on  
        /// </summary>
        /// <param name="Request"> userid, corporationid, fromdate & todate</param>
        /// <returns>returns list of balance sheets related to multiple corporations</returns>
        [Route("BalanceSheetTable")]
        [HttpPost]
        public async Task<IActionResult> GetBalanceSheetTable(AnalyticsRequest Request)
        {
            BalanceSheetTableResponse response = null;
            try
            {

                //Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
                //Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await financialServices.GetBalanceSheetTable(Request, urlInfoID,getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        /// <summary>
        /// To get balance sheet report data monthly for selected corporation  
        /// </summary>
        /// <param name="Request">  corporationId, fromdate & todate</param>
        /// <returns>returns balance sheet data for a corporation for bar graphs data</returns>
        [Route("BalanceSheetGraphical")]
        [HttpPost]
        public async Task<IActionResult> GetBalanceSheetGraphical(FinancialRequest Request)
        {
            BalanceSheetGraphResponse response = null;
            try
            {
                //Request.CorporationId = (!string.IsNullOrEmpty(Request.CorporationId) && !Request.CorporationId.Contains("0x")) ? "0x" + Request.CorporationId : Request.CorporationId;

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await financialServices.GetBalanceSheetGraphical(Request, urlInfoID, getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        /// <summary>
        /// To get chart data for comparing Assets & Liabilities 
        /// </summary>
        /// <param name="Request">UserID,CorpID,comparisonfilter,From & To Dates</param>
        /// <returns>returns month wise aseets & liabilities ratios</returns>
        [Route("AssetLiability")]
        [HttpPost]
        public async Task<IActionResult> GetAssetLiabilities(AssetLiabilityRequest request)
        {
            AssetLiabilityResponse response = null;
            try
            {
                response = await financialServices.GetAssetLiabilityRatios(request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        /// <summary>
        /// It will get the  Profit and Loss details for the multiple corporations
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return the Income,Expense,COGS,OtherIncome,NetIncome,Grosfprofit </returns>
        [Route("PandLTableData")]
        [HttpPost]
        public async Task<IActionResult> GetPandLDetails(AnalyticsRequest Request)
        {
            ProfitandLossResponse response = null;
            try
            {
                //Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
                // Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await financialServices.GetPandLDetails(Request, urlInfoID, getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        [Route("PandLGraphData")]
        [HttpPost]
        /// <summary>
        /// It will get the  Profit and Loss details Monthwise for the selected Corporation
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return the Income,Expense,COGS,OtherIncome,NetIncome,Grosfprofit </returns>
        public async Task<IActionResult> GetPandLGraphDetails(FinancialRequest Request)
        {
            PandLGraphResponse response = null;
            try
            {
                //Request.CorporationId = (!string.IsNullOrEmpty(Request.CorporationId) &&!Request.CorporationId.Contains("0x")) ? "0x" + Request.CorporationId : Request.CorporationId;

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await financialServices.GetPandLGraphData(Request, urlInfoID, getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }
        [Route("ARagingTableData")]
        [HttpPost]
        /// <summary>
        /// It will get the ARaging details for multiple corporations
        /// </summary>
        /// <param name="Request"></param>
        /// <returns> It returns ARaging details i.e ledgers for corporation wise </returns>
        public async Task<IActionResult> GetArAgingTableData(AnalyticsRequest Request)
        {
            ARagingTableViewResponse response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await financialServices.GetAragingTableResponse(Request, urlInfoID, getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        /// <summary>
        /// It will get AR aging details for Non Operating properties based CorporationId's and as of date
        /// </summary>
        /// <param name="Request"></param>
        /// <returns> It returns aging  wise details for all the Non-Operating corporations</returns>
        [Route("NonOperatingARaging")]
        [HttpPost]
        public async Task<IActionResult> GetArAgingNonOperating(AnalyticsRequest Request)
        {
            NonOperatingARagingResponse? response = null;
            try
            {
                //Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
                //Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await financialServices.GetNonOperatingARagingDetails(Request, urlInfoID, getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        /// <summary>
        /// It will get the corporation wise aging totals for non-operating corporations and ledgers totals for operating corporations
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return the corporation wise aging totals for non-operating corporations and ledgers totals for operating corporations</returns>
        [Route("ARTotals")]
        [HttpPost]
        public async Task<IActionResult> GetAROperatingandARNonOperatingTotals(AnalyticsRequest Request)
        {
            ARagingTotalsResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await financialServices.GetARagingTotals(Request, urlInfoID, getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        [Route("PandLGraphDetailsforMobile")]
        [HttpPost]
        /// <summary>
        /// It will get the  Profit and Loss details Monthwise for the selected Corporation
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return the Income,Expense,COGS,OtherIncome,NetIncome,Grosfprofit </returns>
        public async Task<IActionResult> GetPandLGraphDetailsmobile(FinancialRequest Request)
        {
            PandlMobileResponse response = null;
            try
            {
                //Request.CorporationId = (!string.IsNullOrEmpty(Request.CorporationId) &&!Request.CorporationId.Contains("0x")) ? "0x" + Request.CorporationId : Request.CorporationId;

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await financialServices.GetPandLDataforMobile(Request, urlInfoID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }


        #endregion
    }
}
