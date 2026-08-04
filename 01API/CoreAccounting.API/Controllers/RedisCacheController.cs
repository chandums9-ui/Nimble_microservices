using Azure;
using BankFeed.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Common.Infra.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    public class RedisCacheController : BaseController
    {
        #region Fileds
        private readonly IRedisCacheService redisCacheService;
        #endregion

        #region Ctor
        public RedisCacheController(IRedisCacheService redisCacheService)
        {
            this.redisCacheService = redisCacheService;
        }
        #endregion

        [Route("RedisCache/Get")]
        [HttpGet]
        public async Task<IActionResult> GetRedisCacheData([FromQuery] RedisCacheRequest redisCacheRequest)
        {
            try
            {
                var response = redisCacheService.GetData<dynamic>(redisCacheRequest);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);
            }
            catch { throw; }
        }



        [Route("RedisCache/Set")]
        [HttpPost]
        public async Task<IActionResult> AddRedisCacheData(RedisCacheRequest redisCacheRequest)
        {
            
            try
            {
                var response = redisCacheService.SetData<dynamic>(redisCacheRequest.CacheKey, redisCacheRequest.Value);
                if (response != null && response.StatusCode == StatusCodes.Status200OK )
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);
                
            }
            catch { throw; }
            
        }

        [Route("RedisCache/Delete")]
        [HttpPut]
        public async Task<IActionResult> DeleteRedisCacheData(RedisCacheRequest redisCacheRequest)
        {
            RedisCacheResponse? response = null;
            try
            {
                response = await redisCacheService.RemoveData<RedisCacheResponse>(redisCacheRequest);
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
    }

}
