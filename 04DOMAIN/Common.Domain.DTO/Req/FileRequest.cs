using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{
    public class FileRequest
    {
        public List<string> ReferenceID { get; set; }=new List<string>();

        [DefaultValue("")]
        public string? FileName { get; set; }
    }
    public class AwsFileReq
    {
        public string ReferenceID { get; set; }
        public string FileName { get; set; }    

    }
    public class FilesView
    {
        public string Filename { get; set; }
        public string UploadTime { get; set; }
        public string ReqTime { get; set; }

        public string AwsPath { get; set; }

    }

    public class FileView
    {
        public string ReferenceID { get; set; }
        public string Filename { get; set; }
      
        public string AwsPath { get; set; }

    }

    public class UploadFileRequest : ModelBaseIDInt64
    {

        /// <summary>
        /// Gets the file name from the Content-Disposition header.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Streem content of byte array
        /// </summary>
        public byte[] FileContent { get; set; }

        /// <summary>
        /// Gets the file length in bytes.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Gets the raw Content-Type header of the uploaded file.
        /// </summary>
        public string DocumentType { get; set; }

    }
    public class AWSFileInfo
    {
        public string FileName { get; set; }

        /// <summary>
        /// Streem content of byte array
        /// </summary>
        public byte[] FileContent { get; set; }

        /// <summary>
        /// Gets the file length in bytes.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Gets the raw Content-Type header of the uploaded file.
        /// </summary>
        public string DocumentType { get; set; }
    }
   

    public class UploadFileStreem
    {
        /// <summary>
        /// Gets the file name from the Content-Disposition header.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets the file length in bytes.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// FileData Stream
        /// </summary>
        public Stream Stream { get; set; }
    }

    public class UploadFileBaseReq
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
    }

    public class UploadFilesReq : UploadFileBaseReq
    {
        public List<AWSFileInfo> Files { get; set; } = new List<AWSFileInfo>();
    }
    public class MoveFilesReq : UploadFileBaseReq
    {
        public string FilePath { get; set; }
        public string ClientName { get; set; }

    }

    public class BillEntrySendMailDetails
    {
        public string UserName { get; set; }
        public string CorpName { get; set; }
        public string EntryDate { get; set; }
        public string VendorName { get; set; }
        public string BillNumber { get; set; }
        public string Amount { get; set; }
        public string EnteredBY { get; set; }
        public string UrlInfo { get; set; }
        public string CorpID { get; set; }
        public string UserID { get; set; }
    }
}
