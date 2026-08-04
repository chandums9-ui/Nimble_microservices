using Common.API.Authorization;
using Payable.App.Contracts;
using Payable.App.Service;
using Payable.Domain.DTO.Resp;
using Payable.Domain.DTO.Req;
using Common.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Payable.Domain.DTO.Model;
using Common.Domain.DTO.App;
using static Payable.Domain.DTO.Model.EpaymentModels;
using Microsoft.IdentityModel.Tokens;
using Payable.Domain.DTO.Enums;
using Azure;
using Org.BouncyCastle.Ocsp;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;

namespace Payable.API.Controllers
{
    [Route("v1/Epay")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class EPaymentsController : ControllerBase
    {
        #region Fields
        private readonly IEPaymentsService epayService;
        #endregion

        #region ctor
        public EPaymentsController(IEPaymentsService _epayservice)
        {
            this.epayService = _epayservice;
        }
        #endregion

        #region Private Methods

        private string getClientID()
        {
            return (string)HttpContext.Items["ClientId"];
        }
        private string GetUserID()
        {
            return (string)HttpContext.Items["UserId"];
        }
        private string GetClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }

        #endregion

        [Route("GetConfiguredAccounts")]
        [HttpGet]
        public async Task<IActionResult> GetConfiguredAccounts(string CorpID,short PayMethodType)
        {
            ConfigAccountsResponse resp = new ConfigAccountsResponse();
            resp = await epayService.GetConfigAccountsList(CorpID, PayMethodType,getClientID());
            return Ok(resp);
        }

        [Route("ExportEFTExcel")]
        [HttpPost]
        public async Task<IActionResult> ExportEFTData(List<EFTExportRequest> BatchIDs)
        {
            EFTExportResponse resp = new EFTExportResponse();
            resp = await epayService.CreateEFTExport(BatchIDs, GetClientName(),getClientID());
            return Ok(resp.Files);
            //if (result.StatusCode == StatusCodes.Status200OK)
            //    return Ok(result);
            //else
            //    return NotFound(result);
        }
        #region Epay Config
        [Route("loadConfigs")]
        [HttpPost]
        public async Task<IActionResult> GetConfigurations(LoadEPayConfigRequest Req)
        {
            LoadEPayConfigResponse result = await epayService.LoadEPayConfigs(Req, GetUserID());
            return Ok(result);
            
        }

        [Route("loadproviderconfig")]
        [HttpPost]
        public async Task<IActionResult> GetProviderConfiguration(LoadProviderRequest Req)
        {
            var result = await epayService.LoadProviderConfig(Req, GetUserID());
            return Ok(result);
        }

        [Route("loadAuditLog")]
        [HttpPost]
        public async Task<IActionResult> GetProviderAuditLog(LoadProviderRequest Req)
        {
            var result = await epayService.LoadAuditHistory(Req , GetUserID());
            return Ok(result);
        }
        [Route("DDDefaultServiceType")]
        [HttpGet]
        public async Task<IActionResult> DDDefaultServiceType()
        {
            var result = await epayService.DirectDepositDefaultServiceType(getClientID());
                return Ok(result);

        }
        [Route("AddConfig")]
        [HttpPost]
        public async Task<IActionResult> SaveConfiguration(SaveConfigRequest Req)
        {
            var result = await epayService.CreateNewConfiguration(Req, GetUserID(),getClientID());
            if (result != null && result.StatusCode == StatusCodes.Status200OK)
                return Ok(result);
            else if (result != null && !string.IsNullOrEmpty(result.Status))
                return Ok(result);
            else
                return NotFound(result);
        }

        [Route("updateconfig")]
        [HttpPost]
        public async Task<IActionResult> UpdateConfig(SaveConfigRequest Req)
        {
            var result = await epayService.UpdateProviderConfig(Req, GetUserID(),getClientID());
            return Ok(result);
        }

        [Route("deleteconfig")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfig(LoadProviderRequest Req)
        {
            var result = await epayService.DeleteProviderConfig(Req, GetUserID(),getClientID());
            return Ok(result);
        }

        [Route("EFTFormat/Save")]
        [HttpPost]
        public async Task<IActionResult> SaveEFTFormat(EFTFormatConfig Req)
        {
            var result = await epayService.SaveEFTFormatConfig(Req, getClientID());
            return Ok(result);
        }

        [Route("EFTFormat/Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateEFTFormat(EFTFormatConfig Req)
        {
            var result = await epayService.UpdateEFTFormatConfig(Req, getClientID());
            if (result.StatusCode == StatusCodes.Status200OK)
                return Ok(result);
            else
                return NotFound(result);
        }
        [Route("EFTFormat/Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteEFTFormat([FromBody]long FormatID)
        {
            var result = await epayService.DeleteEFTFormatConfig(FormatID);
            if (result != null && result.StatusCode == StatusCodes.Status200OK)
                return Ok(result);
            else if (result != null && !string.IsNullOrEmpty(result.Status))
                return Ok(result);
            else
                return NotFound(result);
        }
        [Route("EFTFormat/FormatsList")]
        [HttpGet]
        public async Task<IActionResult> LoadEFTFormatList()
        {
            var result = await epayService.LoadEFTFormatList(getClientID());
            return Ok(result);
        }
        [Route("EFTFormat/Load")]
        [HttpGet]
        public async Task<IActionResult> LoadEFTFormat([FromQuery] long EntityId)
        {
            var result = await epayService.LoadEFTFormat(EntityId);
            return Ok(result);
            
        }
        [Route("EFTFormat/FormatColumns")]
        [HttpGet]
        public async Task<IActionResult> LoadEFTFormatColumns()
        {
            var result = await epayService.LoadFormatColumns();
            return Ok(result);
        }

        #endregion Epay Config end

        #region EPayments

        /// <summary>
        /// Export Direct Deposit API
        /// </summary>
        /// <param name="EpaymentInfo"></param>
        /// <returns></returns>
        [Route("ExportDirectDepositFile")]
        [HttpPost]
        public async Task<IActionResult> ExportDirectDeposit(DDExportRequest EpaymentInfo)
        {
            List<DirectDepositExportResponse> response = new List<DirectDepositExportResponse>();
            response = await epayService.ExportDD(EpaymentInfo,GetUserID());
            return Ok(response);

        }
        /// <summary>
        /// Get Tobeappoved/ Pending Payments Count
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [Route("EPaymentsCount")]
        [HttpPost]
        public async Task<IActionResult> EPaymentsCountByStatusWise(EPaymentCountRequest Req)
        {

            var result = await epayService.EPaymentsCountByStatusWise(Req, getClientID());
          
                return Ok(result);

        }

        [Route("EPaymentsPendingAmountByCorporation")]
        [HttpPost]
        public async Task<IActionResult> GetEPaymentsPendingAmount(EPayCorpList corpList)
        {

            var result = await epayService.GetEPaymentsPendingAmount(corpList.CorpList);

            return Ok(result);

        }
        [Route("EPaymentsBatchNoGeneration")]
        [HttpPost]
        public async Task<IActionResult> EPaymentsBatchNoGeneration(EPaymentMultipleBatchNoGenerationRequest Req)
        {
            EPaymentsMultipleBatchNoResponse res = new EPaymentsMultipleBatchNoResponse();
            res.EPaymentsBatchNoList = new List<EPaymentsBatchNoResponse>();

            if (Req != null && Req.BatchNoReqList != null && Req.BatchNoReqList.Any())
            {
                string prevCorId, prevAccountID;
                short prevPaymentMeythod = 0;
                prevCorId = prevAccountID = string.Empty;
                long prevBatchNo = 0;

                foreach (var reqItem in Req.BatchNoReqList)
                {
                    EPaymentsBatchNoResponse result = new EPaymentsBatchNoResponse();

                    if (string.IsNullOrEmpty(prevCorId) && string.IsNullOrEmpty(prevAccountID) && prevPaymentMeythod != 0)
                    {
                        prevCorId = reqItem.CorpID;
                        prevAccountID = reqItem.AccountID;
                        prevPaymentMeythod = reqItem.PayMethodType;
                    }
                    reqItem.ClientId = getClientID();
                    if (reqItem.CorpID == prevCorId && reqItem.AccountID == prevAccountID && reqItem.PayMethodType == prevPaymentMeythod)
                    {
                        if (prevBatchNo != 0)
                        {
                            result.BatchNo = prevBatchNo++;
                            result.CorpID = reqItem.CorpID;
                            result.AccountId = reqItem.AccountID;
                            result.PaymentMethod = reqItem.PayMethodType;
                            result.IssueType = reqItem.IssueType;
                            res.EPaymentsBatchNoList.Add(result);
                        }

                    }
                    else
                    {
                        result = await epayService.EPaymentBatchNoGeneration(reqItem);
                        prevBatchNo = result.BatchNo;
                        res.EPaymentsBatchNoList.Add(result);
                    }
                }
            }
            if (res != null && res.EPaymentsBatchNoList != null && res.EPaymentsBatchNoList.Count > 0)
            {
                return Ok(res);
            }
            else
                return NotFound(res);


        }

        /// <summary>
        /// To Get Latest BatchNo's based on AccountId&Corp
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [Route("GenerateEPaymentsBatchNo")]
        [HttpPost]
        public async Task<IActionResult> GenerateEPaymentsBatch(List<EPaymentBatchNoGenerationRequest> Req)
        {

            var resp = await epayService.GenerateBatchNum(Req, getClientID());
            return Ok(resp);

        }
        /// <summary>
        ///  To Get Latest CheckNo's based on AccountId&Corp
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [Route("GenerateEpayCheckNo")]
        [HttpPost]
        public async Task<IActionResult> LoadEPaymentsCheckNo(List<EPayCheckNoRequest> Req)
        {

            var resp = await epayService.GetLatestEPayCheckNum(Req, getClientID(),GetUserID());
            return Ok(resp);

        }
        /// <summary>
        /// Load Pending Epaymnts(EFT/Repay/DD)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("LoadPendingEPayments")]
        [HttpPost]
        public async Task<IActionResult> LoadPendingEPayments(PendingEPaymentsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PendingEPaymentsInfo response = await epayService.LoadPendingEPayments(request, GetUserID(), getClientID());
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
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("LoadEPayCorporationList")]
        [HttpGet]
        public async Task<IActionResult> EPayCorporationList()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await epayService.LoadEPaymentCorpList(GetUserID());
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
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("CheckCorporationLegalName")]
        [HttpGet]
        public async Task<IActionResult> CheckCorporationLegalName()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await epayService.CheckCorporationLegalName(GetUserID());
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
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Load TobeApporove/ Approved/ Processing/ PaidOr Export Epay List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("LoadEPayments")]
        [HttpPost]
        public async Task<IActionResult> LoadEPayments(LoadEPaymentsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LoadEPaymentsInfo response = await epayService.LoadEPayments(request, GetUserID(), getClientID());
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
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Approve EPayments Batch
        /// </summary>
        /// <param name="EpaymentInfo"></param>
        /// <returns></returns>
        [Route("AddEPaymentProcess")]
        [HttpPost]
        public async Task<IActionResult> AddEPayment(EPaymentInfo EpaymentInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EPaymentResponse response = await epayService.CreateEPayments(EpaymentInfo, GetUserID(), getClientID(),GetClientName());
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
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        /// <summary>
        /// Create Batch For Pending Epayments
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("AddPendingEPayments")]
        [HttpPost]
        public async Task<IActionResult> AddPendingEPayments(PostPendingEPaymentInfo req)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await epayService.AddPendingEPayments_V1(req, GetUserID(), getClientID(),GetClientName());
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
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("RemovePendingEPayments")]
        [HttpPost]
        public async Task<IActionResult> RemovePendingEPaymentsFromBatch(BatchIdsRequest req)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    req.userId = GetUserID();
                    var response = await epayService.RemoveEpaymentsFromBatch(req);
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
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }

        [Route("AllowApproveEPayments")]
        [HttpPost]
        public async Task<IActionResult> AllowApproveEPayments(BatchIdsRequest req)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    req.userId = GetUserID();
                    var response = await epayService.AllowEPaymentsBatchApprove(req);
                        return Ok(response);        
                }
                else
                {
                    return BadRequest();
                }
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        }
        [Route("ExportDDSampleFile")]
        [HttpGet]
        public async Task<IActionResult> ExportDDSampleFile(string Id)
        {
        
          var  response = await epayService.ExportDDSample(Id);
            return Ok(response);

        }
        /// <summary>
        ///  Get All BatchNo's based on AccountId&Corp
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [Route("LoadEPayBatchNoList")]
        [HttpPost]
        public async Task<IActionResult> LoadEPayBatchNoList(LoadEPaymentsRequest req)
        {

            var resp = await epayService.LoadEPaymentBatchNoList(getClientID(),req.PaymentType, req.CorpIds, req.ActionType, req.StartDate,req.EndDate,req.VenId);
            return Ok(resp);

        }
        [Route("EPayLogReport")]
        [HttpPost]

        public async Task<IActionResult> EPayLogReport(EPayLogRequest req)
        {
            
            var resp = await epayService.EPayLogReport(req, getClientID());
            return Ok(resp);

        }
       
        [Route("EPayAccess")]
        [HttpGet]

        public async Task<IActionResult> EPaymentAccess()
        {

            var resp = await epayService.EPayUserPermissions(GetUserID());
            if(resp!=null)
            {
                if(resp.Eftaccess!=1)
                {
                    return Ok(false);
                }
                else
                {
                    return Ok(true);
                }
            }
            else
            {
                return Ok(false);
            }
            

        }

        [Route("GetCheckDirectDepositAPI")]
        [HttpPost]

        public async Task<IActionResult> CheckDDAPI(EPayCorpList corpList)
        {
            
            var resp = await epayService.CheckDirectDepositAPI(corpList.CorpList);
            return Ok(resp);


        }
        [Route("DirectDepositLogInfo")]
        [HttpGet]

        public async Task<IActionResult> DirectDepositLogInfo(long Id)
        {

            var resp = await epayService.DirectDepositLogInfo(Id);
            return Ok(resp);


        }

        [Route("EpayReexportDetails")]
        [HttpPost]

        public async Task<IActionResult> EpayReexportDetails(EpayReexportReq req)
        {

            var resp = await epayService.EpayReexportDetails(req,getClientID());
            return Ok(resp);


        }

        [Route("GetEPayLogTempDetails")]
        [HttpGet]

        public async Task<IActionResult> GetEPayLogTempDetails(long Id)
        {

            var resp = await epayService.GetEPayLogTempDetails(Id);
            return Ok(resp);

        }

       
        [Route("CheckEpayProviderPreferences")]
        [HttpPost]
        public async Task<IActionResult> CheckEpayProviderPreferences(EPayCorpList req)
        {
            var resp = await epayService.CheckEpayProviderPreferences(req,getClientID());
            return Ok(resp);
        }



        [Route("RepayProcessedPayments")]
        [HttpPost]
        public async Task<IActionResult> RepayProcessedPayments(GetRepayProcessedList req)
        {
            var resp = await epayService.RepayProcessedPayments(req);
            return Ok(resp);
        }

        [Route("EPaymentVendorList")]
        [HttpPost]
        public async Task<IActionResult> EpayVenors(GetVendorNamesByUserOrCorpRequest req)
        {
            var resp = await epayService.GetEpaymentVendors(req,GetUserID());
            return Ok(resp);
        }
        [Route("GetRepayLogReference")]
        [HttpGet]
        public async Task<IActionResult> GetRepayLogReference(string corpIds, string BatchIds, int paymenttype, string stdate, string eddate)
        {
            DateTime sdate = DateTime.ParseExact(stdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime edate = DateTime.ParseExact(eddate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var resp = await epayService.GetEPayLogRefId(corpIds,null, paymenttype, sdate, edate);
            return Ok(resp);
        }


        [Route("UpdateEpayIntiationDate")]
        [HttpPost]
       
        public async Task<IActionResult> UpdateEpayIntiationDate(List<UpdateDirectDepositBatch> obj)
        {

            var resp = await epayService.UpdateEpayIntiationDate(obj,GetUserID());
            return Ok(resp);
        }





        #endregion Epayments end

        [AllowAnonymous]
        [Route("test")]
        [HttpGet]
        public async Task<IActionResult> test(string Id)
        {

            byte[] encoded = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Id));
            var value = BitConverter.ToUInt32(encoded, 0) % 1000000;
            // return value.ToString();
            return Ok(value);

        }

    }
}
