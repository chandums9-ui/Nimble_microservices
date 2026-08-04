using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
    public class FeedAccountRequest:PageDTO
    {
        public List<KeyValuePairObject<string, string, long>> CorpIDs { get; set; }

        /// <summary>
        /// refer BankAccountStatusEnum
        /// </summary>
        public short AccStatusFilter { get; set; }

        /// <summary>
        /// Interval time in hours for user active connections background sync process 
        /// </summary>
        public int SyncInterval { get; set; } = 1;
        //public PageDTO Page { get; set; } = new PageDTO();

    }
    public class InstitutionRequest: ModelBaseIDInt64
    {

        /// <summary>
        /// refer BankAccountStatusEnum
        /// </summary>
        public short AccStatusFilter { get; set; }
        public short AccStatus { get; set; }
    }
    public class FeedAccountDeleteRequest : FeedAccountDeleteDTO
    {

    }

    public class FeedAccountMappingRequest
    {
        public List<FeedAccountMappingDTO> FeedAccountMapping { get; set; }
    }

    public class MergeSettingsRequest : MergeSettingsListResponse
    {
        public long FeedAccountID { get; set; } = 0;

    }

    public class AccountActiveRequest :LoadByLongIDRequest
    {
        public short Status { get; set; }
    }
    public class FeedAccMappingCheckingRequest
    {
        public long CurrProvID { get; set; }
        public string CorpID { get; set; }
        public string FeedAccID { get; set; }
        public long FeedID { get; set; }
        public long OldFeedID { get; set; }

    }
    public class FeedAccMappingChangingRequest
    {
        public long InsID { get; set; }
        public long FeedID { get; set; }
        public string OldFeedAccID { get; set; }
        public string NewFeedAccID { get; set; }
        public string BankAccName { get; set; }
        public string CorpID { get; set; }

    }
    public class FeedSpecificAccountReq
    {
        public string CorpID { get; set; }
    }
}
