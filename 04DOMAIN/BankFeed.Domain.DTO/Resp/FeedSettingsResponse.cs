using BankFeed.Domain.DTO.Model; using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Resp
{
    public class FeedSettingsResponse : StatusDTO
    {
        public FeedSettingsResponse()
        {
            base.Status = Constants.MSG_NO_DATA_FOUND;
            base.StatusCode = StatusCodes.Status404NotFound;
        }
        public long FeedSettingID { get; set; }
    }

    public class FeedSettingsLoadResponse : FeedSettingsDTO, IStatusDTO
    {
        public FeedSettingsLoadResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status404NotFound;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
}
