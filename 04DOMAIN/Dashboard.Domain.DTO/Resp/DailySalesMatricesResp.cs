using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dashboard.Domain.DTO.Model;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dashboard.Domain.DTO.Resp
{
    public class DailySalesMatricesDbResp
    {
        public string CorporationId { get; set; }
        public string CorpDBAName { get; set; }
        public string CorpLegalName { get; set; }
        //Today
        public decimal TdRoomRevenue { get; set; }
        public decimal TdRoomsAvailable { get; set; }
        public decimal Tdoutoforderrooms { get; set; }
        public decimal TdOccupancy { get; set; }
        public decimal TdRoomsSold { get; set; }
        public decimal TdADR { get; set; }
        public decimal TdRevPar { get; set; }
        public decimal TdTotalRevenue { get; set; }
        public decimal TdOtherRevenue { get; set; }

        public decimal TdLYRoomRevenue { get; set; }
        public decimal TdLYRoomsAvailable { get; set; }
        public decimal TdLYoutoforderrooms { get; set; }
        public decimal TdLYOccupancy { get; set; }
        public decimal TdLYRoomsSold { get; set; }
        public decimal TdLYADR { get; set; }
        public decimal TdLYRevPar { get; set; }
        public decimal TdLYTotalRevenue { get; set; }
        public decimal TdLYOtherRevenue { get; set; }

        public decimal TdBudRoomRevenue { get; set; }
        public decimal TdbudRoomsAvailable { get; set; }
        public decimal Tdbudoutoforderrooms { get; set; }
        public decimal TdBudOccupancy { get; set; }
        public decimal TdBudRoomsSold { get; set; }
        public decimal TdBudADR { get; set; }
        public decimal TdBudRevPar { get; set; }
        public decimal TdBudTotalRevenue { get; set; }
        public decimal TdBudOtherRevenue { get; set; }

        //month
        public decimal MtdRoomRevenue { get; set; }
        public decimal MtdRoomsAvailable { get; set; }
        public decimal Mtdoutoforderrooms { get; set; }
        public decimal MtdOccupancy { get; set; }
        public decimal MtdRoomsSold { get; set; }
        public decimal MtdADR { get; set; }
        public decimal MtdRevPar { get; set; }
        public decimal MtdTotalRevenue { get; set; }
        public decimal MtdOtherRevenue { get; set; }

        public decimal MtdLYRoomRevenue { get; set; }
        public decimal MtdLYRoomsAvailable { get; set; }
        public decimal MtdLYoutoforderrooms { get; set; }
        public decimal MtdLYOccupancy { get; set; }
        public decimal MtdLYRoomsSold { get; set; }
        public decimal MtdLYADR { get; set; }
        public decimal MtdLYRevPar { get; set; }
        public decimal MtdLYTotalRevenue { get; set; }
        public decimal MtdLYOtherRevenue { get; set; }

        public decimal MtdBudRoomRevenue { get; set; }
        public decimal MtdbudRoomsAvailable { get; set; }
        public decimal Mtdbudoutoforderrooms { get; set; }
        public decimal MtdBudOccupancy { get; set; }
        public decimal MtdBudRoomsSold { get; set; }
        public decimal MtdBudADR { get; set; }
        public decimal MtdBudRevPar { get; set; }
        public decimal MtdBudTotalRevenue { get; set; }
        public decimal MtdBudOtherRevenue { get; set; }

        //year
        public decimal YtdRoomRevenue { get; set; }
        public decimal YtdRoomsAvailable { get; set; }
        public decimal Ytdoutoforderrooms { get; set; }
        public decimal YtdOccupancy { get; set; }
        public decimal YtdRoomsSold { get; set; }
        public decimal YtdADR { get; set; }
        public decimal YtdRevPar { get; set; }
        public decimal YtdTotalRevenue { get; set; }
        public decimal YtdOtherRevenue { get; set; }

        public decimal YtdLYRoomRevenue { get; set; }
        public decimal YtdLYRoomsAvailable { get; set; }
        public decimal YtdLYoutoforderrooms { get; set; }
        public decimal YtdLYOccupancy { get; set; }
        public decimal YtdLYRoomsSold { get; set; }
        public decimal YtdLYADR { get; set; }
        public decimal YtdLYRevPar { get; set; }
        public decimal YtdLYTotalRevenue { get; set; }
        public decimal YtdLYOtherRevenue { get; set; }

        public decimal YtdBudRoomRevenue { get; set; }
        public decimal YtdbudRoomsAvailable { get; set; }
        public decimal Ytdbudoutoforderrooms { get; set; }
        public decimal YtdBudOccupancy { get; set; }
        public decimal YtdBudRoomsSold { get; set; }
        public decimal YtdBudADR { get; set; }
        public decimal YtdBudRevPar { get; set; }
        public decimal YtdBudTotalRevenue { get; set; }
        public decimal YtdBudOtherRevenue { get; set; }


    }

    public class DailySalesMatricesResp
    {
        public DataSection data { get; set; }
    }

    public class DataSection
    {

        public List<CorporationsDataToday> sale_date { get; set; }
        public List<CorporationsDataMonth> month_to_date { get; set; }
        public List<CorporationsDataYear> year_to_date { get; set; }
    }
    

    public class CorporationsDataToday
    {
        public string label { get; set; }
        public DateTime display_date { get; set; }
        public CorporationsInfo sale_date { get; set; }
        public Metriccc occupancy { get; set; }
        public Metricc adr { get; set; }
        public Metricc rev_par { get; set; }
        public Metricc room_revenue { get; set; }
        public Metricc other_revenue { get; set; }
        public Metricc total_revenue { get; set; }
    }
    public class CorporationsDataMonth
    {

        public DateTime MonthFromDate { get; set; }
        public DateTime MonthToDate { get; set; }
        public CorporationsInfo month_to_date { get; set; }       
        public Metriccc occupancy { get; set; }
        public Metricc adr { get; set; }
        public Metricc rev_par { get; set; }
        public Metricc room_revenue { get; set; }
        public Metricc other_revenue { get; set; }
        public Metricc total_revenue { get; set; }
    }
    public class CorporationsDataYear
    {
        public DateTime YearFromDate { get; set; }
        public DateTime YearToDate { get; set; }
        public CorporationsInfo year_to_date { get; set; }
        public Metriccc occupancy { get; set; }
        public Metricc adr { get; set; }
        public Metricc rev_par { get; set; }
        public Metricc room_revenue { get; set; }
        public Metricc other_revenue { get; set; }
        public Metricc total_revenue { get; set; }
    }
    public class CorporationsInfo
    {
        public string corporation { get; set; }
        public string corporation_legal {  get; set; }
        public string avail_rooms { get; set; }
        public string sold_rooms { get; set; }
        public string ooo_rooms {  get; set; }
    }   

    public class Metricc
    {

        public decimal actual { get; set; }
        public ComparisonMetric last_year { get; set; }
        public ComparisonMetric budget { get; set; }
    }

    public class Metriccc
    {

        public string actual { get; set; }
        public ComparisonMetricc last_year { get; set; }
        public ComparisonMetricc budget { get; set; }
    }
    public class ComparisonMetric
    {
        public decimal values { get; set; }
        public decimal variance { get; set; }
        public string sign { get; set; }
    }
    public class ComparisonMetricc
    {
        public string values { get; set; }
        public string variance { get; set; }
        public string sign { get; set; }
    }
}