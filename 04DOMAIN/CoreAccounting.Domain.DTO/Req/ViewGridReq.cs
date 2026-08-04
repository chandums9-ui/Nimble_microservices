using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class ViewGridReq

    {
        public string Corporations { get; set; }
        public string SourceTypeList { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; }
        //public int ViewFilter { get; set; }
        public int SortBy { get; set; }
        public List<DropDownFilter> Dropdown { get; set; }
    }

    public class DropDownFilter

    {
        public short ColumnType { get; set; }
        public short OperationType { get; set; }
        public string Value { get; set; }
    }

     public class FilterSaveReq

     {
        public short? SourceType { get; set; }
        public List<DropDownFilter> Filters { get; set; }

     }
 }
