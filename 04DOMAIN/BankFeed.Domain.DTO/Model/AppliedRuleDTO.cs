using Common.Domain.DTO.Globalization;
using Common.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class AppliedRuleDTO
    {
        /// <summary>
        /// PostTypeEnum
        /// </summary>
        public short PostType { get; set; }
        public string PostTypeName { get; set; }
        public string NimbleAccountID { get; set; }
        /// <summary>
        /// Nimble Account Name using in UI
        /// </summary>
        public string NimbleAccountName { get; set; }
        public string NameID { get; set; }
        public string NameType { get; set; }
        /// <summary>
        /// Vendor Name using in UI
        /// </summary>
        public string Name { get; set; }
        public short FundTransferType { get; set; }

        public string Number { get; set; }
        public string Memo { get; set; }
        public DateTime? Date { get; set; }
        public string? ProfitCenterID { get; set; }
        public string? ProfitCenterName { get; set; }
        public string PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public int PaymentMethodType { get; set; }
        public decimal Amount { get; set; } = 0.0M;
        public string DisplayAmount { get { return GlobalizationInfo.GetCultureSpecificAmountForDisplay(this.Amount); } }

        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public decimal DifferenceAmt { get; set; }
        public List<AttachmentVMDTO> Attachments { get; set; }
        /// <summary>
        /// BankFeedActualtrTypeEnum (Payment 1 | receipt=2)
        /// </summary>
        public short ActualTransPostType { get; set; }
    }
}
