using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class STRFileUploadRequest : UploadFileRequest
    {
        [JsonPropertyName("corporation_name")]
        public string CorporationName { get; set; } = string.Empty;

        [JsonPropertyName("str_id")]
        public string StrID { get; set; } = string.Empty;

        [JsonPropertyName("corporation_id")]
        public string CorpID { get; set; } = string.Empty;

        [JsonPropertyName("profit_center_id")]
        public string ProfitCenterID { get; set; } = string.Empty;

        [JsonPropertyName("user_id")]
        public string UserID { get; set; } = string.Empty;

        [JsonPropertyName("client_id")]
        public string ClientID { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        public string profit_center_name { get; set; } = string.Empty;    


    }



    //public class GSSFileUploadRequest
    //{
    //    public string CorporationId { get; set; }
    //    public string ProfitCenterId { get; set; }
    //    public string Year { get; set; }
    //    public string Month { get; set; }
    //    public string UserId { get; set; }
    //    public string UserName { get; set; }
    //    public byte[] FileStream { get; set; }
    //    public string FileName { get; set; }
    //}

    //public class GSSFileUploadResponse:StatusDTO
    //{
    //    [JsonPropertyName("message")]
    //    public string Message { get; set; }

    //    [JsonPropertyName("upload_id")]
    //    public string UploadId { get; set; }

    //}

    public class GSSFileUploadRequest
    {
        public string CorporationId { get; set; }
        public string ProfitCenterId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public byte[] SurveyFileStream { get; set; } // for survey_file
        public string SurveyFileName { get; set; }

        public byte[] ScorecardFileStream { get; set; } 
        public string ScorecardFileName { get; set; }
    }

    public class GSSFileUploadResponse : StatusDTO
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("upload_id")]
        public string UploadId { get; set; }

        [JsonPropertyName("reports")]
        public List<GSSFileReport> Reports { get; set; }
    }

    public class GSSFileReport
    {
        [JsonPropertyName("report_type")]
        public string ReportType { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

}
