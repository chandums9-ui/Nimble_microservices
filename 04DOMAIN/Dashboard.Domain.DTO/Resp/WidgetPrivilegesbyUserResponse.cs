using Common.Domain.DTO.Model.Base.Contracts;
using Dashboard.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class WidgetPrivilegesbyUserResponse 
    {
        /// <summary>
        /// Identifier for the userID OR RoleID
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// Identiter for the SourceType 0 is UserID and 1 is RoleID
        /// </summary>
        public short SourceType { get; set; }

        /// <summary>
        /// Indicates whether the privilege is enabled for Addwidget
        /// </summary>
        public bool IsAddwidget { get; set; } = false;

        /// <summary>
        /// Indicates whether the Privilege includes the option to Export
        /// </summary>
        public bool IsExport { get; set; } = false;
        /// <summary>
        /// Indicates whether the Privilege includes the option to Schedule
        /// </summary>
        public bool IsSchedule { get; set; } = false;
        public List<WidgetPrivilegesByuser> WigetPrivileges { get; set; } = new List<WidgetPrivilegesByuser>();
    }

    public class WidgetPrivilegesByuser
    {
        /// <summary>
        /// Unique identifier for the WidgetPrivilegeDetails.
        /// </summary>
        public long WidgetID { get; set; }
        public string WidgetName { get; set; }

        /// <summary>
        /// Indicates whether the privilege is enabled for viewing.
        /// </summary>
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// Indicates whether navigation (drill down) is allowed.
        /// </summary>
        public bool IsNavigate { get; set; } = false;

        /// <summary>
        /// Indicates whether the privilege includes the option to build formulas.
        /// </summary>
        public bool IsFormulaBuilder { get; set; } = false;

        /// <summary>
        /// Indicates whether the privilege includes the option to delete.
        /// </summary>
        public bool IsDelete { get; set; } = false;
    }
}
