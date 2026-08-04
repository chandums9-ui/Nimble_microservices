using Common.App.Contracts;
using Common.Domain;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Req;
using Microsoft.AspNetCore.Mvc;
using CoreAccounting.Domain.DTO.Resp;

namespace CoreAccounting.API.Controllers
{

    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class CreditCardController : BaseController
    {
        #region Fields
        private readonly ICoreProperty coreProp;
        private readonly ICreditCardService creditCardSrv;
        private readonly IFileService fileSrv;
        private readonly IJournalService journalSrv;

        #endregion

        #region Ctor
        public CreditCardController(ICoreProperty coreProperty, IFileService fileService, ICreditCardService creditCardService, IJournalService journalService)
        {
            this.coreProp = coreProperty;
            this.fileSrv = fileService;
            this.creditCardSrv = creditCardService;
            this.journalSrv = journalService;
        }

        #endregion

        #region CreditCard

        /// <summary>
        /// It will get creditcard entries based on given CoporationID
        /// </summary>
        /// <param name="data">data represents JournalSearchRequest  details </param>
        /// <returns>It returns  creditcard entries list</returns>
        [Route("CreditCard/list")]
        [HttpPost]
        public async Task<IActionResult> GetCreditCards(JournalSearchRequest data)
        {
            JEListResponse? response = null;
            try
            {
                response = await creditCardSrv.GetCreditCardList(data);
                if (response != null && response.Count >= 0)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will get creditcard details based on EntryID
        /// </summary>
        /// <param name="data">data represents LoadByIDRequest</param>
        /// <returns>It returns creditcard details </returns>
        [Route("CreditCard/load")]
        [HttpPost]
        public async Task<IActionResult> GetCreditCardEntry(LoadByIDRequest data)
        {
            CheckLoadResponse? response = null;
            try
            {
                response = await creditCardSrv.GetCreditCardEntry(data);
                if (response != null && response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will create creditcard entry based on CheckEntryRequest
        /// </summary>
        /// <param name="data">It will represents CheckEntryRequest information </param>
        /// <returns>It returns ID,Statusmessage and statuscode</returns>
        [Route("CreditCard/create")]
        [HttpPost]
        public async Task<IActionResult> CreateCreditCardEntry(CheckEntryRequest data)
        {
            JournalResponse? response = null;
            try
            {
                response = await creditCardSrv.CreateCreditCardEntry(data);
                if (response != null && response.StatusCode == 200 && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will update creditcard entry based on CheckEntryRequest details.
        /// </summary>
        /// <param name="data">It will represents CheckEntryRequest information </param>
        /// <returns>It returns ID,Statusmessage and statuscode</returns>
        [Route("CreditCard/update")]
        [HttpPut]
        public async Task<IActionResult> UpdateCreditCardEntry(CheckEntryRequest data)
        {
            JournalResponse? response = null;
            try
            {
                response = await creditCardSrv.UpdateCreditCardEntry(data);
                if (response != null && response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }
        [Route("CreditCard/delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteCreditCardEntry(LoadByIDRequest loadByID)
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
