using Amazon.Runtime.Internal;
using Azure.Core;
using Common.API.Authorization;
using Common.Domain.Common;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Dashboard.App.Contracts;
using Dashboard.App.Services;
using Dashboard.Domain.DataModel;
using Dashboard.Domain.DTO.Model;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Dashboard.API.Controllers
{
    [Route("v1/Widget")]
    [ApiController]
    [Authorize]
    public class WidgetConfigurationController : ControllerBase
    {
        #region Ctor
        private readonly IWidgetConfigService widget;
        private readonly IConfiguration _configuration;

        public WidgetConfigurationController(IWidgetConfigService widget, IConfiguration configuration)
        {
            this.widget = widget;
            _configuration = configuration;
        }

        #endregion

        #region Widget Common

        /// <summary>
        /// Loads widgets based on the provided request criteria.
        /// </summary>
        /// <param name="data">The widget request used to filter widgets.</param>
        /// <returns>A list of widgets.</returns>
        [Route("GetWidgets")]
        [HttpPost]
        public async Task<IActionResult> GetWidgets(WidgetRequest data)
        {
            WidgetResponse response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                string userId = (string)HttpContext.Items["UserId"];
                string ClientID = (string)HttpContext.Items["ClientId"];
                data.ClientID = ClientID;
                if (!userId.IsNullOrEmpty()) data.LoginUserID = userId;

                response = await widget.GetAllWidgets(data, urlInfoID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                if (response != null)
                    response.Dispose();
                response = null;
            }
        }

        #endregion

        private long getUrlID()
        {
            return Convert.ToInt64((string)HttpContext.Items["UrlID"]);
        }

        private bool GetGSSAndDepartmentIncomeEnabledClients(string clientName)
        {

            string EnabledClients = _configuration["WidgetsEnabledClients"];
            var allowedClients = EnabledClients?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return (allowedClients != null && (allowedClients[0] == "default" || allowedClients.Contains(clientName, StringComparer.OrdinalIgnoreCase)));
        }

        #region Widget Privileges

        /// <summary>
        /// It gets Widget privileges for the login user
        /// </summary>
        /// <returns>Returns login user widget privileges</returns>
        [Route("GetPrivileges")]
        [HttpPost]
        public async Task<IActionResult> GetWidgetPrivileges(ModelBaseUserID request)
        {
            WidgetPrivilegeResponse response = new WidgetPrivilegeResponse();
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);

                if (string.IsNullOrEmpty(request.UserID) || request.UserID == "0")
                {
                    request.UserID = (string)HttpContext.Items["UserId"];
                }
                //string userId = (string)HttpContext.Items["UserId"];
                var privileges = await widget.GetWidgetPrivileges(request.UserID, urlInfoID);
                if (privileges != null)
                {
                    response.WidgetUserPrivileges = privileges;
                    response.StatusCode = StatusCodes.Status200OK;
                    return Ok(response);
                }
                return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                if (response != null)
                    response.Dispose();
            }
        }

        /// <summary>
        /// It gets all Widgets privileges for the selected user/role to change the access.
        /// </summary>
        /// <param name="data">Selected user/role id to get all Widgets privileges</param>
        /// <returns></returns>
        [Route("LoadPrivileges")]
        [HttpPost]
        public async Task<IActionResult> LoadAllWidgetPrivileges(WidgetPrivilegeRequest data)
        {
            WidgetPrivilegeResponse response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);

                string ClientID = (string)HttpContext.Items["ClientId"];

                data.ClientID = ClientID;
                if (string.IsNullOrEmpty(data.LoginUserID) || data.LoginUserID == "0")
                {
                    data.LoginUserID = (string)HttpContext.Items["UserId"];
                }
                response = await widget.LoadWidgetPrivileges(data, urlInfoID);

                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                if (response != null)
                    response.Dispose();
            }
        }

        /// <summary>
        /// It will create the widgetPrivileges based on request.
        /// </summary>
        /// <param name="data"> It contains WidgetPrivilegeDetais and widgetPrivilegList. </param>
        /// <returns>It will return the StatusCodes and Statsu response</returns>
        [Route("CreatePrivileges")]
        [HttpPost]
        public async Task<IActionResult> CreateWidgetPrivileges(WidgetPrivilegeCreateRequest data)
        {
            WidgetPrivilegeCreateResponse response = null;
            try
            {

                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);


                response = await widget.CreateWidgetPrivileges(data, urlInfoID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                response = null;
            }
        }


        /// <summary>
        /// Endpoint to assign widget privileges to a list of clients.
        /// </summary>
        /// <param name="request">The request containing the list of user IDs.</param>
        [Route("ClientWidgetPrivileges")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AssignWidgetPrivClients(ClientUserWidgetPrivilegesRequest request)
        {
            StatusDTO response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                string clientName = Convert.ToString(HttpContext.Items["ClientName"]);
                //this is temperory it will be placed default if released to all clients
                bool isEnabledClients = GetGSSAndDepartmentIncomeEnabledClients(clientName);
                request.IsEnabledClients = isEnabledClients;
                response = await widget.AssignWidgetPrivClients(request, urlInfoID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                response = null;
            }

        }

        /// <summary>
        /// Endpoint to assign widget privileges to a list of clients.
        /// </summary>
        /// <param name="request">The request containing the user ID,ReportTo or RoleID.</param>
        [Route("UpdateUserOrRolePrivileges")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserOrRolePrivileges(WidgetPrivilegeRequest request)
        {
            StatusDTO response = null;
            try
            {
                long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);
                response = await widget.UserOrRolePrivileges(request, urlInfoID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; /* return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); */}
            finally
            {
                response = null;
            }

        }

        [Route("GetDailysalesMatrices")]
        [HttpPost]
        public async Task<IActionResult> GetDailysalesMatrices(DailySalesMatricesReq request)
        {

          
            DailySalesMatricesResp Response = await widget.GetDailysalesMatrices(request, getUrlID());
            return Ok(Response);

        }

        //OTB Widgets
        /// <summary>
        /// Endpoint for OTB Overview Details in Hotel View
        /// </summary>
        [Route("GetOTBWidgetOverViewDet")] 
        [HttpPost]
        public async Task<IActionResult> GetOTBWidgetOverViewDet(OTBOverViewDetReq request)
        {

            List<OTBOverviewResponse> Response = await widget.GetOTBWidgetOverViewDet(request, getUrlID());
            return Ok(Response);

        }
        /// <summary>
        /// Endpoint for OTB PickUp Overview Details in Hotel View
        /// </summary>
        //[Route("GetOTBWidgetPickUpOverViewDet")]
        //[HttpPost]
        //public async Task<IActionResult> GetOTBWidgetPickUpOverViewDet(OTBPickUpOverViewDetReq request)
        //{
        //    long urlInfoID = Convert.ToInt64(HttpContext.Items["UrlID"]);

        //    DailySalesMatricesResp Response = await widget.GetDailysalesMatrices(request, urlInfoID);
        //}

        [Route("GetOTBWidgetPickUpOverViewDet")]
        [HttpPost]
        public async Task<IActionResult> GetOTBWidgetPickUpOverViewDet(OTBPickUpOverViewDetReq request)
        {
            List<OTBPickUpOverviewResponse> Response = await widget.GetOTBWidgetPickUpOverViewDet(request, getUrlID());
            return Ok(Response);

        }
        /// <summary>
        /// Endpoint for OTB Performance Details in Group of Hotels
        /// </summary>
        [Route("GetOTBWidgetPerformanceDet")]
        [HttpPost]
        public async Task<IActionResult> GetOTBWidgetPerformanceDet(OTBPerformanceReq request)
        {
            List<OTBPerformanceResponse> Response = await widget.GetOTBWidgetPerformanceDet(request, getUrlID());
            return Ok(Response);

        }
        /// <summary>
        /// Endpoint for OTB PickUp Details in Group of Hotels
        /// </summary>
        [Route("GetOTBWidgetPickUpPerformanceDet")]
        [HttpPost]
        public async Task<IActionResult> GetOTBWidgetPickUpPerformanceDet(OTBPickUpReq request)
        {
            if(request.ViewBy == 1)
            {
                List<OTBPickUpDayWiseResponse> Response = await widget.GetOTBWidgetPickUpPerformanceDet(request, getUrlID());
                return Ok(Response);
            }
            else
            {
                List<OTBPickUpSummaryResponse> Response = await widget.GetOTBWidgetPickUpSummaryPerformanceDet(request, getUrlID());
                return Ok(Response);
            }


        }

        //OTB Widgets End





        #endregion

    }
}
