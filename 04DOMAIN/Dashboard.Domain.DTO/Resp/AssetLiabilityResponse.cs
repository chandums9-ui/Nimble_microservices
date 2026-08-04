using Common.Domain.DTO.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class AssetLiabilityResponse
    {
        public List<KeyValuePairObject<string, RatioResponse>> Ratios { get; set; }
    }

    public class RatioResponse
    {
        public decimal Asset { get; set; }
        public decimal Liability { get; set; }
        public decimal CompareRatio { get; set; }
    }
}
