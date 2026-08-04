using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public  class ImportDetailsDTO
    {
        public string SubCategory { get; set; }
        public string Report { get; set; }
        public string PSCurrStat { get; set; }
        public byte[] LineId { get; set; }
        public short DeptType { get; set; }
        public short Type { get; set; }
        public byte[] CorporationID { get; set; }
        public short Order { get; set; }
        public decimal Amount { get; set; }
        public DateTime BusDate { get; set; }
        public short OnlyStats { get; set; }
        public string SEQ { get; set; }
        public string CGCD { get; set; }
        public string CGS { get; set; }
        public bool IsAllowNeg { get; set; }
        public string TransactionType { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }
        public short RoomType { get; set; }
        public short AdjustLedgerType { get; set; }
        public short IsEnding { get; set; }
        public short pcmDeptType { get; set; }
        public short ExcludeFromActualDeposit { get; set; }
        public byte[] LaborStatisticsParentID { get; set; }
        public short LaborStatisticsLineType { get; set; }
        public short LaborStatisticsAvgLine { get; set; }
        public short? EnableADRORAVG { get; set; }
    }
}
