using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class ClientUserWidgetPrivilegesRequest
    {
       public List<string> UserIDs {  get; set; }= new List<string>();
        public string UrlName { get; set; }
        public bool IsEnabledClients { get; set; } = false;

    }
}
