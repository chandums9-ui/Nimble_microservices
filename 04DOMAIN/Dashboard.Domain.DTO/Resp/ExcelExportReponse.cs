using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class ExcelExportReponse : StatusDTO
    {
        public byte[] ExcelFileBytes { get; set; }
        public string FileName { get; set; }
    }
}
