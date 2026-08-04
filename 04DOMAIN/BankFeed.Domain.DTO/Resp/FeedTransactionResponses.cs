using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Req;
//using Azure;

namespace BankFeed.Domain.DTO.Resp
{
    public class FeedTransactionLookUpResponse : FeedTransactionLookUpDTO, IStatusDTO
    {
        public FeedTransactionLookUpResponse()
        {
            this.Status = Constants.MSG_NO_DATA_FOUND;
            this.StatusCode = StatusCodes.Status204NoContent;
        }

        //feedsettings related to the client
        public FeedSettingsDTO FeedSettings { get; set; } = new FeedSettingsDTO();
        //filters to load the TransactionList if needed
        public FeedTransactionSearchRequest SearchRequest { get; set; } = new FeedTransactionSearchRequest();
        public PageDTO Page { get; set; } = new PageDTO();
        public int StatusCode { get; set; }
        public string Status { get; set; }
        /// <summary>
        /// added here for linkaccount purpose only gets results when isgetransactions is true in the request
        /// </summary>
        public List<FeedAccountTransactionDTO> FeedTransactions { get; set; } = new List<FeedAccountTransactionDTO>();
    }
    public class MultipleFeedTransactionLookUpResponse : StatusDTO
    {
        public List<FeedTransactionLookUpResponse> responses { get; set; } = new List<FeedTransactionLookUpResponse>();
    }
    public class SaveSinglePostResponse : ModelBaseIDInt64, IStatusDTO
    {
        public SaveSinglePostResponse()
        {
            StatusCode = StatusCodes.Status204NoContent;
            Status = Constants.MSG_NO_DATA_FOUND;
        }
        public SinglePostResponse postResponse { get; set; }
        public FeedTransactionMappingResponse mappingResponse { get; set; }
        public FeedRuleResponse ruleResponse { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class GetAppliedRuleResponse : AppliedRuleDTO, IStatusDTO
    {
        public GetAppliedRuleResponse()
        {
            this.Status = Constants.MSG_NO_DATA_FOUND;
            this.StatusCode = StatusCodes.Status204NoContent;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class FeedTransactionRuleApplyResponse : IStatusDTO
    {
        public FeedTransactionRuleApplyResponse()
        {
            this.Status = Constants.MSG_RULE_NULL;
            this.StatusCode = StatusCodes.Status204NoContent;
        }
        public List<FeedAccountTransactionDTO> FeedTransaction { get; set; }
        public List<long> FeedTransactionIDs { get; set; }

        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class MultipleRuleApplyResponse : StatusDTO
    {
        public List<FeedTransactionRuleApplyResponse> responses { get; set; } = new List<FeedTransactionRuleApplyResponse>();
    }
    public class GetMoreAccountsResponse : StatusDTO
    {
        public GetMoreAccountsResponse()
        {
            this.Status = Constants.MSG_NO_DATA_FOUND;
            this.StatusCode = StatusCodes.Status204NoContent;
        }
        public List<AccountSummaryDTO> AccountSummary { get; set; }
    }
    public class FeedTransactionSearchResponse : StatusDTO
    {
        public FeedTransactionSearchResponse()
        {
            this.Transactions = new List<FeedAccountTransactionDTO>();

            base.StatusCode = StatusCodes.Status204NoContent;
            base.Status = Constants.MSG_NO_DATA_FOUND;

        }
        public List<FeedAccountTransactionDTO> Transactions { get; set; }
        #region For UI Grid Refresh

        /// <summary>
        /// StatusTypeFilterEnum
        /// </summary>
        public List<KeyValuePairObject<int, string>> StatusTypes { get; set; }

        /// <summary> 
        /// PostTypeFilterEnum
        /// </summary>
        public List<KeyValuePairObject<int, string>> PostTypes { get; set; }

        public int OpenFeedsCount { get; set; } = 0;
        public PageDTO Page { get; set; }=new PageDTO();
        #endregion

    }
    public class FeedTransactionMappingResponse : ModelBaseIDInt64, IStatusDTO
    {
        public FeedTransactionMappingResponse()
        {
            this.Status = Constants.MSG_NO_DATA_FOUND;
            this.StatusCode = StatusCodes.Status204NoContent;
        }
        //added for fileuploading purpose
        public string JournalID { get; set; } = string.Empty;
        public short FormulaStatus { get; set; }
        public long? FeedTransactionID { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class FeedTransactionResponse : IStatusDTO
    {
        public long ID { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
        
    public class FeedTransactionMultipleMapResponse : StatusDTO
    {
        public FeedTransactionMultipleMapResponse()
        {
            base.StatusCode = StatusCodes.Status204NoContent;
            base.Status = Constants.MSG_NO_DATA_FOUND;
        }
        public List<FeedTransactionMappingResponse> Map { get; set; }
        public List<long> MapID = new List<long>();
    }

    public class FeedTrnsactionMatchResponse :  StatusDTO
    {
        public FeedTrnsactionMatchResponse()
        {
            base.StatusCode = StatusCodes.Status204NoContent;
            base.Status = Constants.MSG_NO_DATA_FOUND;
        }
        // public List<string> MatchedNimbleJournalIDs { get; set; }

        public List<FeedTransactionMatchRes> FeedTransMatchRes { get; set; }
    }
    public class FeedTransactionMatchRes
    {
        public string MatchedNimbleJournalID { get; set; }
        public long FeedTranID { get; set; }
        //public string AccountID { get; set; }
        //public DateTime ClearDate { get; set; }
        public decimal Amount { get; set; }
        public decimal TranType { get; set; }
    }
    public class MultipleFeedTrnsactionMatchResponse : StatusDTO
    {
        public MultipleFeedTrnsactionMatchResponse()
        {
            base.StatusCode = StatusCodes.Status204NoContent;
            base.Status = Constants.MSG_NO_DATA_FOUND;
        }
        // public string MatchedNimbleJournalIDs { get; set; }
       public  FeedTrnsactionMatchResponse MultipleResponses { get; set; }
    }
    public class MappedDataResponse : StatusDTO
    {
        public MappedDataResponse()
        {
            base.StatusCode = StatusCodes.Status204NoContent;
            base.Status = Constants.MSG_NO_DATA_FOUND;
        }
        public List<FeedTransactionMappingDTO> MatchedTrans { get; set; }
    }

    public class FeedTranResponse : StatusDTO
    {
        public FeedTranResponse()
        {
            base.StatusCode = StatusCodes.Status204NoContent;
            base.Status = Constants.MSG_NO_DATA_FOUND;
        }
        public List<FeedTransactionandMappingDTO> TransList { get; set; }
        public List<string> JIDs { get; set; }=new List<string>();

    }
    public class PossibleMatchResponse : StatusDTO
    {
        public PossibleMatchResponse()
        {
            base.Status = Constants.MSG_NO_DATA_FOUND;
            base.StatusCode = StatusCodes.Status204NoContent;
        }
        public List<PossibleMatchDTO> MatchedData { get; set; }
        public long TransCount { get; set; }
    }

    public class GroupTransactionsFilterRsponse : StatusDTO
    {
        public GroupTransactionsFilterRsponse()
        {
            base.Status = Constants.MSG_NO_DATA_FOUND;
            base.StatusCode = StatusCodes.Status204NoContent;
        }
        public List<long> TransIDs { get; set; }
        public List<string> NimbleTransactionIDs { get; set; }
        public List<string> JournalIDs { get; set; }
    }

    public class CheckNumResponse:StatusDTO
    {
        public string CheckNo { get; set; }
    }
}
