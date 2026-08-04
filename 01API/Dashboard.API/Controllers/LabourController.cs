using Azure;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using Dashboard.App.Contracts;
using Dashboard.App.Services;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [Route("v1/LabourAnalysis")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class LabourController:ControllerBase
    {

        #region Fields
        private readonly ILabourService labourService;
        #endregion

        #region Ctor
        public LabourController(ILabourService _labourService)
        {
            this.labourService = _labourService;
        }
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        /// <summary>
        /// To get labourAnalysis details by category for Corporation/s based on CorpId /UserId and dates
        /// </summary>
        /// <param name="Request">userId,Corporation ID and From & To Date</param>
        /// <returns>It returns  Departmentname,payrollexpense,Hours ,percentage of income,PAR, POR </returns>
        [Route("CategoryAnalysis")]
        [HttpPost]
        public async Task<IActionResult> GetPerformanceDetails(LabourRequest Request)
        {
            LabourAnalysisResponse? response = null;
            try
            {
                // Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
                // Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await labourService.GetLabourAnalysisbyCategory(Request, urlInfoID,getClientName());
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
        /// To get labor analysis for multiple corporations based on userID 
        /// </summary>
        /// <param name="Request">UserID,FromDate,ToDate</param>
        /// <returns>returms a list of labour analysis data</returns>
        [Route("CorpWiseAnalysis")]
        [HttpPost]
        public async Task<IActionResult> GetLabourAnalysisCorporationWise(LabourRequest Request)
        {
            LabourAnalysisResponse? response = null;
            try
            {
                // Request.UserId = "0x" + (string)HttpContext.Items["UserId"];
                // Request.ClientId = "0x" + (string)HttpContext.Items["ClientId"];

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await labourService.GetLabourAnalysisbyCorporationWise(Request, urlInfoID, getClientName());
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
        /// It will get the user configured IncomeStatement Departments
        /// </summary>
        /// <returns>It will return the Income statement departments</returns>
        [Route("IncomeStatementDepartments")]
        [HttpPost]
        public async Task<IActionResult> GetIncomestamentDepartments()
        {
            IncomeDepartments? response = null;
            try
            {
                response = await labourService.GetIncomeStatementDepartments();
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
        /// It will get the payroll departments for the respective corporation
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return payroll departments</returns>
        [Route("PayrollDepartments")]
        [HttpPost]  
        public async Task<IActionResult>GetPayrollDepartments(LabourRequest Request)
        {
            PayrollDepatsResponse response = null; 
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await labourService.GetPayrollDeparments(Request, urlInfoID, getClientName());
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
        /// It will get the revenue ,payroll expense, payroll expense percentage and Occupancy percentage for the respective corporation.
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>It will return ,payroll expense, payroll expense percentage and Occupancy percentage </returns>
        [Route("GetRevenuePayrollOccupancyComparisons")]
        [HttpPost]
        public async Task<IActionResult>GetComparisons(LabourRequest Request)
        {
            RevenuePayrollOccupancyResponse response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await labourService.GetComparisons(Request, urlInfoID, getClientName());
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
        /// Retrieves payroll cost details for a specific corporation, type, and year based on the provided request parameters.
        /// </summary>
        /// <param name="corporationId">The unique identifier of the corporation.</param>
        /// <param name="type">The type of payroll cost department.</param>
        /// <param name="year">The fiscal year for which payroll details are requested.</param>
        /// <returns>A collection of payroll cost details matching the specified criteria.</returns>
        [Route("PayrollCostDetails")]
        [HttpPost]
        public async Task<IActionResult> PayrollCostDetails(LabourRequest Request)
        {
            PayrollCostRoomsORHoursResponse response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await labourService.PayrollCostDetails(Request, urlInfoID, getClientName());
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
        [Route("CheckIncconfiguration")]
        [HttpPost]
        public async Task<IActionResult>CheckIncomeConfiguration(LoadByIDRequest Request)
        {
            CheckIncomconfigResponse response = null; 
            try
            {
                response= await labourService.CheckIncomeConfiguration(Request.ID,getClientName());
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
    }
}
