using Common.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Resp;
using CoreAccounting.Domain.DTO.Req;
using StackExchange.Redis;

namespace CoreAccounting.API.Controllers
{
    [Route("v1/BFeed_CoreDataForBankfeed")]
    [ApiController]
    [ValidateModel]
    [Authorize]

    public class BFeed_CoreDataForBankfeedController : BaseController
    {
        #region Fields
        private readonly IBFeed_CoreDataForBankfeedService _coreDataForBankfeed;
        #endregion

        #region Constructor
        public BFeed_CoreDataForBankfeedController(IBFeed_CoreDataForBankfeedService coreDataForBankfeed)
        {
            this._coreDataForBankfeed = coreDataForBankfeed;
        }
        #endregion
        #region public Methods
        public string GetUserID
        {
            get
            {
                return (string)HttpContext.Items["UserId"];
            }
        }
        #endregion

        [Route("GetAccountIDsFromNames")]
        [HttpPost]
        public async Task<IActionResult> GetAccountIDsFromNames(AccountAndPurposeNamesRequest PostReq)
        {
            CoreDataForBankfeedResponse response = new CoreDataForBankfeedResponse();
            try
            {
                response = await _coreDataForBankfeed.GetAccountIDsFromNames(PostReq);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);
            }
            catch { throw; }
        }

        [Route("GetNamesFromIdsCacheKeyAsync")]
        [HttpPost]
        public async Task<IActionResult> GetNamesFromIdsCacheKeyAsync( [FromBody] string cacheKey)
        {
            GetNamesFromIdsCacheResponse response = new GetNamesFromIdsCacheResponse();
            try
            {
                response = await _coreDataForBankfeed.GetNamesFromIdsCacheKeyAsync(cacheKey);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);
            }
            catch { throw; }
        }

        [Route("GetPurposeAndAccountNamesFromIDs")]
        [HttpPost]
        public async Task<IActionResult> GetPurposeAndAccountNamesFromIDs(AccountAndPurposeIdsRequest PostReq)
        {
            CoreDataForBankfeedResponse response = new CoreDataForBankfeedResponse();
            try
            {
                response = await _coreDataForBankfeed.GetAccountAndPurposeNamesFromIds(PostReq);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);
            }
            catch { throw; }
        }
        [Route("GetCorporationIdsByUserId")]
        [HttpPost]
        public async Task<IActionResult> GetCorporationIdsByUserId()
        {
            try
            {
                var response = await _coreDataForBankfeed.GetCorporationIdsByUserIdAsync(GetUserID);

                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);

                if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                return NotFound(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("GetAccountNamesByAccountId")]
        [HttpPost]
        public async Task<IActionResult> GetAccountNamesByAccountId([FromBody] string accountIdsKey)
        {
            try
            {
                var response = await _coreDataForBankfeed.GetAccountNamesByAccountId(accountIdsKey);

                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);

                if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                return NotFound(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
