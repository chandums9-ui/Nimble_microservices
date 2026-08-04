using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Microsoft.AspNetCore.Mvc;

namespace BankFeed.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class FeedImportController : ControllerBase
    {

        #region Fields
        private readonly IFeedImportService feedImportService;
        #endregion

        #region Constructor
        public FeedImportController(IFeedImportService _feedImportService)
        {
            this.feedImportService = _feedImportService;
        }
        #endregion

        #region Feed Import

        [Route("Import/Load")]
        [HttpPost]
        public async Task<IActionResult> LoadImport(LoadByLongIDRequest Request)
        {
            ImportLoadResponse response = null;
            try
            {
                response = await feedImportService.GetImportDetails(Request.ID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("Import/NimAccountCheck")]
        [HttpPost]
        public async Task<IActionResult> NimCOAccountCheck(FeedImportNimbleAccRequest Request)
        {
            NimCOAccountCheckResponse response = null;
            try
            {
                Request.ClientID = HttpContext.Items["ClientId"].ToString();

                response = await feedImportService.NimCOAccountCheck(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }
        /// <summary>
        /// To Save the Feed Import data
        /// </summary>
        /// <param name="Request">FeedImportRequest containing bank data and transactions</param>
        /// <returns>Import ID in ImportResponse</returns>
        [Route("Import/Create")]
        [HttpPost]
        public async Task<IActionResult> SaveImport(FeedImportRequest Request)
        {
            ImportResponse response = null;
            try
            {
                Request.ClientID = HttpContext.Items["ClientId"].ToString();
                Request.ClientName = HttpContext.Items["ClientName"].ToString();
                response = await feedImportService.SaveFeedImport(Request);
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
        /// To remove all the open feed transactions in the previous import
        /// </summary>
        /// <param name="Request">Import ID</param>
        /// <returns>removed transactions ImportID  in ImportResponse </returns>
        [Route("Import/RemoveOpenFeeds")]
        [HttpPost]
        public async Task<IActionResult> RemoveOpenFeeds(LoadByLongIDRequest Request)
        {
            ImportResponse response = null;
            try
            {
                response = await feedImportService.RemoveOpenFeeds(Request.ID);
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
        /// To remove all transactions in the previous/last import
        /// </summary>
        /// <param name="Request">Import ID</param>
        /// <returns>removed transactions ImportID  in ImportResponse </returns>
        [Route("Import/RemoveLastImport")]
        [HttpPost]
        public async Task<IActionResult> RemoveLastImport(LoadByLongIDRequest Request)
        {
            ImportResponse response = null;
            try
            {
                response = await feedImportService.RemoveLastImport(Request.ID);
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

        #region Format Settings

        /// <summary>
        /// To get list of Import formats based on ClientID
        /// </summary>
        /// <returns> list of formats in FormatSettingsListResponse </returns>
        [Route("Format/List")]
        [HttpPost]
        public async Task<IActionResult> GetFormatSettingsList(PageDTO PageDetails)
        {
            FormatSettingsListResponse response = null;
            try
            {
                response = await feedImportService.GetAllFormatSettings(HttpContext.Items["ClientId"].ToString(), PageDetails);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        ///  To view the format settings based on FormatId
        /// </summary>
        /// <param name="Request">Format Id</param>
        /// <returns>Detailed view of format settings in FormatSettingLoadResponse</returns>
        [Route("Format/Load")]
        [HttpPost]
        public async Task<IActionResult> LoadFormatSettings(LoadByLongIDRequest Request)
        {
            FormatSettingLoadResponse response = null;
            try
            {
                response = await feedImportService.LoadFormatSettings(Request.ID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// To Create the Import Format Settings
        /// </summary>
        /// <param name="FormatReq"> Transaction reading conditions from .csv and related settings </param>
        /// <returns>Created Format ID  in FormatResponse</returns>
        [Route("Format/Create")]
        [HttpPost]
        public async Task<IActionResult> CreateFormatSetting(ImportFormatRequest Request)
        {
            FormatResponse response = null;
            try
            {
                response = await feedImportService.CreateFormatSetting(Request, HttpContext.Items["ClientId"].ToString(), HttpContext.Items["ClientName"].ToString());
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
        /// To update existing Import Format Settings based on Import ID
        /// </summary>
        /// <param name="FormatReq"> Transaction reading conditions from .csv and related settings </param>
        /// <returns>Updated Format ID  in FormatResponse</returns>
        [Route("Format/Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateFormatSetting(ImportFormatRequest Request)
        {
            FormatResponse response = null;
            try
            {
                response = await feedImportService.UpdateFormatSetting(Request, HttpContext.Items["ClientId"].ToString(), HttpContext.Items["ClientName"].ToString());
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
        /// To delete format settings based on format ID
        /// </summary>
        /// <param name="FormatId">format Id</param>
        /// <returns>Deleted format Id in FormatResponse</returns>
        [Route("Format/Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteFormatSetting(LoadByLongIDRequest Request)
        {
            FormatResponse response = null;
            try
            {
                response = await feedImportService.DeleteFormatSetting(Request.ID);
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

    }
}
