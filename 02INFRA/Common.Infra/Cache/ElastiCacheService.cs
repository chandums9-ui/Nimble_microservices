using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Common.Infra.Cache
{
    public class ElastiCacheService : IElastiCacheService
    {
        #region Fields
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        #endregion

        #region Constructor
        public ElastiCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = connectionMultiplexer.GetDatabase();
        }
        #endregion

        #region Public Methods
        public async Task<Dictionary<string, object>> GetAllElastiCacheDataAsync()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
                var keys = server.Keys().ToList();

                foreach (var key in keys)
                {
                    var keyType = await _database.KeyTypeAsync(key);

                    if (keyType == RedisType.String)
                    {
                        var value = await _database.StringGetAsync(key);
                        if (value.HasValue)
                        {
                            result[key.ToString()] = value.ToString();
                        }
                    }
                    else if (keyType == RedisType.Set)
                    {
                        var values = await _database.SetMembersAsync(key);
                        result[key.ToString()] = values.Select(v => v.ToString()).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching Redis keys: {ex.Message}", ex);
            }

            return result;
        }

        public async Task<GetRedisCacheResponse<T>> GetElastiCacheDataAsync<T>(string cacheKey, TimeSpan? expiry = null)
        {
            GetRedisCacheResponse<T> response = new GetRedisCacheResponse<T>();

            try
            {
                var value = await _database.StringGetAsync(cacheKey);

                if (value.HasValue)
                {
                    if (typeof(T) == typeof(string))
                    {
                        response.RedisCacheData = (T)(object)value.ToString();
                    }
                    else
                    {
                        response.RedisCacheData = JsonConvert.DeserializeObject<T>(value);
                    }
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Status = Constants.MSG_DATA_FOUND;

                    if (expiry.HasValue)
                    {
                        await _database.KeyExpireAsync(cacheKey, expiry);
                    }
                }
                else
                {
                    response.StatusCode = StatusCodes.Status204NoContent;
                    response.Status = Constants.MSG_NO_DATA_FOUND;
                }
            }
            catch (Exception ex)
            {
                response.Status = $"{Constants.MSG_REDISCACHE_ConnectionLost} - {ex.Message}";
                response.StatusCode = StatusCodes.Status400BadRequest;
            }

            return response;
        }

        public async Task<GetRedisCacheResponse<List<T>>> GetElastiCacheSetDataAsync<T>(string cacheKey)
        {
            GetRedisCacheResponse<List<T>> response = new GetRedisCacheResponse<List<T>>();

            try
            {
                var values = await _database.SetMembersAsync(cacheKey);

                if (values != null && values.Length > 0)
                {
                    List<T> result = new List<T>();

                    foreach (var value in values)
                    {
                        if (typeof(T) == typeof(string))
                        {
                            result.Add((T)(object)value.ToString());
                        }
                        else
                        {
                            result.Add(JsonConvert.DeserializeObject<T>(value));
                        }
                    }
                    response.RedisCacheData = result;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Status = Constants.MSG_DATA_FOUND;
                }
                else
                {
                    response.StatusCode = StatusCodes.Status204NoContent;
                    response.Status = Constants.MSG_NO_DATA_FOUND;
                }
            }
            catch (Exception ex)
            {
                response.Status =
                    $"{Constants.MSG_REDISCACHE_ConnectionLost} - {ex.Message}";
                response.StatusCode = StatusCodes.Status400BadRequest;
            }

            return response;
        }

        public async Task<RedisCacheResponse> SetElastiCacheDataAsync<T>(string cacheKey, T value, TimeSpan? expiry = null)
        {
            var response = new RedisCacheResponse();

            try
            {
                var serialized = JsonConvert.SerializeObject(value);
                TimeSpan expiryTimeSpan = expiry ?? TimeSpan.FromDays(7);
                bool isSuccess = await _database.StringSetAsync(cacheKey, serialized, expiryTimeSpan);

                response.IsSuccess = isSuccess;
                response.StatusCode = isSuccess ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
                response.Status = isSuccess ? Constants.MSG_REDISCACHE_ADD_SUCCESS : Constants.MSG_REDISCACHE_ADD_FAIL;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Status = $"{Constants.MSG_REDISCACHE_ConnectionLost} - {ex.Message}";
            }

            return response;

        }

        public async Task<RedisCacheResponse> RemoveElastiCacheDataAsync(string cacheKey)
        {
            RedisCacheResponse response = new RedisCacheResponse();

            try
            {
                var cacheKeys = cacheKey.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(k => (RedisKey)k.Trim()).ToArray();

                if (cacheKeys.Length == 0)
                {
                    response.IsSuccess = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "No cache keys provided";
                    return response;
                }
                long deletedCount = 0;
                foreach (var key in cacheKeys)
                {
                    if (await _database.KeyDeleteAsync(key))
                        deletedCount++;
                }
                response.IsSuccess = deletedCount > 0;
                response.StatusCode = StatusCodes.Status200OK;
                response.Status = $"{Constants.MSG_REDISCACHE_DELETE_SUCCESS} - Deleted {deletedCount} keys";

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Status = $"{Constants.MSG_REDISCACHE_DELETE_FAIL} - {ex.Message}";
            }

            return response;
        }

        public async Task<RedisCacheResponse> InsertElastiCacheDataAsync<T>(string cacheKey, T newValue)
        {
            var response = new RedisCacheResponse();

            try
            {
                var cachedValue = await _database.StringGetAsync(cacheKey);
                var list = cachedValue.HasValue
                    ? JsonConvert.DeserializeObject<List<T>>(cachedValue)
                    : new List<T>();

                list.Add(newValue);

                var serialized = JsonConvert.SerializeObject(list);
                await _database.StringSetAsync(cacheKey, serialized, TimeSpan.FromDays(7));

                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Status = Constants.MSG_REDISCACHE_ADD_SUCCESS;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Status = $"{Constants.MSG_REDISCACHE_ConnectionLost} - {ex.Message}";
            }

            return response;
        }
        public async Task<RedisCacheResponse> DeleteAllKeysAsync()
        {
            RedisCacheResponse response = new RedisCacheResponse();

            try
            {
                var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());

                var keys = server.Keys();

                int deletedCount = 0;
                foreach (var key in keys)
                {
                    bool deleted = await _database.KeyDeleteAsync(key);
                    if (deleted) deletedCount++;
                }

                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Status = $"{Constants.MSG_REDISCACHE_DELETE_SUCCESS} - Deleted {deletedCount} keys";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Status = $"{Constants.MSG_REDISCACHE_DELETE_FAIL} - {ex.Message}";
            }

            return response;
        }

        public async Task<RedisCacheResponse> SetSlidingElastiCacheAsync<T>(string cacheKey, T value, TimeSpan? expiry = null)
        {
            var response = new RedisCacheResponse();

            try
            {
                var serializedValue = JsonConvert.SerializeObject(value);

                bool isSet = await _database.StringSetAsync(cacheKey, serializedValue, expiry);

                response.IsSuccess = isSet;
                response.StatusCode = isSet ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
                response.Status = isSet
                    ? Constants.MSG_REDISCACHE_ADD_SUCCESS
                    : Constants.MSG_REDISCACHE_ADD_FAIL;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Status = $"{Constants.MSG_REDISCACHE_ConnectionLost} - {ex.Message}";
            }

            return response;
        }

        public async Task<RedisCacheResponse<T>> GetSlidingElastiCacheAsync<T>(string cacheKey, TimeSpan? expiry = null)
        {
            var response = new RedisCacheResponse<T>();

            try
            {
                var value = await _database.StringGetAsync(cacheKey);

                if (!value.IsNullOrEmpty)
                {
                    if (expiry.HasValue)
                    {
                        await _database.KeyExpireAsync(cacheKey, expiry.Value);
                    }
                    var data = JsonConvert.DeserializeObject<T>(value);

                    response.RedisCacheData = data;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Status = Constants.MSG_DATA_FOUND;
                }
                else
                {
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = Constants.MSG_NO_DATA_FOUND;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Status = $"{Constants.MSG_REDISCACHE_ConnectionLost} - {ex.Message}";
            }

            return response;
        }

        public async Task<List<string>> RemoveCacheByMultiCorpAsync(string environmentPrefix, CacheSynchMessage message)
        {
            var deletedKeys = new List<string>();

            try
            {
                string reverseIndexKey = $"{environmentPrefix}:corp_index:{message.URLID}:{message.ClientID}:{message.CorporationID}";

                var cacheKeys = await _database.SetMembersAsync(reverseIndexKey);

                if (cacheKeys == null || cacheKeys.Length == 0)
                    return deletedKeys;

                foreach (var cacheKey in cacheKeys)
                {
                    var key = (RedisKey)cacheKey.ToString();

                    await _database.KeyDeleteAsync(key);
                    deletedKeys.Add(key.ToString());
                }

                // Delete reverse index itself
                await _database.KeyDeleteAsync(reverseIndexKey);

                return deletedKeys;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<string>> GetKeysByPrefixAsync(string prefix)
        {
            var keys = new List<string>();

            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());

            foreach (var key in server.Keys(pattern: $"{prefix}*"))
            {
                keys.Add(key.ToString());
            }

            return keys;
        }

        public async Task<bool> SetAddAsync(string key, string value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                var added = await _database.SetAddAsync(key, value);

                if (expiry.HasValue)
                {
                    await _database.KeyExpireAsync(key, expiry);
                }
                return added;
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}
