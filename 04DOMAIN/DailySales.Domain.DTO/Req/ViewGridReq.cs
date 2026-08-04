using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public class ViewGridReq
    {
        public string Corporations { get; set; }       
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<DropDownFilter> Dropdown { get; set; }
        public int PageNumber { get; set; }
        [DefaultValue(false)]
        public bool IsFromMobile { get; set; }
        public int SortBy { get; set; }

    }
    public class DropDownFilter

    {
        public short ColumnType { get; set; }
        public short Operator { get; set; }
        public string value { get; set; }
    }

    public class FilterSaveReq
    {
        public short? SourceType { get; set; }
        public List<DropDownFilter> Filters { get; set; }

    }
   

}
