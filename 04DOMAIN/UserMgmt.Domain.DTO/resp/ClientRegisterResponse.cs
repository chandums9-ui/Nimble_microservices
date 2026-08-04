using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgmt.Domain.DTO.Resp
{
    public class ClientRegisterResponse :StatusDTO
    {
            public string ClientID { get; set; }
            public string ClientSecret { get; set; }
        
    }
}
