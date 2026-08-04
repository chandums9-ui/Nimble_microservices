using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
        public class SendEmailResponse : IStatusDTO
        {
            public string Status { get; set; }
            public int StatusCode { get; set; }
        }
}
