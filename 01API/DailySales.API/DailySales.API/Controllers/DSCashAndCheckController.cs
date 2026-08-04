using Amazon.S3.Model.Internal.MarshallTransformations;
using Azure;
using Azure.Core;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Resp;
using DailySales.App.Contracts;
using DailySales.App.Services;
using DailySales.Domain.DTO.Model;
using DailySales.Domain.DTO.Req;
using DailySales.Domain.DTO.Resp;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DailySales.API.Controllers
{
    [Route("v1/DSCashAndCheck")]
    [ApiController]
    [ValidateModel]
    [Authorize]

    public class DSCashAndCheckController : ControllerBase
    {
        private readonly IDSCashAndCheckService DsCashAndCheck;

        public DSCashAndCheckController(IDSCashAndCheckService _DsCashAndCheck)
        {
            this.DsCashAndCheck = _DsCashAndCheck;
        }

        /// <summary>
        /// Based on CorporationId and ProfitCenterId, fetch Adjustment Opening Balance details.
        /// </summary>
        /// <param name="CorpID">Corporation ID (Required)</param>
        /// <param name="PCID">Profit Center ID (Optional)</param>
        /// <returns>It will return the Adjustment Opening Balance details with Status and Status Codes</returns>
        [Route("GetAdjustmentOpeningBalance")]
        [HttpGet]
        public async Task<IActionResult> GetAdjustmentOpeningBalance(string CorpID, string PCID)
        {

            GetAdjOpeningBalResp Responce = new GetAdjOpeningBalResp();
            Responce = await DsCashAndCheck.GetAdjustmentOpeningBalance(CorpID, PCID);
            if (Responce != null && Responce.StatusCode == StatusCodes.Status200OK)
                return Ok(Responce);
            else if (Responce != null && !string.IsNullOrEmpty(Responce.Status))
                return Ok(Responce);
            else
                return NotFound(Responce);
        }

        /// <summary>
        /// Based on request, Save or Update Adjustment Opening Balance for a Corporation and Profit Center.
        /// </summary>
        /// <param name="req">Here request contains AdjustmentID, CorporationId, ProfitCenterId, Amount and AsOfDate</param>
        /// <returns>It will return the Status and Status Codes</returns>
        [Route("SaveOrUpdateAdjustmentOpeningBalance")]
        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateAdjustmentOpeningBalance([FromBody] SaveOrUpdateAdjOpeningBalReq req)
        {
            string UserId = (string)HttpContext.Items["UserId"];

            var response = await DsCashAndCheck.SaveOrUpdateAdjustmentOpeningBalance(req, UserId);

            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }



        /// <summary>
        /// Based on  a Corporation and Profit Center we will fetch the pending Deposit sales amount
        /// </summary>
        /// <param name="CorpID"> CorporationId</param>
        /// <param name="PCID"> PCID Is Optional</param>
        /// <returns>It will return the Status and Status Codes with amount</returns>

        //[Route("PendingToDeposit")]
        //[HttpGet]
        //public async Task<IActionResult> PendingToDeposit(string CorpID, string PCID)
        //{
        //    PendingToDepositResp Response = new PendingToDepositResp();
        //    Response = await DsCashAndCheck.PendingToDeposit(CorpID, PCID);
        //    if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
        //        return Ok(Response);
        //    else if (Response != null && !string.IsNullOrEmpty(Response.Status))
        //        return Ok(Response);
        //    else
        //        return NotFound(Response);
        //}


        /// <summary>
        /// Based on  a Corporation and Profit Center we will fetch the pending Deposit sales amount
        /// </summary>
        /// <param name="CorpID"> CorporationId</param>
        /// <param name="PCID"> PCID Is Optional</param>
        /// <returns>It will return the Status and Status Codes with amount</returns>

        [Route("PendingExcessOrShortage")]
        [HttpGet]

        public async Task<IActionResult> PendingExcessOrShortage(string CorpID, string PCID)
        {
            PendingExcessOrShortageResp Response = new PendingExcessOrShortageResp();
            Response = await DsCashAndCheck.PendingExcessOrShortage(CorpID, PCID);
            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);

        }

        [Route("SaveCashAndCheckComment")]
        [HttpPost]
        public async Task<IActionResult> SaveCashAndCheckComment(CashAndCheckCommentRequest CommentRequest)
        {
            string UserId = (string)HttpContext.Items["UserId"];
            try
            {
                var response = await DsCashAndCheck.SaveCashAndCheckComment(CommentRequest, UserId);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("GetCashAndCheckActivityLog")]
        [HttpGet]
        public async Task<IActionResult> GetCashAndCheckActivityLog(string JEID)
        {
            try
            {
                CashAndCheckActivityLogListResponse response = await DsCashAndCheck.GetCashAndCheckActivityLog(JEID);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }



        [Route("GetDailySalesDetails")]
        [HttpPost]

        public async Task<IActionResult> GetDailySalesDetails(GetDailySalesDetailsReq Data)
        {
            DsCashChecksResp Response = new DsCashChecksResp();
            Response = await DsCashAndCheck.GetDailySalesDetails(Data);
            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);
        }


        [Route("MissingDailySalesCount")]
        [HttpGet]

        public async Task<IActionResult> MissingDailySalesCount(string CorpId, string PCID)
        {
            GetMissingSalesCountResp Response = new GetMissingSalesCountResp();
            Response = await DsCashAndCheck.GetMissingSalesCount(CorpId, PCID);
            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);

        }

        /// <summary>
        /// Deletes a specific Deposit record based on the provided Deposit ID.
        /// </summary>
        /// <param name="DepositID">The unique identifier of the Deposit to be deleted.</param>
        /// <returns>
        /// Returns a <see cref="StatusDTO"/> object containing StatusCode and Status message.  
        /// If the deletion is successful, StatusCode will be 200 (OK).  
        /// Otherwise, it returns appropriate status and message indicating the reason.
        /// </returns>
        [Route("DeleteDeposits")]
        [HttpPost]
        public async Task<IActionResult> DeleteDeposit([FromBody] long DepositID)
        {
            StatusDTO response = await DsCashAndCheck.DeleteDeposit(DepositID);

            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }


        [Route("LoadViewGrid")]
        [HttpPost]
        public async Task<IActionResult> ViewGrid(ViewGridReq data)
        {
            ViewGridResp Response = new ViewGridResp();
            string UserId = (string)HttpContext.Items["UserId"];
            Response = await DsCashAndCheck.ViewGrid(data, UserId);
            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);
        }

        /// <summary>
        /// Retrieves all Sale Dates information within the Deposit period for the specified Deposit ID.
        /// </summary>
        /// <param name="depositId">The unique identifier of the Deposit for which Sale Dates information is to be fetched.</param>
        /// <returns>
        /// Returns a <see cref="DepositMenuInfoResp"/> object containing the Sale Dates details,  
        /// along with Status and StatusCode indicating the result of the operation.  
        /// If successful, StatusCode will be 200 (OK); otherwise, it returns the appropriate status and message.
        /// </returns>
        [Route("SaleDatesInfo")]
        [HttpGet]
        public async Task<IActionResult> GetSaleDatesInfo(long depositId)
        {
            DepositMenuInfoResp response = new DepositMenuInfoResp();
            response = await DsCashAndCheck.GetSaleDatesInfo(depositId);

            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("SaveOrEditDeposit")]
        [HttpPost]

        public async Task<IActionResult> SaveOrEditDeposit(DsCashChecksReq data,bool IsEdit)
        {
            string UserId = (string)HttpContext.Items["UserId"];
            StatusDTO Response = new StatusDTO();
            Response = await DsCashAndCheck.SaveOrEditDeposit(data, UserId, IsEdit);
            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);
        }

        /// <summary>
        /// Retrieves the summary of all unapproved deposits for a specific Corporation.
        /// </summary>
        /// <param name="Corporationid">
        /// The unique identifier of the Corporation for which unapproved deposit summary details are to be fetched.
        /// </param>
        /// <returns>
        /// Returns an <see cref="UnapprovedDepositSummaryResp"/> object containing the unapproved deposits count,  
        /// total amount, and other related details along with Status and StatusCode.  
        /// If successful, StatusCode will be 200 (OK); otherwise, it returns an appropriate status and message.
        /// </returns>
        [Route("GetUnapprovedDepositSummary")]
        [HttpGet]
        public async Task<IActionResult> GetUnapprovedDepositSummary(string Corporationid)
        {
            string userId = (string)HttpContext.Items["UserId"];
            UnapprovedDepositSummaryResp response = await DsCashAndCheck.GetUnapprovedDepositSummary(userId, Corporationid);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);

        }

        /// <summary>
        /// Retrieves detailed deposit information, including cash and check details, for the specified Deposit ID.
        /// </summary>
        /// <param name="depositId">
        /// The unique identifier of the Deposit whose detailed information is to be retrieved.
        /// </param>
        /// <returns>
        /// Returns a <see cref="DsCashChecksResp"/> object containing detailed Deposit information,  
        /// including related cash and check details, along with Status and StatusCode.  
        /// If successful, StatusCode will be 200 (OK); otherwise, it returns the appropriate status and message.
        /// </returns>
        [Route("GetDeposits")]
        [HttpGet]
        public async Task<IActionResult>GetDepositsDetails(long depositId)
        {
            DsCashChecksResp response = new DsCashChecksResp();
            response = await DsCashAndCheck.GetDepositsDetails(depositId);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }


        //[Route("PendingExcessATD")]
        //[HttpGet]
        //public async Task<IActionResult> GetPendingExeccOrShortageATD(string CorpID, string PCID)
        //{
        //    PendingExcessOrShortageResp response = new PendingExcessOrShortageResp();
        //    response = await DsCashAndCheck.GetPendingExcessOrShortageATD(CorpID, PCID);
        //    if (response != null && response.StatusCode == StatusCodes.Status200OK)
        //        return Ok(response);
        //    else if (response != null && !string.IsNullOrEmpty(response.Status))
        //        return Ok(response);
        //    else
        //        return NotFound(response);

        //}

        /// <summary>
        /// Gets adjustment opening balance, pending excess/shortage total, and pending deposit total
        /// </summary>
        /// <param name="CorpID">Corporation ID</param>
        /// <param name="PCID">Profit Center ID (optional)</param>
        /// <returns>Combined totals with status</returns>
        [Route("GetDepositAndBalanceSummary")]
        [HttpGet]
        public async Task<IActionResult> GetDepositAndBalanceSummary(string CorpID, string PCID)
        {
            var Response = await DsCashAndCheck.GetDepositAndBalanceSummary(CorpID, PCID);

            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);
        }

        [Route("RejectDeposit")]
        [HttpPost]
        public async Task<IActionResult> RejectDeposit(long DepositID, string AssignedTo, string Comments)
        {
            string UserId = (string)HttpContext.Items["UserId"];
            var Response = await DsCashAndCheck.RejectDeposit(DepositID, AssignedTo, UserId, Comments);
            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);
        }

        [Route("GetApprovalPolicyDetails")]
        [HttpPost]
        public async Task<IActionResult> GetApprovalPolicyUserDetails(string CorpID)
        {
            string UserId = (string)HttpContext.Items["UserId"];
            var Response = await DsCashAndCheck.GetApprovalPolicyDetails(CorpID, UserId);
            if (Response != null)
                return Ok(Response);
            else
                return NotFound(Response);
        }

        [Route("DeleteAdjustOpeningbalance")]
        [HttpPost]
        public async Task<IActionResult> DeleteAdjustOpeningBalance([FromBody] long AdjustmentId)
        {
            StatusDTO response = await DsCashAndCheck.DeleteAdjustOpeningBalance(AdjustmentId);

            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("GetDailySaleFromAndToDates")]
        [HttpGet]
        public async Task<IActionResult> GetDailySaleFromAndToDates(string CorpID,string Pcid)
        {
            var Response = await DsCashAndCheck.GetDailySaleFromAndToDates(CorpID, Pcid);

            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);
        }

        [Route("GetUsers")]
        [HttpGet]
        public async Task<IActionResult> GetUsersbyCorpPC(string CorpId,string PCID)
        {
            string userId = (string)HttpContext.Items["UserId"];
            GetUsersByCorpResp response = await DsCashAndCheck.GetUsersbyCorpPC(userId, CorpId,PCID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("GetEnableClients")]
        [HttpGet]
        public async Task<IActionResult> GetEnableClientDetails ()
        {
            string clientName = Convert.ToString(HttpContext.Items["ClientName"]);
            bool IsEnable = false;
            IsEnable = await DsCashAndCheck.GetEnableClientDetails(clientName);
            return Ok(IsEnable);
        }


    }
}
