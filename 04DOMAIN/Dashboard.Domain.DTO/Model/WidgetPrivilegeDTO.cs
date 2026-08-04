using Common.Domain.DTO.Enums;
using Dashboard.Domain.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Model
{
    /// <summary>
    /// Individual Widget privileges based on User or User Role
    /// </summary>
    public class WidgetPrivilegeDetailsDTO 
    {
        /// <summary>
        /// Unique identifier for the WidgetPrivilegeDetails.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Identifier for the associated widget in the widget table.
        /// </summary>
        public long WidgetID { get; set; }
        /// <summary>
        /// WidgetName
        /// </summary>
        public string WidgetName { get; set; }
        /// <summary>
        /// Group of the widget it belongs to (WigetGroupTypeEnum)
        /// </summary>
        public long GroupType { get; set; }

        public short Type {  get; set; }

        /// <summary>
        /// Widget Group Name
        /// </summary>
        public string GroupName
        {
            get
            {
                return (this.GroupType > 0) ? EnumExtensions.GetEnumValue<WigetGroupTypeEnum>(Convert.ToInt32(this.GroupType)).GetDisplayName() : string.Empty;
            }
        }

        /// <summary>
        /// Indicates whether the widget displays data in graphical or tabular format (WigetViewTypeEnum)
        /// </summary>
        public bool IsGraphicalOrTableView { get; set; }

        /// <summary>
        /// Subgroup name
        /// </summary>
        public short? SubGroup { get; set; }

        /// <summary>
        /// Wiget ViewType Name
        /// </summary>
        public string ViewName
        {
            get
            {
               
                return (this.GroupType > 0) ? EnumExtensions.GetEnumValue<WigetViewTypeEnum>(IsGraphicalOrTableView ? 1 : 2).GetDisplayName() : string.Empty;
            }
        }

        /// <summary>
        /// Widget details to get Name & Group information
        /// </summary>
        //public WidgetsDTO WidgetDTO { get; set; }
        /// <summary>
        /// Identifier for the widgetPrivilege ID .
        /// </summary>
        public long WidgetPrivID { get; set; }

        /// <summary>
        /// Indicates whether the privilege is enabled for viewing.
        /// </summary>
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// Indicates whether navigation (drill down) is allowed.
        /// </summary>
        public bool? IsNavigate { get; set; }
         
        /// <summary>
        /// Indicates whether the privilege includes the option to build formulas.
        /// </summary>
        public bool? IsFormulaBuilder { get; set; } 

        /// <summary>
        /// Indicates whether the privilege includes the option to delete.
        /// </summary>
        public bool? IsDelete { get; set; }
        public long SortOrder { get; set; }

        //public bool IsViewAccess { get; set; } = false;
        //public bool IsNavigateAccess { get; set; } = false;
        //public bool IsFormulaAccess { get; set; } = false;
        //public bool IsDeleteAccess { get; set;} = false;

    }
}
