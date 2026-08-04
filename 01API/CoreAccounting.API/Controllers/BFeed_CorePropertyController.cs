using BankFeed.Domain.DTO.Req;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using CoreAccounting.App.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class BFeed_CorePropertyController : BaseController
    {
        #region Fields
        private readonly BFeed_ICoreProperty BFeed_CoreProp;
        #endregion


        #region Ctor
        public BFeed_CorePropertyController(BFeed_ICoreProperty BfeedCoreProp)
        {
            this.BFeed_CoreProp = BfeedCoreProp;
        }
        #endregion

        [HttpPost]
        [Route("GetPurposesForCorporations")]
        public async Task<IActionResult> GetPurposesForCorporations([FromBody] ListofCorpIdReq req)
        {
            PurposeNamesRes response = new PurposeNamesRes();
            string userId = (string)HttpContext.Items["UserId"];
            try
            {
                response = await BFeed_CoreProp.GetPurposesForListOFCorps(req, userId);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
        }


        [Route("CorpSelectionDetails")]
        [HttpPost]
        public async Task<IActionResult> GetCorporationSelectionDetails(List<string> corpIDs)
        {
            CorporationSelectionDetailsResp res = new CorporationSelectionDetailsResp();
            try
            {
                res = await BFeed_CoreProp.GetCorporationSelectionDetails(corpIDs);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
            finally { res = null; }

        }

        [Route("GetDepartmentsByClientID")]
        [HttpGet]
        public async Task<IActionResult> GetDepartmentsByClientID()
        {
            ListOfDepartmentsRes res = new ListOfDepartmentsRes();
            string ClientID = (string)HttpContext.Items["ClientId"];
            try
            {
                res = await BFeed_CoreProp.GetDepartmentsByClientID(ClientID);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
            finally { res = null; }

        }

        [Route("GetAccountsForListOfCorps")]
        [HttpPost]
        public async Task<IActionResult> GetAccountsForListOfCorps(ListOfDistinctAccountNamesReq req)
        {
            ListOfDistinctAccountNamesRes res = new ListOfDistinctAccountNamesRes();
            string ClientId = (string)HttpContext.Items["ClientId"];
            string userId = (string)HttpContext.Items["UserId"];
            try
            {
                res = await BFeed_CoreProp.GetAccountsForListOfCorps(req, userId, ClientId);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
            finally { res = null; }

        }

        [Route("GetAccountNameForexistingPurpose")]
        [HttpPost]
        public async Task<IActionResult> GetAccountNameForexistingPurpose(PurposeAccountNameReq req)
        {
            PurposeAccountNameRes res = new PurposeAccountNameRes();
            string userId = (string)HttpContext.Items["UserId"];
            try
            {
                res = await BFeed_CoreProp.GetAccountNameForexistingPurpose(req, userId);
                if (res != null && res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
                else if (res != null && !string.IsNullOrEmpty(res.Status))
                    return Ok(res);
                else
                    return NotFound(res);
            }
            catch { throw; }
            finally { res = null; }
        }

        [Route("GetNamesForMultipleCorps")]
        [HttpPost]
        public async Task<IActionResult> GetNamesForMultipleCorps(List<string> CorporationIds)
        {
            NameAndTypeRes res = new NameAndTypeRes();
            string userId = (string)HttpContext.Items["UserId"];
            try
            {
                res = await BFeed_CoreProp.GetNamesForMultipleCorps(CorporationIds, userId);
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
