using Common.Domain.DTO.Model.Base;
using System.Diagnostics.CodeAnalysis;

namespace Dashboard.Domain.DTO.Resp
{
    public class DepartmentalIncomeResponse : StatusDTO
    {

        public List<CorporationDepartment> CorporationDepartments { get; set; } = [];

        public List<DepartmentType> Departments { get; set; }

        public List<BaseDepartment> ClientdepartmentList { get; set; }
    }

    public class CorporationDepartment
    {
        public string CorpDBAName { get; set; }
        public string CorpLegalName { get; set; }
        public long CorpKey { get; set; }

        public string BrandName { get; set; }

        public List<DepartmentalIncome> DepartmentalIncomeData { get; set; } = [];
        public decimal Total { get; set; }
    }

    public class BaseDepartment
    {
        public string Department { get; set; }

        public string SubDepartment { get; set; }

        public int DeptType { get; set; }

        public int Type { get; set; }

        public int? DPType { get; set; }

        public int? DeptOrder { get; set; }

        public int? SubDeptOrder { get; set; }
    }

    public class DepartmentType : IEqualityComparer<DepartmentType>
    {
        public long ID { get; set; }

        public int? DPType { get; set; }

        public int? DeptOrder { get; set; }

        public int? SubDeptOrder { get; set; }

        public string Department { get; set; }

        public string SubDepartment { get; set; }

        public string SubDepartmentWithSuffix { get; set; }

        public short Type { get; set; }


        public short SubType { get; set; }

        public bool Equals(DepartmentType x, DepartmentType y)
        {
            return x != null && x.Type == y.Type && x.Department == y.Department && x.SubDepartment == y.SubDepartment && x.ID == y.ID && x.SubType == y.SubType;
        }

        public int GetHashCode(DepartmentType obj)
        {
            return HashCode.Combine(obj.Department, obj.Type, obj.DPType, obj.ID, obj.SubType, obj.SubDepartment);
        }
    }

    public class DepartmentalIncome
    {
        public string DepartmentName { get; set; }

        public string SubDepartmentName { get; set; }

        public string SubDepartmentNameWithSuffix { get; set; }

        public double Amount { get; set; } = 0;

        public double CompareAmount { get; set; }

        public int? DeptType { get; set; }

        public int? Order { get; set; }

        public int Type { get; set; }

        public int? DPType { get; set; }
    }

    public class DepartmentalIncomeDetails
    {
        public string CorpDBAName { get; set; }
        public string Brand { get; set; }
        public decimal RoomRevenue { get; set; }

        public decimal FBRevenue { get; set; }

        public decimal OtherRevenue { get; set; }

        public decimal DepExpense { get; set; }

        public decimal UndistriExp { get; set; }

        public decimal OtherExp { get; set; }
        public decimal GOP { get; set; }
        public decimal PercentGOP { get; set; }
        public decimal NetIncome { get; set; }

        public decimal Buget { get; set; }
        public decimal Ly { get; set; }
        public decimal Forecast { get; set; }
        public decimal Current { get; set; }
        public decimal TotalIncome { get; set; }

    }

}
