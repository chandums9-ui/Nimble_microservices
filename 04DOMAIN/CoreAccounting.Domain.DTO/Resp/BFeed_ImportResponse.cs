using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
  
        public class AccountReconciliationBalanceResponse : StatusDTO
        {
            public DateTime? LastReconcileDate { get; set; }
            public decimal OpeningBalance { get; set; }
            public string BankFeedEmail { get; set; }
            public bool IsReconciliationDate { get; set;}
        }  
}
