//using Azure;
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
    public class FeedSynchResponse : StatusDTO
    {
        public FeedSynchResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public long SyncID { get; set; }
    }

    public class TimeZoneListResponse : StatusDTO
    {
        public TimeZoneListResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<KeyValuePairObject<int, string>> TimeZones { get; set; }
    }
  

    public class TimeZoneViewResponse : FeedSyncDTO , IStatusDTO
    {
        public TimeZoneViewResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public long ID { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
}
