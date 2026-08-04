using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedRuleDTO : ModelBaseIDInt64, IModelBaseCorporation//IModelBaseCorporationID
    {
        public long ID { get; set; }
        [Required(ErrorMessage = "Please select Corporation ")]
        public string CorpID { get; set; } = string.Empty;
        public string CorpName { get; set; } = string.Empty;
        public long CorpSortOrder { get; set; }
        /// <summary>
        /// Feed Account ID
        /// </summary>
        [Required(ErrorMessage = "Please select Bank Account")]
        public string BankOrCreditAccountID { get; set; }//BankOrCreditAccountID

        /// <summary>
        /// Feed Account Name
        /// </summary>
        public string BankOrCreditAccountName { get; set; }
        public string BankOrCreditAccountAccTypeName { get; set; }

        [Required(ErrorMessage = "Please enter Setup For")]
        public string SetUpFor { get; set; }
        /// <summary>
        /// payments -0 or reciepts -1
        /// </summary>
        [DefaultValue(-1)]
        [Required(ErrorMessage = "Select valid AmountType.")]
        public int AmountType { get; set; } = 1;
        public string Memo { get; set; }
        [Required(ErrorMessage = "Please enter Priority(Allowed to enter between 1 to 200)")]
        [Range(1, 200)]//accepts a priority in between only
        public int Priority { get; set; }
        /// <summary>
        /// Feedrule auto-apply/manual 
        /// </summary>
        public bool AutoApplyEnable { get; set; }
        /// <summary>
        /// FeedRule will cloned to apply on all Bank/CreditCard accounts in the Corporation
        /// </summary>
        public bool CloneEnable { get; set; }
        /// <summary>
        /// select from QueryMatchTypeEnum
        /// </summary>
        [Required(ErrorMessage = "Select valid QueryMatchType.")]
        public int QueryMatchType { get; set; } = 1;
        public short RuleStatus { get; set; } //for rulecreator

    }

}
