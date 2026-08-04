using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static DataModel.Domain.DataModel.Account;

namespace Common.Domain.DTO.Resp
{
    public class AccountListResponse : ModelBaseHeaderDTO
    {
        public List<AccountDTO> Accounts { get; set; }
    }

    public class AccountBalanceResponse : ModelBaseHeaderDTO
    {
      
        public List<AccountBalanceDTO> AccountList { get; set; }
    }

    public class AccountTypelistResponse
    {
        public List<ModelBaseIDNameSortOrderDTO> AccounTypes { get; set; }
    }

    public class SaveAccountResponse : StatusDTO
    {
        public string AccID { get; set; }
        public string CorporationID { get; set; }
        public string AccountTypeID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string ParentAccId { get; set; }
        public Decimal OpeningBalance { get; set; }

        public string JournalID { get; set; }

    }

    public class TransactionDetailRespone
    {
        public byte[] TransactionID { get; set; }
        public long FeedTransactionID { get; set; }   
    }


}
