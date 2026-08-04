using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Resp;
using Payable.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payable.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Components.Web;
namespace Payable.Domain.DTO.Resp
{
    public  class PayableResponseDTO
    {
        public class AutoBillPreferenceResponse:StatusDTO
        {
            public  bool IsAutoBill { get; set; }
            public int PostDays { get; set; }
        }
        public class GetVendorContractsResponse : StatusDTO
        {
            public List<VendorDataa> Vendors { get; set; } = new List<VendorDataa>();
        }
        public class VendorCheckPreferenceResponse : StatusDTO
        {
            public List<VendorCheckPreferenceData> Preferences { get; set; }
        }
        public class VendorCheckPreferenceData
        {
            public string VendorID { get; set; }
            public bool ToBePrinted { get; set; }
            public bool IsPrintCheck { get; set; }
        }
        public class VendorDataa
        {
            public string ID {  get; set; }
            public string VendorID { get; set; } 
            public List<string> Contract { get; set; } =new List<string>();
        }

        public class GetVendorByUserOrCorpResponse : StatusDTO
        {
            public List<VendorNamesData> Vendors { get; set; } = new List<VendorNamesData>();
        }
        
        public class VendorNamesData
        {
            public string VendorID { get; set; }
            public string VendorName { get; set; }
            public string Corporation { get; set; }
            public string CorporationID { get; set; }
            public string CorporationLegalName { get; set; }
            public decimal BillsAmount { get; set; }
            public decimal DebitMemos { get; set; }
            public decimal Balance { get; set; }
            public long BillsCount { get; set; }
            public long DebitMemoCount {  get; set; }
        }
        public class BillEntryResponse : CheckLoadResponse,IStatusDTO
        {
            public DateTime? BillDate { get; set; }
            public DateTime DueDate { get; set; }
            public string? CreditDaysID { get; set; }
            public string? CheckMemo { get; set; }
            [DefaultValue("false")]
            public bool IsHoldPayment { get; set; }
            public List<ApprovalCommentsDTO> Comments { get; set; }
          
        }
    }
    public class PayableLaunchResponse()
    {
        public List<PayableLaunchSummery> Summary { get; set; }=new List<PayableLaunchSummery> (); 
    }
    public class LoadPaymethodsResponse : StatusDTO
    {
        public List<PayMethodData> PayMethods { get; set; } = new List<PayMethodData>();
        public string ClientID { get; set; }
    }

    public class GetPayMethodsOrCreditDays : StatusDTO
    {
        public LoadPaymethodsResponse Paymethods { get; set; } = new LoadPaymethodsResponse();
        public string ClientID { get; set; }

        public GenericIDNameListDTO CreditDays { get; set; } = new GenericIDNameListDTO(); 
    }

    public class PayMethodData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public short? Type { get; set; }
    }

    public class PayableLaunchSummery
    {
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }

        public decimal BillTobeApprovedBalance { get; set; }

        public long BillTobeApproved { get; set; }
        public decimal PendingPaymentsBalance { get; set; }

        public long PendingPayments { get; set; }
        public decimal PayTobeApprovedBalance { get; set; }

        public long PayTobeApproved { get; set; }
        public int expected { get; set; }
        public string PayMethod { get; set; }
        public string PaymentMethodID { get; set; }
        public string Status { get; set; }
        public long TotalCount { get; set; }
        public bool HasAttachments { get; set; }
        public int PageCount { get; set; }
    }
    public class LoadDefaultAccountResponse:StatusDTO
    {
        public string AccountID {  get; set; }  
        public string AccountName { get; set;}
    }
    public class ChatErrorResponse
    {
        public List<Detail> detail { get; set; }=new List<Detail>();
    }
    public class Detail
    {
        public string type { get; set; }
        public List<string> loc { get; set; }
        public string msg { get; set; }
        public ChatRoomInformationRequest Input { get; set; }
    }
    public class ChatURLResponse:StatusDTO
    {
        public string URL { get; set; }

    }
    public class ChatUnreadCountResponse:StatusDTO
    {
        public List<UnreadCountData> MessageCount { get; set; }=new List<UnreadCountData>();
    }
    public class OcrTransCountResponse : StatusDTO
    {
        public int need_attention_count { get; set; }
    }
    public class UnreadCountData
    {
        public string JID { get; set; }
        public int Unread_Count { get; set; }
        public string RoomName { get; set; }
        public string RoomID { get; set; }
    }

        //public class AmountWithCount
        //{
        //    public decimal amount;
        //    public int count;

        //    public AmountWithCount()
        //    {

        //    }
        //    public AmountWithCount(decimal amount, int count)
        //    {
        //        this.amount = amount;
        //        this.count = count;
        //    }


        //    //Override ToString() method to format output
        //    public override string ToString()
        //    {
        //        return $"{amount:F2} ({count})";
        //    }
        //}
    }
