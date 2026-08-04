using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class FileResponse
    {
        public string Message { get; set; }
        public int FileCount { get; set; } = 0;
    }

    public class AWSFileResponse : StatusDTO
    {

        /// <summary>
        /// Gets the file name from the Content-Disposition header.
        /// </summary>
        public string CorporationID { get; set; }

        /// <summary>
        /// Gets the file length in bytes.
        /// </summary>
        public string RefID { get; set; }

        /// <summary>
        /// FileData Stream
        /// </summary>
        public string Type { get; set; }

        public string FolderPath { get; set; }

        public List<FileUploadResp> Files { get; set; } = new List<FileUploadResp>();
    }
    public class FileUploadResp
    {
        public string FileName { get; set; }
        public string Message { get; set; }
        public long Length { get; set; }
        public int Status { get; set; }
    }
    public class AWSFileDownloadResp : StatusDTO
    {
        /// <summary>
        /// Gets the file name from the Content-Disposition header.
        /// </summary>
        public AWSDownloadFiles File { get; set; }
    }

    public class AWSBulkDownloadResp : StatusDTO
    {
        /// <summary>
        /// Gets the file name from the Content-Disposition header.
        /// </summary>
        public List<AWSDownloadFiles> Files { get; set; } = new List<AWSDownloadFiles>();
    }
    public class AWSDownloadFiles
    {
        public string CorporationID { get; set; }

        /// <summary>
        /// Gets the file length in bytes.
        /// </summary>
        public string RefID { get; set; }

        /// <summary>
        /// FileData Stream
        /// </summary>
        public string Type { get; set; }

        public List<AWSFileInfo> Files { get; set; } = new List<AWSFileInfo>();
    }

    public class AWSFileDownloadAllResp : StatusDTO
    {
        /// <summary>
        /// Gets the file name from the Content-Disposition header.
        /// </summary>

    }
    public class FilesViewResponse : StatusDTO
    {
        public List<FileView> Files { get; set; } = new List<FileView>();

    }

    public class DumpFileResponse
    {
        public string CorporationId { get; set; }
        public string JournalEntryID { get; set; }
    }
    public class GetFileUploadurlResponse
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("CorporationID")]
        public string CorporationId { get; set; }

        [Column("JournalEntryID")]
        public string JournalEntryID { get; set; }
        public string TypeName { get; set; }
        public string OldFolderPath {  get; set; }
        public string NewFolderPath { get; set; }
        public short? Status { get; set; }
    }

    public partial class ImportDocumentv2
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("JournalEntryID")]
        public string JournalEntryId { get; set; } // Change from byte[] to int if it's int in DB

        [Column("CorporationID")]
        public string CorporationId { get; set; } // Change from byte[] to int if it's int in DB

        public short? Status { get; set; }

        [StringLength(250)]
        public string TypeName { get; set; }
        public string FolderPath { get; set; }
    }

    public class GetRepayDetailsByGroup
    {
        public string JournalEntryId { get; set; }
        public string CorporationId { get; set; }
    }
    public class RepayDownloadFile
    {
        public string JournalEntryId { get; set; }

        public string GroupName { get; set; }

        public string FileName { get; set; }
    }
}
