using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{
    
    public class RedisCacheRequest
    {
        public dynamic Value { get;set; }
        public string CacheKey { get; set; }
    }
}
