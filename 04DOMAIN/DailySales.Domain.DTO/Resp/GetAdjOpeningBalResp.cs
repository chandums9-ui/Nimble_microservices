using Common.Domain.DTO.Model.Base;
using System;

namespace DailySales.Domain.DTO.Resp
{
    public class GetAdjOpeningBalResp : StatusDTO
    {
        public long AdjustmentID { get; set; }
        public string CorpId { get; set; }
        public string PCID { get; set; }
        public string Amount { get; set; }
        public DateTime? AsOfDate { get; set; }
        public DateTime? MinStartDate { get; set; } 
    }
}
