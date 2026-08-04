
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Dashboard.App.Contracts;
using Dashboard.Domain.DataModel;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [Route("v1/WidgetFilter")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class WidgetFilterController : ControllerBase
    {
        private readonly IWidgetFilterService widgetFilterService;

        /// <summary>
        /// Constructor for WidgetFilterController.
        /// </summary>
        /// <param name="_widgetFilterService">Instance of the widget filter service.</param>
        public WidgetFilterController(IWidgetFilterService _widgetFilterService)
        {
            widgetFilterService = _widgetFilterService;
        }

        #region Widget Filters

        /// <summary>
        /// Endpoint to get widget filter settings.
        /// </summary>
        [Route("GetWidgetFilter")]
        [HttpGet]
        public async Task<IActionResult> GetWidgetFilter(long id)
        {
            WidgetUserSettingsResponse response = new WidgetUserSettingsResponse();
            string userId = (string)HttpContext.Items["UserId"];
            try
            {
                // Retrieve widget filter details
                response = await widgetFilterService.GetWidgetFilter(id, userId);

                // Handle response based on status code and content
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response); // Return OK if successful
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response); // Return OK with status message
                else
                    return BadRequest(response); // Return BadRequest if unsuccessful
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                if (response != null)
                    response.Dispose(); // Dispose the response object if not null
                response = null;
            }
        }

        /// <summary>
        /// Endpoint to save widget filter settings.
        /// </summary>
        [Route("SaveWidgetFilter")]
        [HttpPost]
        public async Task<IActionResult> SaveWidgetFilter(WidgetUserSettingsRequest request)
        {
            // Initialize the response object
            WidgetUserSettingsResponse response = new WidgetUserSettingsResponse();
            string userId = (string)HttpContext.Items["UserId"];
            // Call the widget service to save the filter settings
            response = await widgetFilterService.SaveWidgetFilter(request, userId);

            try
            {
                // Handle response based on status code and content
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response); // Return OK if successful
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response); // Return OK with status message
                else
                    return BadRequest(response); // Return BadRequest if unsuccessful
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                // Dispose the response object if not null
                if (response != null)
                    response.Dispose();
                response = null;
            }
        }

        /// <summary>
        /// Endpoint to delete widget filter settings.
        /// </summary>
        [Route("DeleteWidgetFilter")]
        [HttpPost]
        public async Task<IActionResult> DeleteWidgetFilter(long id)
        {
            WidgetUserSettingsResponse response;
            string userId = (string)HttpContext.Items["UserId"];
            response = await widgetFilterService.DeleteWidgetFilter(id, userId);
            try
            {
                // Handle response based on status code and content
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response); // Return OK if successful
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response); // Return OK with status message
                else
                    return BadRequest(response); // Return BadRequest if unsuccessful
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                if (response != null)
                    response.Dispose(); // Dispose the response object if not null
                response = null;
            }
        }


       
        #endregion

        #region CustomWidget

        /// <summary>
        /// Endpoint to add a custom widget.
        /// </summary>
        [Route("AddCustomWidget")]
        [HttpPost]
        public async Task<IActionResult> AddCustomWidegt(AddCustomWidgetRequest request)
        {
            // Define the user ID and client ID
            string userId = (string)HttpContext.Items["UserId"];
            string clientID = (string)HttpContext.Items["ClientId"];
            List<string> parentUsers = new List<string>();
            // Initialize the response object
            AddCustomWidgetResponse response = new AddCustomWidgetResponse();

            // Call the method to add the custom widget
            response = await widgetFilterService.AddCustomWidget(request, userId, clientID);

            try
            {
                // Handle response based on status code and content
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response); // Return OK with the response
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response); // Return OK with the response
                else
                    return BadRequest(response); // Return Bad Request with the response
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                // Clean up resources
                if (response != null)
                    response.Dispose();
                response = null;
            }
        }

        #endregion

        #region AnalysisProperties

        /// <summary>
        /// Endpoint to get analysis table response based on formulaId.
        /// </summary>
        [Route("AnalysisInfo")]
        [HttpPost]
        public async Task<IActionResult> GetAnalysisDetails(AnalysisRequest request)
        {
            Analysisresponse? response = null;

            try
            {
                // Call the service to retrieve analysis response
                response = await widgetFilterService.GetAnalysisResponse(request);

                // Handle response based on status code and content
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response); // Return OK with the response
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response); // Return OK with the response
                else
                    return NotFound(response); // Return NotFound if unsuccessful
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                response = null;
            }
        }

        #endregion

        [Route("CheckCustomWidgetExists")]
        [HttpGet]
        public async Task<IActionResult> CheckCustomWidgetExists(string WidgetName,long WidgetID)
        {
            bool IsCustomWidgetExists = false;
            try
            {
                IsCustomWidgetExists = await widgetFilterService.CheckCustomWidgetExists(WidgetName,WidgetID);
                if (IsCustomWidgetExists)
                {
                    return Ok(IsCustomWidgetExists);
                }
                else if (IsCustomWidgetExists == false)
                {
                    return Ok(IsCustomWidgetExists);
                }
                return BadRequest(IsCustomWidgetExists);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { IsCustomWidgetExists = false; }
        }
    }
}
