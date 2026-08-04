using Common.API.Authorization;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Req;
using Dashboard.App.Contracts;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;
using static Dashboard.Domain.DTO.Req.IncomeStatementGroupReq;

namespace Dashboard.API.Controllers
{
    [Route("v1/WidgetFormula")]
    [ApiController]
    [Authorize]
    public class WidgetMasterController : ControllerBase
    {
        #region Ctor
        private readonly IWidgetMasterService widgetMasterService;
        public WidgetMasterController(IWidgetMasterService widgetMasterService)
        {
            this.widgetMasterService = widgetMasterService;
        }

        #endregion

        #region  Configuraionpoup

        /// <summary>
        /// It will Get IncomeStaementChartofAccounts
        /// </summary>
        /// <returns></returns>
        [Route("ChartOfAccounts")]
        [HttpPost]
        public async Task<IActionResult> GetChartOfAccounts()
        {
            AccountsResponse? response = null;
            try
            {
                response = await widgetMasterService.GetIncomeChartOfAccounts();
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
        /// It will get Income statement Groups along with department and chartof accounts
        /// </summary>
        /// <returns></returns>
        [Route("IncomeStmtGroups")]
        [HttpPost]
        public async Task<IActionResult> GetIncomeStatementGroups()
        {
            IncomestmntGroupResponse? response = null;
            try
            {
                response = await widgetMasterService.GetIncomeStatementGroups();
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
        /// It will  get PayrollDepartments and their jobtitles 
        /// </summary>
        /// <returns></returns>
        [Route("PayrollDepartments")]
        [HttpPost]
        public async Task<IActionResult> GetPayrollDepartmnets()
        {
            PayrollDeparmentsResponse? response = null;
            try
            {
                response = await widgetMasterService.GetPayrollDeparments();
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
        /// It will get the custom keys based corporationId,Type ,SubType
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("GetCustomizeKeys")]
        [HttpPost]
        public async Task<IActionResult> GetCustFormulaDetails(CustFormulaRequest request)
        {
            CustFormulaResponse? response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                request.UrlKey = urlInfoID;
                response = await widgetMasterService.GetCustFormulaDetails(request);
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

        [Route("GetCustomStatsInfo")]
        [HttpPost]
        public async Task<IActionResult> GetCustomStatsDetails(CustStatsRequest request)
        {
            DepartmnetGraphResponse? response = null;
            try
            {
                response = await widgetMasterService.GetCustomStatsDetails(request);
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

        [Route("GetCustomIncomeGroupsInfo")]
        [HttpPost]
        public async Task<IActionResult> GetCustomIncomeGroupDetails(CustStatsRequest request)
        {
            IncomeGroupsResponse? response = null;
            try
            {
                //clientID
                string clientID = (string)HttpContext.Items["ClientId"];

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                request.UrlKey = Convert.ToInt32(urlInfoID);

                response = await widgetMasterService.GetCustomIncomeGroupResponse(request, clientID);
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
        [Route("GetIncomeGroupGraphDetails")]
        [HttpPost]
        public async Task<IActionResult> GetCustomIncomeGroupGraphDetails(CustStatsRequest request)
        {
            DepartmnetGraphResponse? response = null;
            try
            {
                response = await widgetMasterService.GetCustmIncomeGroupDetails(request);
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

        [Route("GetIncomeStatementSubDeparts")]
        [HttpPost]
        public async Task<IActionResult> GetIncomeStatementSubDeparts(SubDepartmentsRequest request)
        {
            IncomeStatementSubDepartmentsResponse response = null;
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];

                response = await widgetMasterService.GetIncomeStatementSubDeparts(request, clientID);
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

        [Route("GetIncomeStatementDepartWiseSummary")]
        [HttpPost]
        public async Task<IActionResult> GetIncomeStatementDepartWiseSummary(IncomeStatementDepartmentWiseRequest request)
        {
            IncomeStatementDepartmentWiseResonse response = null;
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];

                response = await widgetMasterService.GetIncomeStatementDepartwiseSummary(request, clientID);
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
        [Route("GetIncomeGroupsAccounts")]
        [HttpPost]
        public async Task<IActionResult> GetIncomeGroupAccounts(IncomeGroupAccountsRequest request)
        {
            IncomeGroupsAccountsList response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                request.UrlKey = urlInfoID;
                response = await widgetMasterService.GetIncomeGroupsAccountsList(request);
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

        #endregion

        #region WidgetFormula
        /// <summary>
        /// It will save the configuration formula for the custamized widget 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns>It will return  status and status code</returns>
        [Route("SaveFormula")]
        [HttpPost]
        public async Task<IActionResult> SaveWidgetFormula(WidgetFormulaRequest request)
        {
            WidgetFormulaResponse? response = new WidgetFormulaResponse();
            try
            {
                if (request != null)
                {
                    request.WidgetSettingsFormula.UserId = (string)HttpContext.Items["UserId"];
                    request.WidgetSettingsFormula.ClientID = (string)HttpContext.Items["ClientId"];
                    response = await widgetMasterService.SaveOrUpdateWidgetFormula(request);
                }
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
        /// It will check weather the formula name is exist or not 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns>It will return  status and status code</returns>
        [Route("CheckFormula")]
        [HttpPost]
        public async Task<IActionResult> FormulaCheck(WidgetFormulaRequest widgetVM)
        {
            FormulaCheckResponse response = new FormulaCheckResponse();
            try
            {
                if (!string.IsNullOrEmpty(widgetVM.WidgetName))
                {
                    response = await widgetMasterService.FormulaCheck(widgetVM);
                }
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
        /// It will  get the customization formula for the widgets based on formulaId
        /// </summary>
        /// <param name="request"> It will have FormulaId</param>
        /// <returns>It returns the formula related details </returns>
        [Route("GetWidgetFormulaDetails")]
        [HttpPost]
        public async Task<IActionResult> GetWidgetFormula(LoadByLongIDRequest request)
        {
            WidgetLoadResponse? response = null;
            try
            {
                response = await widgetMasterService.GetWidgetFormulaById(request);
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

        [Route("GetWidgetAllFormulasByClientId")]
        [HttpPost]
        public async Task<IActionResult> GetWidgetAllFomrulas(WidgetFormulasRequest request)
        {
            WidgetAllFormulaResponse? response = null;
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];

                string userId = (string)HttpContext.Items["UserId"];

                request.UserID = userId;
                request.ClientID = clientID;

                response = await widgetMasterService.GetAllFormulasByClientID(request);

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
        /// It will  delete the customization formula  details for the widgets based on formulaId
        /// </summary>
        /// <param name="request"> It will have formulaId</param>
        /// <returns>It returns Staus, statuscode and Id  </returns>
        [Route("DeleteFormula")]
        [HttpPost]
        public async Task<IActionResult> DeleteFormula(CustomWidgetDeleteFormulaRequest request)
        {
            WidgetFormulaResponse? response = null;
            try
            {
                response = await widgetMasterService.DeleteFormula(request);
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
        /// Retrieves widget records based on the provided request.
        /// </summary>
        /// <param name="request">The request containing parameters for retrieving widget records.</param>
        /// <returns>An asynchronous action result representing the response containing widget records.</returns>
        [Route("GetWidgetFormulas")]
        [HttpPost]
        public async Task<IActionResult> GetWidgetFormulas(WidgetRecordsRequest request)
        {
            WidgetFormulaRecordsResponse response = new WidgetFormulaRecordsResponse();
            response.FormulaRecords = new List<FormulaData>();
            try
            {
                string userId = (string)HttpContext.Items["UserId"];
                response = await widgetMasterService.GetWidgetFormulas(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally
            {
                if (response != null)
                    response = null;
            }

        }

        [Route("GetCustomWidgetFromulaData")]
        [HttpPost]
        public async Task<IActionResult> GetCustomWidgetFromulaData(CustomWidgetFomulaRequest request)
        {
            ListOfCustomFormulaResponse response = new ListOfCustomFormulaResponse();

            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                request.Urlkey = Convert.ToInt32(urlInfoID);
                response = await widgetMasterService.GetCustomWidgetFromulaData(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally
            {
                if (response != null)
                    response = null;
            }

        }



    
        #endregion
    }
}
