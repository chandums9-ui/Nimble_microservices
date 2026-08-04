using Common.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Payable.App.Contracts;
using Common.API.Authorization;
using Payable.Domain.DTO.Req;
using Payable.Domain.DTO.Resp;

namespace Payable.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class BillEntryMobileAPPController:ControllerBase
    {
        #region Fields
        private readonly ICommonService commonSrv;
        private readonly IBillEntryService billEntrySrv;
        private readonly IUnitOfWork uow;
        //private readonly IFileService fileSrv;

        #endregion

        #region Ctor
        public BillEntryMobileAPPController(ICommonService coreProperty, IUnitOfWork _uow, IBillEntryService billEntryService)
        {
            this.commonSrv = coreProperty;
            //this.fileSrv = fileService;
            this.billEntrySrv = billEntryService;
            this.uow = _uow;
        }

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
        [Route("MobileApp/GetBills")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadViewGridData(GetBillsRequest request)
        {
            request.UserID = (string)HttpContext.Items["UserId"];
            VewBillsResponse response = await billEntrySrv.GetBillsandDetails(request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else
                return NotFound(response);
        }
    }
}
