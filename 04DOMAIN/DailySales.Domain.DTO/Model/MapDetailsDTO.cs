using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public  class MapDetailsDTO
    {
        public string AccountID { get; set; }
        public string AccountDescription { get; set; }
        public string PMSCurrencyType { get; set; }
        public byte[] LineId { get; set; }
        public string LineName { get; set; }
        public string SEQ { get; set; }
        public short DeptType { get; set; }
        public short pcmDeptType { get; set; }
        public short Type { get; set; }
        public string FacilityId { get; set; }
        public byte[] CorporationID { get; set; }
        public short Order { get; set; }
        public short OnlyStats { get; set; }
        public bool IsAllowNeg { get; set; }
        public string Posting { get; set; }
        public string Category { get; set; }
        public byte[] LaborStatisticsParentID { get; set; }
        public short LaborStatisticsLineType { get; set; }
        public short LaborStatisticsAvgLine { get; set; }
        //public short IsHouseKeepingDepartment { get; set; }
        public short RoomType { get; set; }
        public short IsEnding { get; set; }
        public short ExcludeFromActualDeposit { get; set; }
        public short? DebitCreditMapping { get; set; }
        public short? EnableADRORAVG { get; set; }
    }
}
