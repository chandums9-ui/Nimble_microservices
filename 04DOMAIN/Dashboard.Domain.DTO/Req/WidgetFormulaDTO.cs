using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public  class WidgetFormulaDTO
    {
        public Int64 ID {  get; set; }
        [Required]
        public string WidgetName {  get; set; } 
        public string DisplayFormula {  get; set; } 
        public string ActualFormula {  get; set; }  
        public bool IsDefault {  get; set; }    
        public short Status { get;set; }
    }

}
