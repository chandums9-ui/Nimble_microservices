using Common.Domain.DTO.App;
using CoreAccounting.App.Contracts;
using Dashboard.Domain.DTO.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAccounting.API.Controllers
{
    [Route("v1/Incomestatement")]
    [ApiController]
    public class IncomeStatementController : BaseController
    {
        #region Fields
        private readonly IIncomeStatementService incomeSTService;
        #endregion

        #region Ctor
        public IncomeStatementController(IIncomeStatementService IsService)
            => (incomeSTService)
            = (IsService);


        #endregion
        #region Private Methods

        private string getClientID()
        {
            return (string)HttpContext.Items["ClientId"];
        }
        private string GetUserID()
        {
            return (string)HttpContext.Items["UserId"];
        }
        private string GetClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }

        #endregion

        [Route("IncomeStatementCorpList")]
        [HttpGet]
        public async Task<IActionResult> IncomeStatementCorpList()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await incomeSTService.LoadEPaymentCorpList(GetUserID());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("IncomeStatementCOAs")]
        [HttpPost]
        public async Task<IActionResult> LoadIncomeStatementsCOAs(IS_ChartOfAccountRequest req)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await incomeSTService.LoadIncomeStatementsCOAs(req);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

    }
}
