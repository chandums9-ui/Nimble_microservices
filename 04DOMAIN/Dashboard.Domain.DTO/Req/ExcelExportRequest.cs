using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class ExcelExportRequest
    {
        public string FileName { get; set; }

        public List<ColumnDto> Columns { get; set; }

        public List<List<DataDto>> Data { get; set; } = new List<List<DataDto>>();


        public DateTime? FromDate
        {
            get
            {
                DateTime result;
                return (!string.IsNullOrEmpty(ShortFromDate) && DateTime.TryParse(ShortFromDate, out result)) ? new DateTime?(result) : null;
            }
        }

        public DateTime? ToDate
        {
            get
            {
                DateTime result;
                return (!string.IsNullOrEmpty(ShortToDate) && DateTime.TryParse(ShortToDate, out result)) ? new DateTime?(result) : null;
            }
        }

        public string ShortFromDate { get; set; } = string.Empty;


        public string ShortToDate { get; set; } = string.Empty;


        public string Filter { get; set; }

        public string ManagementGroup { get; set; }

        public string CorporationName { get; set; }

        public string ProfitCenterName { get; set; }

        public string BasedOnName { get; set; }

        public int AddFormula { get; set; }

        public int DateFormate { get; set; }

        public int ColumnFreezeNumber { get; set; }

        public PandLDetails PandLDetails { get; set; } = new PandLDetails();


        public PerformenceDetails PerformenceDetails { get; set; } = new PerformenceDetails();


        public bool IsFirstRequest { get; set; } = true;


        public bool IsCombinationOfExport { get; set; } = false;


        public BalanceSheetDetails BalanceSheetDetails { get; set; } = new BalanceSheetDetails();


        public ARAging ARagingDetails { get; set; }
    }


    public class ColumnDto
    {
        public string Item1 { get; set; }
        public int? Item2 { get; set; }
    }

    public class DataDto
    {
        public object? Item1 { get; set; }
        public object? Item2 { get; set; }
    }

    public class PandLDetails
    {
        public decimal? VsLyGopVar { get; set; } = null;

        public decimal? VsBudgetGopVar { get; set; } = null;
    }

    public class PerformenceDetails
    {
        public string SortBy { get; set; } = null;

        public bool IsPerfomanceLastRow { get; set; } = false;
    }

    public class BalanceSheetDetails
    {
        public bool IsBackGroundColor { get; set; } = false;
    }

    public class ARAging
    {
        public int IsMoreThanOneFile { get; set; } 
    }
}
