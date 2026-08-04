using Common.Domain.DTO.App;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class LabourAnalysisResponse : StatusDTO
    {
        public LabourAnalysisResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<LabourDetails> LabourDetails { get; set; } = new List<LabourDetails>();
    }

	public class PayrollCostRoomsORHoursResponse : StatusDTO
	{
		public PayrollCostRoomsORHoursResponse()
		{
			Status = Constants.MSG_NO_DATA_FOUND;
			StatusCode = StatusCodes.Status204NoContent;
		}
		public List<PayrollStatsDetails> PayrollStatsDetails { get; set; } = new List<PayrollStatsDetails>();
	}

    public class PayrollStatsDetails
    {
        public string PayrollName { get; set; }
        public decimal Budget { get; set; }
        public decimal Actual { get; set; }
        public string Month { get; set; }
    }

    public class LabourDetails: CorporationInfo
    {
        public string DepartmentName { get; set; }
        public string? JobTitle { get; set; }
        public StatisticDetails Hours { get; set; }
        public StatisticDetails PayrollExpense { get; set; }
        public StatisticDetails PercOfIncome { get; set; }
        public StatisticDetails POR { get; set; }
        public StatisticDetails PAR { get; set; }

    }

    public class IncomeDepartments:StatusDTO
    {
        public List<IncomeDepartmentDbResponse> Incomedepartdetails {  get; set; }= new List<IncomeDepartmentDbResponse>(); 

    }
    public class  IncomeDepartmentDbResponse
    {
        public Int32 DepartmentId {  get; set; }    
        public string DepartmentName { get; set;}
        
    }
    public class LabourAnalysisDbResponse: CorpNames
    {
        public string DepartmentName { get; set;}   
        public decimal  ActualHours {  get; set; }   
        public decimal  BudgetHours {  get; set; }   
        public decimal  LyHours {  get; set; }   
        public decimal ForecastHours {  get; set; } 
        public decimal ActualPayrollExpense {  get; set; }   
        public decimal BudgetPayrollExpense { get; set; }   
        public decimal LyPayrollExpense { get; set; }   
        public decimal ForecastPayrollExpense { get; set; }    
        public decimal ActualPercentIncome {  get; set; }   
        public  decimal BudgetPercentIncome { get; set; }   
        public decimal ForecastPercentIncome { get; set; }  
        public decimal LyPercentIncome { get;set; }
        public decimal ActualPOR {  get; set; } 
        public decimal BudgetPOR { get;set; }
        public decimal ForecastPOR { get; set;} 
        public decimal LyPOR {  get; set; } 
        public decimal ActualPAR {  get; set; } 
        public decimal BudgetPAR { get;set; }   
        public decimal ForecastPAR { get; set;}
        public decimal LyPAR { get; set;}

    }
    public class PayrollDepatsResponse:StatusDTO
    {
        public decimal TotalActual { get; set; }
        public decimal TotalBudget { get; set; }
        public List<KeyValuePairObject<string, StatndardComparisonStatistics>> PayrollDepts { get; set; } = new List<KeyValuePairObject<string, StatndardComparisonStatistics>>();
    }

    public class PayrollDepartsDbResponse
    {
        public Int32 DepartmentID {  get; set; }  
        public string DepartmentName {  get; set; }  
        public decimal PayrollDepartmentActual {  get; set; }   
        public decimal PayrollDepartmentBudget {  get; set; } 
    }
    public class RevenuePayrollOccupancyResponse:StatusDTO
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalPayrollExpense {  get; set; }   
        public decimal TotalPayrolExpPer {  get; set; } 
        public decimal TotalOccupancyPer {  get; set; }
        public List<KeyValuePairObject<string, RevePayrolOccupancyGraphResponse>> CompareResponse { get; set; } = new List<KeyValuePairObject<string, RevePayrolOccupancyGraphResponse>>(); 

    }
    public class RevePayrolOccupancyGraphResponse
    {
        public decimal Revenue { get; set; }
        public decimal PayrollExpense { get; set; }
        public decimal PayrollExpensePer {  get; set; } 
        public decimal OccupancyPer { get; set; }   
    }
    public class RevePayrolOccupancyGraphDbResponse
    {
        public string Name {  get; set; }   
        public string MonthName {  get; set; }  
        public decimal Amount { get; set; }  
        public decimal TotalRevenue { get; set; }   
        public decimal TotalPayrollExpense { get; set; }
        public decimal TotalPayrollExpensePercentage { get;set; }
        public decimal TotalOccupancy {  get; set; }    


    }
    public class PayrollCostDbResponse
    {
        public string MonthName { get; set; }   
        public decimal PORActual {  get; set; } 
        public decimal PORBud {  get; set; }    
        public decimal PARActual {  get; set; } 
        public decimal PARBud { get; set;}
        public decimal ActualHours {  get; set; } 
        public decimal BudHours {  get; set; } 
    }
    public class CheckIncomconfigResponse: StatusDTO
    {
        public string CorporationID { get; set; }
        public Int32 IsConfigurationAvailable { get;set; }
        public Int32 PropertyType {  get; set; }    
    }
}
