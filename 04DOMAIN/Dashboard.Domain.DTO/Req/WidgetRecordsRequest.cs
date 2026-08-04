using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class WidgetRecordsRequest
    {
        public long WidgetID { get; set; }
        public string CorpID { get; set; }
    }
    public class WidgetFormulasRequest
    {
        public string UserID { get; set; }
        public string ClientID {  get; set; }   
        public string SourceName {  get; set; } 
    }
}
