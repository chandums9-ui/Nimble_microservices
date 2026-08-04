    using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class RedisCacheResponse : StatusDTO
    {
        public bool IsSuccess { get; set; }
    }

    public class GetRedisCacheResponse<T> : StatusDTO
    {
        public T RedisCacheData { get; set; }
    }
    public class RedisCacheResponse<T> : StatusDTO
    {
        public T RedisCacheData { get; set; }
    }
    public class CacheSynchMessage
    {
        public string MessageID { get; set; } = string.Empty;
        public long CorpKey { get; set; }
        public long URLID { get; set; }
        public string CorporationID { get; set; }
        public string ClientID { get; set; }
        public short TriggerType { get; set; }
        public string ClientName { get; set; }
    }
}
