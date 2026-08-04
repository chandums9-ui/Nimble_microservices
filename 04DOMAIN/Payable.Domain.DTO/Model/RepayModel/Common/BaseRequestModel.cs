
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Payable.Domain.DTO.Model.RepayModel.Common
{
    public class RepayBaseRequestModel
    {
       
        public RepayBaseRequestModel()
        {
            LogTypes = new List<LogLevel>();
        }
        [JsonIgnore]
        public List<LogLevel> LogTypes { get; set; }
    }
}
