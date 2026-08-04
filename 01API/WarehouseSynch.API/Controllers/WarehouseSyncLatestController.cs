using Common.API.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;

using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;


using Microsoft.Extensions.Options;
using WareHouseSynch.App.Contracts;
using WareHouseSynch.App.Service;
using Common.App.Contracts;
using Azure.Core;
using Azure;
namespace CoreAccounting.API.Controllers
{
    [Route("v2/Instant")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class WareHousesyncLatestController : ControllerBase
    {
        #region Fields
        private readonly ILatestInstantSync latestInstantSync;
        private IOptions<List<ServerAnlyticsGroup>> serverGroup;
        private readonly IHttpContextAccessor httpContextAcc;
        private readonly ILoggerService logger;
        #endregion

        #region Constructor
        public WareHousesyncLatestController(ILatestInstantSync _LatestInstantSync, IOptions<List<ServerAnlyticsGroup>> _serverGroup, IHttpContextAccessor httpContextAcc, ILoggerService _logger)
        {
            this.latestInstantSync = _LatestInstantSync;
            this.serverGroup = _serverGroup;
            this.httpContextAcc = httpContextAcc;
            this.logger = _logger;
        }
        #endregion

        #region Private Methods
        private long GetUrlID()
        {
            return Convert.ToInt64((string)HttpContext.Items["UrlID"]);
        }
        private string GetClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        private string GetNPConnection()
        {
            string client = GetClientName();
            string clientServer = "";
            if (serverGroup != null & serverGroup.Value.Count() > 0)
                clientServer = serverGroup.Value.Where(s => s.clients.Contains(client)).Select(s => s.ServerName).FirstOrDefault();
            return (string.IsNullOrEmpty(clientServer) ? "" : clientServer);

        }

        #endregion

        private IActionResult HandleSyncResponse(string methodName, GenericLongResponse response)
        {
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
            {
                logger.LogError($"[{methodName}] Internal Server Error. Details: {response?.Status}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else
            {
                logger.LogError($"[{methodName}] No data found or unexpected status. StatusCode: {response?.StatusCode}, Status: {response?.Status}");
                return NotFound(response);
            }
        }


        [Route("SynchAccrual")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseAccrual(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseAccrual] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncFactAccrualNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseAccrual", response);
        }

        [Route("SynchOAccrual")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseOAccrual(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseOAccrual] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncWareHouseFactOAccrualNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseOAccrual", response);
        }

        [Route("SynchBudget")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseBudget(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseBudget] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncFactBudgetNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseBudget", response);
        }

        [Route("SynchForecast")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseForecast(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseForecast] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncForecastNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseForecast", response);
        }

        [Route("SynchSales")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseSales(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseSales] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncWareHouseDailySaleRevAndStatsNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseSales", response);
        }


        [Route("SynchAccount")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseAccount(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseAccount] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncAccountsNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseAccount", response);
        }

        [Route("SynchIncConfig")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncConfig(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseIncConfig] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncINCConfigNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseIncConfig", response);
        }

        [Route("SynchOther")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseOther(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseOther] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncOthersNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseOther", response);
        }

        [Route("SynchEmp")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseEmp(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseEmp] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncEmployeeNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseEmp", response);
        }

        [Route("SynchDailySaleLines")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseDailySaleLines(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseDailySaleLines] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncDailySaleLinesNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseDailySaleLines", response);
        }

        [Route("SynchPayrollDept")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHousePayrollDept(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHousePayrollDept] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncPayrollDeptNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHousePayrollDept", response);
        }

        [Route("SynchCorp")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseCorporations(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseCorporations] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncCorporationNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseCorporations", response);
        }

        [Route("SynchCorpUpdate")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseCorporationsUpdate(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseCorporationsUpdate] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncCorporationNew(req, GetUrlID(), GetClientName(), GetNPConnection(), false);
            return HandleSyncResponse("SyncWareHouseCorporationsUpdate", response);
        }

        [Route("SynchPC")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHousePC(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHousePC] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncPCNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHousePC", response);
        }

        [Route("SynchVendor")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseVendor(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseVendor] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncVendorNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseVendor", response);
        }

        [Route("SynchCustomer")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseCustomer(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseCustomer] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncCustomerNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseCustomer", response);
        }

        [Route("SynchIncDept")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncDept(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseIncDept] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncINCDeptNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseIncDept", response);
        }

        [Route("SynchIncGroup")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncGroup(WareHouseBinReq req)
        {
            logger.LogInfo($"[SyncWareHouseIncGroup] Request received. ID: {req?.ID}:NULL,CorpID: {req?.CorpID}:NULL, FromDate: {req.FromDate}:0, ToDate: {req.ToDate}:0");
            GenericLongResponse response = await latestInstantSync.SyncINCGroupNew(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("SyncWareHouseIncGroup", response);
        }

        [Route("SynchIncDeptCloning")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncDeptCloning(WareHouseBinReq req)
        {

            GenericLongResponse response = await latestInstantSync.SyncWareHouseIncDeptCloning(req, GetUrlID(), GetClientName(), GetNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchIncConfigCloning")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncConfigCloning(WareHouseBinReq req)
        {

            GenericLongResponse response = await latestInstantSync.SyncWareHouseIncConfigCloning(req, GetUrlID(), GetClientName(), GetNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchIncGroupCloning")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncGroupCloning(WareHouseBinReq req)
        {

            GenericLongResponse response = await latestInstantSync.SyncWareHouseIncomeGroupCloning(req, GetUrlID(), GetClientName(), GetNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }

        [Route("SynchAccountCloning")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseAccountCloning(WareHouseBinReq req)
        {

            GenericLongResponse response = await latestInstantSync.SyncWareHouseAccountCloning(req, GetUrlID(), GetClientName(), GetNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchDailySaleLinesCloning")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseDailySaleLinesCloning(WareHouseBinReq req)
        {

            GenericLongResponse response = await latestInstantSync.SyncWareHouseDailySaleLinesCloning(req, GetUrlID(), GetClientName(), GetNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }


        [Route("SynchAccrualBulk")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseAccrualBulk(WareHouseBinReq req)
        {

            GenericLongResponse response = await latestInstantSync.SyncWareHouseFactAccrualBulkImport(req, GetUrlID(), GetClientName(), GetNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchMoreJournals")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseMoreJournalsBulk(WareHouseBinReq req)
        {

            GenericLongResponse response = await latestInstantSync.SyncWareHouseMoreJournalsBulk(req, GetUrlID(), GetClientName(), GetNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }

        [Route("SynchCloning")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseCloning(WareHouseBinReq req)
        {

            GenericLongResponse response = await latestInstantSync.SyncWareHouseCloning(req, GetUrlID(), GetClientName(), GetNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchDailySaleReceipts")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseDailySaleReceipts(WareHouseBinReq req)
        {
            // logger.LogInfo($"[DumpWareHouseForecast] Request received.");
            GenericLongResponse response = await latestInstantSync.SyncWareHouseDailySaleReceipts(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseForecast", response);
        }

        [Route("SynchCashReconciliation")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseCashReconciliation(WareHouseBinReq req)
        {
            // logger.LogInfo($"[DumpWareHouseForecast] Request received.");
            GenericLongResponse response = await latestInstantSync.SyncCashReconciliation(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseForecast", response);
        }

        [Route("SynchDailySalesAR")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseDailySalesAR(WareHouseBinReq req)
        {
            // logger.LogInfo($"[DumpWareHouseForecast] Request received.");
            GenericLongResponse response = await latestInstantSync.SyncDailySalesAR(req, GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseForecast", response);
        }

        [Route("DumpFactAccrual")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseAccrual()
        {
            logger.LogInfo($"[DumpWareHouseAccrual] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpFactAccrualNew(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseAccrual", response);
        }
        [Route("DumpFactBills")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseFactBills()
        {
            logger.LogInfo($"[DumpWareHouseBills] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpFactBillsNew(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseBills", response);
        }
        [Route("DumpFactBillPayments")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseFactBillPayments()
        {
            logger.LogInfo($"[DumpWareHouseBillPayments] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpFactBillPaymentsNew(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseBillPayments", response);
        }
        [Route("DumpFactBudget")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseBudget()
        {
            logger.LogInfo($"[DumpWareHouseBudget] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpFactBudgetNew(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseBudget", response);
        }
        [Route("DumpFactForecast")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseForecast()
        {
            logger.LogInfo($"[DumpWareHouseForecast] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpFactForecastNew(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseForecast", response);
        }
        [Route("DumpFactSales")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseSales()
        {
            logger.LogInfo($"[DumpWareHouseSales] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpFactSalesNew(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseSales", response);
        }
        [Route("DumpFactAdjustments")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseAdjustments()
        {
            logger.LogInfo($"[DumpWareHouseAdjustments] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpFactAdjustmentsNew(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseAdjustments", response);
        }

        [Route("DumpDailySaleReceipts")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseDailySaleReceipts()
        {
            logger.LogInfo($"[DumpWareHouseAdjustments] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpDailySaleReceipts(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseAdjustments", response);
        }

        [Route("DumpCashReconciliation")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseCashReconciliation()
        {
            logger.LogInfo($"[DumpWareHouseAdjustments] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpCashReconciliation(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseAdjustments", response);
        }

        [Route("DumpDailySalesAR")]
        [HttpGet]
        public async Task<IActionResult> DumpWareHouseDailySalesAR()
        {
            logger.LogInfo($"[DumpWareHouseAdjustments] Request received.");
            GenericLongResponse response = await latestInstantSync.DumpDailySalesAR(GetUrlID(), GetClientName(), GetNPConnection());
            return HandleSyncResponse("DumpWareHouseAdjustments", response);
        }
    }
}

