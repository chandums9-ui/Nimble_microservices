using Common.API.ActionFilters;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Common.API.Authorization;

namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class CommonController : BaseController
    {
        #region fields
        private readonly ICommonService commonSrv;
        #endregion

        #region Private Methods
        private string getUserID()
        {
            return (string)HttpContext.Items["UserId"];
        }
        private string getClientID()
        {
            return (string)HttpContext.Items["ClientId"];
        }
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        #endregion

        #region Ctor

        public CommonController(ICommonService commonSrv)
        {
            this.commonSrv = commonSrv;
        }
        #endregion

        [HttpPost("GetReferences")]
        [ProducesResponseType(typeof(ReferenceListRes), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReferences(ReferenceListReq req)
        {
            ReferenceListRes response = await commonSrv.GetReferenceNames(req, getUserID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

    }
}
