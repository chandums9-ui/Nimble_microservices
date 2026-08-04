using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class RedisCacheDTO
    {
    }

    public class RedisCacheSettings
    {
        public string RedisIp { get; set; }
        public string Password { get; set; }
    }

    public class RedisCacheApprovalDTO
    {
        public string IsEnableRedisCache { get; set; }
        
    }

}
