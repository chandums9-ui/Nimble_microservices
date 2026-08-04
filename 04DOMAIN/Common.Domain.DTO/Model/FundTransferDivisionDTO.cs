using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class FundTransferDivisionDTO
    {
        public string DebitFromAccount { get; set; }
        public string ProfitcenterID { get; set; }
        public string Memo { get; set; }
        public string ToCorpID { get; set; }
        public string ToDebitAccount { get; set; }
        public string ToCreditAccount { get; set; }
        public decimal SplitAmount { get; set; }
        public string ParentID { get; set; }
        public bool IsBankTransactionRefID { get; set; } = false;

        public long BankTransactionRefID = 0;

    }
}
