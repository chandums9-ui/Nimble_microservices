//using Azure;
using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DataModel;
using BankFeed.Domain.DTO.Model; 
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BankFeed.API.Controllers
{
    [Route("v1/FeedRule")]
    [ApiController] 
    [Authorize]
    [ValidateModel]
    public class FeedRuleController : ControllerBase
    {
        #region Fields

        private readonly IFeedRuleService feedRuleService;

        #endregion

        #region Ctor

        public FeedRuleController(IFeedRuleService _feedRuleService)
        {
            this.feedRuleService = _feedRuleService;
        }

        #endregion

        #region Feedrule

        /// <summary>
        /// To get list of feedrules in a corporation/s
        /// </summary>
        /// <param name="Request">CorpIDs - list of corporation ID/s </param>
        /// <returns> List of feedrules in the corporation/s in FeedRuleListResponse</returns>
        [Route("List")]
        [HttpPost]
        public async Task<IActionResult> GetRuleList( CorpIDsRequest Request)
        {
            FeedRuleListResponse response = null;
            try
            {
                response = await feedRuleService.GetFeedRuleList(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// To load feedrule based on RuleID
        /// </summary>
        /// <param name="Request">Feedrule ID</param>
        /// <returns>Feedrule deatils in FeedRuleLoadResponse</returns>
        [Route("Load")]
        [HttpPost]
        public async Task<IActionResult> GetFeedRule( LoadByLongIDRequest Request)
        {
            FeedRuleLoadResponse response = null;
            try
            {
                response = await feedRuleService.GetFeedRule(Request.ID);
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

        /// <summary>
        /// To create feedrule in a corporation/s
        /// </summary>
        /// <param name="Request">Corporation,Feedaccount,Priority,rulename and related fields</param>
        /// <returns>Created ruleID in FeedRuleResponse</returns>
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateFeedRule(FeedRuleRequest1 Request)
        {
            FeedRuleResponse response = null;
            try
            {
                response = await feedRuleService.CreateFeedRule(Request);
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

        /// <summary>
        /// To update feedrule based on feedruleID
        /// </summary>
        /// <param name="Request">Corporation,Feedaccount,Priority,rulename and related fields</param>
        /// <returns>Updated ruleID in FeedRuleResponse</returns>
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateFeedRule(FeedRuleRequest1 Request)
        {
            FeedRuleResponse response = null;
            try
            {
                response = await feedRuleService.UpdateFeedRule(Request);
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
        [Route("UpdateRule")]
        [HttpPost]
        public async Task<IActionResult> UpdateFeedRules(MultipleFeedRuleRequest Request)
        {
            FeedTransactionMultipleRuleResponse response = null;
            try
            {
                response = await feedRuleService.UpdateFeedRules(Request);
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

        /// <summary>
        /// To delete feedrule and related sub entities
        /// </summary>
        /// <param name="FeedRuleId">Feedrule ID</param>
        /// <returns>Deleted feedruleID in FeedRuleResponse</returns>
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteFeedRule( LoadByLongIDRequest Request)
        {
            FeedRuleResponse response = null;
            try
            {
                response = await feedRuleService.DeleteFeedRule(Request.ID);
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
        [Route("RuleTransCount")]
        [HttpPost]
        public async Task<IActionResult> GetRuleAppiedTrans(LoadByLongIDRequest Request)
        {
            FeedTransactionSearchResponse response = null;
            try
            {
                response = await feedRuleService.GetRuleAppiedTrans(Request.ID);
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

        [Route("GetRuleAccountIdsList")]
        [HttpPost]
        public async Task<IActionResult> GetRuleAccountIdsList(RuleSpecificAccountReq Request)
        {
            RuleBankOrMappingAccountRes response = null;
            try
            {
                response = await feedRuleService.GetSpecificAccountsForRules(Request);
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

        #endregion

        #region FeedSyncTime

        /// <summary>
        /// To set up sync time for connected feed accounts
        /// </summary>
        /// <param name="Request">Synctime,Timezone and ClientID </param>
        /// <returns>Saved FeedSyncSettings ID in FeedSynchResponse</returns>
        [Route("UpdateSyncTime")]
        [HttpPost]
        public async Task<IActionResult> SetupSyncTime(FeedSynchRequest Request)
        {
            FeedSynchResponse response = null;
            try
            {
                Request.ClientID = (string)HttpContext.Items["ClientId"];
                Request.ClientInfoID =Convert.ToSByte(HttpContext.Items["ClientInfoID"]);
                response = await feedRuleService.SetFeedSynchTime(Request);
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

        /// <summary>
        /// Get all the list of TimeZones
        /// </summary>
        /// <returns>List of timezones in TimeZoneListResponse </returns>
        [Route("TimeZones")]
        [HttpGet]
        public async Task<IActionResult> LoadTimeZonesList()
        {
            TimeZoneListResponse response = null;
            try
            {
                response = await feedRuleService.GetTimeZones();
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// To get FeedSynch settings based on ClientID
        /// </summary>
        /// <param name="Request">ClientID</param>
        /// <returns> Auto sync settings time and Tome zone details in TimeZoneViewResponse </returns>
        [Route("LoadSyncTime")]
        [HttpPost]
        public async Task<IActionResult> LoadTimeZoneByClientID(LoadByClientID Request)
        {
            TimeZoneViewResponse response = null;
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"].ToString();
                string clientInfoID = (string)HttpContext.Items["ClientInfoID"].ToString();
                response = await feedRuleService.LoadSyncTimeByClient(clientID, clientInfoID);
                if (response != null )
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        #endregion

    }
}
