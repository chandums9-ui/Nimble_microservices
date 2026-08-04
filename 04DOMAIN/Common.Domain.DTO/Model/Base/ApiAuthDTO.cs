using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base
{
    public class ApiAuthDTO
    {
        public string? ClientID { get; set; }
        public string? ClientSecret { get; set; }
    }

    public class ApiAuthDTOUrl : ApiAuthDTO
    {
    

        public string? UrlName { get; set; }
    }
}

