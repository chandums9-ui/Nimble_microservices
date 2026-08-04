using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class WidgetFormulaRecordsResponse : StatusDTO
    {
        public List<FormulaData> FormulaRecords { get; set; } = new List<FormulaData>();
    }

    public class FormulaData
    {
        public long FormulaID { get; set; }

        public string CorpID { get; set; }
    }
}
