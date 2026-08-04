using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Common.App.Contracts;
using Common.Domain.DTO.Model.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheSettings memoryCacheSettings;
        public CacheService(IMemoryCache memoryCache, IOptions<MemoryCacheSettings> _memoryCacheSettings)
        {
            _memoryCache = memoryCache;
            memoryCacheSettings = _memoryCacheSettings.Value;
        }

        public T GetData<T>(string key)
        {
            try
            {
                T item = default(T);
                if(!string.IsNullOrEmpty(key)) 
                    _memoryCache.TryGetValue(key, out item);
                return item;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public bool SetData<T>(string key, T value)
        {
            T item;
            bool res = false;
            if ( !string.IsNullOrEmpty(key) && value!=null  && !_memoryCache.TryGetValue(key, out item))
            {

                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(memoryCacheSettings.SlidingExpirationMinutes),
                    AbsoluteExpiration = DateTime.Now.AddMinutes(memoryCacheSettings.AbsoluteExpirationMinutes)
                };
                _memoryCache.Set(key, value, cacheOptions);
                res = true;
            }
            return res;
        }
        public bool RemoveData<T>(string key)
        {
            T item;
            bool res = false;
            if (_memoryCache.TryGetValue(key, out item))
            {
                _memoryCache.Remove(key);
                res = true;
            }
            return res;
        }
    }
}
