using Common.Domain.DTO.App;
using Common.Domain.DTO.Extensions;
using Common.Domain.DTO.Globalization;
using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{

    public class AccountSummaryDTO : ModelBaseCorporation
    {
        public long FeedAccID { get; set; }
        public long InsID { get; set; }
        public long ProvID { get; set; }

        public long ProvRegID { get; set; }

        public string FeedAccNumberMasked { get; set; }
        public string FeedAccName { get; set; }
        public decimal Balance { get; set; } = 0.0M;
        public string BalanceMasked { get { return string.Concat(NumberFormatInfo.CurrentInfo.CurrencySymbol," ", Constants.Masked_Balance); } }
        public string BalanceToDisplay
        {
            get
            {
                return GlobalizationInfo.GetCultureSpecificAmountForDisplay(this.Balance);
			}
        }
        public decimal AvailableBalance { get; set; } = 0.0M;
        public string AvailableBalanceMasked { get { return string.Concat(NumberFormatInfo.CurrentInfo.CurrencySymbol," ", Constants.Masked_Balance); } }
        public string AvailableBalanceDisplay
        {
            get
            {
                return GlobalizationInfo.GetCultureSpecificAmountForDisplay(this.AvailableBalance);
            }
        }
        public int OpenFeedsCount { get; set; } = 0;
        public string NimbleAccID { get; set; }
        public string NimbleAccName { get; set; }

        /// <summary>
        /// Need to get it from core api for UI
        /// </summary>
        public decimal NimbleAccBalance { get; set; } = 0.0M;
        /// <summary>
        /// Need to get it from core api for UI
        /// </summary>
        public string NimbleAccBalanceMasked { get { return string.Concat(NumberFormatInfo.CurrentInfo.CurrencySymbol," ", Constants.Masked_Balance); } }
        /// <summary>
        /// Need to get it from core api for UI
        /// </summary>
        public string NimbleAccBalanceDisplay
        {
            get
            {
                return GlobalizationInfo.GetCultureSpecificAmountForDisplay(this.NimbleAccBalance);
			}
        }
        public DateTime? LastSynchedOn { get; set; }
        public short AccType { get; set; } = 0;
        public short Status { get; set;}
        public DateTime? LastUpdatedOn { get; set; }
        public string ServiceProvider { get; set; }
        /// <summary>
        /// Connection id
        /// </summary>
        public string ProviderAccessID { get; set; }
        public DateTime? ProviderUpdatedOn { get; set; }
    }

}
