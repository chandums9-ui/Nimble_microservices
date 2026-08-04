using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class STRFileUploadResponse  : StatusDTO
    {
        [JsonPropertyName("file_status")]
        public List<FileStatusItemDto> FileStatus { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("description")]
        public string Description {  get; set; }
        [JsonPropertyName("filename")]
        public string Filename {  get; set; }   
    }

    public class FileStatusItemDto
    {
        [JsonPropertyName("filename")]
        public string FileName { get; set; }

        [JsonPropertyName("s3_key")]
        public string S3Key { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        

    }
}
