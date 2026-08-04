using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class STRGridImportListDTO : StatusDTO
    {
        public string str_id { get; set; }

        public List<Report> WeeklyReport { get; set; } = new List<Report>();

        public List<Report> MonthlyReport { get; set; } = new List<Report>();

        public List<Report> MonthAndWeekReports { get; set; } = new List<Report>();
    }
}
