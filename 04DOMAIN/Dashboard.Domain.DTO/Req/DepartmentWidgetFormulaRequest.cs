using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class DepartmentWidgetFormulaRequest
    {
        string CustomLabel { get; set; }
        short Order { get; set; }
        string DeptType { get; set; }

        string Formula { get; set; }

        string GroupFor { get; set; }

        string ClientId { get; set; }

        string UserId { get; set; }
    }
}
