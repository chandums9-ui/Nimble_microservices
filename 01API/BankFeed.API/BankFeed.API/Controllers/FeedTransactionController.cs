using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Microsoft.AspNetCore.Mvc;
using BankFeed.Domain.Enums;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Req;
//using Azure;
using Common.Domain.DTO.Resp;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using BankFeed.API.Hubs;
using Microsoft.IdentityModel.Tokens;

namespace BankFeed.API.Controllers
{
    [Route("v1/FeedTransactions")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class FeedTransactionController : ControllerBase
    {

        #region Fields
        private readonly IFeedTransactionService feedTransactionService;
        private readonly IHubContext<FeedHub> feedHubContext;
        #endregion

        #region Constructor
        public FeedTransactionController(IFeedTransactionService _feedTransactionService, IHubContext<FeedHub> _feedHubContext)
        {
            this.feedTransactionService = _feedTransactionService;
            this.feedHubContext = _feedHubContext;
        }

        #endregion

        /// <summary>
        /// Gets the lookup details of feedaccoutnTransaction screen based on FeedAccountID
        /// </summary>
        /// <param name="LookupRequest"></param>
        /// <returns>It returns FeedTransactionLookUpResponse</returns>
        [Route("Lookup")]
        [HttpPost]
        public async Task<IActionResult> GetFeedAccountTransactionsLookUp(FeedTransactionLookupRequest LookupRequest)
        {
            FeedTransactionLookUpResponse response = null;

            try
            {
                response = await feedTransactionService.GetFeedAccountTransactionsLookUp(HttpContext.Items["ClientId"].ToString(), LookupRequest);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("MultiReqLookup")]
        [HttpPost]
        public async Task<IActionResult> GetMultipleFeedAccountTransactionsLookUp(MultipleFeedTransactionLookupRequest LookupRequest)
        {
            MultipleFeedTransactionLookUpResponse response = null;

            try
            {
                response = await feedTransactionService.GetMultipleFeedAccountTransactionsLookUp(HttpContext.Items["ClientId"].ToString(), LookupRequest);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }
        /// <summary>
        /// get's the AccountSummary for all the accounts linked with the given InstituitionID excluding the given FeedAccountID
        /// </summary>
        /// <param name="Request">This contains FeedAccountID,InstitutionID</param>
        /// <returns>It returns List of AccountSummaries in GetMoreAccountsResponse</returns>
        [Route("MoreAccounts")]
        [HttpPost]
        public async Task<IActionResult> GetMoreAccounts(FeedTransactionLookupRequest Request)
        {
            GetMoreAccountsResponse response = null;
            try
            {
                response = await feedTransactionService.GetMoreAccounts(HttpContext.Items["ClientId"].ToString(),Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// Gets the list of transactions based on FeedAccountID,InstitutionID and Related Filters
        /// </summary>
        /// <param name="SearchReq">contains AccID and Filter Parameters</param>
        /// <returns>It returns List of Transactions based on Filters as FeedTransactionSearchResponse</returns>
        [Route("List")]
        [HttpPost]
        public async Task<IActionResult> GetFeedAccountTransactions(FeedTransactionSearchRequest SearchReq)
        {
            FeedTransactionSearchResponse response = null;
            DateTime FromDate = default(DateTime);
            DateTime ToDate = default(DateTime);
            try
            {
                if (SearchReq != null)
                {
                    if (!string.IsNullOrEmpty(SearchReq.ShortFromDate))
                        DateTime.TryParse(SearchReq.ShortFromDate, out FromDate);
                    if (!string.IsNullOrEmpty(SearchReq.ShortToDate))
                        DateTime.TryParse(SearchReq.ShortToDate, out ToDate);

                    if (FromDate != default(DateTime))
                        SearchReq.FromDate = FromDate.Date;

                    if (ToDate != default(DateTime))
                        SearchReq.ToDate = ToDate.AddDays(1).AddTicks(-1); //today EOD
                }

                response = await feedTransactionService.GetFeedAccountTransactions(SearchReq);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// Applying matched Feedrules to FeedTransactions based on FeedAccID
        /// </summary>
        /// <param name="RuleApplyReq">This contains FeedAccountID,InstitutionID</param>
        /// <returns>It returns FeedTransactionID's in which FeedRule is Applied as FeedTransactionRuleApplyResponse</returns>
        [Route("ApplyRule")]
        [HttpPost]
        public async Task<IActionResult> ApplyFeedRulesonFeedTransactions(ApplyRuleRequest RuleApplyReq)
        {
            FeedTransactionRuleApplyResponse response = null;
            try
            {
                response = await feedTransactionService.ApplyFeedRulesonFeedTransactions(RuleApplyReq);
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


        [Route("MultipleApplyRule")]
        [HttpPost]
        public async Task<IActionResult> MultipleApplyFeedRulesonFeedTransactions(MultipleApplyRuleRequest RuleApplyReq)
        {
            MultipleRuleApplyResponse response = null;
            try
            {
                response = await feedTransactionService.MultipleApplyFeedRulesonFeedTransactions(RuleApplyReq);
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
        /// It returns mapped data if Feedrule is applied otherwise default data is sent
        /// </summary>
        /// <param name="FeedTransactionReq">It contains FeedTransactionID</param>
        /// <returns>It returns mapped data as GetAppliedRuleResponse</returns>
        [Route("AppliedRule")]
        [HttpPost]
        public async Task<IActionResult> GetAppliedRule(LoadByLongIDRequest FeedTransactionReq)
        {
            GetAppliedRuleResponse response = null;
            try
            {
                response = await feedTransactionService.GetAppliedRule(FeedTransactionReq);
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
        [Route("Split")]
        [HttpPost]
        public async Task<IActionResult> SaveSplitData(SplitPostResponse FeedTransactionReq)
        {
            FeedTransactionMappingResponse response = null;
            try
            {
                //To send failure callback to UI
                if (FeedTransactionReq == null || (FeedTransactionReq != null && FeedTransactionReq.FeedTransactionID <= 0))
                {
                    if (this.feedHubContext != null && this.feedHubContext.Clients != null)
                    {
                        response = new FeedTransactionMappingResponse();
                        await this.feedHubContext.Clients.All.SendAsync("ReceiveSplitSave", response);
                    }
                }

                response = await feedTransactionService.SaveSplitData(FeedTransactionReq);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                {
                    //SignalR callback to UI, to verify FeedTransactionID and refresh the search transactions in UI
                    if (this.feedHubContext != null && this.feedHubContext.Clients != null)
                    {
                        response.StatusCode = StatusCodes.Status200OK;
                        response.FormulaStatus = Convert.ToInt16(FeedStatusEnum.Active);
                        response.FeedTransactionID = FeedTransactionReq.FeedTransactionID;
                        response.JournalID = FeedTransactionReq.JournalID;

                        await this.feedHubContext.Clients.All.SendAsync("ReceiveSplitSave", response);
                    }
                    return Ok(response);
                }
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                {
                    if (this.feedHubContext != null && this.feedHubContext.Clients != null)
                    {
                        response.FeedTransactionID = FeedTransactionReq.FeedTransactionID;
                        await this.feedHubContext.Clients.All.SendAsync("ReceiveSplitSave", response);
                    }
                    return Ok(response);
                }
                else
                {
                    if (this.feedHubContext != null && this.feedHubContext.Clients != null)
                    {
                        if (response == null)
                            response = new FeedTransactionMappingResponse();
                        response.FeedTransactionID = FeedTransactionReq.FeedTransactionID;

                        await this.feedHubContext.Clients.All.SendAsync("ReceiveSplitSave", response);
                    }
                    return NotFound(response);
                }
            }
            catch { throw; }
            finally { response = null; }
        }
        [Route("MovingTransToOpenFeeds")]
        [HttpPost]
        public async Task<IActionResult> MovingTransToOpenFeeds(LoadByLongIDRequest FeedTransactionReq)
        {
            FeedTransactionResponse response = null;
            try
            {
                response = await feedTransactionService.MovingTransToOpenFeeds(FeedTransactionReq);
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
        /// It creates feedrule for posted data if enabled in single post case only 
        /// </summary>
        /// <param name="Request">It has singlepostrequest and singlepostresponse</param>
        /// <returns>It Returns FeedRuleResponse</returns>
        [Route("CreatePostedRule")]
        [HttpPost]
        public async Task<IActionResult> CreatePostedFeedRule(PostingFeedRuleRequest Request)
        {
            FeedRuleResponse response = null;
            try
            {
                response = await feedTransactionService.CreateFeedRule(Request);
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
        /// It creates feedrule for posted data if enabled in single post case only 
        /// </summary>
        /// <param name="Request">It has singlepostrequest and singlepostresponse</param>
        /// <returns>It Returns FeedRuleResponse</returns>
        [Route("CreateMultiplePostedRule")]
        [HttpPost]
        public async Task<IActionResult> CreateMultiplePostedFeedRule(MultiplePostingFeedRuleRequest Request)
        {
            FeedTransactionMultipleRuleResponse response = null;
            try
            {
                response = await feedTransactionService.CreateMultipleFeedRule(Request);
                if (response != null )
                    return Ok(response);
                else if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It save Posted Transactions data in feedtransaction mapping and update the transaction status
        /// </summary>
        /// <param name="PostReq"></param>
        /// <returns>It returns FeedTransactionMultipleMapResponse</returns>
        [Route("MapMultiplePost")]
        [HttpPost]
        public async Task<IActionResult> SaveMultiplePostedData(MultiplePostResponse PostReq)
        {
            FeedTransactionMultipleMapResponse response = null;
            try
            {
                response = await feedTransactionService.SaveMultiplePostedData(PostReq);
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
        /// It saves posted transaction data in FeedTransactionMapping
        /// </summary>
        /// <param name="MappingRequest"></param>
        /// <returns>It returns FeedTransactionMappingResponse</returns>
        [Route("MapSinglePost")]
        [HttpPost]
        public async Task<IActionResult> SaveSinglePostedData(SinglePostResponse MappingRequest)
        {
            FeedTransactionMappingResponse response = null;
            try
            {
                response = await feedTransactionService.SaveSinglePostedData(MappingRequest);
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
        /// It Maps the Already Posted NimbleTransactions Data in FeedTransactionMapping if it Matches with the FeedTransaction Data
        /// </summary>
        /// <param name="Request">This Contains FeedTransactionID,FeedTransactionMapping Details </param>
        /// <returns>It returns Mapped NimbleTransactionID's as FeedTrnsactionMatchResponse</returns>
        [Route("MultipleMatch")]
        [HttpPost]
        public async Task<IActionResult> MultipleMatch(GroupMatchRequest Request)
        {
            List<MultipleFeedTrnsactionMatchResponse> response = null;
            try
            {
                response = await feedTransactionService.MultipleMatch(Request);
                
                if (response != null && response.Any())
                    return Ok(response);
                else if (response != null && response.Any())
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It Maps the Already Posted NimbleTransactions Data in FeedTransactionMapping if it Matches with the FeedTransaction Data
        /// </summary>
        /// <param name="Request">This Contains FeedTransactionID,FeedTransactionMapping Details </param>
        /// <returns>It returns Mapped NimbleTransactionID's as FeedTrnsactionMatchResponse</returns>
        [Route("MatchandPost")]
        [HttpPost]
        public async Task<IActionResult> FeedTransactionMatchandPost(FeedTransactionMatchRequest Request)
        {
            FeedTrnsactionMatchResponse response = null;
            try
            {
                response = await feedTransactionService.FeedTransactionMatchAndMap(Request);
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
        /// It gets the Data in FeedTransactionMapping Based on FeedTransactionID
        /// </summary>
        /// <param name="FeedTransactionID">It Contains FeedTransactionID</param>
        /// <returns>It returns Mapped NimbleTransactionDetails as MappedDataResponse</returns>
        [Route("LoadMatchedandPosted")]
        [HttpPost]
        public async Task<IActionResult> LoadMatchedorPostedFeedTransaction(LoadByLongIDsRequest Request)
        {
            MappedDataResponse response = null;
            try
            {
                response = await feedTransactionService.GetMappedFeedTransactionData(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// Gets List of PossibleMatch Transaction based on Specified Filters
        /// </summary>
        /// <param name="Request">It Contains FeedTransactionID's,ClientID,RequestID</param>
        /// <returns>It returns List of Matched NimbleTransactions as PossibleMatchResponse </returns>
        [Route("PossibleMatches")]
        [HttpPost]
        public async Task<IActionResult> LoadPossibleMatches(PossibleMatchRequest Request)
        {
            PossibleMatchResponse response = null;
            try
            {
                Request.ClientID = "0x" + HttpContext.Items["ClientId"].ToString();
                response = await feedTransactionService.GetPossibleMatches(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }
        [Route("MultiplePossibleMatches")]
        [HttpPost]
        public async Task<IActionResult> LoadMultiplePossibleMatches(MultiplePossibleMatchRequest Request)
        {
            
            PossibleMatchResponse response = null;
            try
            {
                
                string ClientID = "0x" + HttpContext.Items["ClientId"].ToString();
                response = await feedTransactionService.MultiplePossibleMatchTransactions(Request, ClientID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// Gets List of BillMatch Entries based on Specified Filters 
        /// </summary>
        /// <param name="Request">It Contains FeedTransactionID's,ClientID,RequestID</param>
        /// <returns>It returns List of Matched BillEntryTransactions as PossibleMatchResponse</returns>
        [Route("BillMatches")]
        [HttpPost]
        public async Task<IActionResult> LoadBillMatches(PossibleMatchRequest Request)
        {
            PossibleMatchResponse response = null;
            try
            {
                Request.ClientID = "0x" + HttpContext.Items["ClientId"].ToString();
                response = await feedTransactionService.GetBillMatches(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }


        /// <summary>
        /// To load Possible or Bill matches
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [Route("LoadPossibleAndBillMatches")]
        [HttpPost]
        public async Task<IActionResult> LoadMatches(LoadByIDRequest Request)
        {
            PossibleMatchResponse response = null;
            try
            {
                response = await feedTransactionService.LoadPossibleAndBillMatches(Request.ID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// Performs Specified Action in the TransactionGrid filter on Selected  Transactions 
        /// </summary>
        /// <param name="Request">It Contains paramaters related to all actions in Filter</param>
        /// <returns>It returns List/Id's based on selected Action in Filter as GroupTransactionsFilterRsponse</returns>
        [Route("TransactionFilterAction")]
        [HttpPost]
        public async Task<IActionResult> FilterActioninFeedTransactions(TransactionActionRequest request)
        {
            GroupTransactionsFilterRsponse response = null;
            try
            {
                response = await feedTransactionService.FeedTransactionsFilterActions(request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// For failed cases for transaction operations in bankfeed,feedtransaction will be reverted and status chnaged
        /// </summary>
        /// <param name="Request">Feed transaction ID and Failed case status of the operation </param>
        /// <returns>return true/false as revert operation is success or failed</returns>
        [Route("TransactionRevert")]
        [HttpPost]
        public async Task<IActionResult> RevertFailedTranasactions(TransactionRevertRequest request)
        {
            try
            {
                var response=  await feedTransactionService.TransactionRevert(request);
                if (response)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally {  }
        }

        [Route("GetLatestCheckNum")]
        [HttpPost]
        public async Task<IActionResult> GetlatestCheckNum(CheckNumReq request)
        {
            try
            {
                var response = await feedTransactionService.GetLatestCheckNum(request);
                if (response!=null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { }
        }

    }
}
