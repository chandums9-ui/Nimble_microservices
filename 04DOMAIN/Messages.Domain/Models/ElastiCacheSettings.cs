using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Domain.Models
{
    public class ElastiCacheSettings
    {
        public string PrimaryEndPoint { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool Ssl { get; set; }
        public bool AbortOnConnectFail { get; set; }
        public int ConnectTimeout { get; set; }
        public int SyncTimeout { get; set; }
        public string Database { get; set; }
    }
    public class ElastiCacheApprovalDTO
    {
        public string IsEnableElastiCache { get; set; }
        public string CacheKeyPrefix { get; set; }
        public int CacheExpirationTimeInDays { get; set; }

    }
    public class EnvironmentWiseCacheKey
    {
        public string CacheKeyPrefix { get; set; } = string.Empty;
        public int ExpirationTime { get; set; }
    }
}
