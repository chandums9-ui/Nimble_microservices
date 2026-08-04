using Common.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;

using Common.App.Contracts;
using Common.Domain;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;

using Common.Domain.DTO.Enums;
using DataModel.Domain.DataModel;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.SignalR;
using Azure;
using CoreAccounting.API.Hubs;
namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class SignalRController : BaseController
    {
        #region Fields
        private readonly ILoggerService logger;
        private readonly IHubContext<SignalRHub> signalRContext;

        #endregion

        #region Ctor
        public SignalRController(IHubContext<SignalRHub> _signalRContext, ILoggerService _logger)
        {
            signalRContext = _signalRContext;
            this.logger = _logger;
        }
        #endregion
        /// <summary>
        /// gets taxid's for the client
        /// </summary>
        /// <returns>get's list of taxids,names if found successfully</returns>
        [Route("GetSignalR")]
        [HttpPost]
        public async Task<IActionResult> GetSignalRMessage(SignalRequest signalReq)
        {
            SignalResponse signalResponse = new SignalResponse();
            signalResponse.ID = signalReq.ID;
            signalResponse.IsDelVoidStatus = signalReq.IsDelVoidStatus;
            signalResponse.Data = signalReq.Data;
            if (signalRContext != null && signalRContext.Clients != null)
            {
                signalResponse.StatusCode = StatusCodes.Status200OK;
                await this.signalRContext.Clients.All.SendAsync("ReceiveSplitSave", signalResponse);
                return Ok(signalResponse);
            }
            else
            {
                signalResponse.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(signalResponse);
            }
        }
    }
}
