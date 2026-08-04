using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Domain.DTO.Model;

namespace Dashboard.Domain.DTO.Req
{
    public class WidgetPrivilegeCreateRequest : WidgetPrivilegeDTO
    {
        public List<string> DownlineUsers {  get; set; }
    }
}
