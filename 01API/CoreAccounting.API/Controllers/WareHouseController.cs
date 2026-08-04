using Common.API.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Azure;
using Microsoft.Extensions.Options;
namespace CoreAccounting.API.Controllers
{
    [Route("v1/WareHouseSync")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class WareHouseController : BaseController
    {
        #region Fields
        private readonly IWareHouseSynch wareHouseSync;
        private IOptions<List<ServerAnlyticsGroup>> serverGroup;
        #endregion

        #region Constructor
        public WareHouseController(IWareHouseSynch _wareHouseSync, IOptions<List<ServerAnlyticsGroup>> _serverGroup)
        {
            this.wareHouseSync = _wareHouseSync;
            this.serverGroup = _serverGroup;
        }
        #endregion
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

        [Route("SynchAccrual")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseAccrual(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseFactAccrual(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchAccrualJIDs")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseAccrualJIDs(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseFactAccrualJIDS(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchCorp")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseCorporations(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseCorporations(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchAccount")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseAccount(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseAccount(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchCustomer")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseCustomer(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseCustomer(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchVendor")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseVendor(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseVendor(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchPC")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHousePC(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHousePC(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchPayrollDept")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHousePayrollDept(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHousePayrollDepartment(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchOther")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseOther(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseOther(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchEmp")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseEmp(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseEmployee(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchDailySaleLines")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseDailySaleLines(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseDailySaleLines(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchSales")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseSales(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseDailySaleRevAndStats(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchForecast")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseForecasrt(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseForeCast(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchBudget")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseBudget(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseBudget(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchIncConfig")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncConfig(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseIncConfiguration(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchIncDept")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncDept(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseIncomeDept(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }

        [Route("SynchIncGroup")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncGroup(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseIncomeGroup(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchIncDeptCloning")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseIncDeptCloning(WareHouseBinReq req)
        {
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseIncDeptCloning(req, urlKey);
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
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseIncConfigCloning(req, urlKey);
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
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseIncomeGroupCloning(req, urlKey);
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
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseAccountCloning(req, urlKey);
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
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseDailySaleLinesCloning(req, urlKey);
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
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseFactAccrualBulkImport(req, urlKey);
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
            long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
            GenericLongResponse response = await wareHouseSync.SyncWareHouseMoreJournalsBulk(req, urlKey);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("RunYEPSynch")]
        [HttpPost]
        public async Task<IActionResult> RunYEPProcessAndSynch(WareHouseBinReq req)
        {
           
            GenericLongResponse response = await wareHouseSync.RunYEPProcessAndSynch(req, getUrlID(), getClientName(), getNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }

    }
}

