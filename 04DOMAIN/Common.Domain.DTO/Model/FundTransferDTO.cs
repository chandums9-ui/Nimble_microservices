using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class FundTransferDTO
    {
        public DateTime TransactionDate { get; set; }
        public string? ClearedDate { get; set; }
        public string CorpID { get; set; }
        public string EntryNum { get; set; }
        public string RefNumber { get; set; }
        public short FundTransferType { get; set; }
        public bool InterCompanies { get; set; }
        public bool InterAccounts {  get; set; }    
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }
        public bool? IsBankTransactionRefID { get; set; }
        public string? BankTransactionRefID { get; set; }
        public List<FundTransferDivisionDTO> FundTransferDivisions { get; set; } = new List<FundTransferDivisionDTO>();
    }
}
