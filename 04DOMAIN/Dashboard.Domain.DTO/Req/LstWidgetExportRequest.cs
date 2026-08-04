using Common.Domain.DTO.Model.Base;
using Dashboard.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class LstWidgetExportRequest
    {
        public List<WidgetForExportDTO> LstWidgetsForExport { get; set; }
        public string UserID { get; set; }
        public List<CompleteListClass> lstCorporations { get; set; } 
        public UserPreferenceDTO userPreference { get; set; }
        public List<WidgetsDTO> lstWidgets { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long UrlKey { get; set; }
    }

    public class CompleteListClass
    {
        [Key]
        public string ID { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string TypeName { get; set; } = string.Empty;
        /// <summary>
        /// to check is the account is default account or not
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
