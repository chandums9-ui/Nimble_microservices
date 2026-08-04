using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public class UnMappedLinesRequest
    {
        public string UserID { get; set; }
        public string SaleID { get; set; }
        public byte[] CorpID { get; set; }
        public string FacilityID { get; set; }
        public short PMSType { get; set; }
        public string App { get; set; }
    }
}
