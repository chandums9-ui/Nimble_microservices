using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class DeleteImportDataResponse : StatusDTO
    {
        public string CorpID { get;set;}
        public string FilePath { get;set;}
        public long CorpMappingID { get;set;}
    }
}
