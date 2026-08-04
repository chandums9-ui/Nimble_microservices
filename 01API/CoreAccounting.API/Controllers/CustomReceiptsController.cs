using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMgmt.App.Contracts;

namespace CoreAccounting.API.Controllers
{
    [Route("v1/CustomReceipts")]
    [ApiController]
    public class CustomReceiptsController : BaseController
    {
        #region feilds
        private readonly ICustomReceiptsService customReceiptsService;
        #endregion

        #region ctor
        public CustomReceiptsController(ICustomReceiptsService customReceiptsService)
        {
            this.customReceiptsService = customReceiptsService;
        }
        #endregion

        /// <summary>
        /// It will save customer receipt CustomReceiptsRequest
        /// </summary>
        /// <param name="data">It represents CustomreceiptRequest</param>
        /// <returns>It return status , status code and Id</returns>
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomReceipts(CustomReceiptsRequest data)
        {
            JournalResponse result = null;
            try
            {
                result = await customReceiptsService.CreateCustomReceipt(data);
                if (result.Status != null && result.StatusCode == (int)StatusCodes.Status200OK && result.ID != null)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                throw;
            }
            finally { result = null; }
        }

    }
}
