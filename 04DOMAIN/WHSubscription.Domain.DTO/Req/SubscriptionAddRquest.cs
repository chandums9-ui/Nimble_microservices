using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSubscription.Domain.DTO.Req
{
    public class SubscriptionAddRquest: WebHooksubscriptionDTO
    {
        public long wHsubscriptionID {  get; set; }    
        //public WebHooksubscriptionDTO webhookSubscription{get;set;}=new WebHooksubscriptionDTO();
    }
    public class WebHooksubscriptionDTO
    {

        public long Id { get; set; }

        public  string ReferenceId { get; set; }

        public string Name { get; set; }

        public string CallBackUrl { get; set; }

        public string? UrlName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public short? Status { get; set; }

        public Int32 EventType { get; set; }
    }


}
