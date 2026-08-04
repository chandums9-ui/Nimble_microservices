using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class GenericLongStringResponse : StatusDTO
    {
        public long ID { get; set; }
        public string User_Token { get; set; }
    }
    public class GenericLongResponse : StatusDTO
    {
        public long ID { get; set; }
    }
    public class GenericLongListResponse : StatusDTO
    {
        public List<GenericLongListDTO> ListInfo { get; set; }
    }
    public class GenericStringResponse : StatusDTO
    {
        public string Id { get; set; }
    }
    public class GenericBoolResponse : StatusDTO
    {
        public bool IsAllowed { get; set; }
    }
    public class EnvironmentDetailsResponse 
    {
        public string ClientName { get; set; }
        public string Environment { get; set; }
        public string Key { get; set; }
    }

    public class OTBOverviewResponse
    {
        public string SaleDate { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TransRooms { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? GrpRooms { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CurrRoomsSold { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CurrOcc { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CurrADR { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CurrRoomRev { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CurrRevPar { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? BudRoomsSold { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? BudOcc { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? BudADR { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? BudRoomRev { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? BudRevPar { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LYRoomsSold { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? LYOcc { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? LYADR { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? LYRoomRev { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? LYRevPar { get; set; }
    }

    public class OTBPickUpOverviewResponse
    {
        public string SaleDate { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PickUpRooms { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpADR { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRoomRev { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpOcc { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRevPar { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TransRooms { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? GrpRooms { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PickUpRoomsfor3Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpADRfor3Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRoomRevfor3Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpOccfor3Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRevParfor3Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TransRoomsfor3Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? GrpRoomsfor3Days { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PickUpRoomsfor7Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpADRfor7Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRoomRevfor7Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpOccfor7Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRevParfor7Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TransRoomsfor7Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? GrpRoomsfor7Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PickUpRoomsfor14Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpADRfor14Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRoomRevfor14Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpOccfor14Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRevParfor14Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TransRoomsfor14Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? GrpRoomsfor14Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PickUpRoomsfor30Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpADRfor30Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRoomRevfor30Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpOccfor30Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PickUpRevParfor30Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TransRoomsfor30Days { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? GrpRoomsfor30Days { get; set; }

    }

    public class OTBPerformanceResponse
    {
        public string CorpId { get; set; }
        public List<CorpWiseOTBPerformanceDet> CorpWiseOTBPerformanceDet { get; set; }

    }



    public class CorpWiseOTBPerformanceDet
    {
        public string CorpId { get; set; }
        public string Corpname { get; set; }
        public string BrandName { get; set; }
        public int? TotRooms { get; set; }
        public int? CurrRoomsSold { get; set; }
        public decimal? CurrOcc { get; set; }
        public decimal? CurrADR { get; set; }
        public decimal? CurrRoomRev { get; set; }
        public decimal? CurrRevPar { get; set; }
        public int? TotalActualRoomsAvl { get; set; }
        public int? RoomsSold { get; set; }
        public decimal? Occ { get; set; }
        public decimal? ADR { get; set; }
        public decimal? RoomRev { get; set; }
        public decimal? RevPar { get; set; }
    }

    public class CorpWiseOTBPerformanceDetails
    {
        public string CorpId { get; set; }
        public string Corpname { get; set; }
        public string BrandName { get; set; }
        public int? TotRooms { get; set; }
        public int? CurrRoomsSold { get; set; }
        public decimal? CurrOcc { get; set; }
        public decimal? CurrADR { get; set; }
        public decimal? CurrRoomRev { get; set; }
        public decimal? CurrRevPar { get; set; }
        public int? TotalActualRoomsAvl { get; set; }
        public int? RoomsSold { get; set; }
        public decimal? Occ { get; set; }
        public decimal? ADR { get; set; }
        public decimal? RoomRev { get; set; }
        public decimal? RevPar { get; set; }
    }

    public class OTBPickUpDayWiseResponse
    {

        public string CorpId { get; set; }
        public List<CorpWiseOTBPickUpDet> CorpWiseOTBPickUpDet { get; set; }
    }
    public class CorpWiseOTBPickUpDet
    {
        public string SaleDate { get; set; }
        public string CorpId { get; set; }
        public string Corpname { get; set; }
        public string BrandName { get; set; }
        public int? TotRooms { get; set; }
        public int? CurrRoomsSold { get; set; }
      
        public decimal? CurrOcc { get; set; }
      
        public decimal? CurrADR { get; set; }
      
        public decimal? CurrRoomRev { get; set; }
      
        public decimal? CurrRevPar { get; set; }
      
        public int? PickUpRooms { get; set; }
      
        public decimal? PickUpADR { get; set; }
      
        public decimal? PickUpRoomRev { get; set; }
      
        public decimal? PickUpOcc { get; set; }
      
        public decimal? PickUpRevPar { get; set; }
    }

    public class CorpWiseOTBPickUpDetails
    {
        public string CorpId { get; set; }
        public string SaleDate { get; set; }
        public string Corpname { get; set; }
        public string BrandName { get; set; }
        public int? TotRooms { get; set; }
      
        public int? CurrRoomsSold { get; set; }
      
        public decimal? CurrOcc { get; set; }
      
        public decimal? CurrADR { get; set; }
      
        public decimal? CurrRoomRev { get; set; }
      
        public decimal? CurrRevPar { get; set; }
      
        public int? PickUpRooms { get; set; }
      
        public decimal? PickUpADR { get; set; }
      
        public decimal? PickUpRoomRev { get; set; }
      
        public decimal? PickUpOcc { get; set; }
        
        public decimal? PickUpRevPar { get; set; }
    }


    public class OTBPickUpSummaryResponse
    {
        public string CorpId { get; set; }
        public List<CorpWiseOTBPickUpSummDet> CorpWiseOTBPickUpSummDet { get; set; }

    }

    public class CorpWiseOTBPickUpSummDet
    {
        public string CorpId { get; set; }
        public string Corpname { get; set; }
        public string BrandName { get; set; }
        public int TotRooms { get; set; }
      
        public int? CurrRoomsSold { get; set; }
      
        public decimal? CurrOcc { get; set; }
      
        public decimal? CurrADR { get; set; }
      
        public decimal? CurrRoomRev { get; set; }
      
        public decimal? CurrRevPar { get; set; }
      
        public int? PickUpRooms { get; set; }
      
        public decimal? PickUpADR { get; set; }
      
        public decimal? PickUpRoomRev { get; set; }
      
        public decimal? PickUpOcc { get; set; }
      
        public decimal? PickUpRevPar { get; set; }
      
        public int? PickUpRoomsfor3Days { get; set; }
      
        public decimal? PickUpADRfor3Days { get; set; }
      
        public decimal? PickUpRoomRevfor3Days { get; set; }
      
        public decimal? PickUpOccfor3Days { get; set; }
      
        public decimal? PickUpRevParfor3Days { get; set; }
      
        public int? PickUpRoomsfor7Days { get; set; }
      
        public decimal? PickUpADRfor7Days { get; set; }
      
        public decimal? PickUpRoomRevfor7Days { get; set; }
      
        public decimal? PickUpOccfor7Days { get; set; }
      
        public decimal? PickUpRevParfor7Days { get; set; }
      
        public int? PickUpRoomsfor14Days { get; set; }
      
        public decimal? PickUpADRfor14Days { get; set; }
      
        public decimal? PickUpRoomRevfor14Days { get; set; }
      
        public decimal? PickUpOccfor14Days { get; set; }
      
        public decimal? PickUpRevParfor14Days { get; set; }
      
        public int? PickUpRoomsfor30Days { get; set; }
      
        public decimal? PickUpADRfor30Days { get; set; }
      
        public decimal? PickUpRoomRevfor30Days { get; set; }
      
        public decimal? PickUpOccfor30Days { get; set; }
      
        public decimal? PickUpRevParfor30Days { get; set; }
    }

    public class CorpWiseOTBPickUpSummDetails
    {
        public string CorpId { get; set; }
        public string Corpname { get; set; }
        public string BrandName { get; set; }
        public int TotRooms { get; set; }
      
        public int? CurrRoomsSold { get; set; }
      
        public decimal? CurrOcc { get; set; }
      
        public decimal? CurrADR { get; set; }
      
        public decimal? CurrRoomRev { get; set; }
      
        public decimal? CurrRevPar { get; set; }
      
        public int? PickUpRooms { get; set; }
      
        public decimal? PickUpADR { get; set; }
      
        public decimal? PickUpRoomRev { get; set; }
      
        public decimal? PickUpOcc { get; set; }
      
        public decimal? PickUpRevPar { get; set; }
      
        public int? PickUpRoomsfor3Days { get; set; }
      
        public decimal? PickUpADRfor3Days { get; set; }
      
        public decimal? PickUpRoomRevfor3Days { get; set; }
      
        public decimal? PickUpOccfor3Days { get; set; }
      
        public decimal? PickUpRevParfor3Days { get; set; }
      
        public int? PickUpRoomsfor7Days { get; set; }
      
        public decimal? PickUpADRfor7Days { get; set; }
      
        public decimal? PickUpRoomRevfor7Days { get; set; }
      
        public decimal? PickUpOccfor7Days { get; set; }
      
        public decimal? PickUpRevParfor7Days { get; set; }
      
        public int? PickUpRoomsfor14Days { get; set; }
      
        public decimal? PickUpADRfor14Days { get; set; }
      
        public decimal? PickUpRoomRevfor14Days { get; set; }
      
        public decimal? PickUpOccfor14Days { get; set; }
      
        public decimal? PickUpRevParfor14Days { get; set; }
      
        public int? PickUpRoomsfor30Days { get; set; }
      
        public decimal? PickUpADRfor30Days { get; set; }
      
        public decimal? PickUpRoomRevfor30Days { get; set; }
      
        public decimal? PickUpOccfor30Days { get; set; }
        
        public decimal? PickUpRevParfor30Days { get; set; }
    }

}
