using Azure;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Mvc;
using Payable.App.Contracts;
using Payable.App.Service;
using Payable.App.Services;
using Payable.Domain.DTO.Model;
using Payable.Domain.DTO.Req;
using Payable.Domain.DTO.Resp;
using System.Net;
using System.Security.Cryptography;
using static Payable.Domain.DTO.Model.EpaymentModels;

namespace Payable.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class BillPaymentController : ControllerBase
    {
        #region Fields
        private readonly ICommonService commonSrv;
        private readonly IBillPaymentService billPaymentSrv;

        #endregion

        #region Ctor
        public BillPaymentController(ICommonService coreProperty, IBillPaymentService billPaymentSrv, IBillAndBillPaymentLinkService billAndBillPaymentLinkService)
        {
            this.commonSrv = coreProperty;
            this.billPaymentSrv = billPaymentSrv;
        }

        #endregion

        #region Private Methods
        private string getUserID()
        {
            return (string)HttpContext.Items["UserId"];
        }
        private string getClientID()
        {
            return (string)HttpContext.Items["ClientId"];
        }
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        #endregion

        #region BillPayment

        /// <summary>
        /// Updates or voids a bill payment based on the provided request details.
        /// </summary>
        /// <param name="req">The bill payment details used to update or void the payment.</param>
        /// <returns>Returns an IActionResult containing the result of the operation.</returns>
        /// <remarks>
        /// This endpoint is used to update or void a payment transaction.
        /// </remarks>
        /// <response code="200">Successfully updated or voided the bill payment.</response>
        /// <response code="404">Payment record not found.</response>
        /// <response code="500">Internal server error while processing the request.</response>
        [Route("updatevoidpayment")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatevoidPayment(BillPaymentDetails req)
        {
            var response = await billPaymentSrv.UpdateVoidPayment(req, getUserID());
            return Ok(response);
        }

        /// <summary>
        /// Schedules bill payments based on predefined or pending payment data.
        /// </summary>
        /// <returns>Returns an IActionResult containing the result of the scheduled payment operation.</returns>
        /// <remarks>
        /// This endpoint triggers the scheduling of bill payments. It does not take any input parameters.
        /// </remarks>
        /// <response code="200">Bill payments scheduled successfully.</response>
        /// <response code="404">No bill payments found to schedule.</response>
        /// <response code="500">Internal server error while scheduling bill payments.</response>
        [Route("ScheduleBillPay")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ScheduleBillPayments()
        {
            var response = await billPaymentSrv.PostScheduleBillPayments(getClientName());
            return Ok(response);
        }

        /// <summary>
        /// Loads the bill payment data into a view grid based on the provided request parameters.
        /// </summary>
        /// <param name="Req">The request object containing filters and parameters for loading bill payments.</param>
        /// <returns>Returns a response containing the bill payments to be displayed in a grid view.</returns>
        /// <remarks>
        /// This endpoint retrieves a list of bill payments formatted for display in a grid, based on the user's input criteria.
        /// </remarks>
        /// <response code="200">Successfully retrieved the bill payment data.</response>
        /// <response code="404">No bill payment data found matching the criteria.</response>
        /// <response code="500">Internal server error while retrieving the data.</response>
        [Route("BillPay/viewgrid")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillPaysResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBillPayViewGrid(LoadBillPaysRequest Req)
        {
            LoadBillPaysResponse response = await billPaymentSrv.LoadBillPayViewGrid(Req, getUserID(), getClientName());
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }


        /// <summary>
        /// Retrieves the last used e-payment account based on the provided request details.
        /// </summary>
        /// <param name="Req">The request object containing parameters to identify the user's last used e-payment account.</param>
        /// <returns>Returns the last used e-payment account information, if available.</returns>
        /// <remarks>
        /// This endpoint is used to fetch the most recently used account for electronic payments (e-pay), helping streamline future transactions.
        /// </remarks>
        /// <response code="200">Successfully retrieved the last used e-payment account.</response>
        /// <response code="404">No e-payment account found.</response>
        /// <response code="500">Internal server error occurred while retrieving the account.</response>
        [Route("EPayLoadLastUsedAccount")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EPayLoadLastUsedAccount(LoadEPayLastAccountRequest Req)
        {
            try
            {
                var response = await billPaymentSrv.EPayLoadLastUsedAccount(Req);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Retrieves the default payment account for a given corporation.
        /// </summary>
        /// <param name="CorpID">The unique identifier of the corporation whose default payment account is to be retrieved.</param>
        /// <returns>Returns the default payment account details associated with the specified corporation.</returns>
        /// <remarks>
        /// This endpoint fetches the default account used for processing payments for a corporation.
        /// </remarks>
        /// <response code="200">Successfully retrieved the corporation's default payment account.</response>
        /// <response code="404">No default payment account found for the specified corporation.</response>
        /// <response code="500">Internal server error occurred while retrieving the account information.</response>
        [Route("LoadCorporationDefaultPaymentAccount")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadCorporationDefaultPaymentAccount(string CorpID)
        {
            try
            {
                LoadDefaultAccountResponse response = await billPaymentSrv.LoadCorporationDefaultPaymentAccount(CorpID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Retrieves the default bank account for a specified vendor.
        /// </summary>
        /// <param name="VendorID">The unique identifier of the vendor whose default bank account is to be retrieved.</param>
        /// <returns>Returns the default bank account details associated with the given vendor.</returns>
        /// <remarks>
        /// This endpoint is used to fetch the vendor's default bank account, typically used for payment disbursements.
        /// </remarks>
        /// <response code="200">Successfully retrieved the vendor's default bank account.</response>
        /// <response code="404">No default bank account found for the specified vendor.</response>
        /// <response code="500">Internal server error occurred while retrieving the account information.</response>
        [Route("LoadVendorDefaultBankAccount")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadVendorDefaultBankAccount(string VendorID)
        {
            try
            {
                var response = await billPaymentSrv.LoadVendorDefaultBankAccount(VendorID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the last used account information for a bill payment based on the provided request data.
        /// </summary>
        /// <param name="Req">The request object containing parameters to identify the relevant user or context.</param>
        /// <returns>Returns the last used account details for bill payment, if available.</returns>
        /// <remarks>
        /// This endpoint is used to fetch the most recently used account for bill payments, which can help in pre-filling or suggesting accounts in the UI.
        /// </remarks>
        /// <response code="200">Successfully retrieved the last used account.</response>
        /// <response code="404">No last used account found.</response>
        /// <response code="500">Internal server error occurred while processing the request.</response>
        [Route("LoadLastUsedAccount")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadLastUsedAccount(LoadBillPayLastAccountRequest Req)
        {
            try
            {
                var response = await billPaymentSrv.LoadLastUsedAccount(Req);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the current payment balance and generates a new check number for a given account and payment method.
        /// </summary>
        /// <param name="AccountID">The unique identifier of the account for which the balance and check number are requested.</param>
        /// <param name="PaymentMethodID">The identifier of the selected payment method.</param>
        /// <param name="isBalanceRequired">Flag indicating whether the account balance should be retrieved.</param>
        /// <param name="checkNumber">An optional existing check number to be used or validated.</param>
        /// <returns>Returns the current payment balance and a new check number, if applicable.</returns>
        /// <remarks>
        /// This endpoint is used to get the latest balance for an account and/or generate a new check number based on the payment method.
        /// </remarks>
        /// <response code="200">Successfully retrieved the balance and/or generated a new check number.</response>
        /// <response code="404">No data found for the specified account or payment method.</response>
        /// <response code="500">Internal server error occurred while processing the request.</response>
        [Route("LoadPayBalanceAndNewCheckNumber")]
        [HttpGet]
        [ProducesResponseType(typeof(BillPaymentBalanceAndNewCheckNumber), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadPayBalanceAndNewCheckNumber(string AccountID, string PaymentMethodID, bool isBalanceRequired, string checkNumber = null)
        {
            try
            {
                BillPaymentBalanceAndNewCheckNumber response = await billPaymentSrv.LoadPayBalanceAndNewCheckNumber(AccountID, PaymentMethodID, getClientID(), isBalanceRequired, checkNumber);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the total outstanding amount and debit memo amount for a specific corporation and vendor.
        /// </summary>
        /// <param name="CorpID">The unique identifier of the corporation.</param>
        /// <param name="VenID">The unique identifier of the vendor.</param>
        /// <returns>Returns the total outstanding and debit memo amounts for the specified corporation and vendor.</returns>
        /// <remarks>
        /// This endpoint is used to get financial summary details including the outstanding balance and any associated debit memos between a corporation and a vendor.
        /// </remarks>
        /// <response code="200">Successfully retrieved the outstanding and debit memo amounts.</response>
        /// <response code="404">No financial data found for the specified corporation and vendor.</response>
        /// <response code="500">Internal server error occurred while retrieving the data.</response>
        [Route("GetTotalOustandingandDebitMemoAmount")]
        [HttpGet]
        [ProducesResponseType(typeof(TotalOutstandingAndDebitMemoAmount), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalOustandingandDebitMemoAmount(string CorpID, string VenID)
        {
            try
            {
                TotalOutstandingAndDebitMemoAmount response = await billPaymentSrv.TotalOutstandingAndDebitMemoAmount(CorpID, VenID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Loads a list of bill payments based on the provided request criteria.
        /// </summary>
        /// <param name="Request">The request object containing filters such as date range, division, and client context.</param>
        /// <returns>Returns a list of bill payment records grouped by division.</returns>
        /// <remarks>
        /// This endpoint retrieves a filtered list of bill payments for a client. The client ID is automatically assigned based on the current user context.
        /// </remarks>
        /// <response code="200">Successfully retrieved the list of bill payments.</response>
        /// <response code="404">No bill payment records found matching the criteria.</response>
        /// <response code="500">Internal server error occurred while retrieving the bill payments.</response>
        [Route("LoadBillPaymentsList")]
        [HttpPost]
        [ProducesResponseType(typeof(List<BillPaymentDivisions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBillPaymentsList(LoadBillPaymentsListRequest Request)
        {
            try
            {
                Request.ClientId = new PFAID(getClientID()).UID;
                List<BillPaymentDivisions> response = await billPaymentSrv.LoadBillPaymentsList(Request, getUserID());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves bill payment details grouped by PC (payment category) for a specified journal entry.
        /// </summary>
        /// <param name="JEID">The unique identifier of the journal entry for which the bill payment details are requested.</param>
        /// <returns>Returns a list of bill payment divisions grouped by payment category for the specified journal entry.</returns>
        /// <remarks>
        /// This endpoint is used to retrieve bill payments associated with a specific journal entry, categorized by payment category (PC).
        /// </remarks>
        /// <response code="200">Successfully retrieved the bill payment details grouped by payment category.</response>
        /// <response code="404">No bill payment records found for the specified journal entry.</response>
        /// <response code="500">Internal server error occurred while retrieving the bill payment data.</response>
        [Route("LoadBillPaymentPCWise")]
        [HttpGet]
        [ProducesResponseType(typeof(List<PCDvisions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBillPaymentPCWise(string JEID)
        {
            try
            {
                List<PCDvisions> response = await billPaymentSrv.LoadBillPaymentPCWise(JEID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Retrieves the details of a specific bill payment for a given journal entry.
        /// </summary>
        /// <param name="JEID">The unique identifier of the journal entry for which the bill payment details are requested.</param>
        /// <returns>Returns the details of the bill payment associated with the specified journal entry.</returns>
        /// <remarks>
        /// This endpoint is used to retrieve the detailed information about a bill payment based on the journal entry ID. It provides payment details like amount, vendor, etc.
        /// </remarks>
        /// <response code="200">Successfully retrieved the bill payment details.</response>
        /// <response code="404">No bill payment record found for the specified journal entry.</response>
        /// <response code="500">Internal server error occurred while retrieving the bill payment details.</response>
        [Route("LoadBillPayment")]
        [HttpGet]
        [ProducesResponseType(typeof(BillPaymentDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBillPayment(string JEID)
        {
            try
            {
                BillPaymentDetails response = await billPaymentSrv.LoadBillPayment(JEID, getUserID(), getClientID());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Retrieves user access details for a specific bill payment based on the journal entry and entry status.
        /// </summary>
        /// <param name="JEID">The unique identifier of the journal entry for which the user access details are requested.</param>
        /// <param name="EntryStatus">The status of the entry that determines what kind of access the user has to the bill payment.</param>
        /// <returns>Returns a boolean value indicating whether the user has access to the specified bill payment, or null if no access details are available.</returns>
        /// <remarks>
        /// This endpoint checks whether the current user has access to modify or view a bill payment based on the journal entry ID and entry status.
        /// </remarks>
        /// <response code="200">Successfully retrieved the user access details for the bill payment.</response>
        /// <response code="404">No access details found for the specified journal entry and entry status.</response>
        /// <response code="500">Internal server error occurred while retrieving the user access details.</response>
        [Route("GetBillPayUserAccess")]
        [HttpGet]
        [ProducesResponseType(typeof(bool?), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillPayUserAccess(string JEID, short EntryStatus)
        {
            try
            {
                bool? response = await billPaymentSrv.GetBillPayUserAcessDetails(JEID, getUserID(), EntryStatus);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Retrieves bill payment details from the backend for a specified journal entry.
        /// </summary>
        /// <param name="JEID">The unique identifier of the journal entry for which the bill payment details are requested.</param>
        /// <returns>Returns the details of the bill payment associated with the specified journal entry from the backend system.</returns>
        /// <remarks>
        /// This endpoint is used to retrieve detailed bill payment information from the backend based on the provided journal entry ID.
        /// </remarks>
        /// <response code="200">Successfully retrieved the bill payment details from the backend.</response>
        /// <response code="404">No bill payment record found for the specified journal entry.</response>
        /// <response code="500">Internal server error occurred while retrieving the bill payment details from the backend.</response>
        [Route("LoadBillPaymentfromBE")]
        [HttpGet]
        [ProducesResponseType(typeof(BillPaymentDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBillPaymentfromBE(string JEID)
        {
            try
            {
                BillPaymentDetails response = await billPaymentSrv.LoadBillPaymentfromBE(JEID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Gets the Bill Ids based on Bill Payment ID.
        /// </summary>
        /// <param name="billPaymentID">The unique identifier of the Bill Payment ID .</param>
        /// <returns>Returns the result of the bills linked in the Bill Payment.</returns>
        /// <remarks>
        /// This endpoint is retrieve bill ids linked in the bill payment
        /// </remarks>
        /// <response code="200">Successfully retrieved the bill ids for the requested requested bill payment id from the backend.</response>
        /// <response code="404">No bill ids found for the requested bill payment id.</response>
        /// <response code="500">Internal server error occurred while retrieving the bill payment details from the backend.</response>
        [Route("BillIDs")]
        [HttpGet]
        public async Task<IActionResult> GetBillIDByPaymentID(string billPaymentID)
        {
            var response = await billPaymentSrv.GetBillIdsByPaymentID(billPaymentID);
            return Ok(response);
        }

        /// <summary>
        /// Saves or updates a bill payment based on the provided request data.
        /// </summary>
        /// <param name="bpd">The request object containing the details of the bill payment to be saved or updated.</param>
        /// <returns>Returns the result of the bill payment save operation, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint processes a bill payment save or update request. It is typically used to either create a new payment or modify an existing one.
        /// </remarks>
        /// <response code="200">Successfully saved or updated the bill payment.</response>
        /// <response code="404">The bill payment record was not found or could not be processed.</response>
        /// <response code="500">Internal server error occurred while saving or updating the bill payment.</response>
        [Route("BillPaymentSave")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPaymentSave(BillPaymentSaveRequest bpd)
        {
            try
            {
                BillPaymentResponse response = await billPaymentSrv.BillPaymentUpdatedSave(bpd, getUserID(), getClientID(), getClientName());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Processes a request to save a bill payment based on the provided bill payment details.
        /// </summary>
        /// <param name="bpd">The bill payment details to be processed for saving or updating the record.</param>
        /// <returns>Returns the result of the bill payment save request process, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint handles the request to save or update a bill payment. It processes the payment details and returns a response with the status of the operation.
        /// </remarks>
        /// <response code="200">Successfully processed the bill payment save request.</response>
        /// <response code="404">The bill payment record could not be found or processed.</response>
        /// <response code="500">Internal server error occurred while processing the bill payment save request.</response>
        [Route("BillPaymentSaveReqProcess")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentSaveReqProcessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPaymentSaveReqProcess(BillPaymentDetails bpd)
        {
            try
            {
                BillPaymentSaveReqProcessResponse response = await billPaymentSrv.BillPaymentSaveRequestProcess(bpd, getUserID(), getClientID());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Updates the details of an existing bill payment based on the provided bill payment information.
        /// </summary>
        /// <param name="bpd">The bill payment details to be updated.</param>
        /// <returns>Returns the result of the bill payment update operation, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint processes an update request for a specific bill payment. It modifies the existing payment record based on the provided details.
        /// </remarks>
        /// <response code="200">Successfully updated the bill payment.</response>
        /// <response code="404">The specified bill payment record could not be found or processed.</response>
        /// <response code="500">Internal server error occurred while updating the bill payment.</response>
        [Route("BillPaymentUpdate")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPaymentUpdate(BillPaymentDetails bpd)
        {
            try
            {
                BillPaymentResponse response = await billPaymentSrv.UpdateBillPayment(bpd, getUserID(), getClientID(), getClientName());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Deletes a bill payment record based on the provided journal entry ID.
        /// </summary>
        /// <param name="JID">The request object containing the journal entry ID to be deleted and a flag for validation.</param>
        /// <returns>Returns the result of the bill payment deletion operation, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint is used to delete a bill payment record. The journal entry ID is used to identify the record to be deleted.
        /// An optional validation flag can be provided to enforce additional checks during the deletion process.
        /// </remarks>
        /// <response code="200">Successfully deleted the bill payment record.</response>
        /// <response code="404">The specified bill payment record could not be found or deleted.</response>
        /// <response code="500">Internal server error occurred while deleting the bill payment record.</response>
        [Route("BillPaymentDelete")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPaymentDelete(LoadByIDRequest JID)
        {
            try
            {
                JournalResponse response = await billPaymentSrv.BillPaymentDelete(new PFAID(JID.ID).ToString(), getUserID(), JID.IsValidate);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Voids an existing bill payment based on the provided request details.
        /// </summary>
        /// <param name="req">The request object containing the details of the bill payment to be voided.</param>
        /// <returns>Returns the result of the bill payment void operation, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint processes a request to void an existing bill payment. The request should include necessary identifiers 
        /// and validation parameters for the bill payment to be voided.
        /// </remarks>
        /// <response code="200">Successfully voided the bill payment.</response>
        /// <response code="404">The specified bill payment record could not be found or voided.</response>
        /// <response code="500">Internal server error occurred while processing the void request for the bill payment.</response>
        [Route("BillPaymentVoid")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPaymentVoid(BillVoidRequest req)
        {
            try
            {
                JournalResponse response = await billPaymentSrv.BillPaymentVoid(req, getUserID(), getClientID());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("GetHoldBills")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHoldBills(List<string> jeIDs)
        {
            try
            {
                HoldBillResponse response = new HoldBillResponse();
                response = await billPaymentSrv.GetHoldBills(jeIDs);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Processes a bulk bill payment request based on the provided bulk payment details.
        /// </summary>
        /// <param name="bbp">The bulk bill payment request object containing the details of multiple bill payments to be processed.</param>
        /// <returns>Returns the result of the bulk bill payment operation, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint is used to process multiple bill payments in bulk. The request should include all necessary details for each payment 
        /// to be processed in a single operation.
        /// </remarks>
        /// <response code="200">Successfully processed the bulk bill payments.</response>
        /// <response code="404">The specified bulk payment request could not be found or processed.</response>
        /// <response code="500">Internal server error occurred while processing the bulk bill payment request.</response>
        [Route("BulkBillPayment")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkBillPayment(BulkBillPayment bbp)
        {
            try
            {
                BillPaymentResponse response = new BillPaymentResponse();
                response = await billPaymentSrv.BulkBillPayment(bbp, getUserID(), getClientID(), getClientName());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        
        /// <summary>
        /// Approves a bulk bill payment request based on the provided bulk payment approval details.
        /// </summary>
        /// <param name="req">The request object containing the details of the bulk bill payment approval.</param>
        /// <returns>Returns the result of the bulk bill payment approval operation, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint is used to approve a bulk bill payment request. It processes the provided request details and returns the 
        /// result of the approval operation.
        /// </remarks>
        /// <response code="200">Successfully approved the bulk bill payments.</response>
        /// <response code="404">The specified bulk payment approval request could not be found or processed.</response>
        /// <response code="500">Internal server error occurred while processing the bulk bill payment approval request.</response>
        [Route("Bulk/BillPaymentApprove")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkBillPaymentApprove(BulkBillPaymentApproveRequest req)
        {
            try
            {
                string reqIDBatch = Guid.NewGuid().ToString();
                BillPaymentResponse response = await billPaymentSrv.BulkBillPaymentApprove(req, getUserID(), getClientID(), reqIDBatch);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Approves a bill payment request based on the provided approval details.
        /// </summary>
        /// <param name="req">The request object containing the details of the bill payment approval.</param>
        /// <returns>Returns the result of the bill payment approval operation, including any relevant response data, such as the batch ID and bill payment information.</returns>
        /// <remarks>
        /// This endpoint is used to approve an individual bill payment. The request should contain the necessary approval details. 
        /// The response includes a batch ID and the list of approved bill payments.
        /// </remarks>
        /// <response code="200">Successfully approved the bill payment and returned the batch ID and bill payment details.</response>
        /// <response code="404">The specified bill payment could not be found or approved.</response>
        /// <response code="500">Internal server error occurred while processing the bill payment approval request.</response>
        [Route("BillPaymentApprove")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPaymentApprove(BillPaymentApproveRequest req)
        {
            try
            {
                BillPaymentResponse response = new BillPaymentResponse();

                string reqIDBatch = Guid.NewGuid().ToString();
                req.IsSave = true;
                BillPaymentInfoResp bpResp = await billPaymentSrv.BillPaymentApprove(req, getUserID(), getClientID(), req.StartingCheckNumber, reqIDBatch);

                if (bpResp != null)
                {
                    response.BatchID = bpResp.BatchID;
                    response.BillPaymentList.Add(bpResp);
                    response.Status = !string.IsNullOrEmpty(bpResp.Status) || bpResp.StatusCode == StatusCodes.Status404NotFound ? bpResp.Status : Constants.MSG_BBP_APP_SUCESS;
                    response.StatusCode = !string.IsNullOrEmpty(bpResp.Status) || bpResp.StatusCode == StatusCodes.Status404NotFound ? StatusCodes.Status206PartialContent : StatusCodes.Status200OK;

                    return Ok(response);
                }
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Rejects a bulk bill payment request based on the provided list of rejection details.
        /// </summary>
        /// <param name="req">A list of `BillBulkRejectRequest` objects containing the details of the bill payments to be rejected.</param>
        /// <returns>Returns the result of the bulk bill payment rejection operation, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint is used to reject multiple bill payment requests in bulk. The request should contain the necessary details 
        /// for each bill payment to be rejected. The response provides the status of the rejection process for each item in the list.
        /// </remarks>
        /// <response code="200">Successfully rejected the bulk bill payments.</response>
        /// <response code="404">The specified bill payment rejection request could not be found or processed.</response>
        /// <response code="500">Internal server error occurred while processing the bulk bill payment rejection request.</response>
        [Route("Bulk/BillPaymentReject")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkBillPaymentReject(List<BillBulkRejectRequest> req)
        {
            try
            {
                var response = await billPaymentSrv.BulkBillPaymentReject(req, getUserID());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Rejects an individual bill payment request based on the provided rejection details.
        /// </summary>
        /// <param name="req">The request object containing the details of the bill payment to be rejected.</param>
        /// <returns>Returns the result of the bill payment rejection operation, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint is used to reject a specific bill payment. The request should include the necessary rejection details 
        /// for the bill payment to be rejected.
        /// </remarks>
        /// <response code="200">Successfully rejected the bill payment.</response>
        /// <response code="404">The specified bill payment could not be found or rejected.</response>
        /// <response code="500">Internal server error occurred while processing the bill payment rejection request.</response>
        [Route("BillPaymentReject")]
        [HttpPost]
        [ProducesResponseType(typeof(StatusDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPaymentReject(BillPaymentRejectRequest req)
        {
            try
            {
                StatusDTO response = await billPaymentSrv.BillPaymentReject(req, getUserID());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Retrieves the bill payment preferences for the currently authenticated user.
        /// </summary>
        /// <returns>Returns the bill payment preferences for the user, including all relevant settings and configurations.</returns>
        /// <remarks>
        /// This endpoint fetches the user-specific bill payment preferences, such as default payment methods, notification settings, 
        /// or other configurations related to bill payments. It requires the user to be authenticated to retrieve their preferences.
        /// </remarks>
        /// <response code="200">Successfully retrieved the bill payment preferences.</response>
        /// <response code="500">Internal server error occurred while fetching the bill payment preferences.</response>
        [Route("GetBillPaymentPreferences")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillPaymentPreferences()
        {
            try
            {
                using (IBillPaymentService bs = billPaymentSrv)
                {
                    var response = await bs.GetBillPaymentPreferences(getUserID());
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// It Returns List of Users for group chat Based on Corporation
        /// </summary>
        /// <param name="corpId">represents the Corporation ID</param>
        /// <returns>It will display List Of users who have approval policy privileges on bill screen for particular corpId</returns>
        /// <remarks>
        /// This endpoint is used to fetch the list of users who are part of the bill payment group chat for a specific corporation. 
        /// It requires the corporation ID to retrieve the relevant users. The response contains the status code and user details.
        /// </remarks>
        /// <response code="200">Successfully retrieved the list of group chat users.</response>
        /// <response code="404">The specified group chat users could not be found for the given corporation.</response>
        /// <response code="500">Internal server error occurred while fetching the group chat users.</response>
        [Route("GroupChatUsers")]
        [HttpPost]
        [ProducesResponseType(typeof(GroupChatUsersListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillPaymentGroupChatUsersList(string corpId)
        {
            GroupChatUsersListResponse? response = null;
            try
            {
                response = await billPaymentSrv.BillPaymentGetGroupChatUsers(corpId);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }
        
        [Route("GetCheckPrePrintStatus")]
        [HttpGet]
        public async Task<IActionResult> GetPrePrintStatus(string GetPrePrintEnabled)
        {
            ModelBaseStatusBoolean? response = null;
            try
            {
                response = await billPaymentSrv.GetPrePrintEnabled(GetPrePrintEnabled);
                if (response != null && response.Status!=null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        /// <summary>
        /// Saves a quick bill payment based on the provided request data.
        /// </summary>
        /// <param name="bpd">The request object containing the details of the quick bill payment to be saved.</param>
        /// <returns>Returns the result of the quick bill payment save operation, including any relevant response data.</returns>
        /// <remarks>
        /// This endpoint processes a quick bill payment save request. 
        /// It is typically used for simplified or fast payment entry operations 
        /// where minimal payment details are required.
        /// </remarks>
        /// <response code="200">Successfully saved the quick bill payment.</response>
        /// <response code="404">The quick bill payment record was not found or could not be processed.</response>
        /// <response code="500">Internal server error occurred while saving the quick bill payment.</response>

        [Route("QuickBillPaymentSave")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> QuickBillPaymentSave(QuickBillPaymentDetails bpd)
        {
            try
            {
                BillPaymentResponse response = await billPaymentSrv.QuickBillPaymentSave(bpd, getUserID(), getClientID(), getClientName());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        #endregion
    }
}
