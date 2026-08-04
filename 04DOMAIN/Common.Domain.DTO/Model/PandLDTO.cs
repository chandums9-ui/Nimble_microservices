using Common.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class PandLDTO
    {
        public string CorpName { get; set; }
        public DateTime? GeneratedTime { get; set; }
        public List<AccountType> AccountTypes { get; set; }
    }


    public class AccountPandL
    {
        public string ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string ParentAccountName { get; set; }
        public string ParentAccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
    public class AccountType
    {
        public string Name { get; set; }
        public long SortOrder { get; set; }
        public List<AccountPandL> Accounts { get; set; }
        public decimal? Balance { get; set; }
    }
}
