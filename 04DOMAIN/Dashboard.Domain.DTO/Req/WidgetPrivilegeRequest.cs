using Common.Domain.DTO.Model.Base;
using Dashboard.Domain.DTO.Model;
//using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{

    public class WidgetPrivilegeRequest
    {
        /// <summary>
        /// Identifier for the userID or RoleID
        /// </summary>
        public string LoginUserID {  get; set; } 
        /// <summary>
        /// Identifier for the selecteduserID
        /// </summary>
        public string SelectedUserIDOrRoleID {  get; set; }
        /// <summary>
        /// Identifier for the ReportTo
        /// </summary>
        public string ReportTo { get; set; }
        /// <summary>
        /// Identifier for the RoleID
        /// </summary>
        public string RoleID { get; set; }
        /// <summary>
        /// Identifier for the ActualRoleID
        /// </summary>
        public string ActualRoleID { get; set; }
        ///// <summary>
        ///// Identifier for the Type
        ///// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Identifier for the UserId's
        /// </summary>
        public List<string> DownlineUserOrRoles { get; set; }
        /// <summary>
        /// identifier for the Admin user.
        /// </summary>       
        [DefaultValue(false)]
        public bool IsAdmin { get; set; } =false;

        public string ClientID {  get; set; }   

    }
}
