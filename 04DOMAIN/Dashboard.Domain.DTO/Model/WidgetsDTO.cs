using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Dashboard.Domain.DTO.Enums;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Model
{
    public class WidgetsList
    {
        public List<WidgetsDTO> widgets { get; set; } = new List<WidgetsDTO>();
    }
    public class WidgetsDTO
    {
        /// <summary>
        ///  Unique identifier for the widgetID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Name of the widget
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the widget (WidgetTypeEnum)
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// Predefined widget type id (PreDefinedWidgetEnum)
        /// </summary>
        public int? SubType { get; set; }

        /// <summary>
        /// Widget predefined type name
        /// </summary>
        public string SubTypeName
        {
            get
            {
                return (this.SubType.HasValue && this.SubType > 0) ? EnumExtensions.GetEnumValue<PreDefinedWidgetEnum>(this.SubType.Value).ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Group of the widget it belongs to (WigetGroupTypeEnum)
        /// </summary>
        public long GroupType { get; set; }

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
        /// Indicates if the rows in the widget are customized or not (default: false)
        /// </summary>
        public bool IsRowsCustomized { get; set; }

        /// <summary>
        /// Indicates if the columns in the widget are customized (default: false)
        /// </summary>
        public bool IsColumnsCustomized { get; set; }

        /// <summary>
        ///  It is used in UI to display the Widget in a row with the specific width (default: 12)
        /// </summary>
        public short DisplayWidth { get; set; }

        /// <summary>
        /// Default date filter type applied to the widget's data
        /// </summary>
        public int? DefaultFilterType { get; set; }

        /// <summary>
        /// Default WidgetDateFilters
        /// </summary>
        public string DefaultComparisionType { get; set; }

        /// <summary>
        /// Default view type for dates in the widget (1: Daily, 2: Monthly, 3: Quarterly, 4: Yearly)
        /// </summary>
        public short? DefaultDateViewType { get; set; }

        /// <summary>
        /// Order in which the widget should be sorted or displayed
        /// </summary>
        public short SortOrder { get; set; }

        /// <summary>
        /// Status indicator for the widget  (WidgetStatusEnum)
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// It is used in UI, Indicates whether a widget is selected or not
        /// </summary>
        public bool? IsWidgetSeleted { get; set; } = false;
        public WidgetUserSettingsResponse WidgetUserSettings { get; set; }

        //public WidgetPrivilegeBaseDTO widgetPrivileges { get; set; }
        public CustomWidgetFormulaDetails customWidgetFormulaDetails { get; set; }

        public string SelectedPerformanceVarType { get; set; } = "Rooms Sold"; // default
        public string SelectedSortBy { get; set; } = "Corporation";           // default
    }

    public class CustomWidgetFormulaDetails
    {
        public long WidgetId { get; set; }
        public Dictionary<long, string> FormulaDetails { get; set; } = new Dictionary<long, string>();
    }

}
