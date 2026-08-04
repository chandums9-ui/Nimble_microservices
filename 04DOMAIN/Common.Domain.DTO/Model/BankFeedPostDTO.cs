using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model.Base;

namespace Common.Domain.DTO.Model
{
    public class BankFeedPostRequestDTO
    {
        public string CorporationID { get; set; }
        public long FeedTransactionID { get; set; }
        public string NimbleAccountID { get; set; }
        public short TransactionPostType { get; set; }
        public string AccountID { get; set; }
        public string AccountTypeID { get; set; }
        public string ? NameID { get; set; }
        public string NameType { get; set; }=string.Empty;
        public string ProfitCenter { get; set; }
        public string CheckNo { get; set; }
        public DateTime EntryDate { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethodID { get; set; }
        public bool IsCreateFeedRule { get; set; } = false;
        public bool? IsMultiplePost { get; set; } = false;
        public bool HasAttachments { get; set; } = false;
        public DateTime ClearedDate { get; set; }
        public short TransactionType {  get; set; } 
        public string ClientID { get; set; }
        public long BillInfoID { get; set; }
        public bool? IsPossiblematch { get; set; }
    }
    public class BankFeedPostResponseDTO : StatusDTO
    {
        public string JournalID { get; set; }
        public string TransactionID { get; set; }
        public long FeedTransactionID { get; set; }
        //this is the oppposite account to feedAccountID in posting
        public string NimbleAccountID { get; set; }
        public string ProfitCenterID { get; set; } = string.Empty;
        public short TransactionType { get; set; }
        public string NameID { get; set; }
        public string NameType { get; set; }=string.Empty;

        public DateTime EntryDate { get; set; }
        public string CheckNO { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
        public bool IsCreateFeedRule { get; set; } = false;
    }

    public class SplitPostResponseDTO : StatusDTO
    {
        public string JournalID { get; set; }
        public string TransactionID { get; set; }
        public long FeedTransactionID { get; set; }
        public string NimbleAccountID { get; set; }
        public string ProfitCenterID { get; set; } = string.Empty;
        public short TransactionType { get; set; }
        public string NameID { get; set; }
        public string NameType { get; set; } = string.Empty;

        public DateTime EntryDate { get; set; }
        public string CheckNO { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
    public class PaymentMenthodListDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public short SourceType { get; set; }
        public short PrintType { get; set; }
    }

    public class MergeSettingsDTO
    {
        public string LineID { get; set; }
        public string LineName { get; set; }
        /// <summary>
        /// DailyConfigDepartmentTypeEnum
        /// </summary>
        public int DepartmentTypeID { get; set; }
        public string DepartmentName
        {
            get
            {
                return EnumExtensions.GetEnumValue<DailyConfigDepartmentTypeEnum>(this.DepartmentTypeID).GetDisplayName();
            }
        }
        /// <summary>
        /// these are static values start from 0 to 8
        /// </summary>
        public short MergeType { get; set; } = 0;
        public long DSOrder { get; set; } = 0;
    }
    public class BankFeedTransInfo
    {
        public string TransID { get; set; }
        public string FeedDate { get; set; }
    }
}

