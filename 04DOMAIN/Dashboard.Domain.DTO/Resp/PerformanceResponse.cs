using Common.Domain.DTO.App;
using Common.Domain.DTO.Extensions;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Dashboard.Domain.DTO.Enums;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Dashboard.Domain.DTO.Resp
{
    public class PerformanceResponse : StatusDTO
    {
        public PerformanceResponse()
        {
            Performances = new List<PerformanceStats>();
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<PerformanceStats> Performances { get; set; } = new List<PerformanceStats>();
        public string Currency { get; set; } = string.Empty;
    }
    public class PerformanceStats : CorporationInfo
    {
        public RoomStatistics RoomStats { get; set; } = new RoomStatistics();
        public DepartmentalStatistics DepartmentalStats { get; set; } = new DepartmentalStatistics();
    }
    public class DepartmentalStatistics
    {
        public StatisticDetails RoomRevenue { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails FoodandBeverages { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails OtherDepIncome { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails TotalRevenue { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails DepartmentalExpense { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails GOP { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails UndistributedandOtherExp { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails UndistributedExp { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails  OtherExp { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails EBITDA { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails NetIncome { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails TotalExpense { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails MiscellaneousRevenue { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);

    }
    public class DeptExpenses :CorporationInfo
    {
        public string BrandName {  get; set; }   
        public StatisticDetails DeptExpense { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails UndistributedExpense { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails OtherExpense { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails TotalExpense { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
    }
    public class RoomStatistics
    {
        public StatisticDetails TotalRooms { get; set; } = new StatisticDetails(StatisticUnitEnum.Number.ToString());
        //public StatisticDetails Occupied { get; set; } = new StatisticDetails(StatisticUnitEnum.Number.ToString());
        public StatisticDetails RoomsSold { get; set; } = new StatisticDetails(StatisticUnitEnum.Number.ToString());
        public StatisticDetails Vacant { get; set; } = new StatisticDetails(StatisticUnitEnum.Number.ToString());
        public StatisticDetails OutOfOrder { get; set; } = new StatisticDetails(StatisticUnitEnum.Number.ToString());
        public StatisticDetails Comp { get; set; } = new StatisticDetails(StatisticUnitEnum.Number.ToString());
        public StatisticDetails Occupancy { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.PercentSymbol);
        public StatisticDetails ADR { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
        public StatisticDetails RevPAR { get; set; } = new StatisticDetails(NumberFormatInfo.CurrentInfo.CurrencySymbol);
    }
    public class  PerofrmanceExpenseResponse : StatusDTO
    {
        public PerofrmanceExpenseResponse()
        {
            Expenses = new List<DeptExpenses>();
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public string Currency { get; set; } = string.Empty;
        public List<DeptExpenses> Expenses {  get; set; }=new List<DeptExpenses>();
    }

    public class StatisticDetails : StatndardComparisonStatistics
    {
        public StatisticDetails() { this.Unit = NumberFormatInfo.CurrentInfo.CurrencySymbol; }
        public StatisticDetails(String Unit) { this.Unit = Unit; }

        /// <summary>
        /// If the stats are expences related then colcullate the varience with LY
        /// </summary>
        private bool IsExpenceStats { get; set; } = false;
        public short ComparisonFilter { get; set; } = 0;
        public decimal Max { get; set; } = 0.00M;
        public string Unit { get; set; } = string.Empty;
        public decimal SubValue
        {
            get
            {
                if (ComparisonFilter == (short)PropertyFilterEnum.VsBudget)
                    return Budget;
                else if (ComparisonFilter == (short)PropertyFilterEnum.VsLY)
                    return LY;
                else if (ComparisonFilter == (short)PropertyFilterEnum.VsForecast)
                    return Forecast;
                else if (ComparisonFilter == (short)PropertyFilterEnum.VsLYVar)
                    return (IsExpenceStats) ? LY - Current : Current - LY;
                else if (ComparisonFilter == (short)PropertyFilterEnum.VsBudgetVar)
                    return (IsExpenceStats) ? Budget - Current : Current - Budget;
                else if (ComparisonFilter == (short)PropertyFilterEnum.VsForecastVar)
                    return (IsExpenceStats) ? Forecast - Current : Current - Forecast;
                else
                    return 0.0M;
            }
        }

        public decimal GetSubValue(short ComparisonFilter, bool IsExpenceStats = false)
        {
            this.ComparisonFilter = ComparisonFilter;
            this.IsExpenceStats = IsExpenceStats;

            return this.SubValue;
        }
        public string GetSubValueWithSymbol(short ComparisonFilter, bool IsExpenceStats = false, bool IsNumberFormatStats = false)
        {
            this.ComparisonFilter = ComparisonFilter;
            this.IsExpenceStats = IsExpenceStats;

            if (this.Unit == NumberFormatInfo.CurrentInfo.PercentSymbol)
                return $"{this.SubValue}{NumberFormatInfo.CurrentInfo.PercentSymbol}";
            //this is temperory for UI
            else if (this.Unit ==new CultureInfo("en-GB").NumberFormat.CurrencySymbol ||  this.Unit == NumberFormatInfo.CurrentInfo.CurrencySymbol)
                return $"{NumberFormatInfo.CurrentInfo.CurrencySymbol}{this.SubValue.CountingShorthand()}";
            else if (this.Unit == StatisticUnitEnum.Number.ToString())
                return this.SubValue.CountingShorthand(true);
            else
                return this.SubValue.CountingShorthand(IsNumberFormatStats);
        }
    }

    public static class StatisticsDetailsExtension
    {
        public static string GetCurrentDisplayValue(this StatisticDetails stats, bool IsNumberFormatStats = false)
        {
            if (stats.Unit == NumberFormatInfo.CurrentInfo.PercentSymbol)
                return $"{stats.Current}{NumberFormatInfo.CurrentInfo.PercentSymbol}";
            else if (stats.Unit == NumberFormatInfo.CurrentInfo.CurrencySymbol)
                return $"{NumberFormatInfo.CurrentInfo.CurrencySymbol}{stats.Current.CountingShorthand()}";
            else if (stats.Unit == StatisticUnitEnum.Number.ToString())
                return stats.Current.CountingShorthand(true);
            else
                return stats.Current.CountingShorthand(IsNumberFormatStats);
        }
    }

    public class StatndardComparisonStatistics
    {
        public decimal Current { get; set; } = 0.0M;
        public decimal Budget { get; set; } = 0.0M;
        public decimal LY { get; set; } = 0.0M;
        public decimal Forecast { get; set; } = 0.0M;
        public Int32 Order { get; set; }
        public Int32 Type { get; set; }
        public decimal PerIncome { get; set; } = 0.0M;
        public decimal LYPerIncome { get; set; } = 0.0M;
        public decimal BudPerIncome { get; set; } = 0.0M;
        public decimal ForePerIncome { get; set; } = 0.0M;
    }

    public class CorporationInfo : ModelBaseCorporation
    {
        public string LegalName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
    }

    public class PerformanceDbResponse : CorpNames
    {
        public decimal RoomRevenue { get; set; }
        public decimal Occupancy { get; set; }
        public decimal RoomsSold { get; set; }
        public decimal RevPar { get; set; }
        public decimal ADR { get; set; }
        public decimal TotalRevenue { get; set; }
        public string BrandName { get; set; }
        public decimal LYRoomRevenue { get; set; }
        public decimal LYOccupancy { get; set; }
        public decimal LYRoomsSold { get; set; }
        public decimal LYRevPar { get; set; }
        public decimal LYADR { get; set; }
        public decimal LYTotalRevenue { get; set; }
        public decimal BudRoomRevenue { get; set; }
        public decimal BudOccupancy { get; set; }
        public decimal BudRoomsSold { get; set; }
        public decimal BudRevPar { get; set; }
        public decimal BudADR { get; set; }
        public decimal BudTotalRevenue { get; set; }
        public decimal ForecastRoomRevenue { get; set; }
        public decimal ForecastOccupancy { get; set; }
        public decimal ForecastRoomsSold { get; set; }
        public decimal ForecastRevPar { get; set; }
        public decimal ForecastADR { get; set; }
        public decimal ForecastTotalRevenue { get; set; }

    }

    public class AllCorpsStatsDbResponse:CorpNames
    {
        public string BrandName { get; set; }
        public decimal RoomsAvailable {  get; set; }    
        public decimal RoomsSold {  get; set; } 
        public decimal Occupancy {  get; set; } 
        public decimal ADR {  get; set; }  
        public decimal RevPar {  get; set; }    
        public decimal Vacant {  get; set; }    
        public decimal OutOfOrder {  get; set; }    
        public decimal Comp {  get; set; }
        public decimal LYRoomsAvailable { get; set; }
        public decimal LYRoomsSold { get; set; }
        public decimal LYOccupancy { get; set; }
        public decimal LYADR { get; set; }
        public decimal LYRevPar { get; set; }
        public decimal LYVacant { get; set; }
        public decimal LYOutOfOrder { get; set; }
        public decimal LYComp { get; set; }
        public decimal BudRoomsAvailable { get; set; }
        public decimal BudRoomsSold { get; set; }
        public decimal BudOccupancy { get; set; }
        public decimal BudADR { get; set; }
        public decimal BudRevPar { get; set; }
        public decimal BudVacant { get; set; }
        public decimal BudOutOfOrder { get; set; }
        public decimal BudComp { get; set; }
        public decimal ForecastRoomsAvailable { get; set; }
        public decimal ForecastRoomsSold { get; set; }
        public decimal ForecastOccupancy { get; set; }
        public decimal ForecastADR { get; set; }
        public decimal ForecastRevPar { get; set; }
        public decimal ForecastVacant { get; set; }
        public decimal ForecastOutOfOrder { get; set; }
        public decimal ForecastComp { get; set; }
    }

    public class AllCorpsRevenuesDbResponse : CorpNames
    {
        public string BrandName { get; set; }
        public decimal RoomRevenue {  get; set; }   
        public decimal LYRoomRevenue { get; set; }  
        public decimal BudRoomRevenue { get; set; } 
        public decimal ForecastRoomRevenue { get; set; }    
        public decimal TotalRevenue { get; set; }   
        public decimal LYTotalRevenue { get; set; } 
        public decimal BudTotalRevenue { get; set; }    
        public decimal ForecastTotalRevenue { get; set; }   
        public decimal FandBRevenue { get; set; }   
        public decimal LYFandBRevenue { get; set; } 
        public decimal BudFandBRevenue { get; set; }    
        public decimal ForecasFandBRevenue { get; set; }    
        public decimal OtherRevenue { get; set; }   
        public decimal LYOtherRevenue { get; set; } 
        public decimal BudOtherRevenue { get; set; }    
        public decimal ForecastOtherRevenue { get; set; }   
        public decimal MissLiniousRevenue { get; set; }     
        public decimal LYMissLiniousRevenue { get; set; }   
        public decimal BudMissLiniousRevenue { get; set; }  
        public decimal ForecastMissLiniousRevenue { get; set; } 

    }
    public class AllcorpExpenses:CorpNames
    {
        public decimal DeptExpCurrent {  get; set; }
        public decimal DeptExpLY { get; set; }
        public decimal DeptExpBudget { get; set; }
        public decimal DeptExpForecast { get; set; }

        public decimal UndisExpCurrent { get; set; }
        public decimal UndisExpLY { get; set; }
        public decimal UndisExpBudget { get; set; }
        public decimal UndisExpForecast { get; set; }

        public decimal OtherExpCurrent { get; set; }
        public decimal OtherExpLY { get; set; }
        public decimal OtherExpBudget { get; set; }
        public decimal OtherExpForecast { get; set; }

        public decimal TotalExpCurrent { get; set; }
        public decimal TotalExpLY { get; set; }
        public decimal TotalExpBudget { get; set; }
        public decimal TotalExpForecast { get; set; }

    }

    public class PerformanceAllCorpsDbResponse : PerformanceDbResponse
    {
        public decimal RoomsAvailable { get; set; }
        public decimal BudRoomsAvailable { get; set; }
        public decimal LYRoomsAvailable { get; set; }
        public decimal ForecastRoomsAvailable { get; set; }
        public decimal BudOutofOrder { get; set; }
        public decimal LYOutOfOrder { get; set; }
        public decimal ForecastOutofOrder { get; set; }
        public decimal OutofOrder { get; set; }
        public decimal Vacant { get; set; }
        public decimal Comp { get; set; }
        public decimal LYVacant { get; set; }
        public decimal LYComp { get; set; }
        public decimal BudVacant { get; set; }
        public decimal BudComp { get; set; }
        public decimal ForecastComp { get; set; }
        public decimal ForecastVacant { get; set; }
        public decimal FandBRevenue { get; set; }
        public decimal LYFandBRevenue { get; set; }
        public decimal BudFandBRevenue { get; set; }
        public decimal ForecastFandBRevenue { get; set; }
        public decimal OtherRevenue { get; set; }
        public decimal LYOtherRevenue { get; set; }
        public decimal BudOtherRevenue { get; set; }
        public decimal ForecastOtherRevenue { get; set; }
        public decimal DeptExpense { get; set; }
        public decimal BudDeptExpense { get; set; }
        public decimal ForecastDeptExpense { get; set; }
        public decimal LYDeptExpense { get; set; }
        public decimal UndistandOtherExpense { get; set; }
        public decimal LYUndistandOtherExpense { get; set; }
        public decimal BudUndistandOtherExpense { get; set; }
        public decimal ForecastUndistandOtherExpense { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal LYTotalExp { get; set; }
        public decimal BudTotalExp { get; set; }
        public decimal ForecastTotalExp { get; set; }

    }

    public class SalesReviewsDailyResponse : StatusDTO
    {
        public List<SalesReviewsDailyDbResponse> DailyReportDetails { get; set; }
    }
    public class DailyResponse
    {
        public decimal? Actuval { get; set; }
        public decimal? Budget { get; set; }
        public decimal? Forecast { get; set; }
        public DateTime Dates { get; set; }
    }

    public class SalesReviewsDailyDbResponse
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public int RecordType { get; set; }
        public decimal YTDAmt { get; set; }
        public decimal YTDLYAmt { get; set; }
        public decimal YTB { get; set; }
        public decimal YTF { get; set; }
        public DateTime Ddate { get; set; }
    }

    public class SalesandPropertyResponse
    {
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public decimal? LYAmount { get; set; }
        public decimal? Budget { get; set; }
        public decimal?ForeCast { get; set; }
        public Int32 Order { get; set; }
        public Int32 Type { get; set; }
        public decimal MAXCAP { get; set; }
        public decimal PerIncome { get; set; }
        public decimal LYPerIncome {  get; set; }
        public decimal BudPerIncome { get; set; }
        public decimal ForePerIncome { get; set; }

    }
    public class HotelFinancialData : StatusDTO
    {
        public string HotelName { get; set; }
        public string TimePeriod { get; set; }
        public Revenue Revenue { get; set; } = new Revenue();
        public Expenditure Expenditure { get; set; } = new Expenditure();
        public Profitability Profitability { get; set; } = new Profitability();


    }

    public class Revenue
    {
        public DepartmentData RoomsDepartment { get; set; } = new DepartmentData();
        public DepartmentData FoodAndBeverages { get; set; } = new DepartmentData();
        public DepartmentData TotalOperatingRevenue { get; set; } = new DepartmentData();


    }

    public class Expenditure
    {
        public DepartmentData RoomsDepartment { get; set; } = new DepartmentData();
        public DepartmentData FoodAndBeveragesDepartment { get; set; } = new DepartmentData();
        public DepartmentData OtherOperatingDepartments { get; set; } = new DepartmentData();
        public DepartmentData AdministrativeAndGeneralDepartment { get; set; } = new DepartmentData();
        public DepartmentData InformationAndTelecommunicationSystems { get; set; } = new DepartmentData();
        public DepartmentData SalesAndMarketingDepartment { get; set; } = new DepartmentData();
        public DepartmentData FranchiseRelated { get; set; } = new DepartmentData();
        public DepartmentData PropertyOperationAndMaintenanceDepartment { get; set; } = new DepartmentData();
        public DepartmentData Utilities { get; set; } = new DepartmentData();

    }

    public class Profitability
    {
        public DepartmentData GrossOperatingProfit { get; set; } = new DepartmentData();
        public DepartmentData NetIncome { get; set; } = new DepartmentData();

    }

    public class DepartmentData
    {
        public decimal PTD { get; set; }
        public decimal PTD_BUD { get; set; }
        public decimal PTD_LY { get; set; }
        public decimal YTD { get; set; }
        public decimal YTD_BUD { get; set; }
        public decimal YTD_LY { get; set; }

    }

}
