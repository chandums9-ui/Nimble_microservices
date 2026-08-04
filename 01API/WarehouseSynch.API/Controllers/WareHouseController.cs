using Common.API.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;

using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;


using Microsoft.Extensions.Options;
using WareHouseSynch.App.Contracts;
namespace WareHouseSynch.API.Controllers
{
    [Route("v1/Instant")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class WareHouseController : ControllerBase
    {
        #region Fields
        private readonly IInStantSynch inStantSynch;
        private IOptions<List<ServerAnlyticsGroup>> serverGroup;
        #endregion

        #region Constructor
        public WareHouseController(IInStantSynch _inStantSynch, IOptions<List<ServerAnlyticsGroup>> _serverGroup)
        {
            this.inStantSynch = _inStantSynch;
            this.serverGroup = _serverGroup;
        }
        #endregion

        #region Private Methods
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
        #endregion


        [Route("SynchAccrual")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseAccrual(WareHouseBinReq req)
        {
            
            GenericLongResponse response = await inStantSynch.SyncFactAccrual(req, getUrlID(),getClientName(),getNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }


        [Route("SynchOAccrual")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseOAccrual(WareHouseBinReq req)
        {

            GenericLongResponse response = await inStantSynch.SyncWareHouseFactOAccrual(req, getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseDailySaleRevAndStats(req, getUrlID(), getClientName(), getNPConnection());
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
          
            GenericLongResponse response = await inStantSynch.SyncWareHouseCorporations(req, getUrlID(), getClientName(), getNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("SynchCorpUpdate")]
        [HttpPost]
        public async Task<IActionResult> SyncWareHouseCorporationsUpdate(WareHouseBinReq req)
        {

            GenericLongResponse response = await inStantSynch.SyncWareHouseCorporations(req, getUrlID(), getClientName(), getNPConnection(),false);
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseAccount(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseCustomer(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseVendor(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHousePC(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHousePayrollDepartment(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseOther(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseEmployee(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseDailySaleLines(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseForeCast(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseBudget(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseIncConfiguration(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseIncomeDept(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseIncomeGroup(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseIncDeptCloning(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseIncConfigCloning(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseIncomeGroupCloning(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseAccountCloning(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseDailySaleLinesCloning(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseFactAccrualBulkImport(req,  getUrlID(), getClientName(), getNPConnection());
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
            
            GenericLongResponse response = await inStantSynch.SyncWareHouseMoreJournalsBulk(req,  getUrlID(), getClientName(), getNPConnection());
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }

    }
}

