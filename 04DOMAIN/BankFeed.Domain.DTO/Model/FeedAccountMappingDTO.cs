using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedAccountMappingDTO : ModelBaseIDInt64, IModelBaseCorporationID
    {
        public string FeedAccName { get; set; }
        public string CorpID { get; set; }
        public string AccountMappingID { get; set; }
        public DateTime? LastSynchedOn { get; set; }
        public string AccEditName { get; set; }
        public short AccStatus { get; set; }
        public long LinkAccID { get; set; }
        /// <summary>
        /// Bank account type string
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// Bank account sub type string
        /// </summary>
        public string AccSubType { get; set; }
        /// <summary>
        /// BankAccountTypeEnum 
        /// Feed account Mapped nimble COA type which is mapped to BankAccountTypeEnum value
        /// </summary>
        public short AccType { get; set; }
        public string AccountMappingTypeID { get; set; }
        public bool HasHistoricalData { get; set; } = false;
    }
}
