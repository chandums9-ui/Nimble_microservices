using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class WidgetSettingsFormulaDTO
    {
        public Int64 Id { get; set; }
        public Int64 FormulaID {  get; set; }   
        public short SourceType {  get; set; }  
        public string SourceName {  get; set; }
        //public Int64 CorpKey {  get; set; } 
        public string CorporationId { get; set; }
        public string UserId {  get; set; } 
        public short SortOrder {  get; set; }
        public Int64? widgetId { get; set; }
        public short GroupFor {  get; set; }
        public string ClientID { get; set; }
    }
    public class WidgetFormulaDetailsDTO
    {
        public Int64 Id { get; set; }
        public Int64 CustomKeyID { get; set; }
        public Int64 FormulaID { get; set; }    
        public string CustomBinID {  get; set; }    
        public string CustomKeyName {  get; set; }  
        public Int32 Type {  get; set; }    
        public Int32 SubType { get; set; }  
        public string CorpID { get; set; }
        public Int32 IncomeDeptID {  get; set; }    
    }
}
