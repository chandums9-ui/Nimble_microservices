using Dashboard.App.Contracts;
using Dashboard.App.Services;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dashboard.API.Controllers
{
    [Route("v1/ExcelExport")]
    [ApiController]
    public class ExcelExportController : ControllerBase
    {
        private readonly IExcelExportService _excelExportService;

        public ExcelExportController(IExcelExportService excelExportService)
        {
            _excelExportService = excelExportService;
        }

        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        [Route("GenerateCombinedExcelReport")]
        [HttpPost]
        public async Task<IActionResult> GenerateCombinedExcelReport(LstWidgetExportRequest request)
        {

            ExcelExportReponse response = new ExcelExportReponse();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                string userId = (string)HttpContext.Items["UserId"];
                if (!userId.IsNullOrEmpty()) request.UserID = userId;

                response = await _excelExportService.GenerateCombinedExcelReport(request, urlInfoID,getClientName());

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

        [Route("GenerateExcelReport")]
        [HttpPost]
        public async Task<IActionResult> GenerateExcelReport(ExcelExportRequest request)
        {
            ExcelExportReponse response = new ExcelExportReponse();
            try
            {
                string userId = (string)HttpContext.Items["UserId"];

                response = await _excelExportService.GenerateExcelReport(request);

                if (response != null)
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

        [Route("CashGenerateExcelReport")]
        [HttpPost]
        public async Task<IActionResult> CashGenerateExcelReport(ExcelExportRequest request)
        {
            ExcelExportReponse response = new ExcelExportReponse();
            try
            {
                string userId = (string)HttpContext.Items["UserId"];

                response = await _excelExportService.CashGenerateExcelReport(request);

                if (response != null)
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
    }
}
