using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public class SaveOrUpdateAdjOpeningBalReq
    {
        public long? AdjustmentID { get; set; }
        public string CorpId { get; set; }
        public string PCID { get; set; }
        public string Amount { get; set; }
        public DateTime? AsOfDate { get; set; }
        public DateTime? MinStartDate { get; set; }
    }
}
