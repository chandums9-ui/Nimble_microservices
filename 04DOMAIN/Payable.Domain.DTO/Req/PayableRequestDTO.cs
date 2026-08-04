using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Enums;

namespace Payable.Domain.DTO.Req
{
    public class GetZipFileInfoResponse:StatusDTO
    {
        public string CorpID { get; set; }
        public string Type { get; set; }
        public string ZipFileName { get; set; }
    }

    public class DiscountAccountData:StatusDTO
    {
        public long ID { get; set; }
        public string CorpID { get; set; }
        public string CorpName { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
    }
    public class BillEntryRequest : JournalAccountDTO
    {
        public DateTime? BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? CreditDaysID { get; set; }
        public string? CheckMemo { get; set; }
        [DefaultValue("false")]
        public bool IsHoldPayment { get; set; }
        public string? UserID { get; set; }
    }
    public class LoadBillPayLastAccountRequest
    {
        public string CorporationID { get; set; }
        public string PayMethodID { get; set; }
    }
    public class LoadEPayLastAccountRequest
    {
        public string CorporationID { get; set; }
        public string PayMethodID { get; set; }

        public string VenId { get; set; }
    }
    public class LoadBillPaymentsListRequest
    {
        public string CorporationID { get; set; }
        public string VendorID { get; set; }
        public string PayMethodID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsHold { get; set; } 
        public string SkipJEIDs { get; set; }
        /// <summary>
        /// SortByEnum
        /// </summary>
        public short SortOrder { get; set; } = (short)SortByEnum.DefaultSort;
        public int PageNumber { get; set; }

        public string BillInfoIDs { get; set; }

        public bool IsHidePaymethods { get; set; }
        public byte[] ClientId { get; set; }
    }

    public class FiltersSaveRequest
    {
        public int SourceType {  get; set; }    
        public List<FilterData> Filters { get; set; } = new List<FilterData>();

    }
    public class FilterData
    {
        public short Filter { get; set; }
        [DefaultValue((short)7)]
        public short OperationType { get; set; } = 7;
        public string Value {  get; set; } 
    }
    public class ChatParticipants
    {
        public string username { get; set; }
        public string userID { get; set; }
        public string displayName { get; set; }
        public string gender { get; set; }
    }

    public class ChatRoomDetails
    {
        public string vendorName { get; set; }
        public decimal invoiceAmount { get; set; }
        public string corporationName { get; set; }
        public string paymentMethod { get; set; }
        public string Type { get; set; }
    }
    public class GetRoomParticipants:StatusDTO
    {
        public List<ChatParticipants> participates { get; set; }
    }
    public class ChatRoomInformationRequest
    {
        public List<ChatParticipants> participates { get; set; }
        public ChatRoomDetails room_details { get; set; }
    }
    public class GetChatDataRequest
    {
        public string JID { get; set; }
        public string CorpID { get; set; }
        public string RoomName {  get; set; } = "";
        /// <summary>
        /// 148- bill/debitmemo,15-BillPay
        /// </summary>
        public int ScreenType { get; set; }
        public decimal InvoiceAmt { get; set; }
        public string VendorName { get; set; }
        public string BillStatus { get; set; }
        public string CorpName { get; set; }
        public string PayMethodName { get; set; }
        public string Type { get; set; }
    }
    public class GroupCreationRequest
    {
        public string UserName { get; set;}
        public string UserID { get; set;}
    }
    public class ChatURLs
    {
        public string MainIFrameURL { get; set; }
        public string MainChatURL { get; set;}
        public string CreateChatGrouptURL { get; set; }
        public string AddUsersToGrouptURL { get; set; }
        public string RemoveUsersFromGrouptURL { get; set; }
        public string UnreadMessagesCountURL { get; set; }
        public string GetRoomParticipantsURL {  get; set; }
    }
    public class OCRTransctionsURLs
    {
        public string MainURL { get; set; }
        public string client_url { get; set; }
        public string OcrTransactionsCountURL {  get; set; }
    }
    public class BillUploadURLs
    {
        public string MainURL { get; set; }
        public string upload_url { get; set; }
        public string client_url { get; set; }

    }

}
