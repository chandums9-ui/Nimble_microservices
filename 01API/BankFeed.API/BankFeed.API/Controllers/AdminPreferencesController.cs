using BankFeed.App.Contracts;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using Microsoft.AspNetCore.Mvc;

namespace BankFeed.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class AdminPreferencesController : ControllerBase
    {
        #region Fields
        private readonly IAdminServices adminServices;
        #endregion

        #region Ctor
        public AdminPreferencesController(IAdminServices _adminServices)
        {
            this.adminServices = _adminServices;
        }
        #endregion

        #region FeedPreferences

        /// <summary>
        /// To get Bank feed Admin settings/preferences based on ClientID
        /// </summary>
        /// <returns>It returns admin preferences for bankfeed in FeedSettingsLoadResponse</returns>
        [Route("FeedSettings/Load")]
        [HttpGet]
        public async Task<IActionResult> LoadAdminFeedSettings()
        {
            FeedSettingsLoadResponse response = null;
            try
            {
                response = await adminServices.LoadFeedSettings(HttpContext.Items["ClientId"].ToString());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound();
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// To Save or Update Bankfeed Admin settings/preferences based on clientID
        /// </summary>
        /// <param name="Request">ClientID and related settings/preferences to save</param>
        /// <returns>It returns FeedSettings Id in FeedSettingsResponse</returns>
        [Route("FeedSettings/SaveOrUpdate")]
        [HttpPost]
        public async Task<IActionResult> SetAdminFeedSettings(FeedSettingsRequest Request)
        {
            FeedSettingsResponse response = null;
            try
            {
                Request.ClientID = HttpContext.Items["ClientId"].ToString();
                response = await adminServices.SaveOrUpdateFeedSettings(Request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound();
            }
            catch { throw; }
            finally { response = null; }
        }

        #endregion
    }
}
