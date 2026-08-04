using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base
{
    public class StatusDTO : IStatusDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }


}
