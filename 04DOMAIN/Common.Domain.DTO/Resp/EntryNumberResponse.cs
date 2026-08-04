using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class EntryNumberResponse : StatusDTO
    {
        public string EntryNumber { get; set; }
    }
}
