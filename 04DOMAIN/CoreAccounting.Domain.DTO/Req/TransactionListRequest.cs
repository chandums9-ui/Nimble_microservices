using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
   
    public class TransactionUpdateRequest
    {
        public List<TransactionDetailsRequest> TransList { get; set; }
        public bool IsMatchCleared { get; set; }
        [DefaultValue(false)]
        public bool IsPostedDeleted { get; set; }

        public bool IsMatchPost { get; set; }

        public List<string> TransactionID { get; set; } 

    }
    public class TransactionDetailsRequest
    {
        public string JournalEntryID { get; set; }
        public long BankTranID { get; set; }
        public DateTime ClearDate { get; set; }
        public string AccountID { get; set; }
        public string TransactionID { get; set; }   
        public AccountUpdateRequest AccountDetails { get; set; }
    }

    public class AccountUpdateRequest
    {
        public short TransactionType { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransactionRequest
    {
        public List<string> TransactionIDs { get; set; }
    }

    public class TransactionVerificationReq
    {
        public List<verificationTransactions> verificationTransactions { get; set; }
        public bool IsBillMatch { get; set; }
    }
    public class verificationTransactions
    {
        public string JournalEntryID { get; set; }
        public string TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string AccountID { get; set; }
    }
}
