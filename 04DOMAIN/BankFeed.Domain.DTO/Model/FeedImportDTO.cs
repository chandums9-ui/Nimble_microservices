using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedImportDTO
    {
        public long? ImportID { get; set; }
        [Required]
        public string CorpID { get; set; }
        [Required]
        public string NimbleAccountID { get; set; }
        [Required]
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        /// <summary>
        /// depository = 1,credit = 2,loan = 3,other = 4
        /// </summary>
        [Required]
        public short? NimbleAccountType { get; set; }
        [Required]
        public long FormatID { get; set; }
        public long FeedAccId { get; set; }
    }

    // Format Setting
    public class ImportSettingsDTO : ModelBaseIDInt64
    {
        [Required(ErrorMessage = "Format name must be provided.")]
        public string FormatName { get; set; }
    }
    public class FormatSettingDTO : ImportSettingsDTO
    {
        public long ProvRegID { get; set; }
        /// <summary>
        /// Is header exists in the imported .csv file
        /// </summary>
        public bool IsHeader { get; set; }
        /// <summary>
        /// Date to Import from
        /// </summary>
        [Required(ErrorMessage = "Please select Date.")]
        public string Date { get; set; }
        public string? Memo { get; set; }
        public string? Number { get; set; }
        /// <summary>
        /// Number of row to start reading data from 
        /// </summary>
        [Required(ErrorMessage = "DataReadFrom must be provided.")]
        public int DataReadFrom { get; set; }
        public string? TemplatePath { get; set; }
        /// <summary>
        /// CR/DR description(2) line when AmountType is SingleLine(1) 
        /// </summary>
        public string? DRCRColumn { get; set; }
        /// <summary>
        /// Credit column when AmountType is Double-Line(2)
        /// </summary>
        public string? CRColumn { get; set; }
        /// <summary>
        /// Debit column when AmountType is Double-Line(2)
        /// </summary>
        public string? DRColumn { get; set; }
        /// <summary>
        /// Amount column when AmountType is SingleLine
        /// </summary>
        public string AmountColumn { get; set; }
        public int? ColumnCount { get; set; }
        /// <summary>
        /// Single Line-1 / Double Line-2
        /// </summary>
        public short AmountType { get; set; } = 1;
        /// <summary>
        /// With +/-(1) / DR/CR(2)
        /// </summary>
        public short AmountMode { get; set; } = 1;
        /// <summary>
        /// BinaryId for FileDownload
        /// </summary>
        public string FormatRefId { get; set; }

        /// <summary>
        /// Is Format Deleted or Active
        /// </summary>
        public bool IsDeleted { get; set; } = false;

    }
}
