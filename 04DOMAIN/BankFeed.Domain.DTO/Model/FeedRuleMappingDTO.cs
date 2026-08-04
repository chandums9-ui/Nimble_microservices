using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedRuleMappingDTO
    {
        public int TransactionType { get; set; } = -1;
        public string AccountID { get; set; }
        /// <summary>
        /// Nimble Account Name using in UI
        /// </summary>
        public string AccountName { get; set; }
        public string NameID { get; set; }
        /// <summary>
        /// Vendor Name using in UI
        /// </summary>
        public string Name { get; set; }
        public int NameType { get; set; }
        public string PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
    }
}
