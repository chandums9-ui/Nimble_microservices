using Azure;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using Payable.App.Contracts;
using Payable.App.Services;
using Payable.Domain.DTO.Req;
using Payable.Domain.DTO.Resp;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using static Payable.Domain.DTO.Resp.PayableResponseDTO;

namespace Payable.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class PayableLaunchController : ControllerBase
    {
        #region Fields

        private readonly IPayableLaunch _payableLaunch;
        private readonly IBillEntryService billEntrySrv;

        #endregion

        #region CTOR

        public PayableLaunchController(IPayableLaunch payableLaunch, IBillEntryService billEntryService)
        {
            this._payableLaunch = payableLaunch;
            this.billEntrySrv = billEntryService;

        }


        #endregion

        private string GetClientID()
        {
            return (string)HttpContext.Items["ClientId"];
        }
        private string GetUserID()
        {
            return (string)HttpContext.Items["UserId"];
        }
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        #region Methods
        [Route("LoadCheckPrintingDetails")]
        [HttpPost]
        public async Task<IActionResult> LoadCheckPrintingDetails(CheckPrintingReq req)
        {
            var response = await _payableLaunch.LoadCheckPrintingDetails(req, GetUserID());
            return Ok(response);
        }
        [Route("DiscountConfig/Get")]
        [HttpGet]
        public async Task<IActionResult> GetDiscountConfig([FromQuery] string CorpID)
        {
            var response = await _payableLaunch.GetDiscountCOnfiguration(CorpID);
            return Ok(response);
        }
        [Route("DiscountConfig/Save")]
        [HttpPost]
        public async Task<IActionResult> SaveDiscountConfig(DiscountAccountData Req)
        {
            var response = await _payableLaunch.SaveDiscountConfiguration(Req);
            return Ok(response);
        }

        [Route("autobillpaypref")]
        [HttpPost]
        public async Task<IActionResult> GetAutoPayPreference(string CorpID)
        {
            var response = await _payableLaunch.GetAutoBillPaymentPreference(CorpID);
            return Ok(response);
        }
        [Route("viewtransactionrangepref")]
        [HttpGet]
        public async Task<IActionResult> GetViewTransactionRange()
        {
            var response = await _payableLaunch.GetViewTransactionRange(GetUserID());
            return Ok(response);
        }
        [Route("AutoAmountPreference")]
        [HttpGet]
        public async Task<IActionResult> GetAutoAmountDistributionPrefernce()
        {
            var response = await _payableLaunch.GetAutoAmountDistributionPreference(GetUserID());
            return Ok(response);
        }
        [Route("PreviewPreference")]
        [HttpGet]
        public async Task<IActionResult> GetPreviewPreference()
        {
            var response = await _payableLaunch.GetPreviewPreference(GetUserID());
            return Ok(response);
        }
        [Route("VendorDefaultPayMethod")]
        [HttpGet]
        public async Task<IActionResult> GetVednorDefaultPaymentMethod(string VednorID)
        {
            MiscInfoSaveResponse response = await _payableLaunch.GetVendorDefaultPaymethod(VednorID);
            return Ok(response);
        }
        [Route("CheckPref/VednorPref")]
        [HttpPost]
        public async Task<IActionResult> GetVendorCheckPreference(List<string> req)
        {
            var response = await _payableLaunch.GetVendorCheckPreference(req,GetClientID());
            return Ok(response);
        }
        [Route("CheckPref/UserPref")]
        [HttpGet]
        public async Task<IActionResult> GetToBePrintedUserPref()
        {
            var response = await _payableLaunch.GetToBePrintedUserPreference(GetUserID(),GetClientID());
            return Ok(response);
        }
        [Route("CorporationLockCheck")]
        [HttpPost]
        public async Task<IActionResult> GetCorporationLockingStatusForBills(CorporationLockReq req)
        {
            var response = await _payableLaunch.GetCorporationLockingStatusForBills(req);
            return Ok(response);
        }

        [Route("GetVendorContracts")]
        [HttpPost]
        public async Task<IActionResult> GetVendorContracts(GetVendorsContractRequest req)
        {
            var response= await _payableLaunch.GetVendorContracts(req);
            return Ok(response);
        }

        [Route("GetFederalDetails")]
        [HttpGet]
        public async Task<IActionResult> GetFederalDetails(string CorpID)
        {
            try
            {
                var result = await _payableLaunch.GetFederalDetails(CorpID, GetUserID());
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("GetSSNDetails")]
        [HttpGet]
        public async Task<IActionResult> GetSSNDetails(string CorpID)
        {
            try
            {
                var result = await _payableLaunch.GetSSNDetails(CorpID, GetUserID());
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("GetMobileNumDetails")]
        [HttpGet]
        public async Task<IActionResult> GetMobileNumDetails(string CorpID)
        {
            try
            {
                var result = await _payableLaunch.GetMobileNumDetails(CorpID, GetUserID());
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("GetVendorNames")]
        [HttpPost]
        public async Task<IActionResult> GetVendorsByUserOrCorp(GetVendorNamesByUserOrCorpRequest Req)
        {
            try
            {
                var result = await _payableLaunch.GetVendorsByUserOrCorp(Req, GetUserID());
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("PayMethodsList")]
        [HttpPost]
        public async Task<IActionResult> LoadPaymethodsList(LoadPayMethodsReq Req)
        {
            try
            {
                LoadPaymethodsResponse result = await _payableLaunch.LoadPayMethodsList( Req,GetClientID());
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("GetCreditDaysOrPaymentMethods")]
        [HttpPost]
        public async Task<IActionResult> PaymentsOrCreditDays(LoadPayMethodsReq req)
        {
            try
            {
                string clientID = HttpContext.Items["ClientId"]?.ToString();
                GetPayMethodsOrCreditDays result = new GetPayMethodsOrCreditDays();

                // Load Credit Days
                using (IBillEntryService bs = billEntrySrv)
                {
                    result.CreditDays = await bs.GetCreditDaysList(clientID);
                }

                // Load Payment Methods based on the flag
                if (req.isPaymentMethodsReq)
                {
                    result.Paymethods = await _payableLaunch.LoadPayMethodsList(req, GetClientID());
                }

                // Determine if we have any valid data
                bool hasCreditDays = result.CreditDays?.ListInfo?.Any() == true;
                bool hasPayMethods = result.Paymethods?.PayMethods?.Any() == true;

                if (hasCreditDays || hasPayMethods)
                {
                    result.StatusCode = StatusCodes.Status200OK;
                    result.Status = Constants.MSG_DATA_LOAD_SUC;
                    return Ok(result);
                }

                result.StatusCode = StatusCodes.Status404NotFound;
                result.Status = Constants.MSG_FAILED;
                return NotFound(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
        }

        /// <summary>
        /// Retrieves the list of available credit days for the current client.
        /// </summary>
        /// <returns>Returns the list of credit days available for the client.</returns>
        /// <remarks>
        /// This endpoint fetches the credit days configuration for the client, which could be used to determine the payment terms or due dates for transactions.
        /// </remarks>
        /// <response code="200">The credit days list for the client was successfully retrieved.</response>
        /// <response code="404">No credit days list found for the client.</response>
        /// <response code="500">An internal server error occurred while retrieving the credit days list.</response>
        [Route("GetCreditDaysList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCreditDaysList()
        {
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                using (IBillEntryService bs = billEntrySrv)
                {
                    GenericIDNameListDTO response = await bs.GetCreditDaysList(clientID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("PayableLaunch/Cards")]
        [HttpGet]
        public async Task<IActionResult> GetCardsData([FromQuery] string corpId)
        {
            try
            {
                var result = await _payableLaunch.GetCardsData(GetUserID(), corpId,GetClientID(),getClientName());
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }


        [Route("PayableLaunch/SummeryTable")]
        [HttpPost]
        public async Task<IActionResult> GetSummeryTableData(VendorSummaryGridRequest req)
        {

            try
            {
                req.ClientID = GetClientID();
                req.ClientName = getClientName();
                req.UserID = GetUserID();
                var response = await _payableLaunch.GetLaunchSummeryData(req);

                return Ok(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("PayableLaunch/ChatData")]
        [HttpPost]
        public async Task<IActionResult> GetChatURL(GetChatDataRequest Req)
        {
            try
            {
                var result = await _payableLaunch.GetChatGroupData(Req, GetUserID());
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("PayableLaunch/ChatData/UnreadMsgCount")]
        [HttpGet]
        public async Task<IActionResult> GetUnreadMessagesCount()
        {
            try
            {
                var result = await _payableLaunch.GetUnreadMessagesCount(GetUserID());
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("PayableLaunch/OcrTransactionsCount")]
        [HttpGet]
        public async Task<IActionResult> GetOcrTransactionsCount([FromQuery] string CorpId, string clientURL)
        {
            try
            {
                var result = await _payableLaunch.GetOcrTransactionsCount(CorpId, clientURL);
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("PayableLaunch/DirectDepositTable")]
        [HttpPost]

        public async Task<IActionResult> GetDirectDepositTable(DirectDepositReqDto dtocontent)
        {
            try
            {


                var response = await _payableLaunch.GetDirectDepositGrid(dtocontent, GetUserID(), GetClientID());
                return Ok(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }

        }


        [Route("PayableLaunch/DirectDeposit/Export")]
        [HttpPost]

        public async Task<IActionResult> SaveDirectDepositExportBillPayments(DirectDepositSaveRequest req)
        {
            try
            {
                await _payableLaunch.SaveDirectDepositExportBillPaymentsInfo(req, GetUserID());
                return Ok();
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }


        [Route("PayableLaunch/DirectDeposit/Upload")]
        [HttpPost]

        public async Task<IActionResult> SaveDirectDepositUploadBillPayments(DirectDepositSaveRequest req)
        {
            try
            {
                await _payableLaunch.SaveDirectDepositUploadBillPaymentsInfo(req, GetUserID());
                return Ok();
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("PayableLaunch/DirectDeposit/GetDDCorpDetails")]
        [HttpPost]

        public async Task<IActionResult> DDExportSaves(LoadByCorporationIDAccountIDParams args)
        {
            try
            {
                var result = await _payableLaunch.DDExportSave(args);
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }


        [Route("PayableLaunch/DirectDeposit/GetDDVendorDetails")]
        [HttpPost]

        public async Task<IActionResult> DDVendorDetails(string vendorId)
        {
            try
            {
                var result = await _payableLaunch.DDVendorDetails(vendorId);
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }


        [Route("PayableLaunch/DirectDeposit/DDGetCreationNum")]
        [HttpPost]
        private async Task<IActionResult> DDGetCreationNumber(LoadByCorporationIDAccountIDParams args)
        {
            try
            {
                var result = await _payableLaunch.DDGetCreationNumber(args);
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("PayableLaunch/CorpName")]
        [HttpGet]
        public async Task<IActionResult> GetCorporatioName(string CorpId)
        {
            try
            {
                string result = await _payableLaunch.GetCorpName(CorpId, GetUserID());
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("ZipFileData")]
        [HttpGet]
        public async Task<IActionResult> GetZipFileInfo(string ID)
        {
            try
            {
                GetZipFileInfoResponse result = await _payableLaunch.GetZipFileDetials(ID);
                return Ok(result);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        #endregion
    }
}
