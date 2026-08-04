using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class ReturnTransferVoidResponse:StatusDTO
    {
        public string JID { get; set; }

        public bool IsVoided { get; set; }

        public DateTime? VoidDate { get; set; }
    }
}
