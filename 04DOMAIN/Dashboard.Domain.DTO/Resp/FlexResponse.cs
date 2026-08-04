using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class FlexResponse : StatusDTO
    {
        public FlexAndFlowthrough FlexAndFlowthrough { get; set; }=new FlexAndFlowthrough();
        public FlexResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<FlexDbResponse> FlexDetails {  get; set; }

        //public FlexResult RoomSold { get; set; }
        //public FlexResult Revenue { get; set; }
        //public FlexResult Expense { get; set; }
        //public FlexResult GOP { get; set; }
        //public decimal FlowThrough { get; set; }
        //public decimal FlexValue { get; set; }
    }
    public class FlexResult
    {
        public StatisticDetails Amount { get; set; }
        public StatisticDetails POR { get; set; }
    }

    public class FlexDbResponse
    {
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public decimal? POR { get; set; }
        public decimal? LYAmount { get; set; }
        public decimal? LYPOR { get; set; }
        public decimal? BudAmount { get; set; }
        public decimal? BudPOR { get; set; }
        public decimal? ForecastAmount { get; set; }
        public decimal? ForecastPOR { get; set; }

        public decimal? FlowThrough { get; set; }
        public decimal? FlexThrough { get; set; }
    }
    //public class FlexDbResponse
    //{
    //    public string MainHead { get; set; }
    //    public decimal? PTDAmt { get; set; }
    //    public decimal? PTDLYAmt { get; set; }
    //    public decimal? YTDAmt { get; set; }
    //    public decimal? YTDLYAmt { get; set; }
    //    public decimal? PTB { get; set; }
    //    public decimal? YTB { get; set; }
    //    public decimal? PTDRevTotal { get; set; }
    //    public decimal? PTDLYRevTotal { get; set; }
    //    public decimal? YTDRevTotal { get; set; }
    //    public decimal? YTDLYRevTotal { get; set; }
    //    public decimal? PTBRevTotal { get; set; }
    //    public decimal? YTBRevTotal { get; set; }
    //    public decimal? PTF { get; set; }
    //    public decimal? YTF { get; set; }
    //    public decimal? ActPTDAmt { get; set; }
    //    public decimal? ActPTDLYAmt { get; set; }
    //    public decimal? ActYTDAmt { get; set; }
    //    public decimal? ActYTDLYAmt { get; set; }
    //    public decimal? YTDPOR { get; set; }
    //    public decimal? PTDPOR { get; set; }
    //    public decimal? YTDLYPOR { get; set; }
    //    public decimal? PTDLYPOR { get; set; }
    //    public decimal? PTBPOR { get; set; }
    //    public decimal? YTBPOR { get; set; }
    //}

    public class FlexRevenue
    {
        public decimal ThisYearGOP { get; set; }
        public decimal LastYearGop { get; set; }
        public decimal ThisYearRevenue { get; set; }
        public decimal LastYearRevenue { get; set; }
    }
    public class FlexAndFlowthrough
    {
        public string FlowthroughMessage { get; set; }
        public string FlexMessage { get; set; }
        public decimal FlowthroughValue { get; set; }
        public decimal FlexValue { get; set; }
    }

}
