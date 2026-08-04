using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class DeleteImportDataRequest
    {
        public string CorpID { get; set; }
        public String FilePath { get; set; }
        public long CorpMappingID { get; set; }
    }
}
