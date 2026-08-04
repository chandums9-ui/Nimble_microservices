namespace Dashboard.Domain.DTO.Resp
{
    public class DepartmentDBIncomeResponse
    {
        public string CorpDBAName { get; set; }
        public string CorpLegalName { get; set; }
        public Int64 CorpKey { get; set; }
        public string BrandName { get; set; }

        public Int64 DeptID { get; set; }

        public Int64 SubDeptID { get; set; }

        public string DepartmentName { get; set; }

        public Int32 DeptOrder { get; set; }

        public string SubDepartmentName { get; set; }

        public Int32 SubDepartmentOrder { get; set; }

        public decimal Amount { get; set; }

        public decimal ComparedAmount { get; set; }

        public Int32 DeptType { get; set; }

        public Int32 Type { get; set; }

        public Int32 DPType { get; set; }
    }
}
