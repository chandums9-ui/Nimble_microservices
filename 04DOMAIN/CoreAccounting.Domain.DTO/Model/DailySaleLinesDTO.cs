using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Model
{
    public class DailySaleLinesDTO : ModelBaseIDString
    {
        public string LineID { get; set; }
        public string LineName { get; set; }
        public string DepartmentID { get; set; }
        public short DepartmentType { get; set; }
        public string? CreditAccountID { get; set; }
        public string? CreditAccount { get; set; }
        public string? DebitAccountID { get; set; }
        public string? DebitAccount { get; set; }
        public string CorporationName { get; set; }
        public string? StoreName { get; set; }
        public short MergeType { get; set; }
        public long DSOrder { get; set; }

    }
}
