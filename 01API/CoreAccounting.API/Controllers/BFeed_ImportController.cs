using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using CoreAccounting.API.Controllers;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;

[Route("v1/BFeed_Import")]
[ApiController]
[ValidateModel]
[Authorize]
public class BFeed_ImportController : BaseController
{
    private readonly IBFeed_ImportService _service;

    public BFeed_ImportController(IBFeed_ImportService service)
    {
        _service = service;
    }

    
    [Route("GetAccountReconciliationBalance")]
    [HttpPost]
    public async Task<IActionResult> GetAccountReconciliationBalance(BFeed_ImportRequest req)
    {
        AccountReconciliationBalanceResponse response = await _service.GetAccountReconciliationBalance(req);

        if (response != null && response.StatusCode == StatusCodes.Status200OK)
            return Ok(response);

        return NotFound(response);
    }
}