using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Microsoft.AspNetCore.Mvc;

namespace BankFeed.API.Controllers
{
    [Route("v1/NewFeedRule")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    public class BFeed_NewFeedRuleController : Controller
    {
        #region Fields
        private readonly BFeed_INewFeedRuleService NewfeedRuleService;
        #endregion

        #region Ctor
        public BFeed_NewFeedRuleController(BFeed_INewFeedRuleService _newFeedRuleService)
        {
            this.NewfeedRuleService = _newFeedRuleService;
        }
        #endregion


        #region Private Methods
        private string getUserID()
        {
            return (string)HttpContext.Items["UserId"];
        }
        #endregion

        [Route("GetAccountFeedRuleSettingsForIO")]
        [HttpGet]
        public async Task<IActionResult> GetAccountFeedRuleSettingsForIO(string AccountID)
        {
            SpecificAccountFeedRulesResp res = new SpecificAccountFeedRulesResp();
            try
            {
                res = await NewfeedRuleService.GetFeedRulesForSpecificAccount(AccountID);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
        }


        [Route("FeedRuleViewGrid")]
        [HttpPost]
        public async Task<IActionResult>FeedRuleViewGrid(FeedRuleViewGridReq req)
        {
            FeedRuleViewGridRes res = new FeedRuleViewGridRes();
            try
            {
                res = await NewfeedRuleService.FeedRuleViewGrid(req, getUserID());
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
        }

        [Route("SaveOrUpdateFeedRule")]
        [HttpPost]
        public async Task<IActionResult>SaveOrUpdateFeedRule(SaveOrUpdateFeedRuleReq req)
        {
            SaveOrUpdateFeedRuleRes res = new SaveOrUpdateFeedRuleRes();
            try
            {
                res = await NewfeedRuleService.SaveOrUpdateFeedRule(req);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
        }


        [Route("GetFeedRuleDetails")]
        [HttpPost]
        public async Task<IActionResult>GetFeedRuleDetails(LoadFeedRuleDataReq req)
        {
            LoadFeedRuleDataResp res = new LoadFeedRuleDataResp();
            try
            {
                res = await NewfeedRuleService.GetFeedRuleDetails(req);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
        }

        [Route("DeleteNewFeedRule")]
        [HttpPost]
        public async Task<IActionResult>DeleteNewFeedRule([FromBody]  long FeedRuleId)
        {
            SaveOrUpdateFeedRuleRes res = new SaveOrUpdateFeedRuleRes();
            try
            {
                res = await NewfeedRuleService.DeleteNewFeedRule(FeedRuleId);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
        }

        [Route("GetCategories")]
        [HttpPost]
        public async Task<IActionResult>GetCategoryNames(CategoryReq req)
        {
            CategoryResp res = new CategoryResp();
            try
            {
                res = await NewfeedRuleService.GetCategoryNames(req);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
        }


        [Route("GenerateRuleId")]
        [HttpGet]
        public async Task<IActionResult>GenerateRuleId(int CategoryType)
        {
            UniqueRuleIdRes res = new UniqueRuleIdRes();
            try
            {
                res = await NewfeedRuleService.GenerateRuleId(CategoryType);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
        }

       
    }
}
