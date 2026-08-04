using Azure;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Common.Infra.Publishing;
using CoreAccounting.Domain.DTO.Resp;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Mvc;
using Payable.App.Contracts;
using Payable.App.Service;
using Payable.App.Services;
using Payable.Domain.DTO.Model;
using Payable.Domain.DTO.Req;
using Payable.Domain.DTO.Resp;
using static Payable.Domain.DTO.Resp.PayableResponseDTO;

namespace Payable.API.Controllers
{

    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class BillEntryController : Controller
    {
        #region Fields
        private readonly ICommonService commonSrv;
        private readonly IBillEntryService billEntrySrv;
        private readonly IUnitOfWork uow;
        private readonly IPublishService publishService;
        //private readonly IFileService fileSrv;

        #endregion

        #region Ctor
        public BillEntryController(ICommonService coreProperty, IUnitOfWork _uow, IBillEntryService billEntryService, IPublishService publishService)
        {
            this.commonSrv = coreProperty;
            //this.fileSrv = fileService;
            this.billEntrySrv = billEntryService;
            this.uow = _uow;
            this.publishService = publishService;
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

        #region Bill Entry
        /// <summary>
        /// Retrieves the access details for a user based on the provided Bill Entry ID, Entry Status, and Screen Type.
        /// </summary>
        /// <param name="BillEntryID">The unique identifier of the bill entry.</param>
        /// <param name="EntryStatus">The current status of the entry (e.g., pending, approved).</param>
        /// <param name="ScreenType">The type of screen from which the access is being requested.</param>
        /// <returns>Returns an object containing the user access details for the given parameters.</returns>
        /// <remarks>
        /// This endpoint checks the access level of the current user for a specific bill entry and returns relevant permissions or restrictions.
        /// </remarks>
        /// <response code="200">Returns the user access details successfully.</response>
        /// <response code="404">No data found for the provided bill entry ID.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [Route("GetUserAccesForActions")]
        [HttpGet]
        [ProducesResponseType(typeof(IOVendorsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserAcessDetails([FromQuery] string BillEntryID, short EntryStatus, short ScreenType)
        {
            var response = await billEntrySrv.GetUserAcessDetails(BillEntryID, getUserID(), EntryStatus, ScreenType);
            return Ok(response);
        }

        /// <summary>
        /// Retrieve the assigned User names based on the provided Corporation ID.
        /// </summary>
        [Route("GetUserNamesByCorpID")]
        [HttpGet]
        [ProducesResponseType(typeof(IOVendorsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserNamesByCorpID([FromQuery] string CorpID)
        {
            List<CorpAssignedUserName> response = await billEntrySrv.GetUserNamesByCorpID(CorpID, getUserID());
            return Ok(response);
        }

        /// <summary>
        /// Retrieve the assigned User names based on the provided Corporation ID.
        /// </summary>
        [Route("GetTransactionCorpID")]
        [HttpGet]
        [ProducesResponseType(typeof(IOVendorsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionCorpID([FromQuery] string JEID,short type)
        {
            string response = await billEntrySrv.GetTransactionCorpID(JEID, type);
            return Ok(response);
        }



        /// <summary>
        /// Loads recurring bill entry data based on the provided report ID.
        /// </summary>
        /// <param name="RepID">The unique identifier for the recurring bill entry.</param>
        /// <returns>Returns the recurring bill data if found; otherwise, a not found response.</returns>
        /// <remarks>
        /// This endpoint is used to fetch the details of a recurring bill entry using the specified report ID.
        /// </remarks>
        /// <response code="200">Returns the recurring bill data successfully.</response>
        /// <response code="404">Recurring bill data not found for the given report ID.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [Route("Reccuring/Load")]
        [HttpGet]
        [ProducesResponseType(typeof(LoadBillReccuringData), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBillReccuring(string RepID)
        {
            LoadBillReccuringData response = await billEntrySrv.LoadBillEntryReccuring(RepID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else
                return NotFound(response);
        }

        /// <summary>
        /// Updates a recurring bill entry with the provided details.
        /// </summary>
        /// <param name="Req">An object containing the details of the bill entry to be updated.</param>
        /// <returns>Returns a response indicating the result of the update operation.</returns>
        /// <remarks>
        /// This endpoint updates the information of a recurring bill entry based on the provided request data, along with the current user's and client's context.
        /// </remarks>
        /// <response code="200">The recurring bill entry was updated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="500">An internal server error occurred while processing the update.</response>
        [Route("Reccuring/Update")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBillReccuring(BillEntryDetails Req)
        {
            JournalResponse response = await billEntrySrv.UpdateBillEntryReccuring(Req, getUserID(), getClientID(), getClientName());
            return Ok(response);
        }
        /// <summary>
        /// Checks whether the provided recurring name is unique.
        /// </summary>
        /// <param name="Req">The request object containing the name to be checked for uniqueness.</param>
        /// <returns>Returns a response indicating whether the name is unique or already exists.</returns>
        /// <remarks>
        /// This endpoint validates if a recurring bill name is unique in the system to prevent duplicates.
        /// </remarks>
        /// <response code="200">Returns the result of the uniqueness check.</response>
        /// <response code="400">Invalid request data provided.</response>
        /// <response code="500">An internal server error occurred while checking the name.</response>
        [Route("Reccuring/IsUniqeName")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUniqueNameData(UniqueNameCheckRequest Req)
        {
            JournalResponse response = await billEntrySrv.IsUniqueReccuringName(Req);
            return Ok(response);
        }
        /// <summary>
        /// Replaces an existing recurring bill entry with new details.
        /// </summary>
        /// <param name="Req">The request object containing details for replacing the recurring entry.</param>
        /// <returns>Returns a response indicating the outcome of the replacement operation.</returns>
        /// <remarks>
        /// This endpoint replaces an existing recurring entry based on the data provided in the request, typically used for correcting or updating recurring billing records.
        /// </remarks>
        /// <response code="200">Recurring entry replaced successfully.</response>
        /// <response code="400">Invalid or incomplete request data.</response>
        /// <response code="500">An internal server error occurred during the replacement process.</response>
        [Route("Reccuring/ReplaceReccuringEntry")]
        [HttpPost]
        [ProducesResponseType(typeof(ReplaceReccuringEntryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReplaceReccuringEntry(ReplaceReccuringEntryRequest Req)
        {
            ReplaceReccuringEntryResponse response = await billEntrySrv.ReplaceReccuringEntry(Req);
            return Ok(response);
        }
        /// <summary>
        /// Deletes a recurring bill entry based on the provided source ID.
        /// </summary>
        /// <param name="SourceID">The unique identifier of the recurring bill entry to be deleted.</param>
        /// <param name="IsSave">A boolean flag indicating whether to perform a soft delete (true) or a hard delete (false).</param>
        /// <returns>Returns a response indicating the result of the delete operation.</returns>
        /// <remarks>
        /// This endpoint deletes or deactivates a recurring bill entry depending on the value of <c>IsSave</c>.
        /// </remarks>
        /// <response code="200">The recurring bill entry was deleted successfully.</response>
        /// <response code="400">Invalid parameters provided.</response>
        /// <response code="500">An internal server error occurred during the delete operation.</response>
        [Route("Reccuring/Delete")]
        [HttpGet]
        [ProducesResponseType(typeof(JournalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBillReccuring(string SourceID, bool IsSave)
        {
            JournalResponse response = await billEntrySrv.DeleteBillEntryReccuring(SourceID, IsSave);
            return Ok(response);
        }

        /// <summary>
        /// Saves an Inward/Outward (IO) bill entry based on the provided request data.
        /// </summary>
        /// <param name="Request">The request object containing IO bill entry details.</param>
        /// <returns>Returns a response indicating the success or failure of the save operation.</returns>
        /// <remarks>
        /// This endpoint is used to save a new IO bill entry using the client and user context. If the save fails or the entry is not created, a not found response is returned.
        /// </remarks>
        /// <response code="200">The IO bill entry was saved successfully.</response>
        /// <response code="404">The IO bill entry could not be saved or was not created.</response>
        /// <response code="500">An internal server error occurred while saving the bill entry.</response>
        [Route("SaveIOBill")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveIOBill(BillEntryIORequest Request)
        {
            try
            {
                var response = await billEntrySrv.SaveIOBillEntry(Request, getClientID(), getUserID(), getClientName());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception e) { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// fetch data from the BillEntryAiinformationDetails by the BillInformationId
        /// </summary>
        /// <param name="BillInfoId">BillInfoId</param>
        /// <returns>Returns a response with List of BillEntryAIInformation </returns>
        /// <remarks>
        /// This endpoint is used to load a new bill entry AIinformation details used for purpose mapping
        /// </remarks>
        /// <response code="200">BillEntryAiinformationDetails Data Loaded successfully</response>
        /// <response code="404">BillEntryAiinformationDetails Data Loading failed or No BillInforId is sent</response>
        /// <response code="500">An internal server error occurred while loading the BillEntryAiinformationDetails .</response>

        [Route("LoadBillAiInfoDetails")]
        [HttpGet]
        [ProducesResponseType(typeof(BillEntryAIInfoDetailsList), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBillEntryAIInformationDetails(long BillInfoId)
        {
            try
            {
                var response = await billEntrySrv.LoadBillEntryAIInfoDetails(BillInfoId);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception e) { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Update unapproved transcations When user selects the prefernece to map purposes based on VendorAIPurposeDetails
        /// </summary>
        /// <param name="Req">contains vendorId and UpdatedPurposeIds list</param>
        /// <returns>Returns a response indicating the success or failure of the save operation.</returns>
        /// <remarks>
        /// This endpoint is used to update unapproved bills of the given vendorId if user chooses to save VendorAiPurposeDetails and for the Purpose Updating to unapproved bills if purpose account is updated
        /// </remarks>
        /// <response code="200"> Data saved successfully</response>
        /// <response code="404"> Data saving failed</response>
        /// <response code="500">An internal server error occurred while loading the BillEntryAiinformationDetails .</response>

        [Route("AIUnapprovedBillsUpdate")]
        [HttpPost]
        [ProducesResponseType(typeof(StatusDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUnapprovedBillsAI(UpdateUnapprovedBillsAIRequest Req)
        {
            try
            {
                var response = await billEntrySrv.UpdateUnapprovedBillsAI(Req , getUserID() , getClientID(),getClientName());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception e) { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("VendorUnapprovedCount")]
        [HttpGet]
        [ProducesResponseType(typeof(StatusDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVendorUnapprovedCount([FromQuery]string vendorId, [FromQuery] long CurrentBillInfoId)
        {
            try
            {
                var response = await billEntrySrv.GetVendorUnapprovedCount(vendorId, CurrentBillInfoId);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception e) { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// It Returns List of Bills Based on Corporation
        /// </summary>
        /// <param name="BillListReqData">BillListReqData represents BillListReq</param>
        /// <returns>It will display List Of Bills</returns>
        /// <remarks>
        /// This endpoint fetches bill entries from the system using various filter options like date range, status, and bill type.
        /// </remarks>
        /// <response code="200">The list of bill entries was retrieved successfully.</response>
        /// <response code="404">No bill entries were found matching the criteria.</response>
        /// <response code="500">An internal server error occurred while retrieving the bill entries.</response>
        [Route("Bills/List")]
        [HttpPost]
        [ProducesResponseType(typeof(JEListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillEntries([FromQuery] JournalSearchRequest BillListReqData)
        {
            JEListResponse? response = null;
            try
            {
                response = await billEntrySrv.GetBillEntryList(BillListReqData);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }


        /// <summary>
        /// It Returns BillEntry based BillId
        /// </summary>
        /// <param name="BillId">BillId represents LoadByIDRequest</param>
        /// <returns>It returns Bill Data</returns>
        /// <remarks>
        /// This endpoint fetches the detailed information of a single bill entry based on the provided identifier.
        /// </remarks>
        /// <response code="200">The bill entry was retrieved successfully.</response>
        /// <response code="404">No bill entry found for the provided ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the bill entry.</response>
        [Route("Bills/Load")]
        [HttpPost]
        [ProducesResponseType(typeof(BillEntryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillEntry(LoadByIDRequest BillID)
        {
            BillEntryResponse? response = null;
            try
            {
                response = await billEntrySrv.GetBillEntry(BillID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        /// <summary>
        /// Loads a grid view of bill entries based on the provided filter criteria.
        /// </summary>
        /// <param name="BillListReqData">The request object containing filter criteria for the bill entries.</param>
        /// <returns>Returns the grid view data of bill entries matching the filter criteria.</returns>
        /// <remarks>
        /// This endpoint is used to fetch bill entries and display them in a grid format. The data is filtered based on the criteria provided in the request.
        /// </remarks>
        /// <response code="200">The bill entries were retrieved and displayed successfully in the grid format.</response>
        /// <response code="404">No bill entries found for the provided filter criteria.</response>
        /// <response code="500">An internal server error occurred while loading the grid data.</response>
        [Route("Bills/ViewGrid")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadViewGridData(LoadBillsRequest BillListReqData)
        {
            LoadBillsResponse response = await billEntrySrv.LoadBillEntryViewGridData(BillListReqData,getUserID(),getClientName());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else
                return NotFound(response);
        }
        /// <summary>
        /// It deletes the bill based in BillId
        /// </summary>
        /// <param name="BillId">BillId represents LoadByIDReq</param>
        /// <returns>It Returns JournalResponse</returns>
        /// <remarks>
        /// This endpoint deletes a specific bill entry identified by the provided ID. If the bill is not found, a not found response is returned.
        /// </remarks>
        /// <response code="200">The bill entry was deleted successfully.</response>
        /// <response code="404">No bill entry found for the provided ID.</response>
        /// <response code="500">An internal server error occurred while attempting to delete the bill entry.</response>
        [Route("Bills/delete")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBill(LoadByIDRequest BillId)
        {
            try
            {
                var response = await billEntrySrv.DeleteBillEntry(BillId, getUserID());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Gets the list of vendors for the specified Corporation ID.<br/> 
        /// </summary>
        /// <param name="CorpID">The unique identifier of the corporation.</param>
        /// <returns>Returns a list of vendors associated with the given CorpID.</returns>
        /// <remarks>Gets the list of vendors for the specified Corporation ID.</remarks>
        /// <response code="200">Successfully retrieved the vendors list.</response>
        /// <response code="404">No vendors found for the given CorpID.</response>
        /// <response code="500">An error occurred while processing your request.</response>
        [Route("GetVendorsList")]
        [HttpGet]
        [ProducesResponseType(typeof(VendorListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVendorList(string CorpID)
        {
            try
            {
                var response = await billEntrySrv.GetVendorList(CorpID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request."); }
        }

        /// <summary>
        /// Retrieves vendor names based on the provided corporate ID and user ID.
        /// </summary>
        /// <param name="CorpID">The optional corporate ID used to filter vendor names. If not provided, vendors for the current user are retrieved.</param>
        /// <returns>Returns a list of vendor names associated with the specified corporate ID and user ID.</returns>
        /// <remarks>
        /// This endpoint fetches a list of vendor names, either filtered by the provided corporate ID or for the current user if no ID is specified.
        /// </remarks>
        /// <response code="200">The vendor names were retrieved successfully.</response>
        /// <response code="404">No vendor names found for the given corporate ID and user ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the vendor names.</response>
        [Route("GetVendorsNamesByCorpIdUserId")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVendorsNamesByCorpIdUserId(string CorpID = null)
        {
            try
            {
                var response = await billEntrySrv.GetVendorNames(CorpID, getUserID());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves a list of contracts associated with a specific vendor based on the vendor ID.
        /// </summary>
        /// <param name="VenID">The unique identifier of the vendor for whom the contracts are being retrieved.</param>
        /// <returns>Returns a list of contracts associated with the specified vendor ID.</returns>
        /// <remarks>
        /// This endpoint fetches the contracts related to a particular vendor, including contract details and terms.
        /// </remarks>
        /// <response code="200">The list of contracts was retrieved successfully.</response>
        /// <response code="404">No contracts found for the provided vendor ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the contracts list.</response>
        [Route("GetContractsList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContractList(string VenID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.GetContractList(VenID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the details of a specific contract based on the provided contract ID.
        /// </summary>
        /// <param name="ContractID">The unique identifier of the contract whose details are to be retrieved.</param>
        /// <returns>Returns the contract details if found, otherwise a not found response.</returns>
        /// <remarks>
        /// This endpoint fetches the detailed information of a contract, such as terms, conditions, and associated vendor, based on the contract's ID.
        /// </remarks>
        /// <response code="200">The contract details were retrieved successfully.</response>
        /// <response code="404">No contract details found for the provided contract ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the contract details.</response>
        [Route("LoadContractDetails")]
        [HttpGet]
        [ProducesResponseType(typeof(ContractDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContractDetails(string ContractID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    ContractDetailsResponse response = await bs.GetContractDetails(ContractID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the opening balance of a specific vendor based on the provided vendor ID.
        /// </summary>
        /// <param name="VenID">The unique identifier of the vendor whose opening balance is to be retrieved.</param>
        /// <returns>Returns the opening balance of the vendor if found, otherwise a not found response.</returns>
        /// <remarks>
        /// This endpoint fetches the opening balance of a vendor, which represents the balance at the start of a specific period or transaction.
        /// </remarks>
        /// <response code="200">The vendor's opening balance was retrieved successfully.</response>
        /// <response code="404">No opening balance found for the provided vendor ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the vendor's opening balance.</response>
        [Route("GetVendorOpeningBalance")]
        [HttpGet]
        [ProducesResponseType(typeof(VendorBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVendorOpeningBalance(string VenID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    VendorBalanceResponse response = await bs.GetVendorOpeningBalance(VenID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the details of a specific vendor based on the provided vendor ID.
        /// </summary>
        /// <param name="VenID">The unique identifier of the vendor whose details are to be retrieved.</param>
        /// <returns>Returns the vendor details if found, otherwise a not found response.</returns>
        /// <remarks>
        /// This endpoint fetches detailed information about a vendor, such as name, contact information, and other relevant data, using the vendor's ID.
        /// </remarks>
        /// <response code="200">The vendor details were retrieved successfully.</response>
        /// <response code="404">No vendor details found for the provided vendor ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the vendor details.</response>
        [Route("LoadVendorDetails")]
        [HttpGet]
        [ProducesResponseType(typeof(VendorDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVendorDetails(string VenID,string ContractID)
        {
            try
            {
                VendorDetailsResponse response = await billEntrySrv.GetVendorDetails(VenID, ContractID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Checks if the bill date functionality is enabled for the current client.
        /// </summary>
        /// <returns>Returns a response indicating whether the bill date feature is enabled for the client.</returns>
        /// <remarks>
        /// This endpoint checks the configuration or status to determine whether the client is allowed to set or modify bill dates.
        /// </remarks>
        /// <response code="200">The bill date functionality is enabled or disabled successfully retrieved.</response>
        /// <response code="404">The bill date functionality status could not be found for the client.</response>
        /// <response code="500">An internal server error occurred while checking the bill date functionality status.</response>
        [Route("IsBillDateEnabled")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsBillDateEnabled()
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.IsBillDateEnabled(getClientID());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Checks if the void date functionality is enabled for the current client.
        /// </summary>
        /// <returns>Returns a response indicating whether the void date feature is enabled for the client.</returns>
        /// <remarks>
        /// This endpoint checks the configuration or status to determine whether the client is allowed to set or modify void dates for transactions.
        /// </remarks>
        /// <response code="200">The void date functionality status was successfully retrieved.</response>
        /// <response code="404">The void date functionality status could not be found for the client.</response>
        /// <response code="500">An internal server error occurred while checking the void date functionality status.</response>
        [Route("IsVoidDateEnabled")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsVoidDateEnabled()
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.IsVoidDateEnabled(getClientID());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Checks if the use of tax functionality is enabled for a specific corporation based on the provided corporate ID.
        /// </summary>
        /// <param name="CorpId">The unique identifier of the corporation to check the tax functionality for.</param>
        /// <returns>Returns a response indicating whether the tax functionality is enabled for the specified corporation.</returns>
        /// <remarks>
        /// This endpoint verifies whether the tax functionality is enabled for a given corporation, allowing the system to apply tax rules accordingly.
        /// </remarks>
        /// <response code="200">The use of tax functionality status was successfully retrieved.</response>
        /// <response code="404">The tax functionality status could not be found for the provided corporate ID.</response>
        /// <response code="500">An internal server error occurred while checking the tax functionality status.</response>
        [Route("IsUseTaxEnabled")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsUseTaxEnabled(string CorpId)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.IsUseTaxEnabled(getUserID(), CorpId);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Checks if the split line memo copy functionality is enabled for the chart of accounts (COA) for the current client.
        /// </summary>
        /// <returns>Returns a response indicating whether the split line memo copy feature is enabled for the client’s chart of accounts.</returns>
        /// <remarks>
        /// This endpoint verifies if the functionality allowing the splitting of line item memos for chart of accounts entries is enabled for the client.
        /// </remarks>
        /// <response code="200">The status of the split line memo copy feature was successfully retrieved.</response>
        /// <response code="404">The status of the split line memo copy feature could not be found for the client.</response>
        /// <response code="500">An internal server error occurred while checking the split line memo copy status for the COA.</response>
        [Route("IsSplitLineMemoCopyforCOA")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsSplitLineMemoCopyforCOA()
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.IsSplitLineMemoCopyforCOA(getClientID());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Checks if the statistics functionality is enabled for the current user.
        /// </summary>
        /// <returns>Returns a response indicating whether the statistics feature is enabled for the user.</returns>
        /// <remarks>
        /// This endpoint verifies if the statistics functionality, which might be used for tracking or reporting purposes, is enabled for the current user.
        /// </remarks>
        /// <response code="200">The statistics functionality status was successfully retrieved.</response>
        /// <response code="404">The statistics functionality status could not be found for the user.</response>
        /// <response code="500">An internal server error occurred while checking the statistics functionality status.</response>
        [Route("IsStatisticsEnabled")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsStatisticsEnabled()
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.IsStatisticsEnabled(getUserID());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Checks if the tax functionality is enabled in the vendor master for the provided corporate ID.
        /// </summary>
        /// <param name="CorpID">The unique identifier of the corporation for which the tax functionality in the vendor master is to be checked.</param>
        /// <returns>Returns a response indicating whether the tax functionality is enabled in the vendor master for the specified corporation.</returns>
        /// <remarks>
        /// This endpoint checks whether tax settings are enabled for vendors under a specific corporation, allowing for tax-related processing in the vendor master records.
        /// </remarks>
        /// <response code="200">The tax functionality status in the vendor master was successfully retrieved.</response>
        /// <response code="404">The tax functionality status could not be found for the provided corporate ID.</response>
        /// <response code="500">An internal server error occurred while checking the tax functionality in the vendor master.</response>
        [Route("IsTaxEnabledinVendorMaster")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsTaxEnabledinVendorMaster(string CorpID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.IsTaxEnabledinVendorMaster(CorpID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Retrieves the account associated with a specific purpose ID.
        /// </summary>
        /// <param name="PurposeID">The unique identifier of the purpose for which the account is to be retrieved.</param>
        /// <returns>Returns the account details associated with the given purpose ID if found, otherwise a not found response.</returns>
        /// <remarks>
        /// This endpoint fetches the account details related to a specific purpose, which could represent a category, transaction type, or other use cases that require an account association.
        /// </remarks>
        /// <response code="200">The account details for the specified purpose ID were successfully retrieved.</response>
        /// <response code="404">No account found for the provided purpose ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the account for the given purpose ID.</response>
        [Route("GetAccountbyPurposeID")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccountbyPurposeID(string PurposeID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.GetAccountbyPurposeID(PurposeID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the use tax preference settings associated with a specific vendor based on the provided vendor ID.
        /// </summary>
        /// <param name="VenID">The unique identifier of the vendor whose use tax preference settings are to be retrieved.</param>
        /// <returns>Returns the use tax preference settings for the specified vendor if found, otherwise a not found response.</returns>
        /// <remarks>
        /// This endpoint checks the use tax preferences associated with a vendor, which may include whether the vendor is subject to tax or has specific tax-related rules applied.
        /// </remarks>
        /// <response code="200">The use tax preference settings for the specified vendor were successfully retrieved.</response>
        /// <response code="404">No use tax preferences found for the provided vendor ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the use tax preferences for the vendor.</response>
        [Route("GetUseTaxPrefbyVendor")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUseTaxPrefbyVendor(string VenID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.GetUseTaxPrefbyVendor(VenID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the available payment methods for the current client.
        /// </summary>
        /// <returns>Returns a list of payment methods available for the current client.</returns>
        /// <remarks>
        /// This endpoint fetches the payment methods that can be used by the client for transactions or bill payments.
        /// </remarks>
        /// <response code="200">The payment methods for the client were successfully retrieved.</response>
        /// <response code="404">No payment methods were found for the client.</response>
        /// <response code="500">An internal server error occurred while retrieving the payment methods.</response>
        [Route("LoadPaymentMethods")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadPaymentMethods()
        {
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.LoadPaymentMethods(clientID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the tax lines associated with the current client.
        /// </summary>
        /// <returns>Returns the tax lines available for the current client.</returns>
        /// <remarks>
        /// This endpoint fetches the tax line data that is associated with the client, which may include tax rates, calculations, and related details for transactions.
        /// </remarks>
        /// <response code="200">The tax lines for the client were successfully retrieved.</response>
        /// <response code="404">No tax lines were found for the client.</response>
        /// <response code="500">An internal server error occurred while retrieving the tax lines.</response>
        [Route("LoadTaxLines")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadTaxLines()
        {
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.LoadTaxLines(clientID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Retrieves the activity log for a specific bill entry based on the provided journal entry ID (JEID).
        /// </summary>
        /// <param name="JEID">The unique identifier of the journal entry for which the activity log is to be retrieved.</param>
        /// <returns>Returns the activity log details associated with the given journal entry ID.</returns>
        /// <remarks>
        /// This endpoint fetches the history of actions or changes made to a bill entry, which could include updates, status changes, or other relevant activities.
        /// </remarks>
        /// <response code="200">The activity log for the specified journal entry was successfully retrieved.</response>
        /// <response code="404">No activity log found for the provided journal entry ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the activity log for the journal entry.</response>
        [Route("GetBillEntryActivityLog")]
        [HttpGet]
        [ProducesResponseType(typeof(BillEntryActivityLogListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillEntryActivityLog(string JEID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    BillEntryActivityLogListResponse response = await bs.GetBillEntryActivityLog(JEID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Rejects a bill entry based on the provided rejection request details.
        /// </summary>
        /// <param name="BillRejectReq">The request details for rejecting a bill entry, including the necessary information to process the rejection.</param>
        /// <returns>Returns the response indicating the result of the bill entry rejection operation.</returns>
        /// <remarks>
        /// This endpoint allows the rejection of a specific bill entry, which may involve setting its status to "rejected" and performing any necessary actions like notifying users or updating records.
        /// </remarks>
        /// <response code="200">The bill entry was successfully rejected.</response>
        /// <response code="404">The bill entry was not found for rejection.</response>
        /// <response code="500">An internal server error occurred while attempting to reject the bill entry.</response>
        [Route("BillEntryReject")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillEntryReject(BillRejectRequest BillRejectReq)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.BillEntryReject(BillRejectReq, getUserID(), false);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }


        /// <summary>
        /// Saves a comment for a specific bill entry.
        /// </summary>
        /// <param name="BillCommentReq">The request details containing the bill entry comment to be saved.</param>
        /// <returns>Returns a response indicating the result of the save operation.</returns>
        /// <remarks>
        /// This endpoint allows users to add comments to a bill entry, typically for internal tracking or clarification. The comment is saved under the specified bill entry for future reference.
        /// </remarks>
        /// <response code="200">The comment was successfully saved for the bill entry.</response>
        /// <response code="404">The bill entry was not found for the specified comment.</response>
        /// <response code="500">An internal server error occurred while attempting to save the comment for the bill entry.</response>
        [Route("SaveBillEntryComment")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveBillEntryComment(BillCommentRequest BillCommentReq)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.SaveBillEntryComment(BillCommentReq, getUserID());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the last 10 transactions for a specific vendor based on the provided vendor ID.
        /// </summary>
        /// <param name="VenID">The unique identifier of the vendor for which the last 10 transactions are to be retrieved.</param>
        /// <returns>Returns the details of the last 10 transactions for the specified vendor.</returns>
        /// <remarks>
        /// This endpoint fetches the most recent transactions made for the specified vendor, which could be used for reviewing transaction history or performing further analysis.
        /// </remarks>
        /// <response code="200">The last 10 transactions for the specified vendor were successfully retrieved.</response>
        /// <response code="404">No transactions were found for the provided vendor ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the last 10 transactions for the vendor.</response>
        [Route("LoadLast10Transactions")]
        [HttpGet]
        [ProducesResponseType(typeof(TransactionListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadLast10Transactions(string VenID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.LoadLast10Transactions(VenID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Approves a bill entry based on the provided journal entry ID (JID).
        /// </summary>
        /// <param name="BillApproveReq">The request containing the journal entry ID (JID) and validation status for the approval operation.</param>
        /// <returns>Returns a response indicating the result of the bill entry approval operation.</returns>
        /// <remarks>
        /// This endpoint is used to approve a bill entry, which may include validating the entry and processing it based on the specified journal entry ID.
        /// </remarks>
        /// <response code="200">The bill entry was successfully approved.</response>
        /// <response code="404">The bill entry was not found for the specified journal entry ID.</response>
        /// <response code="500">An internal server error occurred while attempting to approve the bill entry.</response>
        [Route("BillEntryApprove")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillEntryApprove(BillApproveRequest BillApproveReq)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.BillEntryApprove(new PFAID(BillApproveReq.JID).ToString(), getUserID(), BillApproveReq.IsValidate, false , BillApproveReq.IsSaveVendorAiPurposeDetails , BillApproveReq.RemovedTransactionsSplitines,BillApproveReq.IsHoldPayment);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Approves multiple bill entries in bulk based on the provided list of journal entry IDs (JID).
        /// </summary>
        /// <param name="jidList">A list of journal entry data transfer objects (DTO) containing the JIDs and relevant information for bulk approval.</param>
        /// <returns>Returns a list of responses indicating the result of the bulk bill entry approval operation.</returns>
        /// <remarks>
        /// This endpoint processes multiple bill entries at once for approval. It validates and approves each bill entry in the provided list. If some entries cannot be processed, the method will provide a partial response indicating which entries failed.
        /// </remarks>
        /// <response code="200">All provided bill entries were successfully approved.</response>
        /// <response code="206">Some bill entries were successfully approved, but certain transactions could not be processed due to policy restrictions.</response>
        /// <response code="500">An internal server error occurred while attempting to approve the bill entries.</response>
        [Route("BillEntryBulkApprove")]
        [HttpPost]
        [ProducesResponseType(typeof(List<JournalBillResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillEntryBulkApprove(List<QuickBillDTO> jidList)
        {
            List<JournalBillResponse> billresponse = new List<JournalBillResponse>();
            JournalBillResponse response = new JournalBillResponse();
            HashSet<string> corpsFailed = new HashSet<string>();
            foreach (var item in jidList)
            {
                response = await billEntrySrv.BillEntryApprove(new PFAID(item.JId).ToString(), getUserID(), true, true);
                if (response.StatusCode == StatusCodes.Status404NotFound)
                {
                    if (jidList.Select(x => x.Corporation).Distinct().Count() == 1)
                    {
                        response.StatusCode = StatusCodes.Status206PartialContent;
                        billresponse.Add(response);
                    }
                    else
                        corpsFailed.Add(item.CorporationName);
                }
                else
                {
                    billresponse.Add(new JournalBillResponse
                    {
                        BillNumber = item.BillNumber,
                        InfoID = response.InfoID,
                        ID = item.JId,
                    });
                }

            }
            var saveResp = await uow.SaveAsync();
            if (saveResp > 0)
                await commonSrv.SaveOrUpdateMultipleAudit(jidList.Select(x => x.JId).ToList());
            if (billresponse != null && billresponse.Any())
            {
                billresponse.ForEach(x =>
                {
                    x.Status = saveResp > 0 ? Constants.MSG_BL_APPROVE_SUCESS : Constants.MSG_JE_COM_STAT;
                    x.StatusCode = saveResp > 0 ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
                });
            }
            if (corpsFailed != null && corpsFailed.Any())
            {
                billresponse.Add(new JournalBillResponse
                {
                    Status = $"Transaction approved successfully, but transactions for {string.Join(',', corpsFailed)} were not processed due to no approval policy for the user",
                    StatusCode = StatusCodes.Status206PartialContent
                });
            }
            return Ok(billresponse);
        }

        /// <summary>
        /// Verifies a bill entry based on the provided journal entry ID (JID).
        /// </summary>
        /// <param name="JID">The request containing the journal entry ID (JID) and validation status for the verification operation.</param>
        /// <returns>Returns a response indicating the result of the bill entry verification operation.</returns>
        /// <remarks>
        /// This endpoint is used to verify a bill entry, which may include validating the entry and processing it based on the specified journal entry ID. It helps ensure that the bill entry is correct before final approval or processing.
        /// </remarks>
        /// <response code="200">The bill entry was successfully verified.</response>
        /// <response code="404">The bill entry was not found for the specified journal entry ID.</response>
        /// <response code="500">An internal server error occurred while attempting to verify the bill entry.</response>
        [Route("BillEntryVerify")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillEntryVerify(LoadByIDRequest JID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.BillEntryVerify(new PFAID(JID.ID).ToString(), getUserID(), JID.IsValidate, false);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Verifies multiple bill entries in bulk based on the provided list of journal entry IDs (JIDs).
        /// </summary>
        /// <param name="JIDList">A list of journal entry data transfer objects (DTO) containing the JIDs and relevant information for bulk verification.</param>
        /// <returns>Returns a list of responses indicating the result of the bulk bill entry verification operation.</returns>
        /// <remarks>
        /// This endpoint processes multiple bill entries for verification at once. It validates each bill entry in the provided list and returns a response with the result of each verification. If all entries are processed successfully, the method will return an OK response. Otherwise, an internal server error message will be returned.
        /// </remarks>
        /// <response code="200">All provided bill entries were successfully verified.</response>
        /// <response code="500">An internal server error occurred while attempting to verify the bill entries.</response>
        [Route("BillEntryBulkVerify")]
        [HttpPost]
        [ProducesResponseType(typeof(List<JournalBillResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkBillEntryVerify(List<QuickBillDTO> JIDList)
        {
            List<JournalBillResponse> billresponse = await billEntrySrv.BillEntryBulkVerify(JIDList, getUserID());
            if (billresponse != null && billresponse.Count() > 0)
            {
                return Ok(billresponse);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }

        }
        /// <summary>
        /// Rejects multiple bill entries in bulk based on the provided list of journal entry IDs (JIDs).
        /// </summary>
        /// <param name="JIDList">A list of BillBulkRejectRequest objects containing the JID, assigned user, comment, and other relevant data for each rejection operation.</param>
        /// <returns>Returns a list of responses indicating the result of each bulk bill entry rejection.</returns>
        /// <remarks>
        /// This endpoint processes multiple bill entries for rejection at once. It performs the rejection of each bill entry in the provided list and returns a response with the result of each rejection. After processing, the method will return a successful response if all entries are handled properly, or an internal server error if there was a failure during the process.
        /// </remarks>
        /// <response code="200">All provided bill entries were successfully rejected.</response>
        /// <response code="500">An internal server error occurred while attempting to reject the bill entries.</response>
        [Route("BillEntryBulkReject")]
        [HttpPost]
        [ProducesResponseType(typeof(List<JournalBillResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillEntryBulkReject(List<BillBulkRejectRequest> JIDList)
        {
            List<JournalBillResponse> billresponse = new List<JournalBillResponse>();
            foreach (var item in JIDList)
            {

                BillRejectRequest billRejectReq = new BillRejectRequest();
                billRejectReq.JID = item.JID;
                billRejectReq.AssignedUser = item.AssignedUser;
                billRejectReq.Comment = item.Comment;
                billRejectReq.SourceType = item.SourceType;
                billRejectReq.IsValidate = true;

                var response = await billEntrySrv.BillEntryReject(billRejectReq, getUserID(), true);
                billresponse.Add(new JournalBillResponse
                {
                    BillNumber = item.BillNumber,
                    ID = item.JID,
                    InfoID = response.InfoID,
                    Status = response.Status,
                    StatusCode = response.StatusCode,
                });
            }
            var beRejectResp = await uow.SaveAsync();
            if (beRejectResp > 0)
                await commonSrv.SaveOrUpdateOCRMultipleAudit(JIDList.Select(x => x.JID).ToList());

            billresponse.ForEach(x =>
            {
                x.Status = beRejectResp > 0 ? Constants.MSG_BL_REJECT_SUCESS : (!string.IsNullOrEmpty(x.Status) ? x.Status : Constants.MSG_JE_COM_STAT);
                x.StatusCode = beRejectResp > 0 ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
            });

            return Ok(billresponse);
        }
        /// <summary>
        /// Loads the details of a specific bill entry based on the provided journal entry ID (JID).
        /// </summary>
        /// <param name="JID">The journal entry ID (JID) of the bill entry whose details need to be loaded.</param>
        /// <returns>Returns the details of the specified bill entry if found, otherwise returns a Not Found status.</returns>
        /// <remarks>
        /// This endpoint fetches the detailed information for a specific bill entry based on the provided JID. The service ensures that the requested bill entry belongs to the current user. If the entry is found, the details are returned; otherwise, a Not Found response is sent.
        /// </remarks>
        /// <response code="200">The bill entry details were successfully loaded.</response>
        /// <response code="404">The bill entry with the provided JID was not found.</response>
        /// <response code="500">An internal server error occurred while attempting to load the bill entry details.</response>
        [Route("LoadBillEntryDetails")]
        [HttpGet]
        [ProducesResponseType(typeof(BillEntryDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBillEntryDetails(string JID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    BillEntryDetails response = await bs.LoadBillEntryDetails(JID, getUserID());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Loads the last transaction for a specific vendor based on the provided Vendor ID (VenID).
        /// </summary>
        /// <param name="VenID">The vendor ID for which the last transaction needs to be loaded.</param>
        /// <returns>Returns the details of the last transaction for the specified vendor if found, otherwise returns a Not Found status.</returns>
        /// <remarks>
        /// This endpoint retrieves the most recent transaction associated with the given vendor. The service checks if the transaction belongs to the current user. If the transaction is found, its details are returned; otherwise, a Not Found response is sent.
        /// </remarks>
        /// <response code="200">The last transaction for the specified vendor was successfully loaded.</response>
        /// <response code="404">No transaction found for the provided vendor ID.</response>
        /// <response code="500">An internal server error occurred while attempting to load the last transaction.</response>
        [Route("LoadLastTransaction")]
        [HttpGet]
        [ProducesResponseType(typeof(TransactionDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadLastTransaction(string VenID)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.LoadLastTransaction(VenID, getUserID());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }

            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Deletes a bill entry identified by the provided Journal ID (JID).
        /// </summary>
        /// <param name="JID">The unique Journal ID (JID) of the bill entry to be deleted.</param>
        /// <param name="IsValidate">Indicates whether to perform validation before deletion. Default is false.</param>
        /// <returns>Returns a success response if the bill entry is successfully deleted, otherwise returns a Not Found status.</returns>
        /// <remarks>
        /// This endpoint deletes the specified bill entry after validating the request. If the entry is not found or deletion fails, a Not Found response is returned.
        /// The IsValidate parameter determines whether additional validation steps should be performed before deletion.
        /// </remarks>
        /// <response code="200">The bill entry was successfully deleted.</response>
        /// <response code="404">No bill entry was found with the provided Journal ID (JID).</response>
        /// <response code="500">An internal server error occurred while attempting to delete the bill entry.</response>
        [Route("BillEntryDelete")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalBillResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillEntryDelete(string JID, bool IsValidate = false)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.BillEntryDelete(JID, getUserID(), IsValidate);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Voids a bill entry identified by the provided request data.
        /// </summary>
        /// <param name="req">The request data containing the details for voiding the bill entry.</param>
        /// <returns>Returns a success response if the bill entry is successfully voided, otherwise returns a Not Found status.</returns>
        /// <remarks>
        /// This endpoint voids the specified bill entry. If the entry is not found or voiding fails, a Not Found response is returned.
        /// </remarks>
        /// <response code="200">The bill entry was successfully voided.</response>
        /// <response code="404">No bill entry was found with the provided details.</response>
        /// <response code="500">An internal server error occurred while attempting to void the bill entry.</response>
        [Route("BillEntryVoid")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalBillResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillEntryVoid(BillVoidRequest req)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.BillEntryVoid(req, getUserID(), getClientID());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Saves the provided bill entry details.
        /// </summary>
        /// <param name="req">The request data containing the details of the bill entry to be saved.</param>
        /// <returns>Returns a success response if the bill entry is successfully saved, otherwise returns a Not Found status.</returns>
        /// <remarks>
        /// This endpoint saves the specified bill entry details. If the entry cannot be saved, a Not Found response is returned.
        /// </remarks>
        /// <response code="200">The bill entry was successfully saved.</response>
        /// <response code="404">The bill entry could not be found or saved.</response>
        /// <response code="500">An internal server error occurred while attempting to save the bill entry.</response>
        [Route("BillEntrySave")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalBillResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillEntrySave(BillEntryDetails req)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.BillEntrySave(req, getUserID(), getClientID(), getClientName());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Splits the provided bill entry based on the provided details.
        /// </summary>
        /// <param name="req">The request data containing the details required to split the bill entry.</param>
        /// <returns>Returns a success response with the split bill entry details if the split is successful, otherwise returns a Not Found status.</returns>
        /// <remarks>
        /// This endpoint splits a bill entry into multiple entries based on the provided request data. If the split cannot be processed, a Not Found response is returned.
        /// </remarks>
        /// <response code="200">The bill entry was successfully split.</response>
        /// <response code="404">The bill entry could not be found or split.</response>
        /// <response code="500">An internal server error occurred while attempting to split the bill entry.</response>
        [Route("BillEntrySplit")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalBillResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillEntrySplit(BillEntrySplit req)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.BillEntrySplit(req, getUserID(), getClientID(), getClientName());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves multiple splits of a bill entry based on the provided details and specified split count.
        /// </summary>
        /// <param name="req">The request data containing the details of the bill entry to be split.</param>
        /// <param name="splitCount">The number of splits to be generated for the given bill entry.</param>
        /// <returns>Returns the split bill entries if the operation is successful, or a Not Found status if no splits are found.</returns>
        /// <remarks>
        /// This endpoint generates multiple splits for a given bill entry. The number of splits is determined by the 'splitCount' parameter. If the splits cannot be found or generated, a Not Found response is returned.
        /// </remarks>
        /// <response code="200">The bill entry splits were successfully retrieved.</response>
        /// <response code="404">No splits were found for the provided bill entry.</response>
        /// <response code="500">An internal server error occurred while attempting to retrieve the bill entry splits.</response>
        [Route("GetBillEntryMultiSplits")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillEntryMultiSplits(BillEntryDetails req, int splitCount)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.GetBillEntryMultiSplits(req, getUserID(), splitCount);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the approval type for the current user based on their user ID.
        /// </summary>
        /// <returns>Returns the user's approval type if found, otherwise returns an internal server error.</returns>
        /// <remarks>
        /// This method retrieves the approval type associated with the currently authenticated user. The approval type indicates 
        /// the kind of approval workflow the user is part of. If successful, the method will return a status 200 (OK) with the response.
        /// </remarks>
        /// <response code="200">Successfully retrieved the user approval type.</response>
        /// <response code="500">An error occurred while attempting to retrieve the user approval type.</response>
        [Route("GetUserApprovalType")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserApprovalType()
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.GetUserApprovalType(getUserID());
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the approval policy user details for a given corporation based on the screen type and user preferences.
        /// </summary>
        /// <param name="CorpID">The unique identifier for the corporation.</param>
        /// <param name="getAllUsers">A flag to indicate whether to retrieve details for all users or just the current user.</param>
        /// <param name="ScreenType">The screen type to determine the specific context of the approval policy.</param>
        /// <returns>Returns the approval policy user details based on the provided parameters.</returns>
        /// <remarks>
        /// This method fetches the approval policy details for the users within a corporation. The results vary depending on 
        /// the screen type and whether the details for all users or just the current user are requested.
        /// </remarks>
        /// <response code="200">Successfully retrieved the approval policy user details.</response>
        /// <response code="500">An error occurred while retrieving the approval policy user details.</response>
        [Route("GetApprovalPolicyUserDetails")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApprovalPolicyUserDetails(string CorpID, bool getAllUsers, short ScreenType)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.GetApprovalPolicyUserDetails(CorpID, ScreenType, getUserID(), getAllUsers);
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Retrieves the approval details for the user based on the provided list of corporation IDs.
        /// </summary>
        /// <param name="CorpIDs">A list of corporation IDs for which approval details need to be retrieved.</param>
        /// <returns>A list of user approval details for the specified corporations, including a flag indicating if the current user is part of the list.</returns>
        /// <remarks>
        /// This method fetches the approval details for each corporation in the provided list. It checks if the current 
        /// user is included in the approval details and sets a flag (`IsCurrentUser`) accordingly. The response is returned
        /// with all the user approval details, where `IsCurrentUser` is true if the current user is associated with the 
        /// approval details for any of the corporations.
        /// </remarks>
        /// <response code="200">Successfully retrieved the user approval details for the specified corporations.</response>
        /// <response code="500">An error occurred while retrieving the user approval details.</response>
        [Route("GetUserApprovalDetails")]
        [HttpPost]
        [ProducesResponseType(typeof(List<UserApprovalDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserApprovalDetails(List<string> CorpIDs,short? screenType=null)
        {
            try
            {
                var lstCorps = string.Join(",", CorpIDs);
                using (IBillEntryService bs = billEntrySrv)
                {
                    List<UserApprovalDetails> response = await bs.GetUserApprovalDetails(getUserID(), lstCorps, screenType);
                    if (response != null && response.Any())
                    {
                        foreach (var item in response)
                        {
                            if (item != null && item.UserID == getUserID())
                            {
                                item.IsCurrentUser = true;
                                break;
                            }
                        }
                    }
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Retrieves the bill entry preferences for the specified corporation.
        /// </summary>
        /// <param name="CorpID">The corporation ID for which bill entry preferences need to be retrieved. If not provided, preferences for the current user are fetched.</param>
        /// <returns>The bill entry preferences for the specified corporation or the current user if no corporation ID is provided.</returns>
        /// <remarks>
        /// This method retrieves the bill entry preferences for a specific corporation. If the `CorpID` parameter is not 
        /// provided, the method defaults to fetching the preferences for the current user and client.
        /// </remarks>
        /// <response code="200">Successfully retrieved the bill entry preferences for the specified corporation or user.</response>
        /// <response code="500">An error occurred while retrieving the bill entry preferences.</response>
        [Route("GetBillEntryPreferences")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillEntryPreferences(string CorpID = null)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.GetBillEntryPreferences(CorpID, getUserID(), getClientID());
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// Checks if a recurring name exists or is valid based on the provided request parameters.
        /// </summary>
        /// <param name="req">The request object containing the recurring name details to be checked.</param>
        /// <returns>A response indicating whether the recurring name is valid or already exists.</returns>
        /// <remarks>
        /// This method is used to check the validity or existence of a recurring name. The service will return a response
        /// indicating if the recurring name is valid or already in use, based on the provided request.
        /// </remarks>
        /// <response code="200">Successfully processed the check for recurring name.</response>
        /// <response code="500">An error occurred while checking the recurring name.</response>
        [Route("CheckRecurringName")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckRecurringName(RecurringCheck req)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.CheckRecurringName(req);
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Validates the provided password for a given corporation ID.
        /// </summary>
        /// <param name="CorpID">The ID of the corporation for which the password needs to be validated.</param>
        /// <param name="Password">The password to be validated for the given corporation.</param>
        /// <returns>A response indicating whether the provided password is valid for the specified corporation.</returns>
        /// <remarks>
        /// This method checks if the provided password is valid for the given `CorpID`. The service will return a response
        /// indicating the result of the password validation.
        /// </remarks>
        /// <response code="200">Successfully validated the password.</response>
        /// <response code="500">An error occurred while validating the password.</response>
        [Route("ValidatePassword")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidatePassword(string CorpID, string Password)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.ValidatePassword(CorpID, Password);
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }


        /// <summary>
        /// It Returns List of Bills Based on Corporation
        /// </summary>
        /// <param name="BillListReqData">BillListReqData represents BillListReq</param>
        /// <returns>It will display List Of Bills</returns>
        /// <remarks>
        /// This method fetches a list of bill entry summaries from the service layer based on the criteria provided in the `ReqData`.
        /// If no entries are found, a `NotFound` response is returned. In case of an error, a 500 internal server error is returned.
        /// </remarks>
        /// <response code="200">Successfully retrieved the bill entry summaries.</response>
        /// <response code="404">No bill entries were found matching the request data.</response>
        /// <response code="500">An error occurred while processing the request.</response>
        [Route("BillEntrySummary")]
        [HttpGet]
        [ProducesResponseType(typeof(List<BillEntrySummaryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillEntrySummary([FromQuery] BillEntrySummaryRequest ReqData)
        {
            List<BillEntrySummaryResponse> response = null;
            try
            {
                response = await billEntrySrv.GetBillEntrySummaryList(ReqData);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            //finally { response = null; }

        }
        /// <summary>
        /// Retrieves bill summary card details based on the provided request data.
        /// </summary>
        /// <param name="ReqData">The request data containing the parameters for retrieving the bill summary cards.</param>
        /// <returns>A list of bill summary card details based on the request data.</returns>
        /// <remarks>
        /// This method fetches a list of bill summary cards from the service layer based on the criteria provided in the `ReqData`.
        /// If no summary cards are found, a `NotFound` response is returned. In case of an error, a 500 internal server error is returned.
        /// </remarks>
        /// <response code="200">Successfully retrieved the bill summary cards.</response>
        /// <response code="404">No bill summary cards were found matching the request data.</response>
        /// <response code="500">An error occurred while processing the request.</response>
        [Route("billSummaryCardLoad")]
        [HttpGet]
        [ProducesResponseType(typeof(List<BillSummaryCardResp>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillSummaryCards([FromQuery] BillSummaryCardReq ReqData)
        {
            List<BillSummaryCardResp> response = null;
            try
            {
                response = await billEntrySrv.GetBillSummaryCardDetails(ReqData);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            //finally { response = null; }

        }
        /// <summary>
        /// Retrieves detailed information for bill entry payments tooltips based on the provided journal entry ID.
        /// </summary>
        /// <param name="JEID">The journal entry ID for which the payment tooltip details are to be retrieved.</param>
        /// <returns>A list of payment tooltip details for the specified journal entry ID.</returns>
        /// <remarks>
        /// This method fetches a list of tooltip details associated with a specific journal entry (JEID).
        /// If no details are found, a `NotFound` response is returned. If there is an error in the process, a 500 internal server error is returned.
        /// </remarks>
        /// <response code="200">Successfully retrieved the payment tooltip details for the specified journal entry ID.</response>
        /// <response code="404">No payment tooltip details were found for the given journal entry ID.</response>
        /// <response code="500">An error occurred while processing the request.</response>
        [Route("GetBillEntryPaymentsToolTipDetails")]
        [HttpGet]
        [ProducesResponseType(typeof(List<BillEntryPaymentToolTipDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillEntryPaymentsToolTipDetails(string JEID)
        {
            List<BillEntryPaymentToolTipDetails> response = null;
            try
            {
                response = await billEntrySrv.GetBillEntryPaymentsToolTipDetails(JEID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        /// <summary>
        /// It Returns List of Users for group chat Based on Corporation
        /// </summary>
        /// <param name="corpId">represents the Corporation ID</param>
        /// <returns>It will display List Of users who have approval policy privileges on bill screen for particular corpId</returns>
        /// <remarks>
        /// This method interacts with the service layer to retrieve users participating in the group chat for a given corporation ID.
        /// If no users are found or there is an issue retrieving the users, a `NotFound` response is returned. If there is an error, a `500 Internal Server Error` is returned.
        /// </remarks>
        /// <response code="200">Successfully retrieved the list of users for the group chat associated with the provided corporation ID.</response>
        /// <response code="404">No users were found for the group chat associated with the provided corporation ID.</response>
        /// <response code="500">An error occurred while processing the request to retrieve group chat users.</response>
        [Route("Bill/GroupChatUsers/List")]
        [HttpPost]
        [ProducesResponseType(typeof(GroupChatUsersListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillEntryGroupChatUsersList(string corpId)
        {
            GroupChatUsersListResponse? response = null;
            try
            {
                response = await billEntrySrv.BillEntryGetGroupChatUsers(corpId);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        /// <summary>
        /// Adds a new purpose name to the system.
        /// </summary>
        /// <param name="request">The request object containing the details of the new purpose name to be added.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        /// <remarks>
        /// This method processes the request to add a new purpose name to the system. If successful, it returns a `200 OK` status code.
        /// If an error occurs during the process, a `500 Internal Server Error` status code is returned.
        /// </remarks>
        /// <response code="200">Successfully added the new purpose name.</response>
        /// <response code="500">An error occurred while processing the request to add the new purpose name.</response>
        [Route("AddingNewPurposeName")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> AddNewPurpose(PurposeReq request)
        {
            try
            {
                await billEntrySrv.AddingPurposeName(request);
                return Ok();
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }

        }

        /// <summary>
        /// Uploads a bill file to the system.
        /// </summary>
        /// <param name="request">The request object containing the details of the bill file to be uploaded.</param>
        /// <returns>A status code indicating the result of the file upload operation.</returns>
        /// <remarks>
        /// This method processes the request to upload a bill file. If successful, it returns a `201 Created` or `202 Accepted` status code.
        /// If the response contains a non-empty status but doesn't meet the successful conditions, it still returns the response with `200 OK`.
        /// In case of any error or unsuccessful upload, it returns a `404 Not Found` or a `500 Internal Server Error` based on the circumstances.
        /// </remarks>
        /// <response code="200">The request was processed successfully, and a non-empty status was returned.</response>
        /// <response code="201">The bill file was successfully uploaded and created.</response>
        /// <response code="202">The bill file upload is accepted and will be processed.</response>
        /// <response code="404">No content or data was found to be processed in the request.</response>
        /// <response code="500">An error occurred during the upload process.</response>
        [Route("UploadBill")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillFilesUpload(UploadBillReq request)
        {
            try
            {
                var response = await billEntrySrv.BillFilesUpload(request, getUserID());
                if (response != null && (response.StatusCode == StatusCodes.Status201Created || response.StatusCode == StatusCodes.Status202Accepted))
                {
                    return Ok(response);
                }
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }


        [Route("BillsWidgetData")]
        [HttpPost]

        public async Task<IActionResult> GetBillsWidgetInformation(BillsDataWidgetReq request)
        {
            BillsWidgetData? response = null;
            try
            {
                string UserID = (string)HttpContext.Items["UserId"];
                request.UserID = UserID;
                response = await billEntrySrv.GetBillBalancesWidgetData(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        /// <summary>
        /// Saves the provided quick bill entry details.
        /// </summary>
        /// <param name="req">The request data containing the details of the quick bill entry to be saved.</param>
        /// <returns>Returns a success response if the quick bill entry is successfully saved, otherwise returns a Not Found status.</returns>
        /// <remarks>
        /// This endpoint saves the specified quick bill entry details. 
        /// It is typically used for simplified or fast bill entry operations. 
        /// If the entry cannot be saved, a Not Found response is returned.
        /// </remarks>
        /// <response code="200">The quick bill entry was successfully saved.</response>
        /// <response code="404">The quick bill entry could not be found or saved.</response>
        /// <response code="500">An internal server error occurred while attempting to save the quick bill entry.</response>
        
        [Route("QuickBillEntrySave")]
        [HttpPost]
        [ProducesResponseType(typeof(JournalBillResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> QuickBillEntrySave(QuickBillEntryDetails req)
        {
            try
            {
                using (IBillEntryService bs = billEntrySrv)
                {
                    var response = await bs.QuickBillEntrySave(req, getUserID(), getClientID(), getClientName());
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("AIUpdateUnapprovedBillsPublish")]
        [HttpPost]
        [ProducesResponseType(typeof(StatusDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AIUpdateUnapprovedBillsPublish(UpdateUnapprovedBillsAIRequest request)

        {
            StatusDTO response = null;
            try
            { 
                response = await publishService.AIUpdateUnapprovedBillsPublish(request,getUserID(), getClientID(), getClientName());

                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch
            {
                throw;
            }
            finally
            {
                response = null;
            }
        }
        #endregion

    }
}
