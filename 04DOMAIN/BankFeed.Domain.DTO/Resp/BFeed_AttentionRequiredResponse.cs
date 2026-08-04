using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Resp
{
    public class BFeed_AttentionRequiredResponse : StatusDTO
    {
        public List<UnmappedAccountsDTO> UnmappedAccounts { get; set; }
    }
        public class UnmappedAccountsDTO
        {
            public long FeedAccountID { get; set; }
            public string CorporationId { get; set; }
            public string CorporationName { get; set; }
            public string FileName { get; set; }
            public string AccNumber { get; set; }
            public string AccountName { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public short? Status { get; set; }
        }

    public class BFeed_MapAccountResponse : StatusDTO
    {
        public string AccountIdsKey { get; set; }
        public List<MappedAccountsDTO> MappedAccounts { get; set; }
    }
    public class MappedAccountsDTO
    {
        public string AccNumber { get; set; }
        public string AccountID { get; set; }
        public long FeedAccountID { get; set; }     

    }
    public class BFeed_SaveResponse : StatusDTO
    {

    }
}
