using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class AccountsResponse:StatusDTO
    {
        public List<ChartOfAccounts> Accounts { get; set; } = new List<ChartOfAccounts>();
    }
    public class ChartOfAccounts
    {

        public string AccountID { get; set; }
        public string AccountTypeID { get; set; }
        public string AccountName { get; set; }
        public short Type { get; set; }
        public string AccountTypeName{get;set;}
        public decimal AccountBalance { get; set; }

     }
}
