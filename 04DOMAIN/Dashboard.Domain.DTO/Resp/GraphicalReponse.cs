using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class GraphicalReponse : StatusDTO
    {
        public GraphicalReponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        /// <summary>
        /// refer GraphUnitType
        /// </summary>
        public short YaxisUnit { get; set; }
        public List<KeyValuePairObject<string, StatndardComparisonStatistics>> GraphData { get; set; } = new List<KeyValuePairObject<string, StatndardComparisonStatistics>>();

    }
    public class MonthGraphResponse
    {
        public string MonthNames { get; set; }
        public string MonthDate { get; set; }
        public Int32 Type { get; set; }
        public Int32 RecordType { get; set; }
        public decimal? YTDAmt { get; set; }
        public decimal? YTDLYAmt { get; set; }
        public decimal? YTB { get; set; }
        public decimal? YTF { get; set; }
        public string Name { get; set; }
    }
    public class TrendsMonthWiseDbResponse : CommonStatsandDepartsTrendDbResponse
    {
        public string MonthORDayORYear { get; set; }
    }
    public class CommonStatsandDepartsTrendDbResponse
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal LYAmount { get; set; }
        public decimal Budget { get; set; }
        public decimal Forecast { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalLYAmount { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal TotalForecast { get; set; }



    }
    public class TrendsDayWiseDbResponse : CommonStatsandDepartsTrendDbResponse
    {
        public string MonthDay { get; set; }
    }

    public class DepartmnetGraphResponse : StatusDTO
    {
        public List<DepartmentOverview> departmentOverviews { get; set; } = new List<DepartmentOverview>();
    }
    public class DepartmentOverview
    {
        public string LabelName { get; set; }
        public List<OverViewDetails> overViewDetails { get; set; } = new List<OverViewDetails>();

    }
    public class OverViewDetails : StatndardComparisonStatistics
    {
        public string MonthName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalLYAmount { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal TotalForecast { get; set; }
        public int SubType { get; set; }
    }


    public class CorporationsRequestIDDetails
    {
        public List<ModelBaseCorporationID> ListCorporations { get; set; } = new List<ModelBaseCorporationID>();
    }
    public class RequestIDResponse : StatusDTO
    {
        public Int64 ID { get; set; }
    }
    public class CustrendsDbResponse
    {
        public string CorporationId { get; set; }
        public string Name { get; set; }
        public string CorporationName { get; set; }
        public string LegalName { get; set; }
        public Int32 Type { get; set; }
        public Int32 SubType { get; set; }
        public decimal ActualValue { get; set; }
        public decimal BudgetValue { get; set; }
        public decimal Forecastvalue { get; set; }
        public decimal LYValue { get; set; }
        public decimal TotalActualValue { get; set; }
        public decimal TotallyValue { get; set; }
        public decimal TotalBudgetValue { get; set; }
        public decimal TotalForecastValue { get; set; }
        public string MonthORDay { get; set; }
        public Int32 QUARTER { get; set; }
        public Int32 YEAR { get; set; }

    }

    public class RequestCustomFormula
    {
        public Int32 ReqID { get; set; }
        public string CorporationID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Int16 Type { get; set; }
        public Int16 SubType { get; set; }
        public Int64 CustomKey { get; set; }
        public string CustomBinID { get; set; }
        public string CustomKeyName { get; set; }
        public long WidgetID { get; set; }
        public Int32 DeptId { get; set; }

    }
    public class PandLAccountsGraphResponse : StatusDTO
    {
        public List<PandLAccountsResponse> pandLResponses { get; set; } = new List<PandLAccountsResponse>();
    }
    public class PandLAccountsResponse
    {
        public string CorporationID { get; set; }
        public string CorpDBAName { get; set; }
        public string CorpLegalName { get; set; }
        public string CustomKeyID { get; set; }
        public Int16 Type { get; set; }
        public Int16 SubType { get; set; }
        public string CustomKeyName { get; set; }
        public decimal ActualValue { get; set; }
        public decimal LYvalue { get; set; }
        public decimal BudgetValue { get; set; }
        public decimal ForecastValue { get; set; }
        public decimal ActualPerincome { get; set; }
        public decimal LYPerincome { get; set; }
        public decimal budgetPerincome { get; set; }
        public decimal forecastPerincome { get; set; }
        public decimal ActualPOR { get; set; }
        public decimal LYPOR { get; set; }
        public decimal BudgetPOR { get; set; }
        public decimal ForecastPOR { get; set; }
        public decimal ActualPAR { get; set; }
        public decimal LYPAR { get; set; }
        public decimal BudgetPAR { get; set; }
        public decimal ForecastPAR { get; set; }
        public decimal ActualSTATS { get; set; }
        public decimal LYSTATS { get; set; }
        public decimal BudgetSTATS { get; set; }
        public decimal ForecastSTATS { get; set; }
        public decimal TotalActualValue { get; set; }
        public decimal TotalLYValue { get; set; }
        public decimal TotalBudgetValue { get; set; }
        public decimal TotalForecastvalue { get; set; }
        public decimal Operatingrevenue { get; set; }
        public decimal LYrevenue { get; set; }
        public decimal ForecastRevenue { get; set; }
        public decimal Budrevenue { get; set; }
        public string MonthName { get; set; }
        public string MonthORDay { get; set; }
        public long CustomKey { get; set; }
    }
    public class IncomeGroupAnalysisTableResponse
    {
        public List<IncomeGroupAnalysisDbResponse> analysisTable { get; set; } = new List<IncomeGroupAnalysisDbResponse>();
    }

    public class IncomeGroupAnalysisDbResponse : IncomeGroupBalanceDbResponse
    {
        public decimal perincomeact { get; set; }
        public decimal perincomebud { get; set; }
        public decimal perincomefr { get; set; }
        public decimal perincomeLY { get; set; }
        public decimal Actualpor { get; set; }
        public decimal BudgetPOR { get; set; }
        public decimal ForecastPOR { get; set; }
        public decimal LYPOR { get; set; }
        public decimal ActualPAR { get; set; }
        public decimal BudgetPAR { get; set; }
        public decimal ForecastPAR { get; set; }
        public decimal LYPAR { get; set; }
        public decimal STATSORHOURS { get; set; }
        public decimal BUDSTATSorHOURS { get; set; }
        public decimal FRSTATSorHOURS { get; set; }
        public decimal lySTATSORHOURS { get; set; }
        public decimal OperatingRevenue { get; set; }
        public decimal Budrevenue { get; set; }
        public decimal ForecastRevenue { get; set; }
        public decimal LYrevenue { get; set; }
       // public int Uniqueid {  get; set; }  

    }
    public class IncomeGroupBalanceDbResponse
    {
        public Int32 URLKEY { get; set; }
        public Int32 Type { get; set; }
        public Int32 Subtype { get; set; }
        public Int32 DeptID { get; set; }
        public Int64 customkeyid { get; set; }
        public Int64 GroupID { get; set; }
        public string DeptName { get; set; }
        public string Group { get; set; }
        public decimal balance { get; set; }
        public decimal BudBalance { get; set; }
        public decimal LYBalance { get; set; }
        public decimal ForecastBalance { get; set; }
        public decimal totalLYbalance { get; set; }
        public decimal totalbalance { get; set; }
        public decimal BUDtotalbalance { get; set; }
        public decimal FRtotalbalance { get; set; }
        public string MonthORDayORYear { get; set; }
    }
    public class PandLAccountsRequest
    {
        public Int16 IsMonthOrDay { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Int32 LYyear { get; set; }
        public long FormulaID { get; set; }
        public long URLkey { get; set; }
    }

    public class PandLRquest
    {
        public List<RequestCustomFormula> requestCustomFormulas { get; set; } = new List<RequestCustomFormula>();
        public PandLAccountsRequest PandLAccountsRequest { get; set; } = new PandLAccountsRequest();
    }

    public class PandLActualValues
    {
        public long CustomKey { get; set; }
        public string MonthName { get; set; }
        public decimal ActualValue { get; set; }
    }

    public class AnalysisOrTrendsResponse : StatusDTO
    {
        public Dictionary<string, double> ActualsMonthOrDayWise { get; set; }
        public Dictionary<string, double> BudgetMonthOrDayWise { get; set; }
        public Dictionary<string, double> ForecastMonthOrDayWise { get; set; }
        public Dictionary<string, double> LYMonthOrDayWise { get; set; }

        public Dictionary<string, double> AcualTotalsMonthOrDayWise { get; set; }
        public Dictionary<string, double> BudgetTotalsMonthOrDayWise { get; set; }
        public Dictionary<string, double> ForecastTotalsMonthOrDayWise { get; set; }
        public Dictionary<string, double> LyTotlasMonthOrDayWise { get; set; }


        public Dictionary<string, double> ActualsMonthOrDayWisePAR { get; set; }
        public Dictionary<string, double> BudgetMonthOrDayWisePAR { get; set; }
        public Dictionary<string, double> ForecastMonthOrDayWisePAR { get; set; }
        public Dictionary<string, double> LYMonthOrDayWisePAR { get; set; }


        public Dictionary<string, double> ActualsMonthOrDayWisePOR { get; set; }
        public Dictionary<string, double> BudgetMonthOrDayWisePOR { get; set; }
        public Dictionary<string, double> ForecastMonthOrDayWisePOR { get; set; }
        public Dictionary<string, double> LYMonthOrDayWisePOR { get; set; }

        public Dictionary<string, double> ActualsMonthOrDayWisePerIncome { get; set; }
        public Dictionary<string, double> BudgetMonthOrDayWisePerIncome { get; set; }
        public Dictionary<string, double> ForecastMonthOrDayWisePerIncome { get; set; }
        public Dictionary<string, double> LYMonthOrDayWisePerIncome { get; set; }

        public Dictionary<string, double> ActualsMonthOrDayWiseHoursOrStats { get; set; }
        public Dictionary<string, double> BudgetMonthOrDayWiseHoursOrStats { get; set; }
        public Dictionary<string, double> ForecastMonthOrDayWiseHoursOrStats { get; set; }
        public Dictionary<string, double> LYMonthOrDayWiseHoursOrStats { get; set; }

        public double ActualsSum { get; set; } = 0;
        public double BudgetSum { get; set; } = 0;
        public double ForecastSum { get; set; } = 0;
        public double LySum { get; set; } = 0;
        public double ActualTotalSum { get; set; } = 0;
        public double BudgetTotalSum { get; set; } = 0;
        public double ForecastTotalsSum { get; set; } = 0;
        public double LyTotalsSum { get; set; } = 0;
        public double ActualPORSum { get; set; } = 0;
        public double BudgetPORSum { get; set; } = 0;
        public double ForecastPORSum { get; set; } = 0;
        public double LyPORSum { get; set; } = 0;
        public double ActualPARSum { get; set; } = 0;
        public double BudgetPARSum { get; set; } = 0;
        public double ForecastPARSum { get; set; } = 0;
        public double LyPARSum { get; set; } = 0;
        public double ActualPerIncomeSum { get; set; } = 0;
        public double BudgetPerIncomeSum { get; set; } = 0;
        public double ForecastPerIncome { get; set; } = 0;
        public double LyPerIncomeSum { get; set; } = 0;
        public double ActualStatsOrHoursSum { get; set; } = 0;
        public double BudgetStatsOrHoursSum { get; set; } = 0;
        public double ForecastStatsOrHoursSum { get; set; } = 0;
        public double LyStatsOrHours { get; set; } = 0;
        public string DepartmentName { get; set; }
        public long FormulaID { get; set; }
        public short SortOrder { get; set; }
        public decimal HoursStats { get; set; } = 0.0M;
        public decimal BudgetHoursStats { get; set; } = 0.0M;
        public decimal ForecastHoursStats { get; set; } = 0.0M;
        public decimal LyHoursStats { get; set; } = 0.0M;
        public decimal Income { get; set; } = 0.0M;
        public decimal IncomeBugdet { get; set; } = 0.0M;
        public decimal IncomeForecast { get; set; } = 0.0M;
        public decimal LyIncome { get; set; } = 0.0M;
        public decimal POR { get; set; } = 0.0M;
        public decimal BudgetPOR { get; set; } = 0.0M;
        public decimal ForecastPOR { get; set; } = 0.0M;
        public decimal LyPOR { get; set; } = 0.0M;
        public decimal PAR { get; set; } = 0.0M;
        public decimal LyPAR { get; set; } = 0.0M;
        public decimal BdugetPAR { get; set; } = 0.0M;
        public decimal ForecastPAR { get; set; } = 0.0M;
        public decimal vsHoursStats { get; set; } = 0.0M;
        public decimal vsIncome { get; set; } = 0.0M;
        public decimal vsPOR { get; set; } = 0.0M;
        public decimal vsPAR { get; set; } = 0.0M;
        public decimal OperatingRevenue { get; set; } = 0.0M;
        public decimal BudgetOperatingRevenue { get; set; } = 0.0M;
        public decimal ForecastOperatingRevenue { get; set; } = 0.0M;
        public decimal LyOperatingRevenue { get; set; } = 0.0M;
        public short Type { get; set; }

    }

    public class ListOfCustomFormulaResponse : StatusDTO
    {
        public List<AnalysisOrTrendsResponse> AnalysisOrTrendsData { get; set; } = new List<AnalysisOrTrendsResponse>();

        public List<CustomTrendsGraphResponse> TrendsData { get; set; } = new List<CustomTrendsGraphResponse>();
        public List<CustomTrendsGraphResponse> IncomeTrendsData { get; set; } = new List<CustomTrendsGraphResponse>();
    }

}
