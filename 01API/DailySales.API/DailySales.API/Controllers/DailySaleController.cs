using Azure.Core;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Resp;
using CoreAccounting.App.Service;
using DailySales.App.Contracts;
using DailySales.Domain.DTO.Model;
using DailySales.Domain.DTO.Req;
using DailySales.Domain.DTO.Resp;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System.Data.SqlTypes;

namespace DailySales.API.Controllers
{
    [Route("v1/DailySale")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class DailySaleController : ControllerBase
    {
        #region Fields
        private readonly IDailySaleService dailySale;
        private readonly ILoggerService logger;
        #endregion

        #region Ctor
        public DailySaleController(IDailySaleService _dailySale,ILoggerService _logger)
        {
            this.dailySale = _dailySale;
            this.logger = _logger;
        }
        #endregion

        #region Daily Sale 
        /// <summary>
        /// Based on data request  Create Daily Sale
        /// </summary>
        /// <param name="data">Here data request Sending the  DailyConfigInputEntry object ,List of DailyConfigInputEntry_Revenue,DailyConfigInputEntry_Receipt,DailyConfigInputEntry_AR,DailyConfigInputEntry_Statistics to createDailySale</param>
        /// <returns> It will Return the Status and Status Codes </returns>
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateDailySale(DailySaleRequest request)
        {
            byte[] data = null;
            DailySaleResponse result = null;
            try
            {
                result = new DailySaleResponse();
                if (result != null) { return Ok(result); }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }
        /// <summary>
        ///  Based on data request  Update Daily Sale
        /// </summary>
        /// <param name="data">Here data request sending the DailyConfigInputEntry object values,List of DailyConfigInputEntry_Revenue,DailyConfigInputEntry_Receipt,DailyConfigInputEntry_AR,DailyConfigInputEntry_Statistics to createDailySale  </param>
        /// <returns>It will Return the Status and Status Codes </returns>
        //[Route("Update")]
        //[HttpPut]
        //public async Task<IActionResult> UpdateDailySale(DailySaleRequest request)
        //{
        //    DailySaleResponse result = null;
        //    try
        //    {
        //        result = await dailySale.UpdateDailySale(request);
        //        if (result != null) { return Ok(result); }
        //        else
        //            return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    finally { result = null; }
        //}
         

        /// <summary>
        /// This returns all the active lines in the corporation
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>It returns DailySaleLinesResponse</returns>
        [Route("Lines")]
        [HttpPost]
        public async Task<IActionResult> GetConfigLines(DailySaleConfigLineRequest LineRequest)
        {
            DailySaleLinesResponse? response = null;
            try
            {
                if (!string.IsNullOrEmpty(LineRequest.CorpID))
                {
                    response = await dailySale.GetConfigLines(LineRequest);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Constants.MSG_CORP_REQ);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }
        /// <summary>
        /// This returns all the Corporation and FacilityID's Based on UserID
        /// </summary>
        /// <returns>It will return the Corporation ID's,Name,Legal Name And Facility ID</returns>
        [Route("CorpFacility")]
        [HttpPost]
        public async Task<IActionResult> GetCorpFacility()
        {
            DailySaleCorpFacilityResponse? response = null;
            try
            {
                string userId = (string)HttpContext.Items["UserId"];
                if (!string.IsNullOrEmpty(userId))
                {
                    response = await dailySale.GetCorpFacilityID(userId);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Constants.MSG_CORP_REQ);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        /// <summary>
        /// It will return the verifications Lines Based on SaleID (DailyConfigInputEntry(ID)).
        /// </summary>
        /// <param name="saleID">Here saleID as parameter For Filter the verification Lines.</param>
        /// <returns>It will return the VerificationLinesResponse List</returns>
        [Route("verificationmatch")]
        [HttpPost]
        public async Task<IActionResult> GetVerficationLines(string saleID)
        {
            VerificationLinesResponse? response = null;
            try
            {
                if (!string.IsNullOrEmpty(saleID))
                {
                    response = await dailySale.GetVerificationLinesMatch(saleID);
                    if (response != null) return Ok(response);
                    else return NotFound(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Constants.MSG_CORP_REQ);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }

        }

        /// <summary>
        /// Based on CorpId it will load Downline Users to assign the rejectd sale
        /// </summary>
        /// <param name="corpID"> Here CorpID send as parameter to match assign user to load the users</param>
        /// <returns>It will return the Status,StatusCode and users</returns>
        //[Route("DownLineUsers")]
        //[HttpPost]
        //public async Task<IActionResult> GetDownLineUsersEntry(string corpID)
        //{
        //    DownLineUsersResponse? response = null;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(corpID))
        //        {
        //            response = await dailySale.GetDownLineUsers(corpID);
        //            if (response != null)
        //                return Ok(response);
        //            else
        //                return NotFound(response);
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, Constants.MSG_CORP_REQ);
        //        }
        //    }
        //    catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        //    finally { response = null; }
        //}

        /// <summary>
        /// Nimble I/O will Post DailySales Lines after extraction files
        /// </summary>
        /// <returns>It will return the Status,StatusCode </returns>
        [Route("Payload")]
        [HttpPost]
        public async Task<IActionResult> DailySalePayload(DailySalePayloadRequest dailySalePayloadReq)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                //HttpContext cntx = HttpContext;
                response = await dailySale.DailySalePayload(dailySalePayloadReq, HttpContext);
                return Ok(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        /// <summary>
        /// Nimble will call this endpoint after Map DailySale Lines.
        /// </summary>
        /// <returns>It will return the Status,StatusCode and users</returns>
        [Route("UpdateUnMappedLines")]
        [HttpPost]
        public async Task<IActionResult> UnMappedLines(UnMappedLinesRequest unMappedLinreq)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                response = await dailySale.UnMappedLines(unMappedLinreq);
                return Ok(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        /// <summary>
        /// Verification (Required) DailySale Lines Based on DailySaleID And FacilityID
        /// </summary>
        /// <returns>It will return DailySale Lines</returns>
        [Route("SaleLines")]
        [HttpPost]
        public async Task<IActionResult> GetCacheDataBySaleID(UnMappedLinesRequest unMappedLinreq)
        {
            List<DailySalePMSDTO> response = new List<DailySalePMSDTO>();
            try
            {
                response = await dailySale.GetCacheDataBySaleID(unMappedLinreq);
                return Ok(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        /// <summary>
        /// Based on CorpId it will load FacilityID
        /// </summary>
        /// <returns>It will return FacilityID</returns>
        [Route("FacilityID")]
        [HttpPost]
        public async Task<IActionResult> GetFacilityID(string corpID)
        {
            string FacilityID = "";
            try
            {
                FacilityID = await dailySale.GetFacilityID(corpID);
                return Ok(FacilityID);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { FacilityID = null; }
        }

        /// <summary>
        /// Nimble I/O will Post Pending Sale Details
        /// </summary>
        /// <returns>It will return the Status,StatusCode</returns>
        [Route("MissingFiles")]
        [HttpPost]
        public async Task<IActionResult> DailySaleMissingFiles(DailySaleMissingFilesRequest dsMissingFiles)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                response = await dailySale.DailySaleMissingFiles(dsMissingFiles);
                return Ok(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }



        [HttpPost]
        [Route("OTBFileUpload")]
        public async Task<IActionResult> OTBFileUpload(OTBFileUploadRequest request)
        {
            string userId = HttpContext.Items["UserId"] as string;
            string clientId = HttpContext.Items["ClientId"] as string;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { detail = "UserId not found in request context" });
            }

            request.UserId = userId;

            var response = await dailySale.OTBFileUpload(request, userId, clientId);

            if (response == null)
            {
                return StatusCode(500, new { detail = "Server did not return a response." });
            }

            if (response.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(response);
            }
            else if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(new { detail = response.Message ?? "Invalid request" });
            }
            else
            {
                return StatusCode(500, new { detail = response.Message ?? "Unknown server error" });
            }
        }

        [Route("OTBFileImport")]
        [HttpPost]
        public async Task<IActionResult> DailySaleForecastPayLoad(OTBReportInfoRequest request)
        {
            OTBResponse result = null;
            try
            {
                logger.LogFile($"DailySaleForecastPayLoadRequest: {JsonConvert.SerializeObject(request)}" + Environment.NewLine, "DailySaleForecastPayLoad", "OTB");
                result = await dailySale.DailySaleForecastPayLoad(request);
                logger.LogFile($"DailySaleForecastPayLoadResponse: {JsonConvert.SerializeObject(result)}" + Environment.NewLine, "DailySaleForecastPayLoad", "OTB");
                if (result != null) { return Ok(result); }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }

        [Route("OTBViewData")]
        [HttpPost]
        public async Task<IActionResult> LoadOTBForecastViewGridData(OTBViewGridInputReq ViewData)
        {
            var UserId = (string)HttpContext.Items["UserId"];
            List<OTBViewData> response = await dailySale.LoadOTBForecastViewGridData(ViewData, UserId);
            if (response != null && response.Any())
                return Ok(response);
            else
                return NotFound(response);
        }
        //[Route("OTBLoadPCs")]
        //[HttpGet]
        //public async Task<IActionResult> LoadOTBProfitCenters(byte[] CorpId, byte[] UserId)
        //{
        //    OTBPcs response = await dailySale.LoadOTBProfitCenters(CorpId, UserId);
        //    if (response != null && response.StatusCode == StatusCodes.Status200OK)
        //        return Ok(response);
        //    else
        //        return NotFound(response);
        //}

        [Route("OTBDelete")]
        [HttpPost]
        public async Task<IActionResult> DeleteOTBSale(DeleteRequest data)
        {
            OTBResponse result = null;
            try
            {
                result = await dailySale.DeleteOTBDailySale(data);
                if (result != null) { return Ok(result); }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }


        [Route("GetCorpAndPCListByMgntGrpWithPmsBrandSerTypes")]
        [HttpGet]
        public async Task<IActionResult> GetCorporationandPCListSelectionListByType(string MgntGrpId)

        {
            var userID = (string)HttpContext.Items["UserId"];
            List<GetCorporationAndPCListByTypeResp> Response = await dailySale.GetCorporationAndPCListSelectionListByType(userID, MgntGrpId);
            return Ok(Response);
 
        }

        [Route("OTBMemorizeSaveDetails")]
        [HttpPost]
        public async Task<IActionResult> OTBMemorizeSaveDetails(OTBMemorizeReq request)
        {
            OTBMemorizeResponse Response = await dailySale.SaveOTBMemorizeDetails(request);
            return Ok(Response);

        }

        [Route("OTBMemorizeGetDetails")]
        [HttpPost]
        public async Task<IActionResult> OTBMemorizeGetDetails(OTBMemorizeGetReq request)
        {
            OTBGetMemorizeResponse Response = await dailySale.GetOTBMemorizeDetails(request);
            return Ok(Response);

        }
        [Route("OTBResetDetails")]
        [HttpPost]
        public async Task<IActionResult> OTBResetDetails(OTBMemorizeGetReq request)
        {
            OTBMemorizeResponse Response = await dailySale.OTBResetDetails(request);
            return Ok(Response);

        }

        #endregion

    }
}
