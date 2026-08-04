using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class AccountDTO
    {
        public string ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountTypeID { get; set; }
        public int AccountTypeOrder { get; set; }
        public string ParentAccountName { get; set; }
        public string ParentAccountNumber { get; set; }
        /// <summary>
        /// Status =17 & AccountTypeID= 0x0FA700000000000000000000000000000024 => default account for UI
        /// </summary>
        public short AccountStatus { get; set; }
        public int IsAllow { get; set; }
        public bool IsAllowAccount { get; set; }
    }

    public class AccountBalanceDTO : AccountDTO, IBalanceDTO
    {
        public decimal Balance { get; set; }

    }

    public class CorporationAccountBalanceDTO : AccountBalanceDTO, IModelBaseHeaderDTO
    {
        public string CorpName { get; set; }
        public new DateTime? GeneratedTime { get; set; }
        public int TotalCount { get; set; }
    }

}
