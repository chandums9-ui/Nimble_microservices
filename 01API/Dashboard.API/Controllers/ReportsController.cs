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
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using UserMgmt.Domain.DataModel;
using static Dashboard.Domain.DTO.Resp.ReportsDashboardResponse;

namespace Dashboard.API.Controllers
{
    [Route("v1/Reports")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        #region Fields
        private readonly IReportsService ReportsServices;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public ReportsController(IReportsService _ReportsServices, IConfiguration configuration)
        {
            this.ReportsServices = _ReportsServices;
            _configuration = configuration;
        }
        [Route("GetReportSummary")]
        [HttpPost]
        public async Task<IActionResult> GetReportSummary(FinancialAnalysisReportReq Request)
        {
            FinancialDashboardResponse response = new FinancialDashboardResponse();
            long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
            response = await ReportsServices.GetFinancialAnalysisReport(Request, urlInfoID);
            return Ok(response);
        }
        #endregion
    }
}

