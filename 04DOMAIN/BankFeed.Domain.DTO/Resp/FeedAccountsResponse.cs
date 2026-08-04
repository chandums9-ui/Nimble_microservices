//using Azure;
using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.App;
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
    public class ConnectedFeedsResponse : StatusDTO
    {
        public ConnectedFeedsResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
            Feeds = new List<FeedAccountInfoDTO>();
        }
        public List<FeedAccountInfoDTO> Feeds { get; set; }
        public int OpenFeedsCount { get; set; } = 0;
        public PageDTO Page { get; set; } = new PageDTO();
        public int ReadyToLinkFeedsCount { get; set; } = 0;

        public int ActiveAccountsFeedsCount { get; set; } = 0;
        public int InProgressAndFailedAccountCount { get; set; } = 0;

        public int ArchiveAccountsFeedsCount { get; set; } = 0;

    }

    public class FeedAccountsStatusResponse : StatusDTO, IModelBaseIDInt64
    {
        public FeedAccountsStatusResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public long ID { get; set; }
    }

    public class FeedAccountsListResponse : StatusDTO
    {
        public FeedAccountsListResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<GenericLongListDTO> Accounts { get; set; }
    }

    public class InstitutionsListResponse : StatusDTO
    {
        public InstitutionsListResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
            Institutions = new List<KeyValuePairObject<long, string, object>>();
        }
        public List<KeyValuePairObject<long, string, object>> Institutions { get; set; }
    }

    public class AccountMappingListResponse : StatusDTO
    {
        public AccountMappingListResponse()
        {
            this.Status = Constants.MSG_NO_DATA_FOUND;
            this.StatusCode = StatusCodes.Status204NoContent;
        }
        public List<FeedAccountMappingDTO> Accounts { get; set; }

        public long ProvId { get; set; }
    }


    public class ActiveFeedAccountResponse : StatusDTO
    {
        public List<ActiveFeedAccountDTO> ActiveAccounts { get; set; }
    }

    public class MapMergeSettingsResponse : FeedAccountsStatusResponse
    {
        public MapMergeSettingsResponse()
        {
            this.StatusCode = StatusCodes.Status204NoContent;
            this.Status = Constants.MSG_NO_DATA_FOUND;
        }
    }

    public class FeedMappingAccountRes : StatusDTO
    {
        
        public List<string> MappingAccIds { get; set; }
    }
    public class FeedAccountCOAChangeResponse : StatusDTO
    {
        public long ID { get; set; }

        public DateTime? LastSyncFrom { get; set; }
    }
}
