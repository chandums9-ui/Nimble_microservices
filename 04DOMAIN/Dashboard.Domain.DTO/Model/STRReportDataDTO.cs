using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Model
{
    public class STRReportGroupData : StatusDTO
    {
        public string ViewBy { get; set; }
        public List<STRReportGroups> CombinedReportData { get; set; } = new List<STRReportGroups>();
    }
    public class STRReportDataDTO 
    {
        public string CorpName { get; set; }
        public object PcName { get; set; }
        public string ViewBy {  get; set; }
        public List<ReportData> Occ {  get; set; }
        public List<ReportData> OccChange { get; set; }
        public List<ReportData> Adr { get; set; }
        public List<ReportData> AdrChange { get; set; }
        public List<ReportData> Revpar { get; set; }
        public List<ReportData> RevparChange { get; set; }
    }

    public class ReportData
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public object MyProperty { get; set; } 
        public object CompSet { get; set; }
        public object Index { get; set; }
        public object MarketScale { get; set; }
        public object Rank { get; set; }
    }

    public class STRReportGroups
    {
        public string CorpName { get; set; }
        public object PcName { get; set; }
        public string ViewBy { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public object OccMyProperty { get; set; }
        public object OccCompSet { get; set; }
        public object OccIndex { get; set; }
        public object OccMarketScale { get; set; }
        public object OccRank { get; set; }
        
        public object OccChangeMyProperty { get; set; }
        public object OccChangeCompSet { get; set; }
        public object OccChangeIndex { get; set; }
        public object OccChangeMarketScale { get; set; }
        public object OccChangeRank { get; set; }

        public object AdrMyProperty { get; set; }
        public object AdrCompSet { get; set; }
        public object AdrIndex { get; set; }
        public object AdrMarketScale { get; set; }
        public object AdrRank { get; set; }

        public object AdrChangeMyProperty { get; set; }
        public object AdrChangeAdrCompSet { get; set; }
        public object AdrChangeAdrIndex { get; set; }
        public object AdrChangeAdrMarketScale { get; set; }
        public object AdrChangeAdrRank { get; set; }

        public object RevparMyProperty { get; set; }
        public object RevparCompSet { get; set; }
        public object RevparIndex { get; set; }
        public object RevParMarketScale { get; set; }
        public object RevparRank { get; set; }

        public object RevparChnageMyProperty { get; set; }
        public object RevparChangeCompSet { get; set; }
        public object RevparChangeIndex { get; set; }
        public object RevParChangeMarketScale { get; set; }
        public object RevparChangeRank { get; set; }
    }
  
}
