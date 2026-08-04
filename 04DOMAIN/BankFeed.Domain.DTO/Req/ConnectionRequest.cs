using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
    public  class ConnectionRequest: IModelBaseIDInt64
    {
        public string public_token {  get; set; }
        public string institution_id { get; set; }
        public string institution_name { get; set; }
        public long ProviderRegID { get;set; }
        //FeedAccID
        public long ID { get; set; }
        public long NewFeedAccID { get; set; }//newfeedAccId for convert to import case

        public long ProviderID { get; set; }

        public string ConnectType { get; set; }
        public bool CanMoveToActive { get; set; } = false;

    }
    public class YodleeConnectionReqDTO: IModelBaseIDInt64
    {
        public string providerAccountID { get; set; }
        //FeedAccID
        public long ID { get; set; }
        public long NewFeedAccID { get; set; }//newfeedAccId for convert to import case

        public long ProviderID { get;set; }
        public long ProviderRegID { get; set; }
        public long InsId { get; set; }
        public string ConnectType { get; set; }
    }
    public class ConnectionCheckingReqDTO 
    {
        public long ProviderID { get; set; }
        public long ProviderRegID { get; set; }
        public string InsName { get; set; }

        public string InsProviderID { get; set; }
        public string ClientID { get; set;}

    }

}
