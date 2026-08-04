//using Azure;
using AutoMapper.Internal;
using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;

namespace BankFeed.API.Controllers
{
    [Route("v1/Feeds")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class FeedAccountsController : ControllerBase
    {
        #region Fields
        private readonly IFeedAccountService feedaccountService;
        #endregion

        #region Ctor
        public FeedAccountsController(IFeedAccountService _feedaccountService)
        {
            this.feedaccountService = _feedaccountService;
        }
        #endregion


        /// <summary>
        /// To get list of connected accounts
        /// </summary>
        /// <param name="Request"> string of corporation id's separated with commas and accStatusFilter (0- accounts overview,1-Active, 2-Pending & Failed,3- Archive)</param>
        /// <returns>list of connected feed sessions </returns>
        [Route("ConnectedAccounts")]
        [HttpPost]
        public async Task<IActionResult> GetConnectedFeedAccounts(FeedAccountRequest Request)
        {
            ConnectedFeedsResponse response = null;
            try
            {
                response = await feedaccountService.GetConnectedFeeds(HttpContext.Items["ClientId"].ToString(), Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// To get list of connected accounts
        /// </summary>
        /// <param name="Request"> string of corporation id's separated with commas and accStatusFilter (0- accounts overview,1-Active, 2-Pending & Failed,3- Archive)</param>
        /// <returns>list of connected feed sessions </returns>
        [Route("CashAndCardDuesAsPerFeeds")]
        [HttpPost]
        public async Task<IActionResult> GetCashAndCardDuesAsPerFeeds(CashAndCardDuesAsPerFeedsRequest Request)
        {
            CashAndCardDuesAsPerFeedsResponse response = null;
            try
            {
                response = await feedaccountService.GetCashAndCardDuesAsPerFeeds(HttpContext.Items["ClientId"].ToString(), Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It gets list of FeedInstitutions Based on ProviderRregisterID
        /// </summary>
        /// <param name="Request">registered provider ID of the client</param>
        /// <returns>It returns InstitutionsListResponse </returns>      
        [Route("ConnectedInstitutions")]
        [HttpPost]
        public async Task<IActionResult> GetConnectedInstitutions([FromBody] LoadByLongIDRequest Request)
        {
            InstitutionsListResponse response = null;
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];

                response = await feedaccountService.GetConnectedInstitutions(Request.ID, clientID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It gets list of ConnectedFeedAccounts based on corporation ID
        /// </summary>
        /// <param name="CorpID"></param>
        /// <returns>returns FeedAccountsList as FeedAccountsListResponse</returns>
        [Route("AccountsByCorpID")]
        [HttpPost]
        public async Task<IActionResult> GetFeedAccountsListByCorpID(CorpIDRequest Request)
        {
            FeedAccountsListResponse response = null;
            try
            {
                response = await feedaccountService.GetFeedAccountsByCorpID(Request.CorpID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It gets list of ConnectedFeedAccounts based on institute ID
        /// </summary>       
        /// <param name="Request"></param>
        /// <returns>List of Account details as AccountMappingListResponse </returns>
        [Route("AccountsByInsID")]
        [HttpPost]
        public async Task<IActionResult> GetFeedAccountsListByInsID(InstitutionRequest Request)
        {
            AccountMappingListResponse response = null;
            try
            {
                response = await feedaccountService.GetFeedAccountsByInstID(Request.ID, Request.AccStatusFilter, Request.AccStatus);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// Delete or Archive Feed Account based on Lists of ID
        /// </summary>
        /// <param name="Request">List of AccountIds, IsDeleted and IsDiconnected</param>
        /// <returns>It returns the status of the operation done</returns>
        [Route("Account/Delete")]
        [HttpPost]
        public async Task<IActionResult> FeedAccountDelete(FeedAccountDeleteRequest Request)
        {
            FeedAccountInfoResponse response = null;
            try
            {
                string ClientInfoID =(HttpContext.Items["ClientInfoID"].ToString());

                response = await feedaccountService.FeedAccountDelete(Request, ClientInfoID);
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
        /// Update FeedAccount and FeedAccountMapping Tables based on FeedAccountID
        /// </summary>
        /// <param name="Request">List of AccountIds, IsDeleted and IsDiconnected</param>
        /// <returns>It returnsFeedAccountInfoResponse</returns>
        [Route("Account/Mapping")]
        [HttpPost]
        public async Task<IActionResult> FeedAccountMapping(FeedAccountMappingRequest Request)
        {
            FeedAccountInfoResponse response = null;
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                string clientName = (string)HttpContext.Items["ClientName"];
                string ClientInfoID = (HttpContext.Items["ClientInfoID"].ToString());

                    // await feedaccountService.MappedAccountsSync(Request, clientID, clientName);

                response = await feedaccountService.FeedAccountMapping(Request, clientID, clientName, ClientInfoID);
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
        /// To  Archive Account based on FeedAccount ID
        /// </summary>
        /// <param name="ID">FeedAccount ID</param>
        /// <returns> It returns FeedAccountsStatusResponse</returns>
        [Route("Account/Archive")]
        [HttpPost]
        public async Task<IActionResult> FeedAccountArchive(LoadByLongIDRequest Request)
        {
            FeedAccountsStatusResponse response = null;
            try
            {
                response = await feedaccountService.AccountArchive(Request.ID);
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

        [Route("Account/MoveToActive")]
        [HttpPost]
        public async Task<IActionResult> MoveArchiveToActive(AccountActiveRequest Request)
        {
            FeedAccountsStatusResponse response = null;
            try
            {
                response = await feedaccountService.MoveArchiveToActive(Request);
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
        /// This call Saves/updates the mergesettings in FeedAccountMergeSettings
        /// </summary>
        /// <param name="MapRequest">contains list of lineid's and merge setting to match</param>
        /// <returns>It returns FeedAccountID and response as MapMergeSettingsResponse</returns>
        [Route("Account/MergeSettings/Save")]
        [HttpPost]
        public async Task<IActionResult> SaveMergeSettings(MergeSettingsRequest MapRequest)
        {
            MapMergeSettingsResponse response = null;
            try
            {
                response = await feedaccountService.SaveMergeSettings(MapRequest);
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


        [Route("CheckingFeedAccMapping")]
        [HttpPost]
        public async Task<IActionResult> CheckingFeedAccMapping(FeedAccMappingCheckingRequest Request)
        {
            try
            {
                string clientID = HttpContext.Items["ClientId"].ToString();
                //Request.ClientID = clientID;
                FeedAccountCOAChangeResponse genResp = await feedaccountService.CheckingFeedAccMapping(Request);
                if (genResp != null) { return Ok(genResp); }
                else
                    return NotFound(genResp);
            }
            catch { throw; }
        }

        [Route("ChangingFeedAccMapping")]
        [HttpPost]
        public async Task<IActionResult> ChangingFeedAccMapping(FeedAccMappingChangingRequest Request)
        {
            try
            {
                string clientID = HttpContext.Items["ClientId"].ToString();
                //Request.ClientID = clientID;
                GenericLongResponse genResp = await feedaccountService.ChangingFeedAccMapping(Request);
                if (genResp != null) { return Ok(genResp); }
                else
                    return NotFound(genResp);
            }
            catch { throw; }
        }
        /// <summary>
        /// Loads already mapped mergesettings
        /// </summary>
        /// <param name="FeedAccount">contains feedaccountID</param>
        /// <returns>Returns list of mapped settings</returns>
        [Route("Account/MergeSettings/Load")]
        [HttpPost]
        public async Task<IActionResult> LoadMergeSettings(LoadByLongIDRequest FeedAccount)
        {
            MergeSettingsListResponse response = null;
            try
            {
                response = await feedaccountService.LoadMergeSettings(FeedAccount);
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

        [Route("Account/MergeSettings/Copy")]
        [HttpPost]
        public async Task<IActionResult> CopyMergeSetttings(BankAccountMergeSettingsReq req)
        {
            BankMergeSettingsResponse response = await feedaccountService.SaveOrUpdateBankAccountMerging(req);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);

        }
        [Route("Account/MergeSettings/Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteCopyMergeSetttings(BankAccountMergeSettingsDefault req)
        {
            BankMergeSettingsResponse response = await feedaccountService.DeleteBankAccountMerging(req);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);

        }
        [Route("GetSpecificAccountsForFeeds")]
        [HttpPost]
        public async Task<IActionResult> GetSpecificAccountsForFeeds(FeedSpecificAccountReq req)
        {
            FeedMappingAccountRes response = await feedaccountService.GetSpecificAccountsForFeeds(HttpContext.Items["ClientId"].ToString(), req);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);

        }

    }
}
