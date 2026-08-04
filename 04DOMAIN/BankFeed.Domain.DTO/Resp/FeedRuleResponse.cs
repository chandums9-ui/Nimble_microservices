using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata;
using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;

namespace BankFeed.Domain.DTO.Resp
{
    public class FeedRuleResponse : ModelBaseIDInt64, IStatusDTO
    {
        public FeedRuleResponse()
        {
            this.StatusCode = StatusCodes.Status204NoContent;
            this.Status = Constants.MSG_NO_DATA_FOUND;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public long FeedRuleID { get; set; }

    }
    public class FeedTransactionMultipleRuleResponse : StatusDTO
    {
        public List<FeedRuleResponse> FeedRule { get; set; }
        public List<long> FeedRuleIds { get; set; }
    }
    public class FeedRuleListResponse : StatusDTO
    {
        public List<FeedRuleViewDTO> Rules { get; set; }
        public PageDTO Page { get; set; } = new PageDTO();
        public List<FeedRuleViewDTO> TotalRules { get; set; }

    }

    public class FeedRuleLoadResponse : FeedRuleDetailsDTO, IStatusDTO
    {
        public FeedRuleLoadResponse()
        {
            this.StatusCode = StatusCodes.Status204NoContent;
            this.Status = Constants.MSG_NO_DATA_FOUND;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        /// <summary>
        /// it returns the count of feed transaction on which the current rule is applied
        /// </summary>
        public int FeedTransactionsCount { get; set; }
    }

    public class FeedAccountInfoResponse : StatusDTO
    {
        public FeedAccountInfoResponse()
        {
            this.StatusCode = StatusCodes.Status204NoContent;
            this.Status = Constants.MSG_NO_DATA_FOUND;
        }
    }

    public class RuleBankOrMappingAccountRes:StatusDTO
    {
        public List<string> BankOrCreditAccountIds { get; set; }
        public List<string> MappingAccIds { get; set; }
    }
    public class MappingAccountID
    {
        public string ID { get; set; }
    }

}
