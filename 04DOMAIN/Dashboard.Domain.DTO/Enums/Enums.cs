using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Enums
{
    public enum StatisticUnitEnum
    {
        Currency,
        Percent,
        Number
    }
    public enum IncStmtDivisionEnum
    {
        Financail = 1,
        Statistics
    }

    /// <summary>
    /// Account change (All,Income,Expense)
    /// </summary>
    public enum CustomAccountTypeEnum
    {
        [Display(Name = "All")]
        All,
        [Display(Name = "Income")]
        Income = 20,
        [Display(Name = "Expense")]
        Expense = 22,
        [Display(Name = "Cost Of Goods Sold")]
        CostofGoodsSold = 21,
        [Display(Name = "Other Income")]
        OtherIncome =23,
        [Display(Name = "Other Expense")]
        OtherExpense =24,
    }

    public enum IncomeStatsDeptEnum
    {
        Statistics = 0,
        Departments = 1

    }
    public enum IncomeStatsStatisticsEnum
    {
        Occupancy = 1,
        ADR = 2,
        REVPAR = 3,
        TotalRooms = 4,
        RoomsSold = 5,
        Vacant = 6,
        OutOfOrder = 7,
        Comp = 8
    }
    public enum IncomeStatsDepartmentsEnum
    {
        RoomRevenue = 1,
        FoodAndBeverageRevenue = 2,
        OtherRevenue = 3,
        TotalRevenue = 4,
        DepartmentalExp = 5,
        GOP = 6,
        UndistributedAndOtherExp = 7,
        EBITDA = 8,
        NetIncome = 9,
        OtherExp=10,
        MisliniousRevenue=11
    }
    public enum AdrRevparOccEnum
    {
        OccupancyPer = 3,
        ADR,
        REVPAR

    }
    public enum DuesAsPerEnum
    {
        [Display(Name = "As Per Reconciliation & BalanceSheet")]
        AsPerReconcilationAndBalanceSheet=0,
        [Display(Name = "As Per Feeds")]
        AsPerFeeds
    }
   
    public enum VendorDueFilterEnum
    {
        AlreadyDue,
        UpcomingDueIn7Days
    }

    public enum PropertyFilterEnum
    {
        [Display(Name = "Vs All")]
        VsAll = 0,
        [Display(Name = "Vs Budget")]
        VsBudget = 1,
        [Display(Name = "Vs Forecast")]
        VsForecast,
        [Display(Name = "Vs LY")]
        VsLY,

        [Display(Name = "Vs Bud and Forecast")]
        VsBudandForecast,
        [Display(Name = "Vs Bud and LY")]
        VsBudAndLY,

        [Display(Name = "Vs Budget var")]
        VsBudgetVar,
        [Display(Name = "Vs Forecast var")]
        VsForecastVar,
        [Display(Name = "Vs LY var")]
        VsLYVar,

        [Display(Name = "Vs Bud var and Forecast var")]
        VsBudVarandForecastVar,
        [Display(Name = "Vs Bud var and LY var")]
        VsBudVarAndLYVar,

        [Display(Name = "% Income")]
        IncomePercent,
        [Display(Name = "POR")]
        POR,
        [Display(Name = "PAR")]
        PAR,


        //Vs Budget,Forecast,Comp,LY
    }
    public enum BudLYComparisonEnum
    {
        [Display(Name = "High Bud/LY Variance")]
        HighBudOrLYVariance,

        [Display(Name = "Low Bud/LY Variance")]
        LowBudOrLYVariance,


    }
    public enum VarianceEnum
    {
        [Display(Name = "High Variance")]
        HighVariance,
        [Display(Name = "Low Variance")]
        LowVariance
    }
    public enum OccupancySortOrderEnum
    {
        [Display(Name = "High Occupancy to Low")]
        HighToLowOccupancy,
        [Display(Name = "Low Occupancy to High")]
        LowToHighOccupancy,
        [Display(Name = "High Occupancy Variance to Low")]
        HighToLowOccupancyVar,
        [Display(Name = "Low Occupancy Variance to High")]
        LowToHighOccupancyVar,
        [Display(Name ="By Corporation")]
        ByCorporation
    }
    public enum AssetLiabilityFilterEnum
    {
        [Display(Name = "AP Turnover")]
        APTurnover = 0,

        [Display(Name = "AR Turnover")]
        ARTurnover,

        //[Display(Name = "Inventory Turnover")]
        //InventoryTurnover,

        [Display(Name = "Net Working Capital Turnover")]
        NetWorkingCapitalTurnover = 3,

        [Display(Name = "Liquidity Current Ratio")]
        LiquidityCurrentRatio,

        //[Display(Name = "Debt Service Coverage Ratio")]
        //DebtServiceCoverageRatio,

        [Display(Name = "Return on Assets")]
        ReturnOnAssets = 6,

        [Display(Name = "Return on Equity")]
        ReturnOnEquity
    }

    public enum WidgetViewFilterEnum
    {
        Yesterday,
        Daily,
        Monthly,
        Quarterly,
        Yearly
    }
    public enum CashCardFilter
    {
        [Display(Name = "Cleared Date")]
        ClearedDate = 0,
        [Display(Name = "Reconciled Date")]
        ReconciledDate = 1,
        [Display(Name = "Deposited Date")]
        DepositedDate = 2
    }

    public enum DepartmentsFilterEnum
    {
        Occupancy,
        ADR,
        RevPAR,
        RoomsSold,
        Vacant,
        OutOfOrder,
        Comp,
        RoomRevenue,
        FoodandBeverages,
        OtherDepIncome,
        TotalRevenue,
        DepartmentalExpense,
        GOP,
        UndistributedandOtherExp,
        EBITDA,
        NetIncome
    }

    public enum BalanceSheetEntitiesEnum
    {
        CashandBank,
        Recievables,
        Payables,
        LongTermLiability,
        NetIncome,
        MonthlyTotalAssets,
        MonthlyTotalLiabilities,
        MonthlyTotalEquities
    }

    public enum WidgetStatusEnum
    {
        InActive = 0,
        Active,
        Deleted
    }
    public enum WidgetTypeEnum
    {
        PreDefined = 0,
        Analysis,
        Trends
    }

    public enum PayrollDepartmentEnum
    {
        AvailableRooms = 1,
        OccupancyRooms = 2,
        HoursPerMonth = 3
    }

    public enum PreDefinedWidgetEnum
    {
        /* My Daily Review grapth */
        SalesPropertyOverview = 1,
        CashCardDueBalances = 2,
        ReceivablesAndAdvances = 3,
        PayablesView = 4,
        FlexAnalysis = 5,

        //TopDuesByVendor = 6,

        /* My Daily Review MC */
        Performance = 7,
        DepartmentalLayout = 8,
        ExpenseAnalysis = 9,

        /* Financial KPI's Grapth */
        BalanceSheetGraph = 10,
        ProfitandLossView = 11,
        AssetLiabilityGraph = 12,

        /* Financial KPI's MC */
        BalanceSheetGrid = 13,
        ProfitandLossTableView = 14,
        APAging = 15,
        ARagingTableView = 16,

        /* Labour graph */
        RevenueVSPayrollvsOccupancy = 17,
        //PayrollDepartments = 18,
        PayrollDepartmentsView = 18,
        LabourAnalysisByCategory = 19,
        PayrollCostperAR = 20,
        PayrollCostperOR = 21,
        PayrollHoursPerMonth = 22,

        /* Labour MC */
        LaborAnalysisGrid = 23,

        /* My Daily Review grapth */
        STR = 24,
        GuestReviews = 25,

        /* STR & GuestReviews Graph */
        STRAnalytics = 26,
        STRKPIs = 27,

        /* STR & GuestReviews MC */
        STRList = 28,
        GuestReviewsGrid = 29,
        //labour cost by Department & Client
        LaborCostByDepartment=30,
        LabourCostByClient=31,
        CashandCardTypesPage = 32,
        OTBWidget=33,
        OTBPickupWidget=34,
        OTBGrid=35,
        OTBGridPickup=36,
        //Management = 23,

        /// <summary>
        /// This is using to identify the widget as default custom widget (i.e type= 99)
        /// </summary>
        DefaultCustomWidgets = 99,
    }

    /// <summary>
    /// Widget groups
    /// </summary>
    public enum WigetGroupTypeEnum
    {
        [Display(Name = "My Daily Review")]
        DailyReview = 1,

        [Display(Name = "Financial KPI's")]
        FinancialKPIs,

        [Display(Name = "Labour")]
        Labour,

        [Display(Name = "Predictive Analytics")]
        Predictive,

        [Display(Name = "STR & Guest Reviews")]
        STR

        
    }
    public enum WigetViewTypeEnum
    {
        [Display(Name = "Graphical View")]
        GraphicalView = 1,

        [Display(Name = "Table View")]
        TableView = 2,
    }

    public enum WidgetColumns
    {
        [Display(Name = "Hours/Stats")]
        Hours_Stats = 1,
        [Display(Name = "Amount")]
        Amount = 2,
        [Display(Name = "% Income")]
        Income = 3,
        [Display(Name = "POR")]
        POR = 4,
        [Display(Name = "PAR")]
        PAR = 5
    }

    public enum DayMonthAndYearEnumForDepartmentalGraph
    {
        Day,
        Month,
        Year
    }
    public enum DepartmentGraphTypeEnum
    {
        Actual,
        Budget,
        Forecast,
        VsLY
    }

    public enum TrendsEnum
    {
        [Display(Name = "LY")]
        VsLY = 1,
        [Display(Name = "Budget")]
        VsBudget = 2,
        [Display(Name = "Forecast")]
        VsForecast = 3
    }

    public enum AnalysisFilterEnum
    {
        [Display(Name = "Vs LY")]
        VsLY = 1,
        [Display(Name = "Vs Budget")]
        VsBudget = 2,
        [Display(Name = "Vs Forecast")]
        VsForecast = 3

    }
    public enum ReconAccountTypeEnum
    {
        All = 0,
        Bank = 2,
        CC = 12
    }
    

    public enum Types
    {
        Status = 3
    }
    public enum BalanceSheetTypeEnum
    {
        Assets,
        Liability,
        Equity
    }
    public enum PandLTypeEnum
    {
        Income,
        Expense,
        [Display(Name = "Net Income")]
        NetIncome
    }



    public enum CustomTrendsEnum
    {
        [Display(Name = "Income Statement Groups")]
        IncomeStatementGroups=1,
        [Display(Name = "Payroll Departments")]
        PayrollDepartments = 3,
        [Display(Name = "P and L Chart of Accounts")]
        PandLChartOfAccounts = 4,
        [Display(Name = "Stats & Others")]
        StatsandOthers = 5,
        [Display(Name = "Balance Sheet Groups")]
        BalanceSheetGroups,
        [Display(Name = " Chart Of Accounts,")]
        ChartOfAccounts,

    }

    public enum CustomAnalysisEnum
    {
        [Display(Name = "Income Statement Groups")]
        IncomeStatementGroups = 0,
        [Display(Name = "Chart of Accounts")]
        ChartOfAccounts = 4,
        [Display(Name = "Payroll Departments")]
        PayrollDepartments = 2,
    }
    public enum AnalysisOrTrendsEnum
    {
        Analysis = 1,
        Trends = 2,
        Default = 99
    }

    public enum CustomWidgetsStatusEnum
    {
        Active = 1,
        Delete = 3
    }


    public enum SubTypeEnum
    {
        Total = 3,
        Profit,
        [Display(Name = "ADR")]
        ADR,
        [Display(Name = "Occupancy%")]
        Occupancy,
        [Display(Name = "RevPar")]
        RevPar,
        RoomsSold,
        RoomsAvailable,
        Vacant,
        Comp,
        OutofOrder,
        [Display(Name = "Total Cost Per Occupied Room")]
        TotalCostPerOccupiedRoom,
        [Display(Name = "Total Cost Per Available Room")]
        TotalCostPerAvailableRoom,
        [Display(Name = "Payroll Cost Per Occupied Room")]
        PayrollCostPerOccupiedRoom,
        [Display(Name = "Payroll Cost Per Available Room")]
        PayrollCostPerAvailableRoom,
        Income = 20,
        COGS,
        Expense,
    }

    public enum MonthOrDayEnum
    {
        Day = 0,
        Month = 1,
        Quaterly= 2,
        Yearly=3
    }

    public enum ValuesType
    {
        Actual = 1,
        Budget = 2,
        Forecast = 3,
        LY=4,
        ActualTotal=5,
        BudgetTotal=6,
        ForecastTotal=7, 
        LYtotal=8, 
    }

    public enum ConfigurationEnum
    {
        [Display(Name = "Income Sheet Configurations")]
        IncomeSheetConfiguration = 1,
        [Display(Name = "Payroll Configurations")]
        PayrollConfigurations
    }
    public enum PayrollDepartmentsEnum
    {
        [Display(Name = "All Payroll Expenses")]
        AllPayrollExpense=1,
        [Display(Name = "Salaries & Wages")]
        SalariesandWages,
        [Display(Name = "Payroll Taxes")]
        PayrollTaxes,
        [Display(Name = "Employee Benefits")]
        EmployeeBenefits
    }

    #region STR
    public enum STRAnalyticsByEnum
    {
        [Display(Name = "Week")]
        Week = 0,
        [Display(Name = "Weekly")]
        Weekly = 1,
        [Display(Name = "Month")]
        Month,
        [Display(Name = "Monthly")]
        Monthly,
        [Display(Name = "Yearly")]
        Yearly,
        [Display(Name = "Range")]
        Range,
    }

    public enum STRGroupEnum
    {
        Occupancy,
        ADR,
        RevPar
    }

    public enum STRMonths
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public enum YearlyViewEnum
    {
        [Display(Name = "2 Years")]
        TwoYears=2,
        [Display(Name = "3 Years")]
        ThreeYears=3,
        [Display(Name = "6 Years")]
        SixYears=6,
        [Display(Name = "12 Years")]
        TwelveYears=12,
    }

    public enum STRWeeklyEnum
    {
        [Display(Name = "Week 1")]
        Week1,
        [Display(Name = "Week 2")]
        Week2,
        [Display(Name = "Week 3")]
        Week3,
        [Display(Name = "Week 4")]
        Week4,
        [Display(Name = "Week 5")]
        Week5,
        [Display(Name = "Week 6")]
        Week6,
        [Display(Name = "Week 7")]
        Week7,
        [Display(Name = "Week 8")]
        Week8,
        [Display(Name = "Week 9")]
        Week9,
        [Display(Name = "Week 10")]
        Week10,
        [Display(Name = "Week 11")]
        Week11,
        [Display(Name = "Week 12")]
        Week12
    }


    #endregion

    public enum FilterMonthEnum
    {

        [Display(Name = "-All-")]
        all = 0,

        [Display(Name = "Jan")]
        Jan,

        [Display(Name = "Feb")]
        Feb,

        [Display(Name = "Mar")]
        Mar,

        [Display(Name = "Apr")]
        Apr,

        [Display(Name = "May")]
        May,

        [Display(Name = "Jun")]
        Jun,

        [Display(Name = "Jul")]
        Jul,

        [Display(Name = "Aug")]
        Aug,

        [Display(Name = "Sep")]
        Sep,

        [Display(Name = "Oct")]
        Oct,

        [Display(Name = "Nov")]
        Nov,

        [Display(Name = "Dec")]
        Dec
    }

    public enum STRFileType
    {
        [Display(Name = "Monthly")]
        Monthly = 1,

        [Display(Name = "Weekly")]
        Weekly = 2,

        [Display(Name = "Both")]
        Both = 3
    }

    public enum STRReportDataViewType
    {

        Day,
        Week,
        Month,
        Year
    }

    public enum STRGroupOptions
    {
        [Display(Name = "My Property")]
        MyProperty,

        [Display(Name = "Competitive Set")]
        CompSet,

        [Display(Name = "Index (MPI)")]
        Index,

        [Display(Name = "Rank")]
        Rank
    }
    public enum STRReportDataType
    {
        [Display(Name = "My Property")]
        MyProperty,
        [Display(Name = "Index (ARI)")]
        IndexARI,
        [Display(Name = "Comp Set")]
        CompSet,
        [Display(Name = "Your rank")]
        Rank,
        [Display(Name = "Index (MPI)")]
        IndexMPI,
        [Display(Name = "Index (RGI)")]
        IndexRGI,
        [Display(Name ="ADR")]
        AdrMarketScale,
        [Display(Name ="Occ")]
        OccMarketScale,
        [Display(Name= "RevPAR")]
        RevParMarketScale
    }


    public enum BalSheetColNamesEnum
    {
        [Display(Name = "Corporation")]
        Corporation,
        [Display(Name = "Legal Name")]
        LegalName,
        [Display(Name = "Cash & Bank")]
        CashBank,
        [Display(Name = "Recievables")]
        Recievables,
        [Display(Name = "Total Assets")]
        TotalAssets,
        [Display(Name = "Payables")]
        Payables,
        [Display(Name = "Long Term Liab")]
        LongLiab,
        [Display(Name = "Net Income")]
        NetIncome,
        [Display(Name = "Total Liabilities")]
        TotalsLiab

    }

    public enum WidgetIDEnum
    {
        BalanceSheetGrid = 23,

    }

    public enum ExcelExportEnum
    {
        FontColor,
        ColumnTotalSum,
        ProfitLossCustomFormula,
        ProfitLossLyOrBudgetFormula,
        AddColumnsWiseSum,
        AddColAndRowSum,
        AsOfDate,
        SortBy,
        IsMoreThanOneFile
    }

    public enum DashboardWidgetsEnum
    {
        BalanceSheetGrid = 13,
        APAging = 15,
        ARAging = 16,
        ProfitAndLoss = 14,
        Performance = 7,
        LaborAnalysisByCorp = 23,
        OTBPerformance=35,
        OTBPickup=36,
    }

    public enum PerformanceColNamesEnum
    {
        [Display(Name = "Corporation Legal/DBA")]
        Corporation,
        [Display(Name = "Brand")]
        Brand,
        [Display(Name = "Rooms Sold")]
        RoomsSold,
        [Display(Name = "Occupancy %")]
        Occupancy,
        [Display(Name = "ADR")]
        ADR,
        [Display(Name = "RevPAR")]
        RevPAR,
        [Display(Name = "Room Revenue")]
        RoomRevenue,
        [Display(Name = "Total Revenue")]
        TotalRevenue,
    }
    public enum LabourAnalysisByCorpColumnNamesEnum
    {
        [Display(Name = "Corporation Legal/DBA")]
        Corporation,
        [Display(Name ="Hours")]
        Hours,
        [Display(Name ="Payroll Expense")]
        PayrollExpense,
        [Display(Name ="% of Income")]
        PercentageOfIncome,
        [Display(Name ="POR")]
        POR,
        [Display(Name ="PAR")]
        PAR
    }

    public enum ProfitLossColumnNamesEnum
    {
        [Display(Name = "Corporation")]
        Corporation,
        [Display(Name = "Legal Name")]
        LegalName,
        [Display(Name = "Income")]
        Income,
        [Display(Name = "COGS")]
        Cogs,
        [Display(Name = "Gross Income")]
        GrossIncome,
        [Display(Name = "GOP%")]
        GOP,
        [Display(Name = "Expense")]
        Expense,
        [Display(Name = "OtherInc/Exp")]
        OtherIncExp,
        [Display(Name = "NetIncome")]
        NetIncome,
        [Display(Name = "Vs Budget Var")]
        VsBudgetVar,
        [Display(Name = "Vs LY Var")]
        VsLYVar
    }

    public enum APAgingColNamesEnum
    {
        [Display(Name = "Corporation")]
        Corporation,
        [Display(Name = "[Current]")]
        Current,
        [Display(Name = "[1-30]")]
        Thirty,
        [Display(Name = "[31-60]")]
        GreaterThiry,
        [Display(Name = "[61-90]")]
        GreaterSixty,
        [Display(Name = "[>90]")]
        GreaterNinty,
        [Display(Name = "[Total]")]
        Total,

    }

    public enum ARAgingColNamesEnum
    {
        [Display(Name = "Corporation")]
        Corporation,
        [Display(Name = "Advance Deposit Ledger")]
        AdvanceDepositLedger,
        [Display(Name = "City Ledger Net")]
        CityLedgerNet,
        [Display(Name = "Guest Ledger")]
        GuestLedger,
        [Display(Name = "Governor Legder Net")]
        GovernorLegderNet,
        [Display(Name = "State Ledger Net")]
        StateLedgerNet,
        [Display(Name = "Package Ledger")]
        PackageLedger,
        [Display(Name = "Total")]
        Total,

    }


    public enum WidgetBalanceType {

        [Display(Name = "As of Today")]
        AsofToday=0,
        [Display(Name = "Month")]
        Month=1
    }

    public enum WidgetCreditCardTypes
    {
        [Display(Name = "Amex")]
        Amex = 1,
        [Display(Name = "Visa & MasterCard")]
        VisaandMasterCard,
        [Display(Name = "Discover")]
        Discover,
        [Display(Name = "Other Cards")]
        OtherCards,
        [Display(Name = "Cash & Checks")]
        CashandChecks,
    }
}
