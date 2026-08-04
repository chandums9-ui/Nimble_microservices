using Common.Domain.DTO.Model.Base;
using Dashboard.Domain.DTO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    /// <summary>
    /// It is to get the Widget details 
    /// </summary>
    public class WidgetRequest
    {
        public string LoginUserID {  get; set; }
        /// <summary>
        /// WigetGroupTypeEnum
        /// </summary>
        public int GroupType { get; set; } = 0;

        /// <summary>
        /// Widget view (Graphical | Table)
        /// </summary>
        public bool GraphicalOrTable { get; set; } = false;
        [DefaultValue(false)]
        public bool ArePrivilegesRequired { get; set; } = false;

        public string RoleId {  get; set; } 

        public string ClientID { get;set; }


    }
    public class AnalysisRequest
    {
        public long FormulaId { get; set; }
        
    }
}
