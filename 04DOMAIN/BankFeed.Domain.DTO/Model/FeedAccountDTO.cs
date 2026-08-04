using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Globalization;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedAccountBaseDTO
    {
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        /// <summary>
        /// BankAccountTypeEnum
        /// </summary>
        public short AccountType { get; set; }
        public string AccountTypeName { get; set; }
        public decimal Balance { get; set; }
        public string MaskedBalance { get; set; }
        public string BalanceToDisplay { get { return GlobalizationInfo.GetCultureSpecificAmountForDisplay(this.Balance); } }
    }

    public class FeedAccountDTO : FeedAccountBaseDTO
    {
        public string MaskedAccountNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(AccountNumber) && AccountNumber.Length > 4)
                    return new string('*', 4) + AccountNumber.Substring(AccountNumber.Length - 4);
                else if (!string.IsNullOrEmpty(AccountNumber))
                    return new string('*', 4) + AccountNumber;
                else return AccountNumber;
            }
        }
        public string AccountNumber { get; set; }
        public long PaymentsCount { get; set; }
        public long RecieptsCount { get; set; }
        public DateTime? LastSyncDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime? ProviderUpdatedOn { get; set; }
        public string SyncType { get; set; }
        /// <summary>
        /// AccountStatusFilterEnum
        /// </summary>
        public short? Status { get; set; }
    }

}
