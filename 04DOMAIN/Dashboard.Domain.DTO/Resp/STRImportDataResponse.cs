using Common.Domain.DTO.Model.Base;
using Dashboard.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class STRImportDataResponse :StatusDTO
    {
        public string CorpID { get; set; }
        public string STRID { get; set; }

        public long CorpMappingID { get; set; }
    }
}
