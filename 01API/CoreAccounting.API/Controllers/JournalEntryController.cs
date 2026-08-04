using Common.App.Contracts;
using Common.Domain;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using CoreAccounting.App.Services;
using Microsoft.AspNetCore.Mvc;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using Common.Domain.DTO.Model;
using Org.BouncyCastle.Ocsp;
using Microsoft.Extensions.Options;

namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class JournalEntryController : BaseController //Controller
    {
        #region Fields
        private readonly ICoreProperty coreProp;
        private readonly IJournalService journalSrv;
        private readonly IFileService fileSrv;
        private readonly IUnitOfWork uow;
        private IOptions<List<ServerAnlyticsGroup>> serverGroup;
        #endregion

        #region Ctor
        public JournalEntryController(ICoreProperty coreProperty, IFileService fileService, IJournalService journalService, IUnitOfWork unitofwork, IOptions<List<ServerAnlyticsGroup>> _serverGroup)
        {
            this.coreProp = coreProperty;
            this.fileSrv = fileService;
            this.journalSrv = journalService;
            this.uow = unitofwork;
            this.serverGroup = _serverGroup;
        }

        #endregion

        #region Journal Entry


        /// <summary>
        /// It gives List of transactions for an account based on dates,days,amount,reconcilation status and sourcetype with page count & offset
        /// </summary>
        /// <param name="Request"> TransactionListRequest with date, amount and related filters  </param>
        /// <returns>It returns list of transactions in TransactionListResponse </returns>

        [Route("journal/TransactionList")]
        [HttpPost]
        public async Task<IActionResult> GetTransactionList(TransactionListRequest Request)
        {
            TransactionListResponse response = null;
            try
            {
                response = await journalSrv.GetTransactions(Request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }



        /// <summary>
        /// Based on CorporationID  It will get JournalLookup data
        /// </summary>
        /// <param name="corpIDReq">Here corpIDReq represents CorpIDRequest</param>
        /// <returns>It will display  PurposeNames, Accounts, ProfitCenters, Names, frequencies and remaind days </returns>
        [Route("journal/lookup")]
        [HttpPost]
        public async Task<IActionResult> GetJournalLookup(CorpIDRequest corpIDReq)
        {
            JournalLookupRespose? response = null;
            try
            {
                response = await journalSrv.GetJournalEntryLookup(corpIDReq, GetClientID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will get List of Journals based on CorporationId
        /// </summary>
        /// <param name="data">data represents JournalSearchRequest</param>
        /// <returns>It will display List Of Journals</returns>
        [Route("journals/viewgrid")]
        [HttpPost]
        public async Task<IActionResult> LoadJournalEntries(JournalEntryGridRequest data)
        {
            JEListResponse? response = null;
            try
            {
                response = await journalSrv.LoadJournalEntryList(data, GetUserID, GetClientID,GetClientName);
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
        /// It will get List of Journals based on CorporationId
        /// </summary>
        /// <param name="data">data represents JournalSearchRequest</param>
        /// <returns>It will display List Of Journals</returns>
        [Route("journals/list")]
        [HttpGet]
        public async Task<IActionResult> GetJournalEntries([FromQuery] JournalEntryGridRequest data)
        {
            JEListResponse? response = null;
            try
            {
                response = await journalSrv.LoadJournalEntryList(data, GetUserID, GetClientID,GetClientName);
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
        /// It will get JournalEntry along with Transactions Based On JournalEntryID
        /// </summary>
        /// <param name="data">data represents LoadByIDRequest</param>
        /// <returns>It will display JournalEntry details along with transactions</returns>
        [Route("journal/load")]
        [HttpPost]
        public async Task<IActionResult> GetJournalEntry(LoadByIDRequest data)
        {
            JournalEntryResponse? response = null;
            try
            {
                response = await journalSrv.GetJournalEntry(data, GetUserID,getClientName());
                if (response != null && response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will create JournalEntry based on JournalEntryRequest
        /// </summary>
        /// <param name="data">data represents JournalEntryRequest</param>
        /// <returns>It will return JournalEntryID ,Status and StatusCode</returns>
        [Route("journals/create")]
        [HttpPost]
        public async Task<IActionResult> CreateJournalEntry(JournalEntryRequest data)
        {
            JournalResponse? response = null;
            try
            {
                if (data != null && data.Divisions != null && data.Divisions.Where(t => (string.IsNullOrEmpty(t.AccountID) && (t.Credit <= 0 && t.Debit <= 0))).Any())
                {
                    data.Divisions.RemoveAll(t => (string.IsNullOrEmpty(t.AccountID) && (t.Credit <= 0 && t.Debit <= 0)));
                }
                response = await journalSrv.CreateJournal(data, GetUserID, GetClientID);
                if (response != null)  //if (response != null && response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will update JournalEntry based JournalEntryRequest
        /// </summary>
        /// <param name="data">It represents JournalEntryRequest</param>
        /// <returns>It will return Updated JournalEntryID ,status and StatusCode</returns>
        [Route("journals/update")]
        [HttpPost]
        public async Task<IActionResult> UpdateJournalEntry(JournalEntryRequest data)
        {
            JournalResponse? response = null;
            try
            {
                if (data != null && data.Divisions != null && data.Divisions.Where(t => (string.IsNullOrEmpty(t.AccountID) && (t.Credit <= 0 && t.Debit <= 0))).Any())
                {
                    data.Divisions.RemoveAll(t => (string.IsNullOrEmpty(t.AccountID) && (t.Credit <= 0 && t.Debit <= 0)));
                }
                response = await journalSrv.UpdateJournal(data, GetUserID, GetClientID);
                //if (response != null && response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.ID))
                if (response != null)
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will make JournalEntry Status filed as '3' i.e delete
        /// </summary>
        /// <param name="data">data represents LoadByIDRequest</param>
        /// <returns>It will returns deleted JournalEntryID ,Status and StatusCode</returns>
        [Route("journals/delete")]
        [HttpPost]
        public async Task<ActionResult<JournalResponse>> DeleteJournalEntry(LoadByIDRequest data)
        {
            JournalResponse? response = null;
            try
            {
                response = await journalSrv.DeleteJournalentryById(data, GetUserID);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if(response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }


        /// <summary>
        /// It will create JournalEntry based on JournalEntryQuickRequest
        /// </summary>
        /// <param name="data">data represents JournalEntryRequest</param>
        /// <returns>It will return JournalEntryID ,Status and StatusCode</returns>
        [Route("journals/saveorupdate")]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateQuickJournalEntry(JournalEntryQuickRequest data)
        {
            JournalResponse? response = null;
            try
            {
                if (data != null && data.Divisions != null && data.Divisions.Where(t => (string.IsNullOrEmpty(t.AccountID) && (t.Credit <= 0 && t.Debit <= 0))).Any())
                {
                    data.Divisions.RemoveAll(t => (string.IsNullOrEmpty(t.AccountID) && (t.Credit <= 0 && t.Debit <= 0)));
                }
                response = await journalSrv.CreateUpdateQuickJournalEntry(data, GetUserID, GetClientID);
                if (response != null)  //if (response != null && response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.ID))
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// It will load based on LoanScheduleTransactions
        /// </summary>
        /// <param name="data">data represents loanScheduleID</param>
        /// <returns>It will return loanScheduleTransaction Amounts ,Status and StatusCode</returns>
        [Route("journal/loanAmounts")]
        [HttpGet]
        public async Task<IActionResult> GetloanScheduleTransactionAmounts([FromQuery] string loanScheduleID)
        {
            LoanScheduleTransactionResp? response = null;
            try
            {
                response = await journalSrv.GetloanScheduleTransactionAmounts(loanScheduleID);
                if (response != null)
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Status)) ? response.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
            finally { response = null; }
        }

        #endregion

        #region Recurring Lookup

        /// <summary>
        /// Based on CorporationID  It will get JournalLookup data
        /// </summary>
        /// <returns>It will display  PurposeNames, Accounts, ProfitCenters, Names, frequencies and remaind days </returns>
        [Route("recurring/lookup")]
        [HttpPost]
        public async Task<IActionResult> GetRecurringLookup()
        {
            RecurringLookupRespose? response = null;
            try
            {
                response = await journalSrv.GetRecurringLookup();

                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }

        }
        [Route("Reccuring/Load")]
        [HttpPost]
        public async Task<IActionResult> LoadJournalReccuring(LoadByIDRequest request)
        {
            JournalEntryResponse response = await journalSrv.LoadJournalReccuring(request, GetUserID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("Reccuring/IsUniqeName")]
        [HttpPost]
        public async Task<IActionResult> GetUniqueReccuringNameData(UniqueNameCheckRequest Req)
        {
            JournalResponse response = await journalSrv.IsUniqueReccuringName(Req);
            return Ok(response);
        }

        [Route("Reccuring/Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateReccursive(JournalEntryRequest Req)
        {
            if (Req != null && Req.Divisions != null && Req.Divisions.Where(t => (string.IsNullOrEmpty(t.AccountID) && (t.Credit <= 0 && t.Debit <= 0))).Any())
            {
                Req.Divisions.RemoveAll(t => (string.IsNullOrEmpty(t.AccountID) && (t.Credit <= 0 && t.Debit <= 0)));
            }

            JournalResponse response = await journalSrv.UpdateReccursive(Req, GetUserID, GetClientID);
            return Ok(response);
        }

        [Route("Reccuring/Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteReccursive(string SourceID, bool IsSave)
        {
            JournalResponse response = await journalSrv.DeleteReccursive(SourceID, IsSave);
            return Ok(response);
        }

        [Route("Reccuring/ReplaceReccuringEntry")]
        [HttpPost]
        public async Task<IActionResult> ReplaceReccuringEntry(JournalEntryRequest Request)
        {
            JournalResponse response = await journalSrv.ReplaceReccursive(Request, GetUserID);
            return Ok(response);
        }
        #endregion Recurring

        #region Approval      


        [Route("journal/Approve")]
        [HttpPost]
        public async Task<IActionResult> JournalEntryApprove(LoadByIDRequest JID)
        {
            try
            {
                using (IJournalService js = journalSrv)
                {
                    var response = await js.ApproveJournalEntry(new PFAID(JID.ID).ToString(), GetUserID, JID.IsValidate, false);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }


        [Route("journal/BulkApprove")]
        [HttpPost]
        public async Task<IActionResult> BulkJournalEntryApprove(List<QuickBillDTO> jidList)
        {
            try
            {
                var response = await journalSrv.ApproveBulkJournalEntries(jidList, GetUserID);
                if (response != null) return Ok(response);
                else return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("journal/Verify")]
        [HttpPost]
        public async Task<IActionResult> VerifyJournalEntry(LoadByIDRequest JID)
        {
            try
            {
                using (IJournalService js = journalSrv)
                {
                    var response = await js.VerifyJournalEntry(new PFAID(JID.ID).ToString(), GetUserID, JID.IsValidate, false);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("journal/BulkVerify")]
        [HttpPost]
        public async Task<IActionResult> BulkJournalEntryVerify(List<QuickBillDTO> JIDList)
        {
            try
            {
                List<JournalBillResponse> response = await journalSrv.VerifyBulkJournalEntries(JIDList, GetUserID);
                if (response != null && response.Count() > 0)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("journal/Reject")]
        [HttpPost]
        public async Task<IActionResult> RejectJournalEntry(JournalEntryRejectRequest BillRejectReq)
        {
            try
            {
                using (IJournalService js = journalSrv)
                {
                    var response = await js.RejectJournalEntry(BillRejectReq, GetUserID, false);
                    if (response != null)
                        return Ok(response);
                    else
                        return NotFound(response);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }


        [Route("journal/BulkReject")]
        [HttpPost]
        public async Task<IActionResult> BulkJournalEntryReject(List<JournalEntryRejectRequest> JIDList)
        {
            try
            {
                List<JournalBillResponse> response = await journalSrv.RejectBulkJournalEntries(JIDList, GetUserID);
                if (response != null && response.Count() > 0)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("journal/CustomerQuickAdd")]
        [HttpPost]
        public async Task<IActionResult> CustomerQuickAdd(QuickCustomerReq Req)
        {
            try
            {
                JEQuickAddResponse response = await journalSrv.SaveQuickCustomer(Req,GetClientID, GetUserID);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("journal/EmployeeQuickAdd")]
        [HttpPost]
        public async Task<IActionResult> EmployeeQuickAdd(QuickEmployeeReq Req)
        {
            try
            {
                JEQuickAddResponse response = await journalSrv.SaveQuickEmployee(Req,GetClientID, GetUserID);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("journal/EmployeeLookup")]
        [HttpGet]
        public async Task<IActionResult> EmployeeLookupLoad([FromQuery]string CorpId)
        {
            try
            {
                var response = await journalSrv.LoadEmpLookup(GetClientID, CorpId);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("journal/SaveJobInfo")]
        [HttpPost]
        public async Task<IActionResult> SaveJobInfo(JobInfoReq Req)
        {
            try
            {
                var response = await journalSrv.SaveJobInfo(Req,GetClientID);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        #endregion

        #region BankFeed
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
        /// To post list of transactions from Nimble to bankfeed transactions sharing for possible matches - based on dates,days,amount,reconcilation status and sourcetype with page count & offset
        /// </summary>
        /// <param name="Request"> TransactionListRequest with date, amount and related filters  </param>
        /// <returns>It returns RequestID ,transactions count in TransactionsShareResponse </returns>
        [Route("journal/PostSharingTransactions")]
        [HttpPost]
        public async Task<IActionResult> TransactionsSharing(TransactionListRequest Request)
        {
            TransactionsShareResponse response = null;
            try
            {
                //Request.ClientID = "0x" + HttpContext.Items["ClientId"].ToString();
                response = await journalSrv.PostTransactionsToBankFeed(Request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        [Route("journal/MultiplePostSharingTransactions")]
        [HttpPost]
        public async Task<IActionResult> MultipleTransactionsSharing(MultipleRequestsOfTransactionList Request)
        {
            MultipleTransactionsShare response = null;
            try
            {
                //Request.ClientID = "0x" + HttpContext.Items["ClientId"].ToString();
                response = await journalSrv.PostMultipleTransactionsToBankFeed(Request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// For Updating nimble transaction for Bankfeed operations (clear posted/matched & matched)
        /// </summary>
        /// <param name="Request">List of Transaction details, IsCleared => (true - clearmatch ,false - match),IsPostedDeleted - for clear posted</param>
        /// <returns> Updated Transaction IDs in TransactionUpdateResponse </returns>
        [Route("journal/UpdateTransaction")]
        [HttpPost]
        public async Task<IActionResult> UpdateTransactions(TransactionUpdateRequest Request)
        {
            TransactionUpdateResponse response = null;
            try
            {
                byte[] userId = new PFAID(GetUserID).UID;
                response = await journalSrv.UpdateTransactions(Request, userId,getUrlID(),getClientName(),getNPConnection());
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }

        /// <summary>
        /// To get specific list of transactions based in transaction Id's
        /// </summary>
        /// <param name="Request"> list of Transaction IDs</param>
        /// <returns> selected Transaction list in TransactionListResponse</returns>
        [Route("journal/GetTransactionByID")]
        [HttpPost]
        public async Task<IActionResult> GetTransactionsBtID(TransactionRequest Request)
        {
            TransactionListResponse response = null;
            try
            {
                response = await journalSrv.GetTransactionsByID(Request);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }


        //[Route("journal/GetTransactionByReconID")]
        //[HttpPost]
        //public async Task<IActionResult> GetTransactionByReconID(TransactionRequest Request)
        //{
        //    TransactionListResponse response = null;
        //    try
        //    {
        //        response = await journalSrv.GetTransactionByReconID(Request);
        //        if (response != null && response.StatusCode == StatusCodes.Status200OK)
        //            return Ok(response);
        //        else
        //            return NotFound(response);
        //    }
        //    catch { throw; }
        //    finally { response = null; }
        //}


        #endregion

    }
}
