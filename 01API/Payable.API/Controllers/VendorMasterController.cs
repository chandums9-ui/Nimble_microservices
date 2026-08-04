using Common.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;
using Payable.App.Contracts;
using Payable.Domain.DTO.Resp;
using Payable.Domain.DTO.Req;


using Common.App.Contracts;
using Common.Domain;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;


using Payable.App.Service;

using static Payable.Domain.DTO.Resp.PayableResponseDTO;
using Common.Domain.DTO.Enums;
using DataModel.Domain.DataModel;
using System.Diagnostics.Contracts;
namespace Payable.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class VendorMasterController : ControllerBase
    {
        #region Fields
        private readonly ICommonService commonSrv;
        private readonly IVendorService vendorService;
        private readonly ILoggerService logger;


        #endregion

        #region Ctor
        public VendorMasterController(ICommonService coreProperty, IVendorService vendorService, ILoggerService _logger)
        {
            this.commonSrv = coreProperty;
            this.vendorService = vendorService;
            this.logger = _logger;
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
        private string GetClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        #region VendorMaster

        /// <summary>
        /// Updates the credit days configuration for a specific vendor.
        /// </summary>
        /// <param name="VendorID">The unique identifier of the vendor.</param>
        /// <param name="CreditDaysID">The identifier representing the new credit days configuration.</param>
        /// <returns>Returns a <see cref="JournalResponse"/> indicating the result of the update operation.</returns>
        /// <remarks>
        /// This endpoint updates the credit days associated with a given vendor.
        /// A successful update returns a 200 status code. If the vendor or credit days ID is not found, a 404 is returned.
        /// Internal server errors return a 500 status code.
        /// </remarks>
        /// <response code="200">The credit days for the vendor were successfully updated.</response>
        /// <response code="404">The vendor or credit days configuration was not found.</response>
        /// <response code="500">An internal server error occurred while updating the credit days.</response>
        [Route("vendor/UpdateCreditdays")]
        [HttpGet]
        [ProducesResponseType(typeof(JournalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVendorCreditDays(string VendorID,string CreditDaysID)
        {
            JournalResponse response = new ();

            response = await vendorService.UpdateVendorCreditDays(VendorID,CreditDaysID);
            return Ok(response);

        }
        /// <summary>
        ///Get all the user preferences related to vendor master
        /// </summary>
        /// <param name="CorpID"></param>
        /// <returns>Returns vendors for that particular corporationID</returns>
        /// <remarks> get all the user preferences related to vendor master</remarks>
        /// <response code="200">Successfully get the vendors.</response>
        /// <response code="404">No vendors  found for the given CorpID.</response>
        /// <response code="500">An error occurred while processing your request.</response>

        [Route("IO/Vendors")]
        [HttpPost]
        [ProducesResponseType(typeof(IOVendorsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVendors(IOVendorRequest Req)
        {
            IOVendorsResponse response = new IOVendorsResponse();
            
                response = await vendorService.GetVendorList(Req);
                return Ok(response);
           
        }

        /// <summary>
        /// Retrieves a list of IO contracts associated with the specified vendor.
        /// </summary>
        /// <param name="VendorID">The unique identifier of the vendor.</param>
        /// <returns>Returns a list of IO contracts wrapped in an <see cref="IOContractResponse"/> object.</returns>
        /// <remarks>
        /// This endpoint returns IO contracts for the given VendorID. 
        /// If no contracts are found, a 404 status code is returned. 
        /// In case of a server error, a 500 status code is returned.
        /// </remarks>
        /// <response code="200">Returns the list of IO contracts.</response>
        /// <response code="404">No IO contracts were found for the specified VendorID.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [Route("IO/Contracts")]
        [HttpGet]
        [ProducesResponseType(typeof(IOContractResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContracts([FromQuery]string VendorID)
        {
            IOContractResponse response = new IOContractResponse();

            response = await vendorService.GetIOContractList(VendorID);
            return Ok(response);

        }
        /// <summary>
        /// Retrieves vendor user preferences for the specified corporate ID.
        /// </summary>
        /// <param name="CorpID">The unique identifier of the corporation.</param>
        /// <returns>Returns vendor user preferences wrapped in a <see cref="VendorUserPreferncesResponse"/> object.</returns>
        /// <remarks>
        /// This endpoint fetches the user preferences associated with a vendor under a specific corporation. 
        /// A 200 status code is returned when preferences are found, while a 404 is returned if no data is found. 
        /// In case of an internal server error, a 500 status code is returned.
        /// </remarks>
        /// <response code="200">Returns the vendor user preferences for the specified CorpID.</response>
        /// <response code="404">No preferences found for the given CorpID.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [Route("VendorMastrerUserPreferences")]
        [HttpGet]
        [ProducesResponseType(typeof(VendorUserPreferncesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVendorPrefernces([FromQuery] string CorpID)
        {
            VendorUserPreferncesResponse response = await vendorService.GetVendorPreferences(GetClientID(), GetUserID(), CorpID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }
        /// <summary>
        /// Retrieves the default country configured in the system.
        /// </summary>
        /// <returns>Returns the default country information.</returns>
        /// <remarks>
        /// This endpoint returns the default country settings used in the application configuration. 
        /// A 200 status code is returned on success. If no country is configured, a 404 is returned. 
        /// In case of a server error, a 500 status code is returned.
        /// </remarks>
        /// <response code="200">Successfully retrieved the default country.</response>
        /// <response code="404">Default country not found.</response>
        /// <response code="500">An internal server error occurred while retrieving the default country.</response>

        [Route("DefaultCountry")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDefaultCountry()
        {
            var country = await vendorService.GetDefaultCountry();
            return Ok(country);
        }
        /// <summary>
        /// Get the list of creditdays based on clientid and type
        /// </summary>
        /// <param name="CreditDaysRequest">contains type param</param>
        /// <returns>It returns list of creditdays details if found</returns>
        /// <remarks>
        /// This endpoint loads the credit days settings associated with the current client.
        /// A 200 status code is returned on success. If no data is available, a 404 is returned. 
        /// In case of an internal error, a 500 status code is returned.
        /// </remarks>
        /// <response code="200">Successfully retrieved the credit days list.</response>
        /// <response code="404">No credit days data found for the client.</response>
        /// <response code="500">An internal server error occurred while retrieving the data.</response>
        [Route("CreditDays/List")]
        [HttpGet]
        [ProducesResponseType(typeof(CreditDaysLoadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCreditDaysList()
        {
            CreditDaysLoadResponse response = await vendorService.LoadCreditDays(GetClientID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }


        /// <summary>
        /// Gets the list of business types based on client id
        /// </summary>
        /// <returns>It returns list of businesstypes if found</returns>
        /// <remarks>
        /// This endpoint loads the business types associated with the current client. 
        /// A 200 status code is returned on success, while a 404 status code is returned if no data is found. 
        /// A 500 status code is returned for any internal server errors.
        /// </remarks>
        /// <response code="200">Successfully retrieved the list of business types.</response>
        /// <response code="404">No business types data found for the client.</response>
        /// <response code="500">An internal server error occurred while retrieving the business types.</response>
        [Route("BusinessTypes/List")]
        [HttpGet]
        [ProducesResponseType(typeof(MiscInfoLoadResponse), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBusinessTypesList()
        {

            MiscInfoLoadResponse response = await vendorService.LoadBusinessTypes(GetClientID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// Gets the list of SendMethods based on client id
        /// </summary>
        /// <returns>It returns list of SendMethods if found</returns>
        /// <remarks>
        /// This endpoint loads the send methods associated with the current client. 
        /// A 200 status code is returned on success. If no send methods are available, a 404 is returned. 
        /// A 500 status code is returned in case of an internal server error.
        /// </remarks>
        /// <response code="200">Successfully retrieved the list of send methods.</response>
        /// <response code="404">No send methods data found for the client.</response>
        /// <response code="500">An internal server error occurred while retrieving the send methods.</response>
        [Route("SendMethods/List")]
        [HttpGet]
        [ProducesResponseType(typeof(MiscInfoLoadResponse), StatusCodes.Status200OK)] // Corrected response type to MiscInfoLoadResponse
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSendMethodsList()
        {

            MiscInfoLoadResponse response = await vendorService.LoadSendMethods(GetClientID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }


        /// <summary>
        /// Gets the list of paymentMethods based on client id.
        /// </summary>
        /// <returns>It returns list of paymentMethods if found</returns>
        /// <remarks>
        /// This endpoint loads the payment methods associated with the current client. 
        /// A 200 status code is returned on success. If no payment methods are available, a 404 status code is returned. 
        /// In case of a server error, a 500 status code is returned.
        /// </remarks>
        /// <response code="200">Successfully retrieved the list of payment methods.</response>
        /// <response code="404">No payment methods data found for the client.</response>
        /// <response code="500">An internal server error occurred while retrieving the payment methods.</response>
        [Route("PaymentMethods/List")]
        [HttpGet]
        [ProducesResponseType(typeof(MiscInfoLoadResponse), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaymentMethodsList()
        {

            MiscInfoLoadResponse response = await vendorService.LoadPaymentMethods(GetClientID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// Retrieves the list of countries available in the system.
        /// </summary>
        /// <returns>Returns a <see cref="CountryLoadResponse"/> containing a list of countries.</returns>
        /// <remarks>
        /// This endpoint loads the list of countries available in the system. 
        /// A 200 status code is returned on success. If no countries are available, a 404 status code is returned. 
        /// A 500 status code is returned in case of an internal server error.
        /// </remarks>
        /// <response code="200">Successfully retrieved the list of countries.</response>
        /// <response code="404">No countries data found.</response>
        /// <response code="500">An internal server error occurred while retrieving the countries.</response>
        [Route("Country/List")]
        [HttpGet]
        [ProducesResponseType(typeof(CountryLoadResponse), StatusCodes.Status200OK)] // Corrected response type to CountryLoadResponse
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {

            CountryLoadResponse response = await vendorService.LoadCountries();
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }


        /// <summary>
        /// Gets the list of states based on countryid
        /// </summary>
        /// <param name="CountryID"></param>
        /// <returns>It returns id,name of states list if found</returns>
        /// <remarks>
        /// This endpoint loads the list of states from the system. 
        /// A 200 status code indicates successful retrieval. 
        /// If no states are found, a 404 is returned. 
        /// A 500 status code is returned in case of an internal server error.
        /// </remarks>
        /// <response code="200">Successfully retrieved the list of states.</response>
        /// <response code="404">No states data found.</response>
        /// <response code="500">An internal server error occurred while retrieving the states.</response>
        [Route("States/List")]
        [HttpGet]
        [ProducesResponseType(typeof(StatesLoadResponse), StatusCodes.Status200OK)] // Corrected response type
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadStates()
        {

            StatesLoadResponse response = await vendorService.LoadStates();
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// gets list of adresses present for the particular vendor
        /// </summary>
        /// <param name="BusinessID">The ID of the business to filter vendor addresses.</param>
        /// <param name="VendorAdressID">The specific vendor address ID to retrieve.</param>
        /// <param name="IsActive">Specifies whether to retrieve only active addresses. Defaults to true.</param>
        /// <returns>It returns adress,contact details as list </returns>
        /// <remarks>
        /// This endpoint loads vendor address records for the specified business and address ID. 
        /// A 200 status code is returned on success. If no data is found, a 404 status code is returned. 
        /// A 500 status code indicates an internal server error.
        /// </remarks>
        /// <response code="200">Successfully retrieved the vendor addresses.</response>
        /// <response code="404">No vendor addresses found for the specified criteria.</response>
        /// <response code="500">An internal server error occurred while retrieving vendor addresses.</response>
        [Route("VendorAdress/List")]
        [HttpGet]
        [ProducesResponseType(typeof(VendorAddressesListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadVendorAddresses([FromQuery] string BusinessID, long VendorAdressID , bool IsActive = true)
        {

            VendorAddressesListResponse response = await vendorService.LoadVendorAddresses(BusinessID, VendorAdressID , IsActive);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// gets the vendorAdresses in a string format
        /// </summary>
        /// <param name="BusinessID">The ID of the business for which vendor addresses are being requested.</param>
        /// <returns>It returns ID,Name if found</returns>
        /// <remarks>
        /// This endpoint loads vendor address records that are relevant to contract creation or association, 
        /// filtered by the given business ID. A 200 status code indicates success, 404 indicates no records found, 
        /// and 500 indicates a server error.
        /// </remarks>
        /// <response code="200">Successfully retrieved vendor addresses for contracts.</response>
        /// <response code="404">No vendor addresses found for the given business ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the data.</response>
        [Route("VendorAdressForContract/List")]
        [HttpGet]
        [ProducesResponseType(typeof(VendorAdressesListForContractsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> LoadVendorAdressesforContract([FromQuery] string BusinessID)
        {

            VendorAdressesListForContractsResponse response = await vendorService.LoadVendorAdressesforContract(BusinessID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// gets vendorcontracts and vendortaxinfo's based on businessid or contractid
        /// </summary>
        /// <param name="BusinessID">The ID of the business to which the contract belongs.</param>
        /// <param name="ContractID">The specific contract ID to retrieve.</param>
        /// <param name="IsActive">Indicates whether to load only active contracts. Defaults to false.</param>
        /// <returns>It returns contract,TaxInfo details if found</returns>
        /// <remarks>
        /// This endpoint loads vendor contract details using the specified business ID and contract ID.
        /// A 200 status code indicates successful retrieval. If the contract is not found or an internal error occurs, 
        /// a 404 or 500 status may be returned accordingly.
        /// </remarks>
        /// <response code="200">Successfully retrieved the vendor contract.</response>
        /// <response code="404">Vendor contract not found for the given parameters.</response>
        /// <response code="500">An internal server error occurred while retrieving the vendor contract.</response>
        [Route("VendorContract/Load")]
        [HttpGet]
        [ProducesResponseType(typeof(VendorContractLoadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> LoadVendorContract([FromQuery] string BusinessID, [FromQuery] string ContractID , [FromQuery] bool IsActive = false)
        {

            VendorContractLoadResponse response = await vendorService.LoadVendorContract(BusinessID, ContractID , IsActive);
            if (response != null && (response.StatusCode == StatusCodes.Status200OK || !string.IsNullOrEmpty(response.Status)))
                return Ok(response);
            return Ok(response);
        }

        /// <summary>
        /// gets the vendor details,adress,contracts etc 
        /// </summary>
        /// <param name="BusinessID">The ID of the business for which vendor data is to be loaded.</param>
        /// <returns>It returns all the details related to vendor if found successfully</returns>
        /// <remarks>
        /// This endpoint retrieves vendor information based on the provided business ID and the authenticated user's ID.
        /// A 200 status code indicates successful retrieval. If no data is found, a 404 is returned. 
        /// A 500 status indicates an internal server error.
        /// </remarks>
        /// <response code="200">Successfully retrieved vendor information.</response>
        /// <response code="404">No vendor data found for the given business ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the vendor data.</response>
        [Route("Vendor/Load")]
        [HttpGet]
        [ProducesResponseType(typeof(VendorLoadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadVendor(string BusinessID)
        {

            VendorLoadResponse response = await vendorService.LoadVendor(BusinessID,GetUserID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        /// <summary>
        /// Saves an audit entry when a vendor's data or DD (Direct Deposit) details are viewed.
        /// </summary>
        /// <param name="saveRequest">The <see cref="VendorLoadResponse"/> containing the vendor details to be audited.</param>
        /// <param name="IsViewDDDetails">Indicates whether the DD (Direct Deposit) details were viewed.</param>
        /// <returns>Returns a status response indicating the result of the save operation.</returns>
        /// <remarks>
        /// This endpoint logs a view audit entry when a user views vendor details, including optional Direct Deposit details.
        /// A 200 status code indicates the entry was saved successfully.
        /// A 404 indicates failure to save or invalid input, while 500 signals a server error.
        /// </remarks>
        /// <response code="200">Audit entry saved successfully.</response>
        /// <response code="404">Audit entry could not be saved or was invalid.</response>
        /// <response code="500">An internal server error occurred while saving the audit entry.</response>
        [Route("Vendor/SaveViewAudit")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveViewAuditEntry(VendorLoadResponse saveRequest ,[FromQuery] bool IsViewDDDetails)
        {

            var response = await vendorService.SaveViewAuditEntry(saveRequest, GetUserID(),IsViewDDDetails);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        /// <summary>
        /// gets the vendor audit log 
        /// </summary>
        /// <param name="BusinessID">The ID of the business whose vendor audit trail is to be retrieved.</param>
        /// <returns>It returns all the log details related to vendor if found successfully</returns>
        /// <remarks>
        /// This endpoint retrieves audit logs related to vendor actions under a specific business ID. 
        /// A 200 status code indicates successful retrieval. If no data is found, a 404 is returned. 
        /// A 500 status indicates a server error occurred during processing.
        /// </remarks>
        /// <response code="200">Successfully retrieved the vendor audit trail.</response>
        /// <response code="404">No audit records found for the specified business ID.</response>
        /// <response code="500">An internal server error occurred while retrieving audit data.</response>
        [Route("VendorAudit/Load")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadVendorAudit([FromQuery] string BusinessID)
        {

            var response = await vendorService.LoadVendorAudit(BusinessID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        /// <summary>
        /// Retrieves the vendor ID based on the given vendor name and corporate ID.
        /// </summary>
        /// <param name="vendorName">The name of the vendor whose ID is to be retrieved.</param>
        /// <param name="CorpId">The corporate ID associated with the vendor.</param>
        /// <returns>Returns a <see cref="JournalResponse"/> containing the vendor ID and status information.</returns>
        /// <remarks>
        /// This endpoint fetches the vendor ID using the provided vendor name and corporate ID. 
        /// A 200 status code indicates successful retrieval or a validation error response. 
        /// A 400 status indicates that one or more required parameters were missing or invalid. 
        /// A 500 status signals an internal server error.
        /// </remarks>
        /// <response code="200">Successfully retrieved the vendor ID or returned a validation message.</response>
        /// <response code="400">Bad request due to missing or invalid input parameters.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [Route("LoadVendorId")]
        [HttpGet]
        [ProducesResponseType(typeof(JournalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadVendorId(string vendorName,string CorpId)
        {
            JournalResponse response=new JournalResponse();
            if (!string.IsNullOrEmpty(vendorName) && !string.IsNullOrEmpty(CorpId))
            {
                 response = await vendorService.LoadVendorId(vendorName, CorpId);
                  return Ok(response);
            }
            else
            {
                response.StatusCode = StatusCodes.Status400BadRequest;  
                return Ok(response);
            }
        }

        /// <summary>
        /// Gets the token details related Nimble ACH (Automated Clearing House),cardpay
        /// </summary>
        /// <param name="BusinessID">The ID of the business whose ACH configurations are to be retrieved.</param>
        /// <returns>it returns token details,list of account info's if found successfully</returns>
        /// <remarks>
        /// This endpoint fetches ACH-related configuration settings based on the provided business ID. 
        /// A 200 status indicates successful data retrieval. A 404 is returned if no data is found, and 
        /// a 500 indicates a server error during processing.
        /// </remarks>
        /// <response code="200">Successfully retrieved ACH configuration details.</response>
        /// <response code="404">No ACH configurations found for the provided business ID.</response>
        /// <response code="500">An internal server error occurred while retrieving the configurations.</response>
        [Route("ACHConfig/Load")]
        [HttpGet]
        [ProducesResponseType(typeof(LoadACHDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> LoadACHConfigurations(string BusinessID)
        {

            LoadACHDetailsResponse response = await vendorService.LoadACHConfigurations(BusinessID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }
        /// <summary>
        /// Checks whether a given vendor name is unique within a specified corporation.
        /// </summary>
        /// <param name="VendorName">The name of the vendor to validate for uniqueness.</param>
        /// <param name="CorpID">The corporate ID within which the vendor name should be unique.</param>
        /// <param name="VendorID">Optional vendor ID to exclude from the uniqueness check (used during updates).</param>
        /// <returns>Returns <c>true</c> if the vendor name is unique; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This endpoint validates whether the provided vendor name is unique under the given corporation.
        /// It can also exclude a specific vendor ID during checks, useful when editing existing vendors.
        /// </remarks>
        /// <response code="200">Returns true if the name is unique, false otherwise.</response>
        /// <response code="400">Bad request due to missing required parameters.</response>
        /// <response code="500">Internal server error during uniqueness check.</response>
        [Route("IsVendorNameUnique/Load")]
        [HttpGet]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsUniqueName(string VendorName, string CorpID, string? VendorID = null)
        {

            bool response = await vendorService.IsUniqueName(VendorName, CorpID, VendorID);
            return Ok(response);

        }

        /// <summary>
        /// gets taxid's for the client
        /// </summary>
        /// <returns>get's list of taxids,names if found successfully</returns>
        /// <remarks>
        /// This endpoint loads Use Tax IDs associated with the currently authenticated client's ID.
        /// A 200 status code indicates successful retrieval or response with status info.
        /// A 404 is returned if no Use Tax data is found.
        /// </remarks>
        /// <response code="200">Successfully retrieved Use Tax IDs.</response>
        /// <response code="404">No Use Tax IDs found for the current client.</response>
        /// <response code="500">An internal server error occurred while retrieving the data.</response>
        [Route("UseTaxIds/Load")]
        [HttpGet]
        [ProducesResponseType(typeof(UseTaxLoadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UseTaxID()
        {

            UseTaxLoadResponse response = await vendorService.UseTaxIDsLoad(GetClientID());
            if (response != null)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }
        /// <summary>
        /// It saves the business type given in miscinfo table
        /// </summary>
        /// <param name="SaveRequest">contains name,descreption</param>
        /// <returns>It returns Id,name if saved successfully</returns>
        /// <remarks>
        /// This endpoint allows creation  of a business type. The data is associated with the current client.
        /// A 200 status indicates a successful operation, while a 404 indicates a failure to save, and 500 signals an internal error.
        /// </remarks>
        /// <response code="200">Business type created  successfully.</response>
        /// <response code="404">Failed to create  the business type.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [Route("BusinessTypes/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(MiscInfoSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateBusinessType(MiscInfoSaveRequest SaveRequest)
        {

            MiscInfoSaveResponse response = await vendorService.SaveBusinessType(SaveRequest, GetClientID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// It saves the sendmethod details given in miscinfo table
        /// </summary>
        /// <param name="SaveRequest">contains name,descreption</param>
        /// <returns>It returns Id,name if saved successfully</returns>
        /// <remarks>
        /// This endpoint saves or updates send method information linked to the authenticated client's ID.
        /// A 200 status indicates success, 404 if the operation fails, and 500 for internal server errors.
        /// </remarks>
        /// <response code="200">Send method created or updated successfully.</response>
        /// <response code="404">Failed to create or update the send method.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [Route("SendMethods/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(MiscInfoSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSendMethod(MiscInfoSaveRequest SaveRequest)
        {

            MiscInfoSaveResponse response = await vendorService.SaveSendMethod(SaveRequest, GetClientID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// creates creditdaysdetails in frequency table
        /// </summary>
        /// <param name="SaveRequest">contains name,interval</param>
        /// <returns>It returns Id,name if sucessfully saved</returns>
        /// /// <remarks>
        /// This endpoint handles the creation or update of credit days configurations tied to the authenticated client's ID.
        /// A successful save returns 200 OK, otherwise 404 Not Found or 500 for unexpected errors.
        /// </remarks>
        /// <response code="200">Credit days created or updated successfully.</response>
        /// <response code="404">Failed to save the credit days data.</response>
        /// <response code="500">An internal server error occurred during processing.</response>
        [Route("CreditDays/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(CreditDaysSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCreditDays(CreditDaysSaveRequest SaveRequest)
        {

            CreditDaysSaveResponse response = await vendorService.SaveCreditDays(SaveRequest, GetClientID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// saves the vendoradress in adress,contact and vendoradress table
        /// </summary>
        /// <param name="saveRequest">represents the data to be saved </param>
        /// <returns>It returns ID,status,statuscode if saved successfully</returns>
        /// /// <remarks>
        /// This endpoint handles creating or updating vendor address data. A successful operation returns 200 OK.  
        /// If the save fails or the result is not found, it returns 404 Not Found. Any unexpected issue returns 500.
        /// </remarks>
        /// <response code="200">Vendor address saved successfully.</response>
        /// <response code="404">Failed to save the vendor address.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("VendorAdress/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorAdressSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateVendorAddres(VendorAddressSaveRequest SaveRequest)
        {

            VendorAdressSaveResponse response = await vendorService.SaveVendorAddres(SaveRequest);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// saves vendorcontract and taxinfodetails based on saveRequest Data
        /// </summary>
        /// <param name="SaveRequest">represents the data used to save</param>
        /// <returns>It returns vendorcontractid if saved successfully</returns>
        /// <remarks>
        /// This endpoint is used to create or update a vendor contract record.  
        /// It returns 200 OK on success, 404 if the operation fails, and 500 for internal server errors.
        /// </remarks>
        /// <response code="200">Vendor contract created or updated successfully.</response>
        /// <response code="404">Failed to save the vendor contract.</response>
        /// <response code="500">An internal server error occurred during the operation.</response>
        [Route("VendorContract/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorContractSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateVendorContract(VendorContractSaveRequest SaveRequest)
        {
            VendorContractSaveResponse response = await vendorService.SaveVendorContract(SaveRequest);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// saves the vendor details based on saverequest
        /// </summary>
        /// <param name="SaveRequest">represents the data to be saved</param>
        /// <returns>It retruns id,status,statuscode if saved successfully</returns>
        /// /// <remarks>
        /// This endpoint creates or updates vendor information for the current client and user context.  
        /// A successful save returns 200 OK. If the vendor data is invalid or not saved, it returns 404 Not Found.  
        /// Internal issues during processing return 500 Internal Server Error.
        /// </remarks>
        /// <response code="200">Vendor record saved successfully.</response>
        /// <response code="404">Failed to save the vendor record.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("Vendor/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveVendor(VendorSaveRequest SaveRequest)
        {
            VendorSaveResponse response = await vendorService.SaveVendor(SaveRequest, GetUserID(),GetClientID(), GetClientName());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }
        /// <summary>
        /// Clones all vendors from the source corporation to the destination corporation.
        /// </summary>
        /// <param name="cloneVendorsRequest">
        /// Contains the source corporation ID (from which vendors are cloned) 
        /// and the destination corporation ID (to which vendors are cloned).
        /// </param>
        /// <returns>
        /// Returns a status DTO containing the operation status and status code.
        /// </returns>
        /// <remarks>
        /// This endpoint clones vendor information from the source corporation to the destination corporation.
        /// A successful operation returns 200 OK.  
        /// If no records are found or cloned, it returns 404 Not Found.  
        /// Internal processing errors return 500 Internal Server Error.
        /// </remarks>
        /// <response code="200">Vendors cloned successfully.</response>
        /// <response code="404">No vendor records found to clone or clone failed.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("Vendor/cloneVendors")]
        [HttpPost]
        [ProducesResponseType(typeof(StatusDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CloneVendorsBetweenCorporations(CloneVendorsRequest cloneVendorsRequest)
        {
            StatusDTO response = await vendorService.CloneVendorsBetweenCorporations(cloneVendorsRequest,GetUserID(),GetClientID(),GetClientName());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        /// <summary>
        /// Imports and saves multiple vendor records.
        /// </summary>
        /// <param name="SaveRequest">The request object containing a batch of vendor records for import.</param>
        /// <returns> Returns a <see cref="VendorSaveResponse"/> indicating the result of the import operation./// </returns>
        /// <remarks>
        /// This endpoint is used to import and persist multiple vendor records in bulk.  
        /// Returns 200 OK if the operation is successful, 404 if saving fails, and 500 if an internal error occurs.
        /// </remarks>
        /// <response code="200">Vendor records imported and saved successfully.</response>
        /// <response code="404">Failed to save vendor records.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("VendorImport/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> SaveVendorImports(VendorImportRequest SaveRequest)
        {
            VendorSaveResponse response = await vendorService.SaveVendorImports(SaveRequest, GetClientID(), GetUserID(),GetClientName());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// saves the ach details in the necessary tables
        /// </summary>
        /// <param name="SaveRequest">represents the details to be saved</param>
        /// <returns>It returns Id,status if saved successfully</returns>
        /// <remarks>
        /// This endpoint saves ACH details associated with a vendor.  
        /// Returns 200 OK on success, 404 if saving fails, and 500 in case of an internal error.
        /// </remarks>
        /// <response code="200">ACH details saved successfully.</response>
        /// <response code="404">Failed to save ACH details.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("ACHDetails/Create")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveACHDetails(ACHSaveRequest SaveRequest)
        {
            VendorSaveResponse response = await vendorService.SaveACHDetails(SaveRequest);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// Loads detailed master information for a specific vendor.
        /// </summary>
        /// <param name="Request">The request object containing identifiers needed to fetch vendor master details.</param>
        /// <returns>
        /// Returns a detailed vendor master information response object.
        /// </returns>
        /// <remarks>
        /// This endpoint retrieves complete vendor master information based on provided input data.  
        /// Returns 200 OK with the vendor data, or 404 if no matching data is found.
        /// </remarks>
        /// <response code="200">Vendor master details loaded successfully.</response>
        /// <response code="404">Vendor master details not found.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("Vendor/LoadVendorMasterDetails")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadVendorMasterDetails(LoadVendorMasterDetailsRequest Request)
        {
            var response = await vendorService.LoadVendorMasterDetails(Request,GetUserID(),GetClientName());
            if (response != null)
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// updates the vendor details based on saverequest
        /// </summary>
        /// <param name="UpdateRequest">represents the data to be updated</param>
        /// <returns>It retruns id,status,statuscode if updates successfully</returns>
        /// <remarks>
        /// This endpoint updates vendor details such as contact info, addresses, and financial configuration.  
        /// Returns 200 OK if the update is successful, or 404 if the update fails or vendor is not found.
        /// </remarks>
        /// <response code="200">Vendor updated successfully.</response>
        /// <response code="404">Vendor not found or update failed.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("Vendor/Update")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVendor(VendorSaveRequest UpdateRequest)
        {
            VendorSaveResponse response = await vendorService.UpdateVendor(UpdateRequest, GetUserID(), GetClientID(),GetClientName());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }
        /// <summary>
        /// updates the vendoradress details in address,contact
        /// </summary>
        /// <param name="UpdateRequest">represents the fields that are to be updated</param>
        /// <returns>It returns vendoraddressid if updated successfully</returns>
        /// <remarks>
        /// This endpoint allows updating address information associated with a vendor.  
        /// Returns 200 OK if the update is successful, or 404 if the address is not found or the update fails.
        /// </remarks>
        /// <response code="200">Vendor address updated successfully.</response>
        /// <response code="404">Vendor address not found or update failed.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("VendorAdress/Update")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorAdressSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVendorAddress(VendorAddressSaveRequest UpdateRequest)
        {

            VendorAdressSaveResponse response = await vendorService.UpdateVendorAddress(UpdateRequest);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }


        /// <summary>
        /// updates the vendorcontract,vendortaxinfo based on updterequest
        /// </summary>
        /// <param name="UpdateRequest">represents the data to be updated</param>
        /// <returns>It returns id, status,statuscode if successfully updated</returns>
        /// <remarks>
        /// This endpoint is used to update vendor contract details.  
        /// It returns 200 OK if the contract is successfully updated, or 404 if not found or update fails.
        /// </remarks>
        /// <response code="200">Vendor contract updated successfully.</response>
        /// <response code="404">Vendor contract not found or update failed.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("VendorContract/Update")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorContractSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVendorContract(VendorContractSaveRequest UpdateRequest)
        {

            VendorContractSaveResponse response = await vendorService.UpdateVendorContract(UpdateRequest);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// Updates ACH (Automated Clearing House) configuration details for a vendor.
        /// </summary>
        /// <param name="UpdateRequest">The request object containing updated ACH details.</param>
        /// <returns>
        /// Returns a <see cref="VendorSaveResponse"/> indicating the result of the update operation.
        /// </returns>
        /// <remarks>
        /// This endpoint updates ACH details for a vendor.  
        /// It returns 200 OK on success, or 404 if the update fails or data is not found.
        /// </remarks>
        /// <response code="200">ACH details updated successfully.</response>
        /// <response code="404">ACH details not found or update failed.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("ACHDetails/Update")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateACHDetails(ACHSaveRequest UpdateRequest)
        {

            VendorSaveResponse response = await vendorService.UpdateACHDetails(UpdateRequest);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// deletes the vendoradress details based on vendoradressid
        /// </summary>
        /// <param name="VendorAddressID"></param>
        /// <returns>It returs status,statuscodes,id if deleted successfully</returns>
        /// <remarks>
        /// This endpoint deletes a vendor address record.  
        /// It returns 200 OK on successful deletion, or 404 if the address is not found.
        /// </remarks>
        /// <response code="200">Vendor address deleted successfully.</response>
        /// <response code="404">Vendor address not found or deletion failed.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("VendorAdress/Delete")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorAdressSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVendorAddress([FromBody] long VendorAddressID)
        {

            VendorAdressSaveResponse response = await vendorService.DeleteVendorAddress(VendorAddressID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// deletes the vendorcontract,vendortaxinfo based on contractid
        /// </summary>
        /// <param name="VendorContractID"></param>
        /// <returns>It returns Id,status,statuscode if deleted succesfully</returns>
        /// <remarks>
        /// This endpoint deletes a vendor contract.  
        /// Returns 200 OK on success, or 404 if not found or failed.
        /// </remarks>
        /// <response code="200">Vendor contract deleted successfully.</response>
        /// <response code="404">Vendor contract not found or deletion failed.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("VendorContract/Delete")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorContractSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVendorContract([FromBody] string VendorContractID, [FromQuery] bool IsSave)
        {

            VendorContractSaveResponse response = await vendorService.DeleteVendorContract(VendorContractID,IsSave);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }

        /// <summary>
        /// deletes the data of vendor based on vendorid
        /// </summary>
        /// <param name="VendorID"></param>
        /// <returns>It returns id ,status,statuscode if deleted successfully</returns>
        /// <remarks>
        /// This endpoint deletes a vendor using the specified business and user details.  
        /// It performs a soft or hard delete depending on business rules.
        /// </remarks>
        /// <response code="200">Vendor deleted successfully.</response>
        /// <response code="404">Vendor not found or deletion failed.</response>
        /// <response code="500">An internal server error occurred.</response>
        [Route("Vendor/Delete")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVendor(DeleteVendorRequest deleteVendorRequest)
        {

            VendorSaveResponse response = await vendorService.DeleteVendor(deleteVendorRequest, GetUserID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);

            else
                return NotFound(response);
        }
        /// <summary>
        /// Deletes the ACH (Automated Clearing House) configuration for the specified business.
        /// </summary>
        /// <param name="BusinessID">The unique identifier of the business whose ACH configuration should be deleted.</param>
        /// <returns>   </returns>
        /// <remarks>
        /// This endpoint removes the ACH configuration associated with the provided BusinessID.
        /// </remarks>
        /// <response code="200">ACH configuration deleted successfully.</response>
        /// <response code="404">ACH configuration not found.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [Route("ACHConfiguration/Delete")]
        [HttpPost]
        [ProducesResponseType(typeof(VendorSaveResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteACHConfiguration([FromBody] string BusinessID)
        {

            VendorSaveResponse response = await vendorService.DeleteACHConfiguration(BusinessID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        /// <summary>
        /// Retrieves vendor details (ID and CorporationID) for the specified vendor IDs.
        /// </summary>
        /// <param name="vendorIDs">A comma-separated list of vendor IDs for which details need to be fetched.</param>
        /// <returns>Returns a response containing vendor details such as ID and CorporationID.</returns>
        /// <remarks>
        /// This endpoint fetches vendor information for the provided vendor IDs.
        /// The data can be used for vendor subscription or synchronization with external systems.
        /// </remarks>
        /// <response code="200">Vendor details retrieved successfully.</response>
        /// <response code="404">No vendor details found for the given IDs.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [Route("VendorDetailsForSubscription")]
        [HttpGet]
        [ProducesResponseType(typeof(VendorInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VendorDetailsForSubscription([FromQuery] string vendorIDs)
        {
            VendorInfoResponse response = await vendorService.VendorDetailsForSubscription(vendorIDs);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        /// <summary>
        /// Retrieves the list of vendor contract master records for the specified corporation.
        /// </summary>
        /// <param name="CorpID">
        /// The unique identifier of the corporation for which vendor contract details are required.
        /// </param>
        /// <returns>
        /// Returns a response containing vendor contract master data associated with the given corporation.
        /// </returns>
        /// <remarks>
        /// This endpoint fetches vendor contract master information for a specific corporation.
        /// The data can be used for vendor synchronization, contract validation,
        /// or integration with external procurement and accounting systems.
        /// </remarks>
        /// <response code="200">Vendor contract master data retrieved successfully.</response>
        /// <response code="404">No vendor contract data found for the given corporation.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        /// 
        [Route("VendorContractMaster/List")]
        [HttpGet]
        public async Task<IActionResult> VendorContractMasterList(string CorpID)
        {
            VendorContractMasterResponse response = await vendorService.VendorContractMasterList(CorpID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        #endregion
    }
}
