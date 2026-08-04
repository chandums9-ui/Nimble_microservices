using Amazon.S3.Model;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;

namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class FundTransferController: BaseController
    {
        #region Fields

        private readonly  IFundTransferService fundTransferService;

        #endregion

        #region Ctor
        public FundTransferController(IFundTransferService fundTransferService)
        {
            this.fundTransferService = fundTransferService; 
        }

        #endregion

        [Route("LoadViewGrid")]
        [HttpPost]
        public async Task<IActionResult> ViewGrid(ViewGridReq data)
        {
            ViewGridResp Response = new ViewGridResp();
            string UserId = (string)HttpContext.Items["UserId"];
            Response = await fundTransferService.ViewGrid(data, UserId);
            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);
        }

        [Route("SaveOrEditFundTransfer")]
        [HttpPost]

        public async Task<IActionResult> SaveOrEditFundTransfer(SaveOrEditFundTransferReq data, bool IsEdit)
        {
            string UserId = (string)HttpContext.Items["UserId"];
            string ClientId = (string)HttpContext.Items["ClientId"];
            SaveOrEditFundTransferResponse Response = new SaveOrEditFundTransferResponse();
            Response = await fundTransferService.SaveOrEditFundTransfer(data, UserId,ClientId, IsEdit);
            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);
        }

        [Route("SaveOrEditReturnFundTransfer")]
        [HttpPost]

        public async Task<IActionResult> SaveOrEditReturnFundTransfer(SaveOrEditFundTransferReq data, bool IsEdit)
        {
            string UserId = (string)HttpContext.Items["UserId"];
            string ClientId = (string)HttpContext.Items["ClientId"];
            SaveOrEditFundTransferResponse Response = new SaveOrEditFundTransferResponse();
            Response = await fundTransferService.SaveOrEditReturnFundTransfer(data, UserId,ClientId, IsEdit);
            if (Response != null && Response.StatusCode == StatusCodes.Status200OK)
                return Ok(Response);
            else if (Response != null && !string.IsNullOrEmpty(Response.Status))
                return Ok(Response);
            else
                return NotFound(Response);
        }


        [Route("GetLatestAccounts")]
        [HttpGet]
        public async Task<IActionResult> GetLatestAccounts(string CorpId, bool isFundTransfer)
        {
            GetLatestDebitCreditAccountsResp response = new GetLatestDebitCreditAccountsResp();
            response = await fundTransferService.GetLatestAccounts(CorpId,isFundTransfer);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);

        }

        [Route("GetFundOrReturnTransfer")]
        [HttpGet]
        public async Task<IActionResult> GetFundOrReturnTransferDetails(string FromJournalEntryid)
        {
            GetFundTransferDetailsResp response = new GetFundTransferDetailsResp();
            response = await fundTransferService.GetFundOrReturnTransferDetails(FromJournalEntryid);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }


        [Route("DeleteFundOrReturnTransfer")]
        [HttpPost]
        public async Task<IActionResult> DeleteFundOrReturnTransfer([FromBody] string FromJournalEntryid, bool IsValidate)
        {
            SaveOrEditFundTransferResponse response = await fundTransferService.DeleteFundOrReturnTransfer(FromJournalEntryid, IsValidate);

            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }


        [Route("FundTransferVoid")]
        [HttpPost]
        public async Task<IActionResult> FundTransferVoid(FundTransferVoidRequest req)
        {
            string UserId = (string)HttpContext.Items["UserId"];
            string ClientId= (string)HttpContext.Items["ClientId"];
            StatusDTO response = await fundTransferService.FundTransferVoid(req,UserId,ClientId);

            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("ReturnTransferVoid")]
        [HttpPost]
        public async Task<IActionResult>ReturnTransferVoid(ReturnTransferVoidRequest req)
        {
            string UserId = (string)HttpContext.Items["UserId"];
            string ClientId = (string)HttpContext.Items["ClientId"];
            StatusDTO response = await fundTransferService.ReturnTransferVoid(req, UserId, ClientId);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("CorporationLockCheckTransfer")]
        [HttpPost]
        public async Task<IActionResult> GetCorporationLockingStatusForTransfer(CorporationLockRequest req)
        {
            var response = await fundTransferService.GetCorporationLockingStatusForTransfer(req);
            return Ok(response);
        }

        [Route("InterCompanyReport")]
        [HttpPost]
        public async Task<IActionResult> GetInterCompanyReportDetails([FromBody] InterCompanyReportRequest req)
        {
            string UserId = (string)HttpContext.Items["UserId"];

            InterCompanyReportResponse response = new InterCompanyReportResponse();
            response = await fundTransferService.GetInterCompanyReportDetails(req, UserId);

            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("GetCoaRuleCorporations")]
        [HttpPost]
        public async Task<IActionResult> GetCoaRulesCorporations(GetCoaRuleCorporationsReq req)
        {
            string UserId = (string)HttpContext.Items["UserId"];
            string ClientId = (string)HttpContext.Items["ClientId"];

            GetCoaRuleCorporationsListResp response = new GetCoaRuleCorporationsListResp();
            response = await fundTransferService.GetCoaRulesCorporations(req, ClientId);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);

        }

        [Route("getBalanceforAccount")]
        [HttpPost]
        public async Task<IActionResult>getBalanceforAccount([FromBody] AccountBalanceReq req)
        {
            AccountBalanceRes response = new AccountBalanceRes();
            response = await fundTransferService.GetBalanceforAccount(req);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }



    }
}
