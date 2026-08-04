using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Messages.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace WHSubscription.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class PublishController : ControllerBase
    {
        #region Fields
        private IOptions<List<ServerAnlyticsGroup>> serverGroup;
        private readonly ILoggerService logger;
        private readonly IPublishService publishService;
        #endregion

        #region Constructor
        public PublishController(IOptions<List<ServerAnlyticsGroup>> _serverGroup, ILoggerService _logger, IPublishService _publishService)
        {   
            this.serverGroup = _serverGroup;
            this.logger = _logger;
            this.publishService = _publishService;
        }
        #endregion

        #region Private Methods
        private long GetUrlID()
        {
            return Convert.ToInt64((string)HttpContext.Items["UrlID"]);
        }
        private string GetClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        private string GetNPConnection()
        {
            string client = GetClientName();
            string clientServer = "";
            if (serverGroup != null & serverGroup.Value.Count() > 0)
                clientServer = serverGroup.Value.Where(s => s.clients.Contains(client)).Select(s => s.ServerName).FirstOrDefault();
            return (string.IsNullOrEmpty(clientServer) ? "" : clientServer);

        }

        #endregion


        [Route("SynchMessagePublish")]
        [HttpPost]

        public async Task<IActionResult> SynchMessagePublish(WareHouseBinReq req)
        {
           
            try
            {
                SynchMessage sm = new()
                {
                    MessageID = new PFAID(new PFAID().UID).ToString(),
                    TransferConnectionString = GetNPConnection(),
                    EventType = req.EventType??0,
                    ID = req.ID,
                    CloneID = req.CloneID,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    ClientName = GetClientName(),
                    CorpID = req.CorpID,
                    IsUpdatePrevious = req.IsUpdatePrevious,
                    UrlID= GetUrlID()
                };
                StatusDTO response = await publishService.SynchMessagePublish(sm);
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
