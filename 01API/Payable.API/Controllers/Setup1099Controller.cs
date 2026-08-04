using Common.API.ActionFilters;
using Common.Domain.DTO.Req;
using Microsoft.AspNetCore.Mvc;
using Payable.App.Contracts;
using Common.API.Authorization;
using static Payable.Domain.DTO.Req.Setup1099Req;
using static Payable.Domain.DTO.Resp.Setup1099Res;

namespace Payable.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class Setup1099Controller : Controller
    {

        #region Fields
        private readonly ICommonService commonSrv;
        private readonly ISetup1099Service setupPrint1099Srv;
        private readonly IUnitOfWork uow;
        //private readonly IFileService fileSrv;

        #endregion

        #region Ctor
        public Setup1099Controller(ICommonService coreProperty, IUnitOfWork _uow, ISetup1099Service setupPrint1099Service)
        {
            this.commonSrv = coreProperty;
            this.setupPrint1099Srv = setupPrint1099Service;
            this.uow = _uow;
        }

        #endregion

        #region private methods

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

        #region 1099 setup

        //get 1099setup related VendorList
        [Route("Get1099SetupVendors")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get1099SetupVendors(Load1099SetupVendorsReq request)
        {
            var response = await setupPrint1099Srv.Get1099SetupVendors(request);
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("GetUserPreferences")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserPrefernces()
        {
            var response = await setupPrint1099Srv.GetUserPrefernces(getUserID(), getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        //GetExclude Settings by Client Wise


        [Route("Get1099ExcludeSettings")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get1099ExcludeSettings()
        {
            var response = await setupPrint1099Srv.Get1099ExcludeSettings(getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        //Add Exclude Settings by Client Wise

        [Route("Add1099ExcludeSettings")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add1099ExcludeSettings(ExcludeSettingsSaveReq reuest)
        {
            var response = await setupPrint1099Srv.SaveOrUpdateExculdeSettings(reuest, getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }




        //Delete Exclude Settings by Client Wise

        [Route("Delete1099ExcludeSettings")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete1099ExcludeSettings(LoadByLongIDRequest request)
        {
            var response = await setupPrint1099Srv.DeleteExcludeSettings(request.ID, getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }


        [Route("Get1099VendorDetailsByCorp")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get1099VendorDetailsByCorp(VendorDetailsOf1099Req request)
        {
            var response = await setupPrint1099Srv.Get1099VendorDetailsByCorpID(request, getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("Get1099Thresholds")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get1099Thresholds(ThresholdReq request)
        {
            var response = await setupPrint1099Srv.Get1099Thresholds(request, getClientID(),getUserID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("Update1099Thresholds")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update1099Thresholds(UpdateThresholdReq request)
        {
            var response = await setupPrint1099Srv.UpdateThresholds(request, getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("ResetThresholds")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetThresholds(UpdateThresholdReq request)
        {
            var response = await setupPrint1099Srv.ResetThresholds(request, getClientID(),getUserID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("Get1099BoxLines")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get1099BoxLines(BoxLinesReq request)
        {
            var response = await setupPrint1099Srv.Get1099BoxLines(request);
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("Load1099VendorCOAAccounts")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Load1099VendorCOAAccounts(VendorDetailsOf1099Req request)
        {
            var response = await setupPrint1099Srv.LoadCOAsWithBoxLines(request, getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }


        [Route("Update1099COAMappingsWithBoxLines")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update1099COAMappingsWithBoxLines(UpdateCOAMappingwithBoxLinesReq request)
        {
            var response = await setupPrint1099Srv.Update1099COAMappingsWithBoxLines(request, getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("GetChangeMappingReqID")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetChangeMappingReqID(ChangeMappingTempReq request)
        {
            var response = await setupPrint1099Srv.GetReqIDFromChangeMappingTemp(request, getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("SaveOrUpdateChangeMapp")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveOrUpdateChangeMapp(UpdateCOAMappingwithBoxLinesReq request)
        {
            var response = await setupPrint1099Srv.SaveUpdateChangeMappingsTemp(request, getClientID());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("DeleteChangeMapp")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteChangeMapp(DeleteChangeMapTempReq request)
        {
            var response = await setupPrint1099Srv.DeleteChangeMappingsTemp(request);
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }


        [Route("GenerateExcelReport")]
        [HttpPost]
        public async Task<IActionResult> GenerateExcelReport(ExcelExportReq request)
        {
            ExcelExportRes response = new ExcelExportRes();
            try
            {
                string userId = (string)HttpContext.Items["UserId"];

                response = await setupPrint1099Srv.GenerateExcelReport(request);

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

        #endregion
    }
}

