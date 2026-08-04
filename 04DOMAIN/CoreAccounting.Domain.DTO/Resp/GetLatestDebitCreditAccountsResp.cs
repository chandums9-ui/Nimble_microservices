using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class GetLatestDebitCreditAccountsResp:StatusDTO
    {
        public string AccountID { get; set; }
        public string AccountName { get; set; }
    }

    public class GetLatestDebitCreditAccountsDBResp
    {
        public string AccountID { get; set; }
        public string AccountName { get; set; }

    }
}
