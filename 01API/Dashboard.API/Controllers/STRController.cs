using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Microsoft.AspNetCore.Mvc;
using Dashboard.App.Contracts;
using Dashboard.App.Services;
using Dashboard.Domain.DTO.Req;
using Azure;
using Dashboard.Domain.DTO.Resp;
using Common.Domain.DTO.Model.Base;
using Azure.Core;
using System.Globalization;
using Amazon.Runtime.Internal;
using Dashboard.Domain.DTO.Model;

namespace Dashboard.API.Controllers
{
    [Route("v1/STR")]
    [ApiController]
    public class STRController : ControllerBase
    {

        #region Fields
        private readonly ISTRService STRService;
        #endregion

        #region Ctor
        public STRController(ISTRService _STRService)
        {
            this.STRService = _STRService;
        }
        #endregion

        #region Endpoints



        /// <summary>
        /// Generates a list of weeks for the specified year, organized by quarters.
        /// </summary>
        /// <param name="year">The year for which to generate the weeks.</param>
        /// <returns>Returns a list of weeks for the specified year, organized by quarters.</returns>
        [Route("GetWeeksOfYear")]
        [HttpPost]
        public async Task<IActionResult> GetWeeksOfYear([FromBody] int year)
        {
            GetWeeksOfYearResponse response = new GetWeeksOfYearResponse();
            try
            {
                response = await STRService.GetWeeksOfYear(year);

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

        #region MultiCorporation 

        /// <summary>
        /// Retrieves data for a specified week for the Multi Corporation.
        /// </summary>
        /// <param name="request">Details of the week or date range.</param>
        /// <returns>Returns STR week data for the Multi Corporation.</returns>
        [Route("STRMultiCorpWeekData")]
        [HttpPost]
        public async Task<IActionResult> GetMultiCorpWeekData(STRWeekORRangeRequest request)
        {
            STRResponse? response = null;
            try
            {
                response = await STRService.GetMultiCorpWeekData(request);
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
        /// Retrieves weekly data for the Multi Corporation based on the specified request parameters.
        /// </summary>
        /// <param name="request">Details of the week for which data is requested.</param>
        /// <returns>Returns STR weekly data for the Multi Corporation.</returns>
        [Route("GetMultiCorpWeeklyData")]
        [HttpPost]
        public async Task<IActionResult> GetMultiCorpWeeklyData(STRWeeklyRequest request)
        {
            STRResponse? response = null;
            try
            {
                response = await STRService.GetMultiCorpWeeklyData(request);
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
        /// Retrieves monthly data for the Multi Corporation.
        /// </summary>
        /// <param name="request">Details of the month for which data is requested.</param>
        /// <returns>Returns STR monthly data for the Multi Corporation.</returns>
        [Route("GetMultiCorpMonthlyData")]
        [HttpPost]
        public async Task<IActionResult> GetMultiCorpMonthlyData(STRMontlyRequest request)
        {
            STRResponse? response = null;
            try
            {
                response = await STRService.GetMultiCorpMonthlyData(request);
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
        /// Retrieves yearly data for the Multi Corporation.
        /// </summary>
        /// <param name="request">Details of the year for which data is requested.</param>
        /// <returns>Returns STR yearly data for the Multi Corporation.</returns>
        [Route("GetMultiCorpYearlyData")]
        [HttpPost]
        public async Task<IActionResult> GetMultiCorpYearlyData(STRYearlyRequest request)
        {
            STRResponse? response = null;
            try
            {
                response = await STRService.GetMultiCorpYearlyData(request);
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
        /// Retrieves data for a specific date range for the Multi Corporation.
        /// </summary>
        /// <param name="request">Details of the start and end date for the range.</param>
        /// <returns>Returns STR range data for the Multi Corporation.</returns>
        [Route("GetMultiCorpRangeData")]
        [HttpPost]
        public async Task<IActionResult> GetMultiCorpRangeData(STRWeekORRangeRequest request)
        {
            STRResponse? response = null;
            try
            {
                response = await STRService.GetMultiCorpRangeData(request);
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

        [Route("GetMultiCorpMonthData")]
        [HttpPost]
        public async Task<IActionResult> GetMultiCorpMonthData(STRMutliCorpMonthRequest request)
        {
            STRMonthDataGroupDTO? response = null;
            try
            {
                response = await STRService.GetSTRMultiCorpMonthData(request);
                if (response != null) /*&& response.StatusCode == StatusCodes.Status200OK*/
                    return Ok(response);
                else if (response != null) /* && !string.IsNullOrEmpty(response.Status))*/
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }


        #endregion

        #region SingleCorporation
        /// <summary>
        /// Retrieves week data from the external API.
        /// </summary>
        /// <returns>Returns week data from the external API</returns>
        [Route("STRWeekData")]
        [HttpPost]
        public async Task<IActionResult> GetWeekData(STRWeekORRangeRequest request)
        {
            STROptions? response = null;
            try
            {
                response = await STRService.GetWeekData(request);
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
        /// Retrieves week data from the external API.
        /// </summary>
        /// <returns>Returns week data from the external API</returns>
        [Route("STRWeeklyData")]
        [HttpPost]
        public async Task<IActionResult> STRWeeklyData(STRWeeklyRequest request)
        {
            STROptions? response = null;
            try
            {
                response = await STRService.GetWeeklyData(request);
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
        /// Retrieves week data from the external API.
        /// </summary>
        /// <returns>Returns week data from the external API</returns>
        [Route("STRMontlyData")]
        [HttpPost]
        public async Task<IActionResult> STRMontlyData(STRMontlyRequest request)
        {
            STROptions? response = null;
            try
            {
                response = await STRService.GetMontlyData(request);
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
        /// Retrieves week data from the external API.
        /// </summary>
        /// <returns>Returns week data from the external API</returns>
        [Route("STRYearlyData")]
        [HttpPost]
        public async Task<IActionResult> STRYearlyData(STRYearlyRequest request)
        {
            STROptions? response = null;
            try
            {
                response = await STRService.GetYearlyData(request);
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
        /// Retrieves week data from the external API.
        /// </summary>
        /// <returns>Returns week data from the external API</returns>
        [Route("STRRangeData")]
        [HttpPost]
        public async Task<IActionResult> STRRangeData(STRWeekORRangeRequest request)
        {
            STROptions? response = null;
            try
            {
                response = await STRService.GetRangeData(request);
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

        /// <summary>
        /// Retrieves latest available data from the external api.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("LatestAvailableDate")]
        [HttpPost]
        public async Task<IActionResult> GetLatestAvailableDate(LatestAvailableDateRequest request)
        {
            LatestAvailableDateResponse? response = null;
            try
            {
                response = await STRService.GetLatestAvailableDate(request);
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
        /// Endpoint to retrieve monthly STR (Short-Term Rental) data based on the provided request.
        /// </summary>
        /// <param name="request">Request object containing parameters for querying STR data.</param>
        /// <returns>
        /// Returns an IActionResult representing the HTTP response with the requested STR data.
        /// - Returns 200 OK if data is successfully retrieved.
        /// - Returns 404 Not Found if no data is found for the given request.
        /// - Throws an exception if there is an internal server error (status code 500).
        /// </returns>
        [Route("GetSTRMonthData")]
        [HttpPost]
        public async Task<IActionResult> GetSTRMonthData(STRMonthDataRequest request)
        {
            STRMonthDataGroupDTO? response = null;
            try
            {
                response = await STRService.GetSTRMonthData(request);
                if (response != null)
                    return Ok(response);
                else if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// Endpoint to retrieve day-wise STR (Short-Term Rental) data for a specific month based on the provided request.
        /// </summary>
        /// <param name="request">Request object containing parameters for querying day-wise STR data for a month.</param>
        /// <returns>
        /// Returns an IActionResult representing the HTTP response with the requested day-wise STR data.
        /// - Returns 200 OK if data is successfully retrieved and the status code is 200 OK.
        /// - Returns 404 Not Found if no data is found for the given request.
        /// - Throws an exception if there is an internal server error (status code 500).
        /// </returns>
        [Route("GetSTRDayWiseMonthData")]
        [HttpPost]
        public async Task<IActionResult> GetSTRDayWiseMonthData(STRDayWiseMonthRequest request)
        {
            STRDayWiseMonthData? response = null;
            try
            {
                response = await STRService.STRDayWiseMonthData(request);

                // Check if response is not null and the status code is 200 OK
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);  // Return 200 OK with response data

                else if (response != null)
                    return Ok(response);  // Return 200 OK with response data (status code might not be 200 OK)

                else
                    return NotFound(response);  // Return 404 Not Found (response is null)
            }
            catch { throw; }
            finally { response = null; }  // Ensure response variable is nullified at the end
        }

        #endregion

        #region STRImportData
        /// <summary>
        /// Endpoint to handle file upload for Short-Term Rental (STR) data.
        /// </summary>
        /// <param name="request">Request object containing file upload parameters.</param>
        /// <returns>
        /// Returns an IActionResult representing the HTTP response with the result of the file upload operation.
        /// - Returns 200 OK if file upload is successful and the status code is 200 OK.
        /// - Returns 404 Not Found if the uploaded file or response is null.
        /// - Throws an exception if there is an internal server error (status code 500).
        /// </returns>
        [Route("FileUpload")]
        [HttpPost]
        public async Task<IActionResult> STRFileUpload(STRFileUploadRequest request)
        {
            STRFileUploadResponse? response = null;
            try
            {
                // Extract user ID and client ID from HttpContext.Items
                string userId = (string)HttpContext.Items["UserId"];
                string clientId = (string)HttpContext.Items["ClientId"];

                // Call service to perform file upload
                response = await STRService.STRFileUpload(request, userId, clientId);

                // Check if response is not null and the status code is 200 OK
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);  // Return 200 OK with response data

                else if (response != null)
                    return Ok(response);  // Return 200 OK with response data (status code might not be 200 OK)

                else
                    return NotFound(response);  // Return 404 Not Found (response is null)
            }
            catch { throw; }
            finally { response = null; }  // Ensure response variable is nullified at the end
        }



        /// <summary>
        /// Endpoint to import files list and retrieve Short-Term Rental (STR) grid data based on the provided request.
        /// </summary>
        /// <param name="request">Request object containing parameters for importing files list and querying STR grid data.</param>
        /// <returns>
        /// Returns an IActionResult representing the HTTP response with the requested STR grid import list data.
        /// - Returns 200 OK if data is successfully retrieved and the status code is 200 OK.
        /// - Returns 404 Not Found if no data is found for the given request.
        /// - Throws an exception if there is an internal server error (status code 500).
        /// </returns>
        [Route("ImportFilesList")]
        [HttpPost]
        public async Task<IActionResult> STRGridData(STRGridDataRequest request)
        {
            STRGridImportListDTO? response = null;
            try
            {
                // Call service to fetch STR grid data based on request
                response = await STRService.STRGridData(request);

                // Check if response is not null and the status code is 200 OK
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);  // Return 200 OK with response data

                else if (response != null)
                    return Ok(response);  // Return 200 OK with response data (status code might not be 200 OK)

                else
                    return NotFound(response);  // Return 404 Not Found (response is null)
            }
            catch { throw; }
            finally { response = null; }  // Ensure response variable is nullified at the end
        }


        /// <summary>
        /// Endpoint to handle downloading of Short-Term Rental (STR) files based on the provided request.
        /// </summary>
        /// <param name="request">Request object containing parameters for downloading an STR file.</param>
        /// <returns>
        /// Returns an IActionResult representing the HTTP response with the downloaded STR file.
        /// - Returns 200 OK if file download is successful and the status code is 200 OK.
        /// - Returns 404 Not Found if no file is found or response is null.
        /// - Throws an exception if there is an HTTP request exception.
        /// </returns>
        [Route("STRFileDownload")]
        [HttpPost]
        public async Task<IActionResult> STRFileDownload(DownloadSTRFileRequest request)
        {
            DownloadSTRFileResponse response = null;
            try
            {
                // Call service to download STR file based on request
                response = await STRService.DownloadFileAsync(request);

                // Check if response is not null and the status code is 200 OK
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);  // Return 200 OK with response data

                else if (response != null)
                    return Ok(response);  // Return 200 OK with response data (status code might not be 200 OK)

                else
                    return NotFound(response);  // Return 404 Not Found (response is null)
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        [Route("STRFileDelete")]
        [HttpPost]
        public async Task<IActionResult> STRFileDelete(DeleteSTRFileRequest request)
        {
            DeleteSTRFileResponse response = null;
            try
            {
                // Extract user ID and client ID from HttpContext.Items
                string userId = (string)HttpContext.Items["UserId"];
                request.userId = userId;
                // Call service to download STR file based on request
                response = await STRService.DeleteSTRFile(request);

                // Check if response is not null and the status code is 200 OK
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);  // Return 200 OK with response data

                else if (response != null)
                    return Ok(response);  // Return 200 OK with response data (status code might not be 200 OK)

                else
                    return NotFound(response);  // Return 404 Not Found (response is null)
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }





        #endregion

        #region STRReportData
        [Route("STRReportData")]
        [HttpPost]
        public async Task<IActionResult> GetSTRReportData(STRReportRequest request)
        {
            STRReportGroupData response = null;
            try
            {

                // Call service to download STR file based on request
                response = await STRService.GetSTRReportData(request);

                // Check if response is not null and the status code is 200 OK
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);  // Return 200 OK with response data

                else if (response != null)
                    return Ok(response);  // Return 200 OK with response data (status code might not be 200 OK)

                else
                    return NotFound(response);  // Return 404 Not Found (response is null)
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }


        #endregion



        #region STR Overview

        [Route("STROverview")]
        [HttpPost]
        public async Task<IActionResult> GetSTROverview(STRWeekORRangeRequest request)
        {
            STROptions? response = null; response = null;
            try
            {
                // Call service to download STR file based on request
                response = await STRService.GetSTROverview(request);

                // Check if response is not null and the status code is 200 OK
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);  // Return 200 OK with response data

                else if (response != null)
                    return Ok(response);  // Return 200 OK with response data (status code might not be 200 OK)

                else
                    return NotFound(response);  // Return 404 Not Found (response is null)
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion


        #region GSS
        [HttpPost]
        [Route("GSSFileUpload")]
        public async Task<IActionResult> GSSFileUpload(GSSFileUploadRequest request)
        {
            string userId = (string)HttpContext.Items["UserId"];
            string clientId = (string)HttpContext.Items["ClientId"];

            bool excludeDates = false;
            if (Request.Query.TryGetValue("exclude_dates", out var excludeVal))
            {
                bool.TryParse(excludeVal, out excludeDates);
            }

            var response = await STRService.GSSFileUpload(request, userId, clientId, excludeDates);

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
        #endregion
        //[Route("ScorecardUpload")]
        //[HttpPost]
        //public async Task<IActionResult> ScorecardUpload([FromForm] GSSFileUploadRequest request)
        //{
        //    GSSFileUploadResponse? response = null;
        //    try
        //    {
        //        // Extract user ID and client ID from HttpContext.Items
        //        string userId = (string)HttpContext.Items["UserId"];
        //        string clientId = (string)HttpContext.Items["ClientId"];

        //        // Call the STRService method for scorecard upload
        //        response = await STRService.GSSScorecardUpload(request, userId, clientId);

        //        // Check if response is not null and the status code is 200 OK
        //        if (response != null && response.StatusCode == StatusCodes.Status200OK)
        //            return Ok(response);

        //        else if (response != null)
        //            return Ok(response);  // Still return 200 with message for partial success or warnings

        //        else
        //            return NotFound(response);  // In case response is null
        //    }
        //    catch { throw; }
        //    finally { response = null; }
        //}

    }
}
