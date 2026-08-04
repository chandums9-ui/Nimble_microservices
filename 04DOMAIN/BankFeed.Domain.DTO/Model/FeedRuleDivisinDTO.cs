using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedRuleDivisinDTO : ModelBaseIDInt64
    {
        /// <summary>
        /// select from RuleTypeEnum
        /// </summary>
        [Required]
        public int RuleOn { get; set; } = 1;//Description or Amount
        /// <summary>
        /// select from DescriptionFilterTypeEnum for Description ,AmountFilterTypeEnum for Amount 
        /// </summary>
        [Required] 
        public int Filter { get; set; } = 1;
        //[Required(ErrorMessage = "Amount/Discription is required.")]
        public string RuleDiscription { get; set; }
        //[Required(ErrorMessage = "Amount/Discription is required.")]
        public decimal RuleAmount { get; set; } = 0.00M;
    }
}
