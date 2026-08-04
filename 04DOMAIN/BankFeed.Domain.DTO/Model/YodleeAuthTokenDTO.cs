using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class YodleeAuthTokenDTO
    {
        public string loginName { get; set; }
        public string clientID { get; set; }
        public string clientSecret { get; set; }
    }
}
