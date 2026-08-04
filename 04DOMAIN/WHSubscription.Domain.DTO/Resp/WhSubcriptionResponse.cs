using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Model.Base;

namespace WHSubscription.Domain.DTO.Resp
{
    public  class WhSubcriptionResponse: StatusDTO
    {
        public long SubscriptionID {  get; set; }
        
    }
}
