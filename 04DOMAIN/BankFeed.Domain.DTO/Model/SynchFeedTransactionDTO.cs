using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    //Synch FeedTransactions 
    public class SynchFeedTransactionDTO : FeedAccountTransactionDTO
    {
        public string pending_transaction_id { get; set; }
        public string account_id { get; set; }
        public string financial_account_id { get; set; }
        public string transaction_id { get; set; }
        public string name { get; set; }
    }
}
