using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Common.Infra.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        #region Fileds
        private readonly RedisCacheSettings redisCacheSettings;
        #endregion

        private ConnectionMultiplexer redis = null;

        #region Ctor

        public RedisCacheService(IOptions<RedisCacheSettings> redisCacheSettings)
        {
            this.redisCacheSettings = redisCacheSettings.Value;
            //LoadRedisCacheCredentials();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves data from Redis cache database for a given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public GetRedisCacheResponse<T> GetData<T>(RedisCacheRequest redisCacheRequest)
        {
            GetRedisCacheResponse<T> response = new GetRedisCacheResponse<T>();
            var redisConnection = $"{redisCacheSettings.RedisIp}"; // $"{redisCacheSettings.RedisIp},password={redisCacheSettings.Password}";
            redis = ConnectionMultiplexer.Connect(redisConnection);
            if (redis != null)
            {
                var value = redis.GetDatabase().StringGet(redisCacheRequest.CacheKey);
                if (!string.IsNullOrEmpty(value))
                {
                    // Deserialize the cached JSON string into the expected type.
                    response.RedisCacheData = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);

                    response.StatusCode = StatusCodes.Status200OK;
                    response.Status = Constants.MSG_DATA_FOUND;
                }
                else
                {
                    // If value is not found, set the status code to 204 (No Content)
                    response.StatusCode = StatusCodes.Status204NoContent;
                    response.Status = Constants.MSG_NO_DATA_FOUND;
                }
            }
            else
            {
                response.Status=Constants.MSG_REDISCACHE_ConnectionLost;
                response.StatusCode = StatusCodes.Status400BadRequest;
            }
            redis.Close();
            redis = null;
            return response;
        }

        /// <summary>
        /// Sets data in the Redis cache for a given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        public async Task<RedisCacheResponse> SetData<T>(string cacheKey, T Value)
        {
            RedisCacheResponse response = new RedisCacheResponse();
            var redisConnection = $"{redisCacheSettings.RedisIp}";//,password={redisCacheSettings.Password}";
            redis = ConnectionMultiplexer.Connect(redisConnection);
            if (redis != null)
            {
                // Check if the key already exists in the cache
                if (await redis.GetDatabase().KeyExistsAsync(cacheKey))
                {
                    response.IsSuccess = false;
                    response.Status = Constants.MSG_REDISCACHE_KEY_EXIST;
                    response.StatusCode = StatusCodes.Status200OK;
                    return response;
                }

                // Serialize the value to a JSON string
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(Value);

                // Set the serialized data in the Redis cache with the specified key.
                bool isAddRedisData = redis.GetDatabase().StringSet(cacheKey, serializedData);

                if (isAddRedisData)
                {
                    response.IsSuccess = isAddRedisData;
                    response.Status = Constants.MSG_REDISCACHE_ADD_SUCCESS;
                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    response.IsSuccess = isAddRedisData;
                    response.Status = Constants.MSG_REDISCACHE_ADD_FAIL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                response.Status = Constants.MSG_REDISCACHE_ConnectionLost;
                response.StatusCode = StatusCodes.Status400BadRequest;
            }
            redis.Close();
            redis = null;
            return response;
        }

        /// <summary>
        /// Removes data from the Redis cache for a given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<RedisCacheResponse> RemoveData<T>(RedisCacheRequest redisCacheRequest)
        {
            RedisCacheResponse response = new RedisCacheResponse();
            bool isDeleteRedisCache = false;
            var redisConnection = $"{redisCacheSettings.RedisIp}"; //$"{redisCacheSettings.RedisIp},password={redisCacheSettings.Password}";
            redis = ConnectionMultiplexer.Connect(redisConnection);
            if (redis!=null)
            {
                if (await redis.GetDatabase().KeyExistsAsync(redisCacheRequest.CacheKey))
                {
                    isDeleteRedisCache = await DeleteRedisCacheKeys(redisCacheRequest.CacheKey);
                }
                if (isDeleteRedisCache)
                {
                    response.IsSuccess = isDeleteRedisCache;
                    response.Status = Constants.MSG_REDISCACHE_DELETE_SUCCESS;
                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    response.IsSuccess = isDeleteRedisCache;
                    response.Status = Constants.MSG_REDISCACHE_DELETE_FAIL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                response.Status = Constants.MSG_REDISCACHE_ConnectionLost;
                response.StatusCode = StatusCodes.Status400BadRequest;
            }
            redis.Close();
            redis = null;
            return response;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method to access Redis Database credentials
        /// </summary>
        private void LoadRedisCacheCredentials()
        {
            
        }

        /// <summary>
        /// This method returns all the matched keys from the redis cache database and delete them.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private async Task<bool> DeleteRedisCacheKeys(string cacheKey)
        {
            IDatabase db = redis.GetDatabase();

            var matchingKeys2 = await GetKeysByPattern(redis, $"{cacheKey.Split('|')[0]}|*");

            matchingKeys2 = matchingKeys2.Where(x => x == $"{cacheKey.Split('|')[0]}|{cacheKey.Split('|').LastOrDefault()}" || x.Contains(cacheKey.Split('|')[1])).ToList();

            bool isDeleteRedisCache = false;
            if (matchingKeys2.Any())
            {
                foreach (var key in matchingKeys2)
                {
                    // Delete the cached data from Redis using the provided key
                    isDeleteRedisCache = await db.KeyDeleteAsync(key);
                }
            }
            return isDeleteRedisCache;
        }

        /// <summary>
        /// Returns all the keys from the redis cache that match a pattern.
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> GetKeysByPattern(ConnectionMultiplexer redis, string pattern)
        {
            var server = redis.GetServer(redis.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern);
            return keys.Select(k => k.ToString());
        }
        #endregion
    }
}
