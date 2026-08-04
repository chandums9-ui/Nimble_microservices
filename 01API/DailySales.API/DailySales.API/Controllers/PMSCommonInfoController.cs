using Common.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;
using DailySales.App.Contracts;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.App;
using DailySales.Domain.DTO.Req;

namespace DailySales.API.Controllers
{
    [Route("v1/PMSCommon")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class PMSCommonInfoController : Controller
    {
        #region Fields
        private readonly IPMSCommonInfoService pmscommoninfo;
        #endregion

        #region Ctor
        public PMSCommonInfoController(IPMSCommonInfoService pmscommoninfo)
        {
            this.pmscommoninfo = pmscommoninfo;
        }
        #endregion

        #region PMS Info
        /// <summary>
        /// Based on data request  Create PMS Info
        /// </summary>
        /// <param name="data">Here data request Sending the  PMSInfo,PMSInfoDetails to CreatePMSInfo</param>
        /// <returns> It will Return the Status and Status Codes </returns>
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreatePMSInfo(PMSInfoRequest data)
        {
            StatusDTO response = null;
            try
            {
                 response = await pmscommoninfo.CreatePMSInfo(data);
                if (response != null) { return Ok(response); }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { response = null; }
        }      
        /// <summary>
        /// Based on data request  Delete PMS Info
        /// </summary>
        /// <param name="data"> Here data Sending the PMSInfo,PMSInfoDetails to Change Status has Delete</param>
        /// <returns>It will Return the Status and Status Codes</returns>
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeletePmsInfo(long data)
        {
            StatusDTO response = null;
            try
            {
                response = await pmscommoninfo.DeletePMSInfo(data);
                if (response != null) { return Ok(response); }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { response = null; }
        }

        #endregion

        //#region PMS Client Info
        ///// <summary>
        ///// Based on data request  Create PMS Client Info
        ///// </summary>
        ///// <param name="data">Here data request Sending the  PMSClientInfo,PMSClientInfoDetails to CreatePMSClientInfo</param>
        ///// <returns> It will Return the Status and Status Codes </returns>
        //[Route("PMSClientInfo/Create")]
        //[HttpPost]
        //public async Task<IActionResult> CreatePMSClientInfo(PMSClientInfoRequest data)
        //{
        //    StatusDTO response = null;
        //    try
        //    {
        //        response = await pmscommoninfo.CreatePMSClientInfo(data);
        //        if (response != null) { return Ok(response); }
        //        else
        //            return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    finally { response = null; }
        //}
        ///// <summary>
        ///// Based on data request  Delete PMS Client Info
        ///// </summary>
        ///// <param name="data"> Here data Sending the PMSClientInfo,PMSClientInfoDetails to Change Status has Delete</param>
        ///// <returns>It will Return the Status and Status Codes</returns>
        //[Route("PMSClientInfo/Delete")]
        //[HttpPost]
        //public async Task<IActionResult> DeletePMSClientInfo(long data)
        //{
        //    StatusDTO response = null;
        //    try
        //    {
        //        response = await pmscommoninfo.DeletePMSClientInfo(data);
        //        if (response != null) { return Ok(response); }
        //        else
        //            return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    finally { response = null; }
        //}

        //#endregion

        //#region PMS Facility Map
        ///// <summary>
        ///// Based on data request  Create PMS Facility Map
        ///// </summary>
        ///// <param name="data">Here data request Sending the  PMSFacilityMap to CreatePMSFacilityMap</param>
        ///// <returns> It will Return the Status and Status Codes </returns>
        //[Route("PMSFacilityMap/Create")]
        //[HttpPost]
        //public async Task<IActionResult> CreatePMSFacilityMap(PMSFacilityMapRequest data)
        //{
        //    StatusDTO response = null;
        //    try
        //    {
        //        response = await pmscommoninfo.CreatePMSFacilityMap(data);
        //        if (response != null) { return Ok(response); }
        //        else
        //            return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    finally { response = null; }
        //}
        ///// <summary>
        ///// Based on data request  Delete PMS Client Info
        ///// </summary>
        ///// <param name="data"> Here data Sending the PMSFacilityMap to Change Status has Delete</param>
        ///// <returns>It will Return the Status and Status Codes</returns>
        //[Route("PMSFacilityMap/Delete")]
        //[HttpPost]
        //public async Task<IActionResult> DeletePMSFacilityMap(long data)
        //{
        //    StatusDTO response = null;
        //    try
        //    {
        //        response = await pmscommoninfo.DeletePMSFacilityMap(data);
        //        if (response != null) { return Ok(response); }
        //        else
        //            return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    finally { response = null; }
        //}

        //#endregion
    }
}
