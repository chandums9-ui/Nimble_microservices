using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using DailySales.App.Contracts;
using DailySales.Domain.DTO.Req;
using DailySales.Domain.DTO.Resp;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace DailySales.API.Controllers
{
    [Route("v1/MerchantReconciliation")]
    [ApiController]
    [Authorize]
    public class MerchantReconciliationController : ControllerBase
    {
        #region Fields
        private readonly IMerchantReconciliationService MerchantReconciliation;
        private readonly ILoggerService logger;
        #endregion


        #region Ctor
        public MerchantReconciliationController(IMerchantReconciliationService _MerchantReconciliation, ILoggerService _logger)
        {
            this.MerchantReconciliation = _MerchantReconciliation;
            this.logger = _logger;
        }
        #endregion

        #region MerchantReconciliationApi'S

        /// <summary>
        /// Based on data request  get DailySale details
        /// </summary>
        /// <param name="data">Here data request Sending the  MechantDailySalesRequest object  to Get DailySale Details For a particular account to a corp</param>
        /// <returns> It will Return the Status , Status Codes and Sales Receipts </returns>

        [Route("MerchantReconciliationDailySales")]
        [HttpPost]
        public async Task<IActionResult> MerchantReconciliationDailySales(MechantDailySalesRequest data)
        {
            MechantDailySalesResponce Responce = null;
            if (data != null)
            {
                Responce = await MerchantReconciliation.MerchantReconciliationDailySales(data);
                return Ok(Responce);
            }
            else
            {
                return BadRequest(StatusCode(StatusCodes.Status400BadRequest));
            }
        }


        [Route("MerchantReconciliationDailySalesMultiCorp")]
        [HttpPost]
        public async Task<IActionResult> MerchantReconciliationDailySalesMultiCorp(MechantDailySalesMultiCorpRequest Request)
        {
            DailysaleReceiptsMultiCorpResponse Responce = null;
            if (Request != null)
            {
                Responce = await MerchantReconciliation.MerchantReconciliationDailySalesMulticorp(Request);
                return Ok(Responce);
            }
            else
            {
                return BadRequest(StatusCode(StatusCodes.Status400BadRequest));
            }
        }
        /// <summary>
        /// Based on data request  To Save Or Update the COA Preference
        /// </summary>
        /// <param name="data">Here data request Sending the  MerchanatReconCOAReq object  to Save Or Update the accounts for a Particular Corporation</param>
        /// <returns> It will Return the Status and  Status Codes With Id </returns>


        [Route("SaveOrUpdateCOAPreference")]
        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateCOAPreference(MerchanatReconCOAReq data)
        {
            MerchantReconCOAResp Responce = null;
            if (data != null)
            {
                Responce = await MerchantReconciliation.SaveOrUpdateCOAPreference(data);
                return Ok(Responce);
            }
            else
            {
                return BadRequest(StatusCode(StatusCodes.Status400BadRequest));
            }
        }

        /// <summary>
        /// Based on CorpId It Will Get COA Preference deatils. 
        /// </summary>
        /// <param name="CorpId">Here The CorpId  will send to get the details of  a corporation </param>
        /// <returns> It will Return the CorporationID ,PCID and Account details</returns>
        /// //ToDo: Here we need to consider PC if selected then get the coa preference by corpID and pcID
        [Route("GetCOAPreference")]
        [HttpPost]

        public async Task<IActionResult> GetCOAPreference(MerchanatReconCOAReq request)
        {


            MerchantCOApreferenceResponse? response = null;
            try
            {
                response = await MerchantReconciliation.GetCOAPreference(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }

        }

        /// <summary>
        /// Based on Data request It Will Save the MerchantReconciliation Statement  deatils in to database
        /// </summary>
        /// <param name="data">Here The AutoMapMerchantReconStmtReq data object   will send   the records to tables and added details of  a statement </param>
        /// <returns> I will Return the status code and status message </returns>


        [Route("AutoMapMerchantReconStmt")]
        [HttpPost]

        public async Task<IActionResult> SaveAutoMapMerchantReconStmt(AutoMapMerchantReconStmtReq data)
        {
            AutoMapMerchantReconStmtResp Responce = new AutoMapMerchantReconStmtResp();
            string userId = (string)HttpContext.Items["UserId"];
            if (data != null)
            {
                Responce = await MerchantReconciliation.SaveAutoMapMerchantReconStmt(data, userId);
                return Ok(Responce);
            }
            else
            {
                return BadRequest(StatusCode(StatusCodes.Status400BadRequest));
            }
        }

        [Route("AutMatchMerchantRecon")]
        [HttpPost]
        public async Task<IActionResult> AutoMatchMerchantReconStmt(AutoMapMerchantReconStmtReq data)
        {
            AutoMapMerchantReconStmtResp Responce = new AutoMapMerchantReconStmtResp();
            string userId = (string)HttpContext.Items["UserId"];
            if (data != null)
            {
                Responce = await MerchantReconciliation.AutoMatchMerchantRecon(data, userId);
                return Ok(Responce);
            }
            else
            {
                return BadRequest(StatusCode(StatusCodes.Status400BadRequest));
            }
        }



        /// <summary>
        /// Based on Data request It Will Save the MerchantReconciliation Statement  deatils in to database
        /// </summary>
        /// <param name="data">Here The ManualMapMerchantReconStmtReq data object   will send   the records to tables and added details of  a statement </param>
        /// <returns> I will Return the status code and status message </returns>

        [Route("ManualMerchantRecon")]
        [HttpPost]

        public async Task<IActionResult> SaveManualMapMerchantReconStmt(ManualMapMerchantReconStmtReq data)
        {
            AutoMapMerchantReconStmtResp Responce = new AutoMapMerchantReconStmtResp();
            string userId = (string)HttpContext.Items["UserId"];
            if (data != null)
            {
                // Responce = await MerchantReconciliation.SaveManualMapMerchantReconStmt(data, userId);
                return Ok(Responce);
            }
            else
            {
                return BadRequest(StatusCode(StatusCodes.Status400BadRequest));
            }
        }

        /// <summary>
        /// Based on Data request It Will Unreconcile  the MerchantReconciliation Statement  deatils in to database
        /// </summary>
        /// <param name="data">Here The MerchantUnReconReq data object will send Unreconcile the data in the database</param>
        /// <returns> I will Return the status code and status message </returns>


        [Route("MerchantUnReconciliation")]
        [HttpPost]
        public async Task<IActionResult> MerchantUnReconciliation(MerchantUnReconReq data)
        {
            AutoMapMerchantReconStmtResp Responce = new AutoMapMerchantReconStmtResp();
            if (data != null)
            {
                Responce = await MerchantReconciliation.MerchantUnReconciliation(data);
                return Ok(Responce);
            }
            else
            {
                return BadRequest(StatusCode(StatusCodes.Status400BadRequest));
            }
        }

        [Route("GetMultipleEnableStatus")]
        [HttpGet]
        public async Task<IActionResult> GetMultipleEnableStatus(string LineID)
        {
            GetMultipleEnableStatusResp Responce = new GetMultipleEnableStatusResp();
            if (LineID != null)
            {
                Responce = await MerchantReconciliation.GetMultipleEnableStatus(LineID);
                return Ok(Responce);
            }
            else
            {
                return BadRequest(StatusCode(StatusCodes.Status400BadRequest));
            }

        }


        [Route("GetDailyConfigCardTypes")]
        [HttpPost]

        public async Task<IActionResult> GetDailyConfigurationCardTypes()
        {

            CardTypeResponse? response = null;
            try
            {
                response = await MerchantReconciliation.GetCardTypes();
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }

        }

        [Route("GetDailyConfigAccounts")]
        [HttpPost]

        public async Task<IActionResult> GetDailyConfigurationAccounts(MechantDailySalesRequest request)
        {

            CardsConfiguredAccounts? response = null;
            try
            {
                response = await MerchantReconciliation.GetDailySaleConfiguredAccounts(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }


            finally { response = null; }

        }
        [Route("SavechargebackandFeeadjustEntrys")]
        [HttpPost]
        public async Task<IActionResult> SaveJournalEntryTransactions(SaveTransactionRequest request)
        {
            SaveTranactionsResponse response = null;
            string userId = (string)HttpContext.Items["UserId"];
            request.UserID = userId;
            try
            {

                response = await MerchantReconciliation.SaveJournalEntryTransaction(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        [Route("UndoImport")]
        [HttpPost]
        public async Task<IActionResult> UndoImportChanges(MerchantUnReconReq request)
        {
            AutoMapMerchantReconStmtResp response = null;
            string userId = (string)HttpContext.Items["UserId"];
            try
            {

                response = await MerchantReconciliation.UndoImport(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }



        [Route("AutoOrManualMatch")]
        [HttpPost]
        public async Task<IActionResult> AutoOrManualMatchTransactions(AutoMatchOrManulaMatchRequest request)
        {
            AutoMapMerchantReconStmtResp response = null;

            try
            {

                response = await MerchantReconciliation.AutoOrManualMapMerchantReconStmt(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }


        [Route("SaveImport")]
        [HttpPost]
        public async Task<IActionResult> SaveImportFileData(ImportReconStmtRequest request)
        {
            AutoMapMerchantReconStmtResp response = null;

            try
            {
                request.CreatedBy = (string)HttpContext.Items["UserId"];

                response = await MerchantReconciliation.GetSaveImportData(request);

                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        [Route("SplitTransaction")]
        [HttpPost]
        public async Task<IActionResult> SaveSplitTransaction(SplitTransactionRequest request)
        {
            SplitTransactionResponse response = null;

            try
            {
                request.UserId = (string)HttpContext.Items["UserId"];

                response = await MerchantReconciliation.SplitDailySaleTransaction(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        [Route("AutoOrManualMatchUpdated")]
        [HttpPost]
        public async Task<IActionResult> AutoOrManualMatchTransactionsUpdated(AutoMatchOrManulaMatchRequest request)
        {
            AutoMapMerchantReconStmtResp response = null;

            try
            {

                response = await MerchantReconciliation.UpdateTransactionAndJournalEntries(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        [Route("UndoValidationMessage")]
        [HttpPost]
        public async Task<IActionResult> UndoValidationcheck(UndoValidationRequest request)
        {
            UndoValidationResponse response = null;

            try
            {
                string UserID = (string)HttpContext.Items["UserId"];
                logger.LogInfo( $"UndoValidationcheck Request: {JsonConvert.SerializeObject(request)}");
                //response = await MerchantReconciliation.UndoValidation(request);
                logger.LogInfo( $"UndoValidationcheck Failed Response: {JsonConvert.SerializeObject(response)}");
                response = await MerchantReconciliation.UndoValidationLatest(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch (Exception ex)
            {
                logger.LogInfo(
                    $"UndoValidationcheck Exception: ERROR Message: {ex.Message}, StackTrace: {ex.StackTrace}, Request: {JsonConvert.SerializeObject(request)}"
                    );

                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { response = null; }

        }
        #endregion
    }
}
