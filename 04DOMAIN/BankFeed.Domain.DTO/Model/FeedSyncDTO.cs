using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedSyncDTO 
    {
        public string SyncHour { get; set; }
        public int TimeZoneID { get; set; }
    }
}
