using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
   public  class IncomestmntGroupResponse:StatusDTO
    {
        public List<IncometmntGroups>IncometmntGroups { get; set; } =new List<IncometmntGroups>();

    }
    public class IncometmntGroups
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public short Type { get;set; }
        public short GroupType { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public decimal GroupIncome {  get; set; }
        public decimal GroupBudIncome {  get; set; }    
        public decimal GroupLyIncome {  get; set; } 
        public int stas {  get; set; }  
        public int Lystats {  get; set; }   
        public int BudStats {  get; set; }  
        public decimal ActualPOR {  get; set; } 
        public decimal BudPOR {  get; set; }    
        public decimal LyPOR { get; set; }
        public decimal ActualPAR { get; set; }
        public decimal BudPAR { get; set; }
        public decimal LyPAR { get; set; }
        public decimal PerIncome {  get; set; } 
        public decimal BudPerIncome {  get; set; }  
        public decimal LyPerIncome { get; set; }    

        public List<ChartOfAccounts> Accounts { get; set; } = new List<ChartOfAccounts>();
    }
    public class PayrollDeparmentsResponse:StatusDTO
    {
        public List<PayrollDepartmnets> PayrollDepartmnets { get; set; } = new List<PayrollDepartmnets>();
    }
    public class PayrollDepartmnets
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public short Type { get; set; } 
        public string JobTitle { get; set; }
        public decimal DeptAmount { get; set; }
    }


    public class CustFormulaResponse:StatusDTO
    {
        public List<CustFormulaDbResponse> DbResponse { get; set; }=new List<CustFormulaDbResponse> ();
    }
    public class CustFormulaDbResponse
    {
        public Int64 CustomizeKey { get; set; }
        public string SourceBinID { get; set; }
        public string Name { get;set; }
        public string GroupName { get; set; }
        public Int16 Type { get; set; }
        public Int16 SUbType {  get; set; } 
        public  string AccountNumber {  get; set; } 
        public string AccountName {  get; set; }  
        public decimal ConvertedAccountNumber { get; set; }
        public Int32 ChartofAccountsCount {  get; set; }
        public Int32 IncDeptID {  get; set; }   
        public Int32 RevorIncExp {  get; set; }
        public Int64 UniqID { get; set; }
    }

    public class IncomeStatementGroupsDbResponse:StatusDTO
    {
        public string Group { get; set; }
        public string DeptName {  get; set; }   
        public Int32 COACount {  get; set; }    
      //  public Int64 CorpKey {  get; set; } 
        public Int32 Type {  get; set; }    
        public Int32 Subtype { get;set; }
        public Int32 revexptype {  get; set; }  
        public Int32 GroupID2 { get; set;}
        public Int32 GroupID1 { get;set; }
        public Int64 GroupID {  get; set; } 
        public Int64 DeptID {  get; set; }  
        public Int32 DepartmentID1 { get;set; }
        public Int64 uniqueid { get; set; }
        // public Int64 Uniqueid {  get; set; }    

    }
    public class IncomeGroupAccounts
    {
        public string AccountName { get; set; }
        public string DepartMentName { get; set; }
        public string Group { get; set; }
        public Int32 AccountType {  get; set; } 
        public string AccountTypeName {  get; set; }    
    }
    public class IncomeGroupsAccountsList :StatusDTO
    {
        public List<IncomeGroupAccounts> Dbresponse { get; set; } = new List<IncomeGroupAccounts>();
    }
    public class IncomeGroupsResponse:StatusDTO
    {
        public List<IncomeStatementGroupsDbResponse> Dbresponse { get; set; } = new List<IncomeStatementGroupsDbResponse>();    
    }
    public class IncomeStatementSubDepartmentsResponse: IncomeStatementGroupsDbResponse
    {
        
    }
   
    public class IncomeStatementDepartmentWiseResonse : StatusDTO
    {
        public List<DepartmentWiseResonse> DepartmentWiseRes {  get; set; }
    }
    public class DepartmentWiseResonse
    {
        public string CorpID { get; set; }
        public string CorpName { get; set;}
        public string Brand
        {
            get; set;
        }
        public decimal RoomRevenue { get; set; }
        public decimal FBRevenue { get; set; }
        public decimal OtherRevenue { get; set; }
        public decimal DepExpense {  get; set; }
       public decimal UndistExpense  { get; set;}
        public decimal OtherExpense { get; set; }
        public decimal GOP {  get; set; }
        public decimal GOPPercentage { get; set; }
        public decimal NetIncome { get; set; }
        public decimal LYRoomRevenue { get; set; }
        public decimal BudRoomRevenue { get; set; }
        public decimal FCRoomRevenue { get; set; }
        public decimal LYFBRevenue { get; set; }
        public decimal BudFBRevenue { get; set; }
        public decimal FCFBRevenue { get; set; }
        public decimal LYOtherRevenue { get; set; }
        public decimal BudOtherRevenue { get; set; }
        public decimal FCOtherRevenue { get; set; }
        public decimal LYDepExpense { get; set; }
        public decimal BudDepExpense { get; set; }
        public decimal FCDepExpense { get; set; }
        public decimal LYUndistExpense { get; set; }
        public decimal BudUndistExpense { get; set; }
        public decimal FCUndistExpense { get; set; }
        public decimal LYOtherExpense { get; set; }
        public decimal BudOtherExpense { get; set; }
        public decimal FCOtherExpense { get; set; }

        public decimal LYGOP { get; set; }
        public decimal LYGOPPercentage { get; set; }
        public decimal LYNetIncome { get; set; }
        public decimal BudGOP { get; set; }
        public decimal BudGOPPercentage { get; set; }
        public decimal BudNetIncome { get; set; }
        public decimal FCGOP { get; set; }
        public decimal FCGOPPercentage { get; set; }
        public decimal FCNetIncome { get; set; }
    }
}
