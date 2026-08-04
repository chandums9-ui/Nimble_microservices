using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Model
{
    /// <summary>
    /// Base widget previledge for the user 
    /// </summary>
    public class WidgetPrivilegeDTO
    {
        /// <summary>
        /// Unique identifier for the WidgetPrivilege.
        /// </summary>
        public long Id { get; set; }

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
            public bool? IsAddwidget { get; set; }

        /// <summary>
        /// Indicates whether the Privilege includes the option to Export
        /// </summary>
        public bool? IsExport { get; set; }
        /// <summary>
        /// Indicates whether the Privilege includes the option to Schedule
        /// </summary>
        public bool? IsSchedule { get; set; }

        /// <summary>
        /// Widget leval privileges
        /// </summary>
        public long TotalCount { get; set; } = 0;
        public long WidgetCount { get; set; } = 0;
        public long UserInfoID { get; set; } = 0;
        public long ClientInfoID { get; set; } = 0;

        /// <summary>
        /// Individual widget privileges
        /// </summary>
        public List<WidgetPrivilegeDetailsDTO> PrivilegeDetails { get; set; } = new List<WidgetPrivilegeDetailsDTO>();
    }
}
