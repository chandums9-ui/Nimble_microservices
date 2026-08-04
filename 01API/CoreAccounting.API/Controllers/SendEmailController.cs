using Common.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Resp;
using CoreAccounting.Domain.DTO.Req;

namespace CoreAccounting.API.Controllers
{
    [Route("v1/SendEmail")]
    [ApiController]
    [ValidateModel]
    
    public class SendEmailController : BaseController
    {
        #region Fields
        private readonly ISendEmailService _emailSender;
        #endregion

        #region Constructor
        public SendEmailController(ISendEmailService emailSender)
        {
            this._emailSender = emailSender;
        }
        #endregion

        [Route("SendEmail")]
        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailMessageRequest PostReq)
        {
            SendEmailResponse response = new SendEmailResponse();
            try
            {
                response = await _emailSender.SendEmailAsync(PostReq);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);
            }
            catch { throw; }
        }
    }
}
