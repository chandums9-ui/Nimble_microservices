using Amazon.Runtime.Internal;
using Azure;
using Azure.Core;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Dashboard.App.Contracts;
using Dashboard.App.Services;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Diagnostics;
using System.Globalization;

namespace Dashboard.API.Controllers
{
    [Route("v1/DailyReview")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class DailyReviewController : ControllerBase
    {
        #region Fields
        private readonly IDailyReviewService dailyServices;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public DailyReviewController(IDailyReviewService _dailyServices, IConfiguration configuration)
        {
            this.dailyServices = _dailyServices;
            _configuration = configuration;
        }
        #endregion

        #region Endpoints
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        /// <summary>
        /// To get performance details for Corporation/s based on CorpId /UserId and dates
        /// </summary>
        /// <param name="Request">userId,Corporation ID and From & To Date</param>
        /// <returns>returns a list of fields decribing property's performance like.,occupancy,revenue,etc.. in PerformanceResponse </returns>
        [Route("Performance")]
        [HttpPost]
        public async Task<IActionResult> GetPerformanceDetails(AnalyticsRequest Request)
        {
            PerformanceResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetPropertyPerformance(Request, urlInfoID,getClientName());
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
        /// To get Departmental Expenses and Incomes for Corporation/s based on CorpId /UserId and dates
        /// </summary>
        /// <param name="Request">userId,Corporation ID and From & To Date</param>
        /// <returns>returns list of Departmental expenses,incomes etc.. in DepartmentalResponse </returns>
        [Route("DepartmetalInfo")]
        [HttpPost]
        public async Task<IActionResult> GetDepartmentDetails(AnalyticsRequest Request)
        {
            PerformanceResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                //just for data visualization need to remove the static data while moving to server
                response = await dailyServices.GetPropertyDepartmentalInfo(Request, urlInfoID, getClientName());
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

        ///// <summary>
        ///// To get graphical data to visualize selected group statistics 
        ///// </summary>
        ///// <param name="Request">selected GroupId, CorporationId,From & To Dates, UserID</param>
        ///// <returns>returns graph data</returns>
        //[Route("GraphData")]
        //[HttpPost]
        //public async Task<IActionResult> GetGraphicalData(GraphicalRequest Request)
        //{
        //    GraphicalReponse? response = null;
        //    try
        //    {
        //        Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
        //        Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];

        //        response = await dailyServices.GetGraphData(Request);
        //        if (response != null && response.StatusCode == StatusCodes.Status200OK)
        //            return Ok(response);
        //        else if (response != null && !string.IsNullOrEmpty(response.Status))
        //            return Ok(response);
        //        else
        //            return NotFound(response);
        //    }
        //    catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
        //    finally { response = null; }
        //}

        /// <summary>
        /// To get all the expenses for corporation/s based on UserID & Corporation ID
        /// </summary>
        /// <param name="Request">UserId,CorpID,From & To Dates</param>
        /// <returns> returns list of expenses </returns>
        [Route("Expense")]
        [HttpPost]
        public async Task<IActionResult> GetExpenseAnalysis(AnalyticsRequest Request)
        {
            ExpenseResponse? response = null;
            try
            {
                // Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
                // Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];

                response = await dailyServices.GetExpenseAnalysis(Request, getClientName());
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


        ///// <summary>
        ///// To get management groups based on ClientID
        ///// </summary>
        ///// <returns>list of management group name & Id's</returns>
        //[Route("ManagementGroups")]
        //[HttpGet]
        //public async Task<IActionResult> GetManagementGroups()
        //{
        //    PortfolioResponse? response = null;
        //    try
        //    {
        //Request.UserId = "0x" + (string) HttpContext.Items["UserId"];
        //Request.ClientId = "0x" + (string) HttpContext.Items["ClientId"];
        //        response = await dailyServices.GetPortfolios(HttpContext.Items["ClientId"].ToString());
        //        if (response != null)
        //        {
        //            return Ok(response);
        //        }
        //        else
        //            return NotFound(response);
        //    }
        //    catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
        //    finally { response = null; }
        //}

        /// <summary>
        /// To get flex analysis details  based on corporationId 
        /// </summary>
        /// <param name="Request"> corpId,userId,from & to dates </param>
        /// <returns>returns list of flex analysis</returns>
        [Route("FlexAnalysis")]
        [HttpPost]
        public async Task<IActionResult> GetFlexAnalysis(AnalyticsRequest Request)
        {
            FlexResponse? response = null;
            try
            {
                //Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
                //Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];
                //Request.CorporationId = (!string.IsNullOrEmpty(Request.CorporationId) && !Request.CorporationId.Contains("0x")) ? "0x" + Request.CorporationId : Request.CorporationId;

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetFlexAnalysis(Request, urlInfoID, getClientName());
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
        /// It will return the cash and card due balances details a based on the provided CorporationId and as of date.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 

        //Not using this method

        [Route("CashandCardDetails")]
        [HttpPost]
        public async Task<IActionResult> GetCashandCardDeatils(CashCardRequest request)
        {
            CashCardsResponse? response = null;
            try
            {
                response = await dailyServices.GetCashandCardDetails(request, getClientName());
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



        [Route("CashandCardTypesDetails")]
        [HttpPost]
        public async Task<IActionResult> GetCashandCardTypesDeatils(CashandCardSummaryReq request)
        {
            CashCardsResponse response = new CashCardsResponse();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                request.UrlKey = urlInfoID;
                response = await dailyServices.GetCashandCardTypeDetails(request, getClientName());
                return Ok(response ?? new CashCardsResponse());
                //if (response != null && response.StatusCode == StatusCodes.Status200OK)
                //    return Ok(response);
                //else if (response != null && !string.IsNullOrEmpty(response.Status))
                //    return Ok(response);
                //else
                //    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        [Route("CashandCardGridDetails")]
        [HttpPost]
        public async Task<IActionResult> GetCashandCardGridDeatils(CashandCardWidgetReq request)
        {
            CCGridList response = new CCGridList();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                request.UrlKey = urlInfoID;

                response = await dailyServices.GetCashandCardGridDetails(request, getClientName());
                return Ok(response ?? new CCGridList());
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
        ///  It will return the ledger details based on the provided CorporationId and AsofDate. 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [Route("ReceiveblesDetails")]
        [HttpPost]
        public async Task<IActionResult> GetReceviblesandAdvances(AnalyticsRequest Request)
        {
            ReceivablesResponse? response = null;
            try
            {
                //Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
                //Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];
                //Request.CorporationId = (!string.IsNullOrEmpty(Request.CorporationId) && !Request.CorporationId.Contains("0x")) ? "0x" + Request.CorporationId : Request.CorporationId;

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetReceivablesDetails(Request, urlInfoID, getClientName());
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
        /// To get cash balance and credit card due balances as of date for given corporation 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return cash balance and credit card due blance details as of date </returns>
        [Route("DuebalanceDetails")]
        [HttpPost]
        public async Task<IActionResult> GetDuedetail(DueBalancesRequest request)
        {
            DueBalancesResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetDueBalances(request, urlInfoID, getClientName());
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
        /// To get cash balance and credit card due balances as of date for all corporation 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return cash balance and credit card due blance details as of date for all corporation </returns>
        [Route("MultiCorpDuebalanceDetails")]
        [HttpPost]
        public async Task<IActionResult> GetMultiCorpDuebalanceDetails(AnalyticsRequest request)
        {
            CashandCardDuesResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                var startTime = Stopwatch.StartNew();
                var startTimestamp = DateTime.Now;
                response = await dailyServices.GetMultiCorpDueBalances(request, urlInfoID, getClientName());
                startTime.Stop();
                var endTimestamp = DateTime.Now;
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
        /// It will return already due and upcoming dues for vendors based on the provided CorporationId and AsOfDate.
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return Top ten dues for the vendor along with the details</returns>

        [Route("VendorDues")]
        [HttpPost]
        public async Task<IActionResult> GetVendorDues(VendorDuesRequest Request)
        {
            VendorDuesResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                string clientName = Convert.ToString(HttpContext.Items["ClientName"]);
                bool isAPEnabled = GetAPEnabledClients(clientName);
                Request.Client = isAPEnabled ? "newclient" : "oldclient";
                
                response = await dailyServices.GetVendorDues(Request, urlInfoID, getClientName());
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

        private bool GetAPEnabledClients(string clientName)
        {
            
            string apEnabledClients = _configuration["APEnabledClients"];
            var allowedClients = apEnabledClients?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return (allowedClients != null && (allowedClients[0] == "default" || allowedClients.Contains(clientName, StringComparer.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// To get payable related information
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return amount between the aging days </returns>
        [Route("PayableDetails")]
        [HttpPost]
        public async Task<IActionResult> GetPayableDetails(PayablesRequest Request)
        {
            PayableResponse? response = null;
            try
            {
                //request.ToDate = DateTime.Now.Date;               
                //Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
                //Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];
                //Request.CorporationId = (!string.IsNullOrEmpty(Request.CorporationId) && !Request.CorporationId.Contains("0x")) ? "0x" + Request.CorporationId : Request.CorporationId;

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                string clientName = Convert.ToString(HttpContext.Items["ClientName"]);
                bool isAPEnabled = GetAPEnabledClients(clientName);
                Request.Client = isAPEnabled ? "newclient" : "oldclient";

                response = await dailyServices.GetPayableResponses(Request, urlInfoID, getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw;/*return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);*/ }
            finally { response = null; }
        }

        /// <summary>
        /// It will get Month wise departmnets and statics  trends data for the sales and property over view
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It returns month wise details for the sales and property over view</returns>
        [Route("MonthlyDepartmentgraph")]
        [HttpPost]
        public async Task<IActionResult> GetDepartmentmonthlygraph(TrendsGraphRequest Request)
        {
            DepartmnetGraphResponse? response = null;
            try
            {

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetDepartmnetGraph(Request, urlInfoID, getClientName());
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
        /// Retrieves the income statement details on a daily basis.
        /// </summary>
        /// <param name="Request">GraphicalRequest object containing request parameters.</param>
        /// <returns>Returns a DepartmnetGraphResponse object.</returns>
        [Route("DailyDepartmentgraph")]
        [HttpPost]
        public async Task<IActionResult> GetDailyDepartmentgraph(TrendsGraphRequest Request)
        {
            DepartmnetGraphResponse? response = null;
            try
            {
                // Request.UserId = "0x" + HttpContext.Items["ClientId"].ToString();
                // Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];
                // Request.CorporationId = (!string.IsNullOrEmpty(Request.CorporationId) && !Request.CorporationId.Contains("0x")) ? "0x" + Request.CorporationId : Request.CorporationId;

                response = await dailyServices.GetIncomeStatementDailyDeatils(Request, getClientName());
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
        /// It will get Performances related to multiple corporations 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("MultiCorpPerformances")]
        [HttpPost]
        public async Task<IActionResult> GetMultiCorpPerformances(AnalyticsRequest request)
        {
            PerformanceResponse response = new PerformanceResponse();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetPerformancesforAllCorp(request, urlInfoID, getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; }
            finally { response = null; }

        }
        /// <summary>
        /// It will get required corporations related stats and Departments revenues based on  from date and to date.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>It will return required corporations related stats and Departments revenues based on  from date and to date</returns>
        [Route("MulticorpDeptsandStats")]
        [HttpPost]
        public async Task<IActionResult> GetMultiCorpDeptsandStats(AnalyticsRequest request)
        {
            PerformanceResponse response = new PerformanceResponse();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetStatsandDepartmentsforAllCorp(request, urlInfoID, getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; }
            finally { response = null; }

        }
        /// <summary>
        /// It will save the request IdS for the corporations 
        /// </summary>
        /// <param name="ListCorporations"></param>
        /// <returns></returns>
        [Route("GetCorporationsRequestID")]
        [HttpPost]
        public async Task<IActionResult> GetCorporationsRequestID(List<ModelBaseCorporationID> Corporations)
        {
            RequestIDResponse? response = null;
            try
            {
                response = await dailyServices.SaveCorporationsRequestID(Corporations);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; }
            finally { }
        }

        /// <summary>
        /// It will get Revenues , Expenses and Profits for the given Corporation  based on from data and to date 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>It will return the Revenues,Expenses and Profits for the given Corporation  based on from data and to date </returns>
        [Route("FinancialIncomeStatement")]
        [HttpPost]
        public async Task<IActionResult> GetFinancialData(AnalyticsRequest request)
        {
            HotelFinancialData? response = null;
            try
            {
                response = await dailyServices.GetFinancialData(request, getClientName());
                if (response != null)
                    return Ok(response);
                //else if (response != null && !string.IsNullOrEmpty(response.Status))
                //    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; }
            finally { }
        }


        [Route("CashandCardCardData")]
        [HttpPost]
        public async Task<IActionResult> GetCashandCardCardDetails(CashandCardWidgetReq request)
        {
            CashCardWidgetCardResponse response = new CashCardWidgetCardResponse();
            try
            {
                response = await dailyServices.GetCashCardWidgetCardData(request, getClientName());
                return Ok(response);
                //if (response != null && response.StatusCode == StatusCodes.Status200OK)
                //    return Ok(response);
                //else if (response != null && !string.IsNullOrEmpty(response.Status))
                //    return Ok(response);
                //else
                //    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        [Route("CashandCardGridData")]
        [HttpPost]

        public async Task<IActionResult> GetCashCardWidgetGrid(CashandCardWidgetReq request)
        {
            CashandCardWidgetResponse response = new CashandCardWidgetResponse();
            try
            {
                response = await dailyServices.GetCashCardWidgetGridData(request);
                return Ok(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("GetDepartmentData")]
        [HttpPost]
        public async Task<IActionResult> GetDepartmentData(DepartmentIncomeRequest request)
        {
            try
            {
                var response = await dailyServices.GetDepartmentIncomeDetails(Convert.ToInt64(HttpContext.Items["UrlID"]), request, getClientName());
                return response != null && response.StatusCode == StatusCodes.Status200OK ? Ok(response) : (response != null && !string.IsNullOrEmpty(response.Status)) ? Ok(response) : NotFound(response);
            }
            catch
            {
                return BadRequest("Error while processing request");
            }
        }

        [Route("AllCorpStats")]
        [HttpPost]
        public async Task<IActionResult> GetAllCorporationsStats(AnalyticsRequest request)
        {

            PerformanceResponse response = new PerformanceResponse();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetAllCorpStats(request, urlInfoID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; }
            finally { response = null; }
        }


        [Route("AllCorpRevenues")]
        [HttpPost]
        public async Task<IActionResult> GetAllCorporationsRevenues(AnalyticsRequest request)
        {

            PerformanceResponse response = new PerformanceResponse();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetAllCorpRevenues(request, urlInfoID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("AllCorpExpenses")]
        [HttpPost]
        public async Task<IActionResult> GetAllCorporationsExpenses(AnalyticsRequest request)
        {

            PerofrmanceExpenseResponse response = new PerofrmanceExpenseResponse();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetAllCorpExpenses(request, urlInfoID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("StatsAndDepartmentsForSingleCorp")]
        [HttpPost]
        public async Task<IActionResult> GetStatsAndDepartmentDetailsforSingleCorp(AnalyticsRequest Request)
        {
            PerformanceResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetStatsAndDepartmentsIncomeAndExepnses(Request, urlInfoID, getClientName());
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

        [Route("PendingCashreciptsforAllCorp")]
        [HttpPost]
        public async Task<IActionResult> GetSPendingCashrecieptsForAllCorp(AnalyticsRequest Request)
        {
            List<CorporationCardsResponse>? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await dailyServices.GetPendingCashrecieptsforAllcorps(Request, urlInfoID);
                if (response != null && response.Any())
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally { response = null; }
        }

        [Route("GetPayableAgingDetails")]
        [HttpPost]
        public async Task<IActionResult> GetPayableAgingData(PayablesRequest Request)
        {
            PayablesAgingResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                string clientName = Convert.ToString(HttpContext.Items["ClientName"]);
                bool isAPEnabled = GetAPEnabledClients(clientName);
                Request.Client = isAPEnabled ? "newclient" : "oldclient";

                response = await dailyServices.GetPayableAgingDetails(Request, urlInfoID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw;/*return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);*/ }
            finally { response = null; }

        }
        [Route("AllVendorDues")]
        [HttpPost]
        public async Task<IActionResult> GetAllVendorDues(VendorDuesRequest Request)
        {
            VendorDuesResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                string clientName = Convert.ToString(HttpContext.Items["ClientName"]);
                bool isAPEnabled = GetAPEnabledClients(clientName);
                Request.Client = isAPEnabled ? "newclient" : "oldclient";

                response = await dailyServices.GetDuesofAnVendor(Request, urlInfoID);
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
        [HttpPost]
        [Route("IsAPEnabled")]
        public IActionResult GetAPEnabledClientsCheck()
        {
            try
            {
                string clientName = HttpContext.Items["ClientName"]?.ToString();

                if (string.IsNullOrEmpty(clientName))
                    return BadRequest("ClientName not found in request context.");

                bool isAPEnabled = GetAPEnabledClients(clientName);

                return Ok(isAPEnabled);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    Constants.MSG_ENDPOINT_ERROR
                );
            }
        }
        #endregion
    }
}
