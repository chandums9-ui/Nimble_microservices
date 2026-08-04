using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
   public class CorporationDetails
   {
        public string CorporationName {  get; set; }    
        public string CroporationId {  get; set; } 
        public string PeriodStartDate { get; set; }
        public string PeriodEndDate { get; set; }
        public string PropertyType {  get; set; }   
        public short IsDualBrand {  get; set; } 
        public short IsFisicalOrFinancial {  get; set; }    
    }
}
