using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using DailySales.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class DailySaleResponse : StatusDTO
    {
        public string SaleID { get; set; }
    }
    public class DailySaleCorpFacilityResponse : StatusDTO
    {
        public List<CorpFacilityDTO> Corporation { get; set; }
    }

    public class DailySaleLinesResponse : ModelBaseCorporationID, IStatusDTO
    {
        public int TotalCount { get; set; }
        public List<DailySaleConfigLinesDTO>? Lines { get; set; }
        public string? ProfitCenterID { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class DownLineUsersResponse : StatusDTO
    {
        public List<DailySaleDownlineUSers> Users { get; set; }
    }

    public class DailySaleDownlineUSers
    {
        public string ID { get; set; }
        public string UserName { get; set; }
    }

    public class VerificationLinesResponse : StatusDTO
    {
        public List<VerificationLines1> VerificationLines { get; set; }
        public string SaleID { get; set; }
    }

    public class VerificationLines1
    {
        public string LineID { get; set; }
        public decimal? Amount { get; set; }
        public string LineName { get; set; }
    }

    public class OTBFileUploadRequest
    {
        public byte[] FileStream { get; set; }
        public string FileName { get; set; }
        public string CorporationId { get; set; }
        public string ProfitCenterId { get; set; }
        public string UserId { get; set; }
        public string AuthId { get; set; }
    }
    public class OTBFileUploadResponse
    {
        [JsonPropertyName("upload_id")]
        public int UploadId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        public int StatusCode { get; set; }
    }

    public class OTBResponse : StatusDTO
    {
        public int ID { get; set; }
    }
    public class OTBViewData
    {
        public long? ID { get; set; }
        public string CorpName { get; set; }
        public string PCName { get; set; }
        public string CurrentDate { get; set; }
        public string OTBStatus { get; set; }
        public string CreatedBy { get; set; }
        public string s3Key { get; set; }
    }
    public class OTBPcs : StatusDTO
    {
        public byte[] Id { get; set; }
        public string PCName { get; set; }
    }
    public class GetCorporationAndPCListByTypeResp
    {
        public string CorporationID { get; set; }
        public string PCID { get; set; }
        public string CorporationName { get; set; }
        public string PCName { get; set; }
        public string PropertyType { get; set; }

        public string Service { get; set; }
        public string Brand { get; set; }
        public string PMS { get; set; }

    }
    public class OTBMemorizeResponse : StatusDTO
    {
        public long MemorizeID { get; set; }
    }
    public class OTBGetMemorizeResponse : StatusDTO
    {
        public int ViewTransType { get; set; }
        public int PickUpdays { get; set; }
        public int ShowNegFig { get; set; }
        public bool InRed { get; set; }
        public bool WOCents { get; set; }
        public bool ExcludeZero { get; set; }
        public string CustomizeString { get; set; }
        public int DisplayTextFormat { get; set; }
    }
}
