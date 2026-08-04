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
    public class ReceiptController : BaseController
    {
        #region Fields
        private readonly ICoreProperty coreProp;
        private readonly IReceiptService receiptSrv;
        private readonly IFileService fileSrv;
        private readonly IJournalService journalSrv;

        #endregion

        #region Ctor
        public ReceiptController(ICoreProperty coreProperty, IFileService fileService, IReceiptService receiptService, IJournalService journalService)
        {
            this.coreProp = coreProperty;
            this.fileSrv = fileService;
            this.receiptSrv = receiptService;
            this.journalSrv = journalService;
        }

        #endregion

        #region Receipt 

        /// <summary>
        /// It will get receipt entries based on given CoporationID
        /// </summary>
        /// <param name="data">data represents JournalSearchRequest  details </param>
        /// <returns>It returns receipt entries list</returns>
        [Route("receipts/list")]
        [HttpPost]
        public async Task<IActionResult> GetReceipts(JournalSearchRequest data)
        {
            JEListResponse? response = null;
            try
            {
                response = await receiptSrv.GetReceiptEntryList(data);
                if (response.Count >= 0)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will get receipt details based on EntryID
        /// </summary>
        /// <param name="data">data represents LoadByIDRequest</param>
        /// <returns>It returns creditcard details </returns>
        [Route("receipts/load")]
        [HttpPost]
        public async Task<IActionResult> GetReceiptEntry(LoadByIDRequest data)
        {
            CheckLoadResponse? response = null;
            try
            {
                response = await receiptSrv.GetReceiptEntry(data);
                if (response != null && response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will create Receipt based on given CheckEntryRequest
        /// </summary>
        /// <param name="data">It will represents CheckEntryRequest information </param>
        /// <returns>It returns ID,Statusmessage and statuscode</returns>
        [Route("receipts/create")]
        [HttpPost]
        public async Task<IActionResult> CreateReceiptEntry(CheckEntryRequest data)
        {
            JournalResponse? response = null;
            try
            {
                response = await receiptSrv.CreateReceiptEntry(data);
                if (response != null && response.StatusCode == 200 && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will update Receipt entry based on CheckEntryRequest
        /// </summary>
        /// <param name="data">It will represents CheckEntryRequest information </param>
        /// <returns>It returns ID,Statusmessage and statuscode</returns>
        [Route("receipts/update")]
        [HttpPut]
        public async Task<IActionResult> UpdateReceiptEntry(CheckEntryRequest data)
        {
            JournalResponse? response = null;
            try
            {
                response = await receiptSrv.UpdateReceiptEntry(data);
                if (response != null && response.StatusCode == 200 && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }
        [Route("receipts/delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteReceiptEntry(LoadByIDRequest loadByID)
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
