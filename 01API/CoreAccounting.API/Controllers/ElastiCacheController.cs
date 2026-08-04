using Common.API.ActionFilters;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    public class ElastiCacheController : ControllerBase
    {
        #region Fileds

        private readonly IElastiCacheService elastiCacheService;

        #endregion

        #region Ctor
        public ElastiCacheController(IElastiCacheService elastiCacheService)
        {
            this.elastiCacheService = elastiCacheService;
        }
        #endregion

        #region Public Methods

        [Route("ElastiCache/GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAllRedisCacheData()
        {
            try
            {
                var allData = await elastiCacheService.GetAllElastiCacheDataAsync();

                if (allData != null && allData.Any())
                    return Ok(new { StatusCode = StatusCodes.Status200OK, Status = Constants.MSG_DATA_FOUND, Data = allData });
                else
                    return Ok(new { StatusCode = StatusCodes.Status204NoContent, Status = Constants.MSG_NO_DATA_FOUND, Data = allData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { StatusCode = StatusCodes.Status500InternalServerError, Status = Constants.MSG_REDISCACHE_ConnectionLost, Message = ex.Message });
            }
        }

        [Route("ElastiCache/GetByCacheKey")]
        [HttpGet]
        public async Task<IActionResult> GetRedisCacheData([FromQuery] string CacheKey)
        {
            try
            {
                var response = await elastiCacheService.GetElastiCacheDataAsync<string>(CacheKey);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);
            }
            catch { throw; }
        }

        [Route("ElastiCache/GetSetByCacheKey")]
        [HttpGet]
        public async Task<IActionResult> GetRedisSetCacheData([FromQuery] string CacheKey)
        {
            try
            {
                var response = await elastiCacheService.GetElastiCacheSetDataAsync<string>(CacheKey);

                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);

                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);
            }
            catch
            {
                throw;
            }
        }

        [Route("ElastiCache/Set")]
        [HttpPost]
        public async Task<IActionResult> AddRedisCacheData(RedisCacheRequest redisCacheRequest)
        {

            try
            {
                var response = await elastiCacheService.SetElastiCacheDataAsync<dynamic>(redisCacheRequest.CacheKey, redisCacheRequest.Value);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else if (response != null && !string.IsNullOrEmpty(response.Status))
                    return Ok(response);

                else
                    return NotFound(response);

            }
            catch { throw; }

        }

        [Route("ElastiCache/Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteRedisCacheData(RedisCacheRequest redisCacheRequest)
        {
            RedisCacheResponse? response = null;
            try
            {
                response = await elastiCacheService.RemoveElastiCacheDataAsync(redisCacheRequest.CacheKey);
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

        [Route("ElastiCache/DeleteAll")]
        [HttpPost]
        public async Task<IActionResult> DeleteAllRedisCacheData()
        {
            RedisCacheResponse? response = null;
            try
            {
                response = await elastiCacheService.DeleteAllKeysAsync();
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

        [Route("ElastiCache/GetKeysByPrefix")]
        [HttpGet]
        public async Task<IActionResult> GetRedisKeysByPrefix([FromQuery] string prefix)
        {
            List<string>? keys = null;

            try
            {
                if (string.IsNullOrWhiteSpace(prefix))
                    return BadRequest("Prefix is required.");

                keys = await elastiCacheService.GetKeysByPrefixAsync(prefix);

                if (keys != null && keys.Any())
                    return Ok(keys);

                return NotFound($"No keys found with prefix: {prefix}");
            }
            catch
            {
                throw;
            }
            finally
            {
                keys = null;
            }
        }

        #endregion
    }
}
