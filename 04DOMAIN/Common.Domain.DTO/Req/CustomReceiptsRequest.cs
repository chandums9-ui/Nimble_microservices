using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{
    public class CustomReceiptsRequest : CustomReceiptsDTO
    {

        public bool IsMultiplePost { get; set; } = false;
        public bool IsBankTransRefID { get; set; } = false;
        public long BankTransRefID { get; set; } = 0;
        public bool HasAttachments { get; set; } = false;
        public DateTime ClearedDate { get; set; }
        public string ClientID {  get; set; }
    }
}
