using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedTransactionMappingDTO
    {
        public long ID { get; set; }
        public long FeedTranID { get; set; }
        [Required]
        public string JournalEntryId { get; set; }
        [Required]
        public string TransactionId { get; set; }
        public short? TransactionType { get; set; }
        public string NameId { get; set; }
        public string PayeeName { get; set; }

        public short? NameType { get; set; }
        public string AccountId { get; set; }
        public string Pcid { get; set; }
        public decimal? Amount { get; set; }//get amount from transaction using transactionID for Load
        public DateTime? Date { get; set; }
        public string CheckNO { get; set; }

        public string Description { get; set; }

        public string TransactionTypeName { get; set; }
    }
}
