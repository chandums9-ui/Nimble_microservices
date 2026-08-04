using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts; 
using Common.Domain.DTO.Model.Base;
using Messages.Domain.Synch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net; 
using System.Text;
using System.Text.Json;
using WHSubscription.App.Contract;
using WHSubscription.Domain.DTO.Req;
using WHSubscription.Domain.DTO.Resp;

namespace WHSubscription.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class WhSubscriptionController : ControllerBase
    {
        #region Fields

        private readonly ILoggerService logger;
        private readonly IWhSubcription whService;
        private readonly IPublishService publishService;
        #endregion

        #region Private Methods
        private string GetClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }

        #endregion

        #region Constructor
        public WhSubscriptionController(ILoggerService _logger, IWhSubcription _whService, IPublishService _publishService)
        {
            this.whService = _whService;
            this.logger = _logger;
            this.publishService = _publishService;
        }
        #endregion

        [Route("RegisterOrUpdateRegisterSubscription")]
        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateSubscriptionRegistration(SubscriptionAddRquest Request)
        {
            WhSubcriptionResponse response = null;
            try
            {
                response = await whService.SaveOrUpdateWhSubscription(Request);
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



        [Route("UnregisterSubscription")]
        [HttpPost]
        public async Task<IActionResult> UnregisterSubscription([Required] long subscriptionID)
        {
            WhSubcriptionResponse response = null;
            try
            {
                response = await whService.UnregisterSubscription(subscriptionID);
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

        [Route("CorporationEventPublish")]
        [HttpPost]
        public async Task<IActionResult> CorporationEventPublish(CorporationMessageEvent request)
        {
            try
            {
                StatusDTO response = await publishService.CorporationEventPublish(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new StatusDTO
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Status = $"Failed to publish message: {ex.Message}"
                });
            }
        }

        [Route("PCEventPublish")]
        [HttpPost]
        public async Task<IActionResult> PCEventPublish(PCMessageEvent request)
        {
            try
            {
                StatusDTO response = await publishService.PCEventPublish(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new StatusDTO
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Status = $"Failed to publish message: {ex.Message}"
                });
            }
        }

        [Route("VendorEventPublish")]
        [HttpPost]
        public async Task<IActionResult> VendorEventPublish(VendorMessageEvent request)
        {
            try
            {
                StatusDTO response = await publishService.VendorEventPublish(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new StatusDTO
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Status = $"Failed to publish message: {ex.Message}"
                });
            }
        }

        [Route("ContractEventPublish")]
        [HttpPost]
        public async Task<IActionResult> ContractEventPublish(ContractMessageEvent request)
        {
            try
            {
                StatusDTO response = await publishService.ContractEventPublish(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new StatusDTO
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Status = $"Failed to publish message: {ex.Message}"
                });
            }
        }

        [Route("AccountEventPublish")]
        [HttpPost]
        public async Task<IActionResult> AccountEventPublish(AccountMessageEvent request)
        {
            try
            { 
                request.AccountEventPayload.ClientName = GetClientName();
                StatusDTO response = await publishService.AccountEventPublish(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new StatusDTO
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Status = $"Failed to publish message: {ex.Message}"
                });
            }
        } 
    }
}
 
