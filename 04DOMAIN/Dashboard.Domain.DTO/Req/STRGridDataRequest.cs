using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class STRGridDataRequest
    {
        [JsonProperty("corporation_id")]
        public string corporation_id { get; set; } = string.Empty;

        [JsonProperty("filetype")]
        public string filetype { get; set; } = string.Empty;

        [JsonProperty("year")]
        public string year { get; set; } = string.Empty;

        [JsonProperty("month")]
        public string month { get; set; } = string.Empty;

        [JsonProperty("profit_center")]
        public string profit_center { get; set; } = string.Empty;
    }
}
