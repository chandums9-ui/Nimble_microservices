using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public class DailySalePayloadDTO
    {

    }

    public class DailySaleMissingFiles
    {
        public DateTime SaleDate { get; set; }
        public string CorpID { get; set; }
        public string Pcid { get; set; }
        public short? PmsType { get; set; }
        public short IsAutomatic { get; set; }
    }
    public class DailySalePosPayload
    {
        public List<PosPayload> PosPayload { get; set; }
    }
    public class PosPayload
    {
        public string dailysaleid { get; set; }
        public string Date { get; set; }
        public string CorpID { get; set; }
        public string PCID { get; set; }
        public string imagefolderID { get; set; }
        public string userid { get; set; }
        public string POS { get; set; }
        public string FacilityID { get; set; }
        public string[] S3key { get; set; }
        public string PMS { get; set; }
        public short PmsType { get; set; }
        public string FileData { get; set; }
        public List<DailySaleLines> POSLines { get; set; }
    }
    public class DailySalePayload
    {
        public string dailysaleid { get; set; }
        public string Date { get; set; }
        public string CorpID { get; set; }
        public string PCID { get; set; }
        public string PMS { get; set; }
        public string POS { get; set; }
        public short PmsType { get; set; }
        public string FileData { get; set; }
        public string FacilityID { get; set; }
        public string[] S3key { get; set; }
        public string imagefolderID { get; set; }
        public string userid { get; set; }
        public List<DailySaleLines> DailySaleLines { get; set; }
    }
    public class AliasLabels
    {
        public string ClassLabel { get; set; }
        public string Department { get; set; }
        public string SubDepartment { get; set; }

    }
    public class DailySaleLines
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string LineItemDescription { get; set; }
        public decimal? Netamount { get; set; }
        public decimal? Balanceamount { get; set; }
        public long stat { get; set; }
        public decimal? ActualTodayDebits { get; set; }
        public decimal? AdjustedCredits { get; set; }
        public string ClassLabel { get; set; }
        public string Department { get; set; }
        public string ReportName { get; set; }
        public string PSCurrentStat { get; set; }
        public List<AliasLabels> AliasLabels { get; set; }

    }
}
