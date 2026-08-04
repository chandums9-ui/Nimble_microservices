using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankFeed.API.Controllers
{
    [Route("v1/BFeed_AttentionRequired")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class BFeed_AttentionRequiredController : ControllerBase
    {
        #region Fields
        private readonly IBFeed_AttentionRequiredService AttentionRequiredService;
        #endregion

        #region Ctor
        public BFeed_AttentionRequiredController(IBFeed_AttentionRequiredService _attentionRequiredService)
        {
            this.AttentionRequiredService = _attentionRequiredService;
        }
        #endregion

        [Route("AttentionRequiredGrid")]
        [HttpPost]
        public async Task<IActionResult> AttentionRequiredGrid(BFeed_AttentionRequiredRequest Request)
        {
            BFeed_AttentionRequiredResponse response = null;
            try
            {
                response = await AttentionRequiredService.AttentionRequiredGrid(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }
        [Route("GetMapAccountList")]
        [HttpPost]
        public async Task<IActionResult> GetMapAccountList(BFeed_AttentionRequiredRequest Request)
        {
            BFeed_MapAccountResponse response = null;
            try
            {
                response = await AttentionRequiredService.GetMapAccountList(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }
        [Route("SaveMapAccounts")]
        [HttpPost]
        public async Task<IActionResult> SaveMapAccounts(BFeed_MapAccountRequest Request)
        {
            BFeed_SaveResponse response = null;
            try
            {
                response = await AttentionRequiredService.SaveMapAccounts(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }
        [Route("RemoveMappedAccount")]
        [HttpPost]
        public async Task<IActionResult> RemoveMappedAccount(BFeed_RemoveMapAccountRequest Request)
        {
            BFeed_SaveResponse response = null;
            try
            {
                response = await AttentionRequiredService.RemoveMappedAccount(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

    }
}
