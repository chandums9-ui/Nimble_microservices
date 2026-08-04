using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class DeleteSTRFileResponse : StatusDTO
    {
        public int status_code { get; set; }
        public string message { get; set; }
    }
}
