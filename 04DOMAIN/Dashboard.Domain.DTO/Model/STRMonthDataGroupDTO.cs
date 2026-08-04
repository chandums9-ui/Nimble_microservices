using Common.Domain.DTO.Model.Base;
using Dashboard.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Model
{

    public class STRMonthDataGroupDTO : StatusDTO
    {
        public List<MonthDataGroupDTO> monthDataGroups { get; set; } = new List<MonthDataGroupDTO>();

        public bool IsMultiCorp { get; set; } = false;
    }
    public class MonthDataGroupDTO 
    {
       public string CorpName { get; set; } 
       public string CorpID { get; set; }
       public string PcID { get; set; }
       public string PcName { get; set; }
       public GroupMonthData Groups { get; set; } = new GroupMonthData();
    }

    public class GroupMonthData
    {
        public List<AdrMonthlyAvg> AdrMyProperty { get; set; } = new List<AdrMonthlyAvg>();
        public List<AdrMonthlyAvg> AdrCompetitiveSet { get; set; } = new List<AdrMonthlyAvg>();
        public List<AdrMonthlyAvg> AdrIndex { get; set; } = new List<AdrMonthlyAvg>();
        public List<AdrMonthlyAvg> AdrRank { get; set; } = new List<AdrMonthlyAvg>();

        public List<AdrMonthlyAvgsGlance> AdrMyPropertyRunning { get; set; } = new List<AdrMonthlyAvgsGlance>();
        public List<AdrMonthlyAvgsGlance> AdrCompetitiveSetRunning { get; set; } = new List<AdrMonthlyAvgsGlance>();
        public List<AdrMonthlyAvgsGlance> AdrIndexRunning { get; set; } = new List<AdrMonthlyAvgsGlance>();
        public List<AdrMonthlyAvgsGlance> AdrRankRunning { get; set; } = new List<AdrMonthlyAvgsGlance>();

        public List<OccupancyMonthlyAvg> OccMyProperty { get; set; } = new List<OccupancyMonthlyAvg>();
        public List<OccupancyMonthlyAvg> OccCompetitiveSet { get; set; } = new List<OccupancyMonthlyAvg>();
        public List<OccupancyMonthlyAvg> OccIndex { get; set; } = new List<OccupancyMonthlyAvg>();
        public List<OccupancyMonthlyAvg> OccRank { get; set; } = new List<OccupancyMonthlyAvg>();

        public List<OccupancyMonthlyAvgsGlance> OccMyPropertyRunning { get; set; } = new List<OccupancyMonthlyAvgsGlance>();
        public List<OccupancyMonthlyAvgsGlance> OccCompetitiveSetRunning { get; set; } = new List<OccupancyMonthlyAvgsGlance>();
        public List<OccupancyMonthlyAvgsGlance> OccIndexRunning { get; set; } = new List<OccupancyMonthlyAvgsGlance>();
        public List<OccupancyMonthlyAvgsGlance> OccRankRunning { get; set; } = new List<OccupancyMonthlyAvgsGlance>();

        public List<RevparMonthlyAvg> RevParMyProperty { get; set; } = new List<RevparMonthlyAvg>();
        public List<RevparMonthlyAvg> RevParCompetitiveSet { get; set; } = new List<RevparMonthlyAvg>();
        public List<RevparMonthlyAvg> RevParIndex { get; set; } = new List<RevparMonthlyAvg>();
        public List<RevparMonthlyAvg> RevParRank { get; set; } = new List<RevparMonthlyAvg>();

        public List<RevparMonthlyAvgsGlance> RevParMyPropertyRunning { get; set; } = new List<RevparMonthlyAvgsGlance>();
        public List<RevparMonthlyAvgsGlance> RevParCompetitiveSetRunning { get; set; } = new List<RevparMonthlyAvgsGlance>();
        public List<RevparMonthlyAvgsGlance> RevParIndexRunning { get; set; } = new List<RevparMonthlyAvgsGlance>();
        public List<RevparMonthlyAvgsGlance> RevParRankRunning { get; set; } = new List<RevparMonthlyAvgsGlance>();

    }
}
