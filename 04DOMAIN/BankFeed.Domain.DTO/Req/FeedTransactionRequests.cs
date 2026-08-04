using BankFeed.Domain.DTO.Model;
using BankFeed.Domain.Enums;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
    public class FeedTransactionLookupRequest : PageDTO
    {
        public long FeedAccountID { get; set; }
        //if want to load the transactions also while getting lookup then give True or else  False
        public bool IsGetTransactions { get; set; }
        public long InstitutionID { get; set; }
        public string CorpID { get; set; }
    }
    public class MultipleFeedTransactionLookupRequest
    {
        public List<FeedTransactionLookupRequest> Request { get; set; }
    }
    public class FeedTransactionSearchRequest : TransactionFilters
    {
        public FeedTransactionSearchRequest()
        {

        }
        public FeedTransactionSearchRequest(long feedAccountID, long instutionID)
        {
            this.FeedAccountID = feedAccountID;
            this.InstutionID = instutionID;
        }
        public long FeedAccountID { get; set; }
        /// <summary>
        /// Feed account Mapped nimble COA type which is mapped to BankAccountTypeEnum
        /// </summary>
        public short MapAccountType { get; set; }
        public long InstutionID { get; set; }
        public bool EnablePendingTransactions { get; set; } = false;
        public PageDTO Page { get; set; } = new PageDTO();

    }

    
    public class ApplyRuleRequest
    {
        public long InstutionID { get; set; }
        public long FeedAccountID { get; set; }
        public string NimbleAccID { get; set; }


    }
    public class MultipleApplyRuleRequest
    {
        public List<ApplyRuleRequest> ApplyRuleRequests { get; set; }
    }
    public class AutoRuleTransPostingRequest
    {
        public List<FeedAccountTransactionDTO> AutoRuleTrans { get; set; }
        public string CorpID { get; set; }
    }
    public class PostingFeedRuleRequest
    {
        public SinglePostRequest SinglePostReq { get; set; }
        public SinglePostResponse SinglePostResp { get; set; }
    }
    //public class PostingMultipleFeedRuleRequest
    //{
    //    public List<SinglePostRequest> SinglePostReq { get; set; }
    //    public List<SinglePostResponse> SinglePostResp { get; set; }
    //}
    
    public class FeedTransactionMatchRequest
    {
        [Required]
        public long? FeedTranID { get; set; }
        public short Status { get; set; }
        public List<FeedTransactionMappingDTO> MappedTrans { get; set; } = new List<FeedTransactionMappingDTO>();

    }

    public class GroupMatchRequest
    {
        public List<FeedTransactionMatchRequest> Matches { get; set; }
    }

    public class PossibleMatchRequest : LoadByClientID
    {
        /// <summary>
        /// send a list of feed transaction Ids seperated by commas 
        /// </summary>
        public List<long> FeedTranIDs { get; set; }
        public long FeedAccountID { get; set; }
        public string RequestID { get; set; }
    }
    public class MultiplePossibleMatchRequest
    {
        public List<PossibleMatchRequest> requests { get; set; }
    }
    public class TransactionActionRequest
    {
        /// <summary>
        /// select from BulkTranOperationEnum doesn' work for Action=1
        /// </summary>
        public short Action { get; set; }
        public List<long> FeedTransIDs { get; set; } = new List<long>();
        public CommonDataSelectRequest? CommonData { get; set; } = new CommonDataSelectRequest();
        public MultiplePostResponse? BulkPostData { get; set; } = new MultiplePostResponse();

    }

    public class MultiplePostingFeedRuleRequest
    {
        public List<PostingFeedRuleRequest> requests { get; set; } = new List<PostingFeedRuleRequest>();
    }
    public class CommonDataSelectRequest
    {
        public string AccountID { get; set; }
        public string AccountTypeID { get; set; }
        public string AccountName { get; set; }

        public string PayeeID { get; set; }
        public string PayeeName { get; set; }
        /// <summary>
        /// refer to BankAccountTypeEnum
        /// </summary>
        public short FeedAccTypeID { get; set; }
        public short TransType { get; set; }
        public string TransTypeName { get; set; }
        /// <summary>
        /// 0-Deposit(Reciept),1-Payment
        /// </summary>
        public short? TransPostTypeID { get; set; }

        public string PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public int PaymentMethodType { get; set; }
        public string PCID { get; set; }
        public short PayeeType { get; set; }
        public short FundTransferType { get; set; }
        public bool IsChooseCommonData { get; set; } = false;
    }

    public class TransactionRevertRequest
    {
        public List<long> TransactionIDs { get; set; }
        public short FailedStatus { get; set; }
    }

    public class CheckNumReq
    {
        public long FeedAccId { get; set; }
    }
}
