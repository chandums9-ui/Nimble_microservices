using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class CustomWidgetDeleteFormulaRequest
    {
        public long ID { get; set; }

        public string CorpID { get; set; }

        public short FormulaStatus { get; set; }
    }
}
