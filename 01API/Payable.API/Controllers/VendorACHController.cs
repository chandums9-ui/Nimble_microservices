using Common.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;
using Payable.App.Contracts;
using Payable.App.Service;
using Payable.Domain.DTO.Resp;
using Payable.Domain.DTO.Req;

namespace Payable.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class VendorACHController : ControllerBase
    {
        #region Fields
        private readonly IVendorACHService achService;
        #endregion

        #region ctor
        public VendorACHController(IVendorACHService _achservice)
        {
            this.achService = _achservice;
        }
        #endregion

        private string GetUserID()
        {
            return (string)HttpContext.Items["UserId"];
        }

        [Route("ACHDetails")]
        [HttpGet]
        public async Task<IActionResult> GetACHDetails([FromQuery] string CorpID)
        {
            var response = await achService.GetACHDetailsOfUserAndCorporation(CorpID, GetUserID());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("CreateACHVendor")]
        [HttpPost]
        public async Task<IActionResult> CreateACHVendor(CreateACHVendorRequest Request)
        {
            var response = await achService.CreateACHVendor(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("CreateFundingSource")]
        [HttpPost]
        public async Task<IActionResult> CreateVendorAccountDetails(CreateAccDetailsRequest Request)
        {
            var response = await achService.CreateVendorAccountDetails(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("CreateCCFundingSource")]
        [HttpPost]
        public async Task<IActionResult> CreateCCDetails(CreateCCDetailsRequest Request)
        {
            var response = await achService.CreateVendorCCDetails(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("UpdateAddress")]
        [HttpPost]
        public async Task<IActionResult> UpdateACHAddress(UpdateAddressDetailsRequest Request)
        {
            var response = await achService.UpdateAdress(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("UpdateACHVendor")]
        [HttpPost]
        public async Task<IActionResult> UpdateACHVendor(UpdateACHVendorRequest Request)
        {
            var response = await achService.UpdateACHVendor(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("GetFundingSource")]
        [HttpPost]
        public async Task<IActionResult> GetFundingSource(GetVendorAccDetailsRequest Request)
        {
            var response = await achService.GetVendorAccountDetails(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("DeleteFundingSource")]
        [HttpPost]
        public async Task<IActionResult> DeleteFundingSource(GetVendorAccDetailsRequest Request)
        {
            var response = await achService.DeleteVendorAccDetails(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("DeleteACHVendor")]
        [HttpPost]
        public async Task<IActionResult> DeleteACHVendor(DeleteACHVendorRequest Request)
        {
            var response = await achService.DeleteACHVendor(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("AddressValidation")]
        [HttpPost]
        public async Task<IActionResult> AddressValidation(CreateACHVendorRequest Request)
        {
            var response = await achService.HasAddressDetails(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("FundingSource/Validation")]
        [HttpPost]
        public async Task<IActionResult> FundingSourceValidation(CreateAccDetailsRequest Request)
        {
            var response = await achService.AccountDetailsValidations(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
        [Route("FundingCC/Validation")]
        [HttpPost]
        public async Task<IActionResult> FundingccValidation(CreateCCDetailsRequest Request)
        {
            var response = await achService.CCDetailsValidations(Request);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }
    }
}
