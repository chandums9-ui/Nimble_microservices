using BankFeed.Domain.Enums;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Globalization;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedAccountTransactionDTO : ModelBaseIDInt64, IModelBaseCorporationID
    {
        public long FeedAccID { get; set; }
        /// <summary>
        /// refer to BankAccountTypeEnum
        /// </summary>
        public short FeedAccTypeID { get; set; }
        public string TransProviderID { get; set; }
        public string? TransType { get; set; }
        public string Description { get; set; }
        public string CheckNo { get; set; }
        public short PayeeType { get; set; }
        public string PayeeID { get; set; }
        /// <summary>
        /// TransName
        /// </summary>
        public string PayeeName { get; set; }
        public string NimAccType { get; set; }
        /// <summary>
        /// Amount split into two, payments/Reciepts
        /// </summary>
        public decimal Payments
        {
            get
            {
                return Convert.ToDecimal(String.Format("{0:F2}", (this.TransPostTypeID == Convert.ToInt16(BankFeedActualtrTypeEnum.Payments) ? this.Amount : 0.0M)));
            }
        }
        public string AmountForDisplay
        {
            get
            {
				return GlobalizationInfo.GetCultureSpecificAmountForDisplay((this.TransPostTypeID == Convert.ToInt16(BankFeedActualtrTypeEnum.Payments) ? this.Payments : this.Receipts));
            }
        }

        /// <summary>
        /// Amount split into two, payments/Reciepts
        /// </summary>
        public decimal Receipts
        {
            get
            {
                return Convert.ToDecimal(String.Format("{0:F2}", (this.TransPostTypeID == Convert.ToInt16(BankFeedActualtrTypeEnum.Receipts) ? this.Amount : 0.0M)));
            }
        }
        public DateTime? Date { get; set; }
        public long FeedRuleId { get; set; }
        //if the transaction is rule applied it returns all the rules defined in that particular applied feedrule id as a string
        //used in ui mere the message symbol is there it shows the text when hovered on it
        public string RuleQuery { get; set; } = string.Empty;
        public bool IsAutoRuleApplied { get; set; } = false;
        /// <summary>
        /// 0-Deposit(Reciept),1-Payment
        /// </summary>
        public short? TransPostTypeID { get; set; }
        public string TransPostType
        {
            get
            {
                return (this.TransPostTypeID.HasValue) ? EnumExtensions.GetEnumValue<BankFeedActualtrTypeEnum>(Convert.ToInt32(this.TransPostTypeID.Value)).GetDisplayName() : string.Empty;
            }
        }
        public decimal RunningBalance { get; set; }
        public string RunningBalanceForDisplay
        {
            get
            {
				return GlobalizationInfo.GetCultureSpecificAmountForDisplay(this.RunningBalance);
			}
		}
        public int? PageNo { get; set; }
        /// <summary>
        /// 0-Pending,1-New Feed,2-Posted,3-Matched,4-Possible Match Identified,5-Ignored,6-Bill Match
        /// </summary>
        public short? Status { get; set; }
        public string RecurringStatus { get; set; }
        //public string StatusMsg
        //{
        //    get
        //    {
        //        return (this.Status.HasValue) ? EnumExtensions.GetEnumValue<FeedTransStatusEnum>(Convert.ToInt32(this.Status.Value)).GetDisplayName() : string.Empty;
        //    }
        //}
        public string MatchMsg
        {
            get
            {
                if (!this.Status.HasValue)
                    return string.Empty;
                else if (this.Status == Convert.ToInt16(FeedTransStatusEnum.Pending))
                    return "Pending";// FeedTransMatchStatusEnum.Pending.GetDisplayName();
                else if ((this.Status == Convert.ToInt16(FeedTransStatusEnum.NewFeed) || this.Status == Convert.ToInt16(FeedTransStatusEnum.RuleModified)) && this.FeedRuleId > 0)
                    return "Rule Applied";// FeedTransMatchStatusEnum.Unassinged.GetDisplayName();
                else if (this.Status == Convert.ToInt16(FeedTransStatusEnum.NewFeed) && this.FeedRuleId <= 0)
                    return "Unassinged";// FeedTransMatchStatusEnum.Unassinged.GetDisplayName();
                else if (this.Status == Convert.ToInt16(FeedTransStatusEnum.PostedByPossibleMatch))
                    return "By Possible Match";
                else if (this.Status == Convert.ToInt16(FeedTransStatusEnum.PostedByBillMatch))
                    return "By Bill Match";
                else if (IsPosted/*(new List<int> { Convert.ToInt16(FeedTransStatusEnum.Posted), Convert.ToInt16(FeedTransStatusEnum.Matched), Convert.ToInt16(FeedTransStatusEnum.AutoMatched) }).Contains(this.Status.Value)*/)
                {
                    if ((this.Status == Convert.ToInt16(FeedTransStatusEnum.Posted)   && this.FeedRuleId <= 0))//|| (this.Status == Convert.ToInt16(FeedTransStatusEnum.Matched))
                        return "By Manual";
                    else if ((this.Status == Convert.ToInt16(FeedTransStatusEnum.Posted) || this.Status == Convert.ToInt16(FeedTransStatusEnum.PostedButRuleModified)) && this.FeedRuleId > 0)
                        return "By Rule";
                    else if (this.Status == Convert.ToInt16(FeedTransStatusEnum.Matched))
                        return "By Manual";
                    else if (this.Status == Convert.ToInt16(FeedTransStatusEnum.AutoMatched) && this.FeedRuleId > 0)
                        return "By Auto Rule";
                    else
                        return "Match Found";// FeedTransMatchStatusEnum.MatchFound.GetDisplayName();
                }
                else if (this.Status == Convert.ToInt16(FeedTransStatusEnum.PossibleMatch) || this.Status == Convert.ToInt16(FeedTransStatusEnum.DSCashPossibleMatch)
                    || this.Status == Convert.ToInt16(FeedTransStatusEnum.MergePossibleMatch))
                {
                    if (this.TransType == BankFeedActualtrTypeEnum.Payments.GetDisplayName())
                        return "Payment Found";
                    else if (this.TransType == BankFeedActualtrTypeEnum.Receipts.GetDisplayName())
                        return "Receipt Found";
                    else
                        return "Possible Match";
                }
                else if (this.Status == Convert.ToInt16(FeedTransStatusEnum.Ignored))
                    return "Ignored";
                else if (this.Status == Convert.ToInt16(FeedTransStatusEnum.BillMatch))
                    return "Bill Match Found";
                
                else
                    return string.Empty;// eturn (this.Status.HasValue) ? EnumExtensions.GetEnumValue<FeedTransMatchStatusEnum>(Convert.ToInt32(this.Status.Value)).GetDisplayName() : string.Empty;

            }
        }
        public string? LogSequenceID { get; set; }
        public decimal Amount { get; set; }
        public string CorpID { get; set; }
        /// <summary>
        /// Applied Nimble account ID
        /// </summary>
        public string NimbleAccID { get; set; }
        /// <summary>
        /// Mapped Parent Nimble account of Transaction's Feed Account
        /// </summary>
        public string ParentNimbleAccID { get; set; }
        public string ParentNimbleAccName { get; set; }
        public string ParentNimbleAccTypeID { get; set; }
        public string NimbleAccName { get; set; }
        public string? JournalEntryID { get; set; }
        public int TransMapTypeID { get; set; } = 0;
        public string TransMapTypeName
        {
            get
            {
                return EnumExtensions.GetEnumValue<TransMapTypeEnum>(this.TransMapTypeID).GetDisplayName();
            }
        }
        public bool IsSelected { get; set; } = false;
        public bool IsPosted
        {
            get
            {
                return (Status == Convert.ToInt16(FeedTransStatusEnum.Posted) || Status == Convert.ToInt16(FeedTransStatusEnum.AutoMatched) || Status == Convert.ToInt16(FeedTransStatusEnum.Matched)
                    || Status == Convert.ToInt16(FeedTransStatusEnum.PostedByPossibleMatch) || Status == Convert.ToInt16(FeedTransStatusEnum.PostedByBillMatch) || Status == Convert.ToInt16(FeedTransStatusEnum.PostedButRuleModified)) ?
                    true : false;
            }
        }
        public bool IsChooseCommonData { get; set; } = false;
        public FeedTransactionMappingDTO MatchesData { get; set; } = new FeedTransactionMappingDTO();
        public string PaymentMethodID { get; set; } = string.Empty;
        public string PaymentMethodName { get; set; } = string.Empty;
        public int PaymentMethodType { get; set; }
        public DateTime? TranClearedDate { get; set; }

        public string? ProfitCenterID { get; set; }
        public string? ProfitCenterName { get; set; }
    }

    public class PossibleMatchDTO
    {
        public long ID { get; set; }
        public long? FeedTransId { get; set; }
        public string JournalEntryId { get; set; }
        public string TransactionId { get; set; }
        public DateTime TransDate { get; set; }
        public string CheckNo { get; set; }
        public string PayeeName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public short TransactionType { get; set; }
        public string TransactionTypeName { get; set; }
        public bool IsSelected { get; set; }

        /// <summary>
        /// 1-Matched,2- most possible match
        /// </summary>
        public short? Status { get; set; }
    }

    public class FeedTransactionandMappingDTO : ModelBaseIDInt64
    {
        public string CorpID { get; set; }
        public long FeedTransactionID { get; set; }

        public string NimbleAccountID { get; set; }
        public short TransactionPostType { get; set; }
        public string AccountID { get; set; }
        public string NameID { get; set; }
        public string NameType { get; set; }
        public string CheckNo { get; set; }
        public string Memo { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal Amount { get; set; }
        public short TransactionType { get; set; }
    }

    }
