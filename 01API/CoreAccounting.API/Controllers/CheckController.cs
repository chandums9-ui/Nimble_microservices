//using Azure;
using Common.App.Contracts;
using Common.Domain;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using CoreAccounting.App.Services;
using CoreAccounting.Domain.DTO.Req;
using Microsoft.AspNetCore.Mvc;
using CoreAccounting.Domain.DTO.Resp;

namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class CheckController : BaseController
    {
        #region Fields
        private readonly ICoreProperty coreProp;
        private readonly ICheckService checkSrv;
        private readonly IFileService fileSrv;
        private readonly IJournalService journalSrv;

        #endregion

        #region Ctor
        public CheckController(ICoreProperty coreProperty, IFileService fileService, ICheckService checkService, IJournalService journalService)
        {
            this.coreProp = coreProperty;
            this.checkSrv = checkService;
            fileSrv = fileService;
            this.journalSrv = journalService;
        }

        #endregion

        #region check

        /// <summary>
        /// It will get check entries based on given CoporationID ,todate ,fromdate,count and offset
        /// </summary>
        /// <param name="data">I represents JournalSearchRequest</param>
        /// <returns>It returns  check entries list</returns>
        [Route("checks/list")]
        [HttpPost]
        public async Task<IActionResult> GetCheckList(JournalSearchRequest data)
        {
            JEListResponse? response = null;
            try
            {
                response = await checkSrv.GetCheckEntryList(data);
                if (response != null && response.Count >= 0)
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will get check entry details based on EntryID
        /// </summary>
        /// <param name="data">data represents LoadByIDRequest</param>
        /// <returns>It returns check details </returns>
        [Route("checks/load")]
        [HttpPost]
        public async Task<IActionResult> GetCheckEntry(LoadByIDRequest data)
        {
            CheckLoadResponse? response = null;
            try
            {
                response = await checkSrv.GetCheckEntry(data);
                if (response != null && response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }

        }

        /// <summary>
        /// It will create check based on given CheckEntryRequest details
        /// </summary>
        /// <param name="data">It will represents CheckEntryRequest </param>
        /// <returns>It returns ID,Statusmessage and statuscode</returns>
        [Route("checks/create")]
        [HttpPost]
        public async Task<IActionResult> CreateCheckEntry(CheckEntryRequest data)
        {
            JournalResponse? response = null;
            try
            {
                response = await checkSrv.CreateCheckEntry(data);
                if (response != null && response.StatusCode == 200 && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will update check Entry based on given CheckEntryRequest details
        /// </summary>
        /// <param name="data">It will represents CheckEntryRequest </param>
        /// <returns>It returns ID,Statusmessage and statuscode</returns>
        [Route("checks/update")]
        [HttpPut]
        public async Task<IActionResult> UpdateCheckEntry(CheckEntryRequest data)
        {
            JournalResponse? response = null;
            try
            {
                response = await checkSrv.UpdateCheckEntry(data);
                if (response != null && response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }
        [Route("checks/delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteCheckEntry(LoadByIDRequest loadByID)
        {
            JournalResponse? response = null;
            try
            {
                response = await journalSrv.DeleteJournalentryById(loadByID, GetUserID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }
        #endregion
    }
}
