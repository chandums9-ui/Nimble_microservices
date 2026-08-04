using Common.Domain.DTO.Model.Base;
using DailySales.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public class DailySaleRequest
    {
        public DailySaleDTO DailyInputEntry { get; set; }
        public List<DailySaleEntryDTO> Revenue { get; set; }
        public List<DailySaleEntryDTO> Receipt { get; set; }
        public List<DailySaleEntryDTO> ARDetails { get; set; }
        public List<DailySaleEntryDTO> Statistics { get; set; }
        public List<VerificationLines> VerificationLines { get; set; }

    }

    public class OTBReportInfoRequest
    {
        [JsonPropertyName("report_info")]
        public OTBReportInfoDTO ReportInfo { get; set; }
        [JsonPropertyName("forecast_data")]
        public List<OTBForecastDataDTO> ForecastData { get; set; }
    }
    public class DeleteRequest
    {
        public string ID { get; set; }
    }

    public class DailySaleConfigLineRequest : ModelBaseCorporationID
    {
        public string? ProfitCenterID { get; set; }
    }

    public class OTBReportInfoDTO
    {
        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("upload_time")]
        public DateTime? UploadTime { get; set; }

        [JsonPropertyName("upload_type")]
        public string UploadType { get; set; }

        [JsonPropertyName("facility_id")]
        public string FacilityId { get; set; }

        [JsonPropertyName("corporation_id")]
        public string CorporationId { get; set; }

        [JsonPropertyName("pc_id")]
        public string PcId { get; set; }

        [JsonPropertyName("forecast_date")]
        public DateTime? ForecastDate { get; set; }

        [JsonPropertyName("imported_by")]
        public string FromEmail { get; set; }
        [JsonPropertyName("s3_key")]
        public string s3key { get; set; }
    }

    public class OTBForecastDataDTO
    {
        [JsonPropertyName("sale_date")]
        public DateTime? SaleDate { get; set; }

        [JsonPropertyName("transient_rooms")]
        public string TransientRooms { get; set; }

        [JsonPropertyName("transient_people")]
        public string TransientPpl { get; set; }

        [JsonPropertyName("group_rooms")]
        public string GroupRooms { get; set; }

        [JsonPropertyName("group_people")]
        public string GroupPpl { get; set; }

        [JsonPropertyName("total_occupied_rooms")]
        public string TotalOccupiedRooms { get; set; }

        [JsonPropertyName("total_people")]
        public string TotalPpl { get; set; }

        [JsonPropertyName("occupancy")]
        public string OccPercent { get; set; }

        [JsonPropertyName("room_revenue")]
        public string RoomRev { get; set; }

        [JsonPropertyName("revpar")]
        public string RevPar { get; set; }

        [JsonPropertyName("adr")]
        public string Adr { get; set; }
        [JsonPropertyName("available_rooms")]
        public string TotalAvlRooms { get; set; }
    }

    public class OTBViewGridInputReq
    {
        public string CorpId { get; set; }
        public DateTime? FromDate
        {
            get
            {
                DateTime fromDate;
                return (!string.IsNullOrEmpty(ShortFromDate) && DateTime.TryParse(ShortFromDate, out fromDate)) ? Convert.ToDateTime(ShortFromDate) : DateTime.Today;
            }
        }

        public DateTime? ToDate
        {
            get
            {
                DateTime todate;
                return (!string.IsNullOrEmpty(ShortToDate) && DateTime.TryParse(ShortToDate, out todate)) ? Convert.ToDateTime(ShortToDate) : DateTime.Now;
            }
        }
        public string PCID { get; set; }
        public int Status { get; set; }
        public string ShortFromDate { get; set; } = string.Empty;
        public string ShortToDate { get; set; } = string.Empty;
    }

    public class OTBMemorizeReq
    {
        public string UserId { get; set; }
        public string MenuId { get; set; }
        public int ViewTransType { get; set; }
        public int PickUpdays { get; set; }
        public string ReportTitle { get; set; }
        public int ShowNegFig { get; set; }
        public bool InRed { get; set; }
        public bool WOCents { get; set; }
        public bool ExcludeZero { get; set; }
        public string CustomizeString { get; set; }
        public int DisplayTextFormat { get; set; }
    }
    public class OTBMemorizeGetReq
    {   
        public string UserId { get; set; }
        public string MenuId { get; set; }
    }

}
