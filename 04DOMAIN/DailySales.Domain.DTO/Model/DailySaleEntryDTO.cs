using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public class DailySaleEntryDTO
    {
        //For Revenue and cash/Receipt 
        public string LineID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DeptType { get; set; }
        public short? SignMapping { get; set; }
    }
}
