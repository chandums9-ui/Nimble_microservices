using BankFeed.Domain.Enums;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedAccountInfoDTO : ModelBaseCorporation
    {
        public FeedAccountBaseDTO NimbleAccount { get; set; }
        public FeedAccountDTO BankAccount { get; set; }
        public long InsID { get; set; }
        public string InsName { get; set; }
        public long AccountsCount { get; set; }
        public long ProviderRegID { get; set; }
        public long ProviderID { get; set; }
        /// <summary>
        /// Provider InstitutionID / Meld InstututionID
        /// </summary>
        public string ProviderInstitutionID { get; set; }
        /// <summary>
        /// Provider access id/ Meld ConnectionID
        /// </summary>
        public string ProviderAccessID { get; set; }
        /// <summary>
        /// Meld Customer ID
        /// </summary>
        public string ProviderCustomerID { get; set; }
        /// <summary>
        /// Meld AccountID
        /// </summary>
        public string ProviderAccountID { get; set; }

        /// <summary>
        /// Service provider name incase of Meld
        /// </summary>
        public string ServiceProvider { get; set; }
        /// <summary>
        /// Service provider status
        /// </summary>
        public string ServiceProviderStatus { get; set; }
        public long ImportID { get; set; } = 0;
        public string AccessToken { get; set; }
        public string ProvAccountID { get; set; }
        public bool ShowReconnect
        {
            get
            {
                if (this.ProviderID == (long)NimbleProvidersEnum.Meld)
                {
                    if (BankAccount != null)
                    {
                        if (BankAccount.Status == Convert.ToInt16(BankAccountStatusEnum.ReconnectAvailable) || BankAccount.Status == Convert.ToInt16(BankAccountStatusEnum.OptForRelogin))
                            return true;
                        else if (BankAccount.Status == Convert.ToInt16(BankAccountStatusEnum.Pending) && !string.IsNullOrEmpty(this.ServiceProviderStatus) && (this.ServiceProviderStatus.ToUpper().Contains(MeldConnectionStatusEnum.RECONNECT_AVAILABLE.ToString()) || this.ServiceProviderStatus.ToUpper().Contains(MeldConnectionStatusEnum.RECONNECT_REQUIRED.ToString()) || this.ServiceProviderStatus.ToUpper().Contains(MeldConnectionStatusEnum.UNDETERMINED.ToString())))
                            return true;
                        else
                            return false;
                    }
                    else return false;
                }
                else
                    return BankAccount != null ? (BankAccount.Status == Convert.ToInt16(BankAccountStatusEnum.IssueWithAccountOptReloginFromAccountStatus) ||
                         BankAccount.Status == Convert.ToInt16(BankAccountStatusEnum.ReloginSuccessfulButNoFeeds) ||
                         BankAccount.Status == Convert.ToInt16(BankAccountStatusEnum.OptForRelogin)) ? true : false : false;


            }
        }
        public bool IsAuto
        {
            get
            {
                return (BankAccount != null && !string.IsNullOrEmpty(BankAccount.SyncType)) ? (BankAccount.SyncType == Constants.MSG_AUT ? true : false) : false;
            }
        }
        public bool IsMappedFeedAccount
        {
            get
            {
                return BankAccount != null ? (BankAccount.Status == Convert.ToInt32(BankAccountStatusEnum.Active) ||
                                            BankAccount.Status == Convert.ToInt32(BankAccountStatusEnum.ReloginSuccessfulButNoFeeds) ||
                                            BankAccount.Status == Convert.ToInt32(BankAccountStatusEnum.AccountArchived) ||
                                            BankAccount.Status == Convert.ToInt32(BankAccountStatusEnum.IssueWithAccountOptReloginFromAccountStatus) ||
                                            BankAccount.Status == Convert.ToInt32(BankAccountStatusEnum.OptForRelogin) ||
                                            BankAccount.Status == Convert.ToInt32(BankAccountStatusEnum.ReconnectAvailable) ||
                                            BankAccount.Status == Convert.ToInt32(BankAccountStatusEnum.DeletedByProvider) ||
                                            BankAccount.Status == Convert.ToInt32(BankAccountStatusEnum.UnRecoverable)) ? true : false : false;
            }
        }

        public string InSyncText { get; set; }
        public bool IsReconnect { get; set; } = false;
        public long CorpSortOrder { get; set; }
    }


}
