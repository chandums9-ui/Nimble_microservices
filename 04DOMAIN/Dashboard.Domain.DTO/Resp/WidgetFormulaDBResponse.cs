using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class WidgetFormulaDBResponse
    {
        public long FormulaSettingsID { get; set; }
        public long FormulaID { get; set; }
        public string SourceName { get; set; }
        public short SortOrder { get; set; }
        public long WidgetID { get; set; }
        public long FormulaDetailsID { get; set; }
        public long CustomKeyID { get; set; }
        public long DetailsFormulaID { get; set; }
        public string CustomBinID { get; set; }
        public int DeptId {  get; set; }    
        public string CustomKeyName { get; set; }
        public int DetailsType { get; set; }
        public int SubType { get; set; }
        public long WidgetFormulaID { get; set; }
        public string WidgetFormulaName { get; set; }
        public string DisplayFormula { get; set; }
        public string ActualFormula { get; set; }
    }

    public class FormulaDto
    {
        public long FormulaSettingsID { get; set; }
        public long FormulaID { get; set; }
        public string SourceName { get; set; }
        public short SortOrder { get; set; }
        public long WidgetID { get; set; }
        public List<FormulaDetailDto> Details { get; set; }
        public long WidgetFormulaID { get; set; }
        public string WidgetFormulaName { get; set; }
        public string DisplayFormula { get; set; }
        public string ActualFormula { get; set; }
       
    }

    public class FormulaDetailDto
    {


        public long FormulaDetailsID { get; set; }
        public long CustomKeyID { get; set; }
        public long DetailsFormulaID { get; set; }
        public string CustomBinID { get; set; }
        public string CustomKeyName { get; set; }
        public int Type { get; set; }
        public int SubType { get; set; }
        public Int32 DeptId { get; set; }
    }

}
