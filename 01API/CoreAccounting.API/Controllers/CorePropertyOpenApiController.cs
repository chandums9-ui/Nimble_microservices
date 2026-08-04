using Common.App.Contracts;
using Common.Domain;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Common.Domain.DTO.Model;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using Common.Infra.Cache;
using Azure.Core;
using Azure;
using Common.Domain.DTO.Enums;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;

namespace CoreAccounting.API.Controllers
{

    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class CorePropertyOpenApiController : BaseController
    {

        #region Fields
        private readonly ICoreProperty coreProp;
        private readonly IFileService fileSrv;
        private readonly ICacheService cache;
        private readonly ILoggerService logger;
        private IOptions<List<ServerAnlyticsGroup>> serverGroup;
        //private readonly JWTSettingsDTO jwtSettingsDTO; 

        #endregion

        #region Ctor
        public CorePropertyOpenApiController(ICoreProperty coreProperty, IFileService fileService, ICacheService _cache, ILoggerService _logger, IOptions<List<ServerAnlyticsGroup>> _serverGroup)//, IOptions<JWTSettingsDTO>jwtSettingsDto )
        {
            this.coreProp = coreProperty;
            fileSrv = fileService;
            cache = _cache;
            logger = _logger;
            this.serverGroup = _serverGroup;
            //this.jwtSettingsDTO = jwtSettingsDto.Value;
        }

        #endregion

        #region PandL
        private long getUrlID()
        {
            return Convert.ToInt64((string)HttpContext.Items["UrlID"]);
        }
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        private string getNPConnection()
        {
            string client = getClientName();
            string clientServer = "";
            if (serverGroup != null & serverGroup.Value.Count() > 0)
                clientServer = serverGroup.Value.Where(s => s.clients.Contains(client)).Select(s => s.ServerName).FirstOrDefault();
            return (string.IsNullOrEmpty(clientServer) ? "" : clientServer);

        }
        /// <summary>
        /// It will get all the profit and loss accounts with balances in the corporation
        /// </summary>
        /// <param name="data">data represents CorpSearchRequest</param>
        /// <returns>It will display all the profit and loss accounts with balances in the corporation</returns>

        [Route("profitloss")]
        [HttpGet]
        public async Task<IActionResult> GetPandL([FromQuery] CorpSearchRequest data)
        {
            PandLDTO? pl = null;
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    if (DateTime.Parse(data.FromDate) < DateTime.Parse(data.ToDate))
                    {
                        pl = await cp.GetPandL(data);
                    }
                    if (pl != null)
                        return Ok(pl);
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
                }
            }
            catch { throw; }
            finally { pl = null; }
        }
        #endregion

        /// <summary>
        /// It  will get all the accounts with balances in the corporation 
        /// </summary>
        /// <param name="data">data represents the CorpIDAndAccTypeIDRequest</param>
        /// <returns>It will display all the accounts with balances in the corporation </returns>
        [Route("accountbal")]
        [HttpPost]
        public async Task<IActionResult> GetAccountBalances(CorpIDAndAccBalancesRequest data)
        {
            try
            {
                return Ok(await coreProp.GetAccountBalances(data));
            }
            catch { throw; }
        }

        [Route("IO/corporations")]
        [HttpGet]
        public async Task<IActionResult> GetCorporationDetails( [FromQuery] short Status = 2)
        {
            IOCorporationsResponse corp = new IOCorporationsResponse();
            try
            {
                corp = await coreProp.GetCorpsList(Status);
                return Ok(corp);

            }
            catch { throw; }
        }
        [Route("speficficaccounts")]
        [HttpPost]
        public async Task<IActionResult> GetAccountBalancesByIds(AccIDAndAccBalancesRequest data)
        {
            try
            {
                return Ok(await coreProp.GetAccountBalancesByAccIDs(data));
            }
            catch { throw; }
        }

        [Route("GetLegalNameorDbName")]
        [HttpGet]
        public async Task<IActionResult> GetLegalNameOrDbName()
        {
            try
            {
                string userId = (string)HttpContext.Items["UserId"];
                return Ok(await coreProp.GetLegalNameOrDbName(userId));
            }
            catch { throw; }
        }

        [Route("DefaultPaymentMethod")]
        [HttpGet]
        public async Task<IActionResult> GetDefaultPaymentMethod()
        {
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                return Ok(await coreProp.GetDefaultPaymentMethod(clientID));
            }
            catch { throw; }
        }

        [Route("ValidatePaymethodNumber")]
        [HttpPost]
        public async Task<IActionResult> ValidatePaymethodNumber(CheckOrVoucherNumValidationReq req)
        {
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                return Ok(await coreProp.ValidateCheckOrVoucherNumbByAccountAndPaymethoID(req));
            }
            catch { throw; }
        }

        [Route("GetLatestCheckNum")]
        [HttpPost]
        public async Task<IActionResult> GetLatestCheckNum(CheckOrVoucherNumValidationReq req)
        {
            try
            {
                return Ok(await coreProp.GetLatestCheckNum(req));
            }
            catch { throw; }
        }
        [Route("RunYEP")]
        [HttpPost]
        public async Task<IActionResult> RunYEP(CorpIDRequest req)
        {
            try
            {
                GenericStringResponse gresp = await coreProp.RunYEPProcess(req, getUrlID(), getClientName(), getNPConnection());
                return Ok(gresp);
            }
            catch { throw; }
        }
        #region Core

        /// <summary>
        /// It will return LatestEntryNumber Based on CorporationId and JournalType
        /// </summary>
        /// <param name="data">data represents EntryNumberRequest class</param>
        /// <returns>It returns EntryNumber,StatusCode,StatusMessage</returns>
        [Route("entrynumber")]
        [HttpPost]
        public async Task<IActionResult> GetEntryNumber(EntryNumberRequest data)
        {
            try
            {
                EntryNumberResponse entryNumberResp = await coreProp.GetEntryNumber(data);
                if (entryNumberResp != null && entryNumberResp.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(entryNumberResp.EntryNumber))
                    return Ok(entryNumberResp);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(entryNumberResp.Status)) ? entryNumberResp.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
        }


        /// <summary>
        /// It displays List of corporations based on UserID
        /// </summary>
        /// <returns>It will displays List of Corporations</returns>
        [Route("corporations")]
        [HttpGet]
        public async Task<IActionResult> GetCorporations(bool IsShowAll = false)
        {
            // logger.LogTrace("This is Test");
            CorporationsResponse corp = new CorporationsResponse();
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    string clientID = (string)HttpContext.Items["ClientId"];

                    string userId = (string)HttpContext.Items["UserId"];
                    if (!IsShowAll && string.IsNullOrEmpty(userId)) { return NotFound(corp); }
                    //string cacheKey = string.Concat((!IsShowAll ? userId : clientID), "CorpList");
                    //corp = cache.GetData<CorporationsResponse>(cacheKey);

                    if (corp.Corporations == null)
                    {
                        corp = await cp.GetCorporations(IsShowAll ? string.Empty : userId, clientID);
                        // cache.SetData<CorporationsResponse>(cacheKey, corp);
                    }
                    //logger.LogTrace("This is end Test");
                    if (corp == null || corp.Corporations == null)
                        corp = new CorporationsResponse();
                    return Ok(corp);

                }
            }
            catch { throw; }
        }


        [Route("SavePurpose")]
        [HttpPost]
        public async Task<IActionResult> SavePurpose(SavePurposeRequest data)
        {
            try
            {
                SavePurposeResponse al;
                using (ICoreProperty cp = coreProp)
                {
                    al = await cp.SavePurposeData(data);
                    return Ok(al);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// It will displays AccountList
        /// </summary>
        /// <param name="data">Data represents CorpIDAndAccTypeIDRequest</param>
        /// <returns>It will displays List of Accounts</returns>
        [Route("accounts")]
        [HttpGet]
        public async Task<IActionResult> GetAccounts([FromQuery] CorpIDAndAccTypeIDRequest data)
        {
            try
            {
                AccountListResponse al;
                using (ICoreProperty cp = coreProp)
                {
                    ////var userId = HttpContext.Items["UserId"];
                    //string cacheKey = string.Concat(data.CorpID, "AccList");
                    //al = cache.GetData<AccountListResponse>(cacheKey);
                    //if (al == null)
                    //{
                    //    al = await cp.GetAccounts(data);
                    //    cache.SetData<AccountListResponse>(cacheKey, al);
                    //}
                    string clientID = (string)HttpContext.Items["ClientId"];
                    al = await cp.GetAccounts(data, clientID);
                    if (al == null || al.Accounts == null)
                        al = new AccountListResponse();
                    return Ok(al);
                }
            }
            catch { throw; }
        }


        /// <summary>
        /// It will displays AccountList but not the types accountpayable and account recievable
        /// </summary>
        /// <param name="data">Data represents CorpIDAndAccTypeIDRequest</param>
        /// <returns>It will displays List of Accounts</returns>
        [Route("GetPayableaccounts")]
        [HttpGet]
        public async Task<IActionResult> GetPayableAccounts(string corpID)
        {

            AccountListResponse al;
            using (ICoreProperty cp = coreProp)
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                al = await cp.GetPayableAccounts(new CorpIDAndAccTypeIDRequest() { CorpID=corpID}, clientID);
                return Ok(al);
            }
        }
        /// <summary>
        /// It saves the account
        /// </summary>
        /// <param name="data">Data represents SaveAccountRequst</param>
        /// <returns>It will saves the account</returns>
        [Route("saveaccount")]
        [HttpPost]
        public async Task<IActionResult> SaveAccount(SaveAccountRequst data)
        {
            try
            {
                string userId = (string)HttpContext.Items["UserId"];
                string clientID = (string)HttpContext.Items["ClientId"];
                SaveAccountResponse al = new SaveAccountResponse();
                using (ICoreProperty cp = coreProp)
                {
                    al = await cp.SaveAccount(data, userId, clientID);
                    return Ok(al);

                }
            }
            catch { throw; }
        }

        [Route("HeadAccPref")]
        [HttpGet]
        public async Task<IActionResult> GetHeadAccountPreference(string accountID)
        {
            string userId = (string)HttpContext.Items["UserId"];
            string clientID = (string)HttpContext.Items["ClientId"];
            var response = await coreProp.GetHeadAccountPreference(accountID, userId, clientID);
            return Ok(response);
        }
        /// <summary>
        /// It will get List of Account Types Based on UserID
        /// </summary>
        /// <returns>It will displays List of Account Types </returns>
        [Route("accounttypes")]
        [HttpGet]
        public async Task<IActionResult> GetAccountTypes([FromQuery] string SortOrderValues)
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    //string userId = "0x" + (string)HttpContext.Items["UserId"];
                    AccountTypelistResponse atResp = await cp.GetAccountTypes(SortOrderValues);
                    if (atResp == null || atResp.AccounTypes == null)
                        atResp = new AccountTypelistResponse();

                    return Ok(atResp);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// It will get List of ProfitCenters based on CorporationID
        /// </summary>
        /// <param name="data">data represents CorpIDRequest</param>
        /// <returns>It will display List of ProfitCenters </returns>
        [Route("pcs")]
        [HttpGet]
        public async Task<IActionResult> GetProfitCenters([FromQuery] CorpIDRequest data, [FromQuery] bool IsActive = false)
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    GenericSortListDTO response = await cp.GetProfitCenters(data, IsActive);
                    if (response == null || response.ListInfo == null)
                        response = new GenericSortListDTO();
                    return Ok(response);
                }
            }
            catch { throw; }
        }


        /// <summary>
        /// Based on CorporationIDs by Seperated Commas it will dispaly Profit Centers
        /// </summary>
        /// <param name="CorpID,CorpID">Here we can send CorporationIDs </param>
        /// <returns></returns>
        [Route("MulUserPcs")]
        [HttpPost]
        public async Task<IActionResult> GetMulProfitCentersByCorpIDs(ModelBaseIDString data)
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    GenericSortListDTO response = await cp.GetMulProfitCentersByCorpIDs(data);
                    if (response == null || response.ListInfo == null)
                        response = new GenericSortListDTO();
                    return Ok(response);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// It will get List of Names based on CorpIDAndTypeRequest
        /// </summary>
        /// <param name="data">data represents CorpIDAndTypeRequest</param>
        /// <returns>It will display NamesList</returns>
        [Route("names")]
        [HttpGet]
        public async Task<IActionResult> GetNames([FromQuery] CorpIDAndTypeRequest data, [FromQuery] bool IsActive = false)
        {
            try
            {
                GenericSortListDTO nl;
                using (ICoreProperty cp = coreProp)
                {
                    //string cacheKey = string.Concat(data.CorpID, "NameList");
                    //nl = cache.GetData<GenericSortListDTO>(cacheKey);
                    //if (nl == null)
                    //{
                    nl = await cp.GetNames(data,IsActive);
                    //cache.SetData<GenericSortListDTO>(cacheKey, nl);
                    //}

                    if (nl == null || nl.ListInfo == null)
                        nl = new GenericSortListDTO();
                    return Ok(nl);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// It will get Purposelist based on CorpIDRequest
        /// </summary>
        /// <param name="corpIDReq"> corpIDReq represents CorpIDRequest</param>
        /// <returns>It will display List of purposes</returns>
        [Route("purposeList")]
        [HttpGet]
        public async Task<IActionResult> GetPurposes([FromQuery] CorpIDRequest corpIDReq)
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    GenericIDNameListDTO rensponse = await cp.GetPurposes(corpIDReq);
                    if (rensponse == null || rensponse.ListInfo == null)
                        rensponse = new GenericIDNameListDTO();
                    return Ok(rensponse);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// It will get List of frequencies
        /// </summary>
        /// <returns> It will display List of frequencies</returns>
        [Route("frequencies")]
        [HttpGet]
        public async Task<IActionResult> GetFrequencies()
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    return Ok(await cp.GetFrequencies());
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// It will get List of remaind days
        /// </summary>
        /// <returns>It will display List Of Remaind days</returns>
        [Route("remainders")]
        [HttpGet]
        public async Task<IActionResult> GetRemaindDays()
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    return Ok(await cp.GetRemainds());
                }
            }
            catch { throw; }
        }

        [Route("GetDates")]
        [HttpPost]
        public async Task<IActionResult> GetDateRanges(string CorpID, short filter)
        {
            FromdateandTodate response = null;
            try
            {
                response = await coreProp.GetDateRange(CorpID, filter);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                    return NotFound(response);
            }
            catch { throw; }
        }
        /// <summary>
        /// It will get userpreferences based on userRequest
        /// </summary>
        /// <param name="userRequest">It represents UserRequest</param>
        /// <returns>It will return UserResponse</returns>
        [Route("UserPreferences")]
        [HttpPost]
        public async Task<IActionResult> GetUserPreferences(UserPreferencesRequest userPreferenceRequest)
        {
            try
            {
                var uid = (string)HttpContext.Items["UserId"];
                userPreferenceRequest.UserID = !string.IsNullOrEmpty(uid) ? uid : string.Empty;
                var res = await coreProp.GetUserPreferenceSettings(userPreferenceRequest, GetClientID);
                if (res != null)
                    return Ok(res);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
        }

        [Route("IsUseTaxEnabled")]
        [HttpGet]
        public async Task<IActionResult> IsUseTaxEnabled(string CorpId)
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    var response = await cp.IsUseTaxEnabled(GetUserID, CorpId);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("LoadTaxLines")]
        [HttpGet]
        public async Task<IActionResult> LoadTaxLines()
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    var response = await cp.LoadTaxLines(GetClientID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// It will get PayeeNamesList based on corporationID
        /// </summary>
        /// <param name="corpIDReq">It represents CorpIDRequest</param>
        /// <returns>It returns PayeeNamesList</returns>
        [Route("PayeeNamesList")]
        [HttpPost]
        public async Task<IActionResult> GetPayeeNames(CorpIDRequest corpIDReq)
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    GenericIDNameListDTO rensponse = await cp.GetPyeeeNames(corpIDReq);
                    if (rensponse == null || rensponse.ListInfo == null)
                        rensponse = new GenericIDNameListDTO();
                    return Ok(rensponse);
                }
            }
            catch { throw; }
        }

        [Route("CorporationDetails")]
        [HttpPost]
        public async Task<IActionResult> GetCorporationDetails(CorpIDRequest corpIDReq)
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    var uid = (string)HttpContext.Items["UserId"];
                    CorporationDetails response = await cp.GetCorporationPeriodDates(corpIDReq, uid);
                    if (response == null)
                        response = new CorporationDetails();
                    return Ok(response);
                }

            }
            catch { throw; }
        }
        [Route("Previlages")]
        [HttpPost]
        public async Task<IActionResult> GetPrevilages(PrevilagesRequest previlagesReq)
        {
            try
            {
                var uid = (string)HttpContext.Items["UserId"];
                previlagesReq.UserID = !string.IsNullOrEmpty(uid) ? uid : string.Empty;

                using (ICoreProperty cp = coreProp)
                {
                    PrevilageResponse response = await cp.GetPrevilageResponse(previlagesReq);
                    if (response == null)
                        response = new PrevilageResponse();
                    return Ok(response);

                }
            }
            catch { throw; }
        }

        [Route("GetParentUsers")]
        [HttpGet]
        public async Task<IActionResult> GetParentUserDetails()
        {
            try
            {
                var uid = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    UserParentDetailsResponse response = await cp.GetUserParentUsers(uid);
                    if (response == null)
                        response = new UserParentDetailsResponse();
                    return Ok(response);
                }
            }
            catch { throw; }
        }
        [Route("RoleIdsAndNamesList")]
        [HttpGet]
        public async Task<IActionResult> GetRoleIDsAndNames()
        {
            try
            {
                var uid = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    RoleIDsAndNameResponse response = await cp.GetRoleIDsDetails(uid);
                    if (response == null)
                        response = new RoleIDsAndNameResponse();
                    return Ok(response);
                }
            }
            catch { throw; }
        }

        [Route("GetReportUserDetails")]
        [HttpGet]
        public async Task<IActionResult> GetUserReportDetails()
        {
            try
            {

                var uid = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    UserandReportToResponse response = await cp.GetUserReportToDetails(uid);
                    if (response == null)
                        response = new UserandReportToResponse();
                    return Ok(response);
                }
            }
            catch { throw; }
        }
        [Route("GetRoleIDsByUserIDs")]
        [HttpPost]

        public async Task<IActionResult> GetRoleIDsListByUserIDs(LoadByIDRequest request)
        {
            RoleIDsAndNameResponse response = new RoleIDsAndNameResponse();
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    response = await cp.GetRoleIDsByUserIDs(request);
                    if (response == null)
                        response = new RoleIDsAndNameResponse();
                    return Ok(response);
                }
            }
            catch { throw; }
        }



        [Route("GetDownLineUsers")]
        [HttpPost]
        public async Task<IActionResult> GetDownLineUsers(ModelBaseUserID request)
        {
            try
            {
                //var uid = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    List<string> response = await cp.GetDownLineUsersOrRoles(request.UserID, request.RoleID);
                    //if (response.Count == 0)
                    //    response = await cp.GetDownLineUsersByRoleID(request.UserID);
                    if (response == null)
                        response = new List<string>();
                    return Ok(response);
                }
            }
            catch { throw; }
        }

        [Route("GetClientSubUsers")]
        [HttpGet]
        public async Task<IActionResult> GetClientSubUsers()
        {
            ClientSubUsersResponse response = new ClientSubUsersResponse();
            try
            {
                var uid = (string)HttpContext.Items["ClientId"];
                using (ICoreProperty cp = coreProp)
                {
                    response = await cp.GetClientSubUsers(uid);
                    if (response.ClientSubUsers == null)
                        response.ClientSubUsers = new List<ClientSubUsers>(); // Ensure the list is initialized
                    return Ok(response);
                }
            }
            catch
            {
                throw;
            }
        }

        [Route("GetSourceType")]
        [HttpPost]
        public async Task<IActionResult> GetSourceType(ModelBaseUserID request)
        {
            try
            {
                // var uid = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    long? response = await cp.GetSourceType(request.UserID);
                    if (response == null)
                        response = new long();
                    return Ok(response);
                }
            }
            catch { throw; }
        }


        /// <summary>
        /// This method retrieves portfolios with brand names.
        /// </summary>
        /// <returns>Returns a list of PortfolioResponse.</returns>
        [Route("GetPortifoliosWithBrands")]
        [HttpGet]
        public async Task<IActionResult> GetPortifoliosWithBrands()
        {
            PortfolioResponse? response = null;
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    response = await cp.GetPortifoliosWithBrands(HttpContext.Items["ClientId"].ToString());
                    if (response == null)
                        response = new PortfolioResponse();
                    return Ok(response);

                }
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("CorporationByMgmtGroup")]
        [HttpPost]
        public async Task<IActionResult> GetManagementGroups(string GroupID)
        {
            ManagementGroupCorpResponse? response = null;
            try
            {
                var userID = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    response = await cp.GetManagemtCorporations(GroupID, userID);
                    if (response == null)
                        response = new ManagementGroupCorpResponse();
                    return Ok(response);

                }
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("CorporationPcsByMgmtGroup")]
        [HttpPost]
        public async Task<IActionResult> GetManagementGroupsPcs(string GroupID)
        {
            ManagementGroupCorpResponse? response = null;
            try
            {
                var userID = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    response = await cp.GetManagemtCorporationPcs(GroupID, userID);
                    if (response == null)
                        response = new ManagementGroupCorpResponse();
                    return Ok(response);

                }
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("GetUserRole")]
        [HttpPost]
        public async Task<IActionResult> GetUserRole(ModelBaseUserID request)
        {
            UserRoleResponse? response = null;
            try
            {
                if (string.IsNullOrEmpty(request.UserID) || request.UserID == "0")
                {
                    request.UserID = (string)HttpContext.Items["UserId"];
                }
                //var userID = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    response = await cp.GetRole(request);
                    if (response == null)
                        response = new UserRoleResponse();
                    return Ok(response);

                }
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("GetReportTo")]
        [HttpPost]
        public async Task<IActionResult> GetReportToUser(ModelBaseUserID request)
        {
            UserParentDetailsResponse? response = null;
            try
            {
                //var userID = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    response = await cp.GetParentUser(request);
                    if (response == null)
                        response = new UserParentDetailsResponse();
                    return Ok(response);
                }
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("GetParentUserOrRoleIDs")]
        [HttpPost]
        public async Task<IActionResult> GetUserParentUserRoleID()
        {
            try
            {
                var uid = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    UserParentDetailsResponse response = await cp.GetUserParentRoleUsers(uid);
                    if (response == null)
                        response = new UserParentDetailsResponse();
                    return Ok(response);
                }
            }
            catch { throw; }

        }

        [Route("DashboardPrivilegeRole")]
        [HttpPost]
        public async Task<IActionResult> UpdateDashboardPrivilegeRole(ModelBaseIDString request)
        {
            StatusDTO? response = null;
            try
            {
                //var userID = (string)HttpContext.Items["UserId"];
                using (ICoreProperty cp = coreProp)
                {
                    response = await cp.DashboardPrivilegeRole(request);
                    if (response == null)
                        response = new UserParentDetailsResponse();
                    return Ok(response);
                }
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("VerifyTransaction")]
        [HttpPost]
        public async Task<IActionResult> VerifyTransaction(TransactionVerificationReq request)
        {
            TransactionVerificationResp? response = null;
            //var userID = (string)HttpContext.Items["UserId"];
            using (ICoreProperty cp = coreProp)
            {
                response = await cp.VerifyTransaction(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);

            }


        }
        [Route("ValidateCheckOrVoucherNum")]
        [HttpPost]
        public async Task<IActionResult> ValidateCheckOrVoucherNum(CheckOrVoucherNumValidationReq request)
        {
            VerificationStatusResp? response = null;
            //var userID = (string)HttpContext.Items["UserId"];
            using (ICoreProperty cp = coreProp)
            {
                response = await cp.ValidateCheckOrVoucherNumbByAccountAndPaymethoID(request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);

            }
        }


        [Route("GetScheduledReportMenuInfo")]
        [HttpGet]
        public async Task<IActionResult> GetScheduledReportMenuInfo()
        {
            GetScheduledReportMenuInfoResp Response = null;
            using (ICoreProperty cp = coreProp)
            {
                Response = await cp.GetScheduledReportMenuInfo();

                return Ok(Response);

            }

        }

        [Route("GetManagementGroupsList")]
        [HttpGet]
        public async Task<IActionResult> GetManagementGroupsList()
        {
            List<GetManagementGroupsListResp> Response = null;
            using (ICoreProperty cp = coreProp)
            {
                Response = await cp.GetManagementGroupsList(HttpContext.Items["ClientId"].ToString());

                return Ok(Response);

            }

        }



        [Route("GetCorpsByMgntGrpWithPmsBrandSerTypes")]
        [HttpGet]
        public async Task<IActionResult> GetCorporationSelectionListByType(string MgntGrpId)

        {
            var userID = (string)HttpContext.Items["UserId"];
            using (ICoreProperty cp = coreProp)
            {
                List<GetCorporationSelectionListByTypeResp> Response = await cp.GetCorporationSelectionListByType(userID, MgntGrpId);
                return Ok(Response);
            }
        }


        [Route("GetSubUserListByUserId")]
        [HttpGet]
        public async Task<IActionResult> GetSubUserListByUserId()
        {
            using (ICoreProperty cp = coreProp)
            {
                GetUserCorporationsResp Response = await cp.GetSubUserListByUserId(HttpContext.Items["UserId"].ToString());
                return Ok(Response);
            }
        }




        #endregion


        #region Approval

        [Route("GetUserApprovalType")]
        [HttpGet]
        public async Task<IActionResult> GetUserApprovalType()
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    var response = await cp.GetUserApprovalType(GetUserID);
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("GetApprovalPolicyUserDetails")]
        [HttpGet]
        public async Task<IActionResult> GetApprovalPolicyUserDetails(string CorpID, bool getAllUsers, short ScreenType)
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    var response = await cp.GetApprovalPolicyUserDetails(CorpID, ScreenType, base.GetUserID, getAllUsers);
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("IsCorporationLocked")]
        [HttpPost]
        public async Task<IActionResult> GetCorporationLockingStatus(CorporationLockRequest req)
        {
            try
            {
                using (ICoreProperty cp = coreProp)
                {
                    var response = await cp.IsCorporationLocked(req.CorpID, req.Type, Convert.ToDateTime(req.BooksDate));
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("GetUserApprovalDetails")]
        [HttpPost]
        public async Task<IActionResult> GetUserApprovalDetails(UserApprovalDetailsRequest request)//List<string> CorpIDs)
        {
            try
            {
                var lstCorps = (request.CorpIDs!=null && request.CorpIDs.Any())? string.Join(",", request.CorpIDs): Constants.SystemUser;
                //request.CurrentUserID = GetUserID;

                using (ICoreProperty cp = coreProp)
                {
                    List<UserApprovalDetails> response = await cp.GetUserApprovalDetails(GetUserID, lstCorps, request.ScreenType);
                    if (response != null && response.Any())
                    {
                        foreach (var item in response)
                        {
                            if (item != null && item.UserID == GetUserID)
                            {
                                item.IsCurrentUser = true;
                                //break;
                            }
                        }
                    }
                    return Ok(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("BillEntryUnApprovedSendEmail")]
        [HttpPost]
        public async Task<IActionResult> BillEntryUnApproved(BillEntrySendMailDetails req)
        {
            JournalResponse response = new JournalResponse();
            try
            {
                req.UserID = GetUserID;
                response = await coreProp.BillEntryUnApprovedSendEmail(req);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);
            }
            catch { throw; }
        }
        #endregion
        [Route("LoadCorporationList")]
        [HttpGet]
        public async Task<IActionResult> GetCorporationList()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await coreProp.LoadCorpList(GetUserID);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
        }

        [Route("LoadRestaurantCorporationList")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantCorporationList(int BussinessType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await coreProp.LoadRestaurantCorpList(GetUserID, BussinessType);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
        }



        [Route("UOM/List")]
        [HttpGet]
        public async Task<IActionResult> GetUOMList()
        {
            UOMListResponse response = new UOMListResponse();
            try
            {
                response = await coreProp.GetUOMList(GetClientID);
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

        [Route("ShipVia/List")]
        [HttpGet]
        public async Task<IActionResult> GetShipViaList()
        {
            ShipViaListResponse response = new ShipViaListResponse();
            try
            {
                response = await coreProp.GetShipViaList(GetClientID);
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

        [Route("UserInformationDetails")]
        [HttpGet]
        public async Task<IActionResult> GetUserInformation()
        {
            try
            {
                string userId = (string)HttpContext.Items["UserId"];

                var response = await coreProp.GetUserinformation(userId);
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
        [Route("IncomeStatementLayoutsByCorpID")]
        [HttpPost]
        public async Task<IActionResult> GetIncomeStatementLayouts(string CorporationID)
        {
            try
            {
                string userId = (string)HttpContext.Items["ClientId"];

                var response = await coreProp.GetIncomeStatementLayouts(CorporationID);
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

        [Route("GetIncstmntLayoutsByClientID")]
        [HttpPost]
        public async Task<IActionResult> GetIncomeStmtLayouts(ClientOrUserIDRequest request)
        {
            try
            {
                string clientID = (string)HttpContext.Items["ClientId"];
                request.ClientID= clientID;

                var response = await coreProp.GetLayouts(request);
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

        [Route("GetCorporationsByLayoutID")]
        [HttpPost]
        public async Task<IActionResult> GetCorporationsbyLayoutID(Int64 LayoutID)
        {
            try
            {
                var response = await coreProp.GetCorporationsByLayoutID(LayoutID);
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
        [Route("GetCorporationsByBusinessType")]
        [HttpGet]
        public async Task<IActionResult> GetCorporationsByBusinessType(int BusinessType)
        {
            try
            {
                string userId = (string)HttpContext.Items["UserId"];
                var response = await coreProp.GetCorporationsByBusinessType(BusinessType, userId);
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

        [Route("GetPCsByBusinessType")]
        [HttpGet]
        public async Task<IActionResult> GetCorporationsByBusinessType(string CorporationId, int BusinessType, [FromQuery] bool IsActive = false)
        {
            try
            {
                string userId = (string)HttpContext.Items["UserId"];
                var response = await coreProp.GetPCsByBusinessType(CorporationId, BusinessType, userId);
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




        [Route("CheckIncomeStatementConfig")]
        [HttpPost]
        public async Task<IActionResult> CheckIncomeStatementConfig(string CorporationID)
        {
            try
            {
                string userId = (string)HttpContext.Items["ClientId"];

                var response = await coreProp.CheckIncomeConfig(CorporationID);
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

    }

}
