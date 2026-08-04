using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class GetImportDataResponse : StatusDTO 
    {
        public List<ImportData> ImportData { get; set; } = new List<ImportData>();
    }

    public class ImportData
    {
        public string CorpID { get; set; }
        public string STRID { get; set; }
        public string PCID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FileType { get; set; }
        public DateTime ImportedDate { get; set; }
        public int ImportedBy { get; set; }
        public string UserName { get; set; }
        public string FilePath { get; set; }
        public long CorpMappingID { get; set; }
    }
}
