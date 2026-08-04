using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public class DsCashChecksReq
    {
        public long? DepositeId { get; set; }

        public string CorporationID { get; set; }

        public string PCID { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public DateTime? DepositeDate { get; set; }

        public string? Memo { get; set; }

        public decimal? Totalamount { get; set; }

        public decimal? TotalDeposite { get; set; }

        public decimal? difference { get; set; }

        public short? IsAttachment { get; set; }
        public string? AttachmentId { get; set; }

        public string? ActivityComments { get; set; }

        public short? Status { get; set; }

        public List<Depositeinfo> Depositeinfo { get; set; }

    }

    public class Depositeinfo
    {
        public long? DepositeInfoId { get; set; }
        public string DailyConfigInputId { get; set; }

        public DateTime? SaleDate { get; set; }
        public decimal? Difference { get; set; }
        public string? Comments { get; set; }
        public string AdjustmentAccountId { get; set; }
        public List<InfoDetail> InfoDetail { get; set; }
        public bool IsSelected { get; set; } = false;
        public string SelectedAccountID { get; set; } = string.Empty;
        public decimal TotalDeposit { get; set; }
    }


    public class InfoDetail
    {
        public long? InfoDetailId { get; set; }
        public string? ReceiptId { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public int? LineOrder { get; set; }      
        public decimal Amount { get; set; }
        public decimal Difference { get; set; }        
        public string JournalEntryid { get; set; }
        public decimal EditAmount { get; set; }
        public string EditAmountStr { get; set; }
        public string TempEditAmount { get; set; } = string.Empty;
        public bool IsReconciled { get; set; }
        public bool IsResumeReconciled { get; set; }
        public bool IsSaleReconciled { get; set; }
        public bool IsSaleResumeReconciled { get; set; }
        public bool EnableNegative { get; set; }       

    }

    public class    GetDailySalesDetailsReq
    {
        public string CorporationID { get; set; }
        public string PCID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }

    public class PostJournalEntryReq
    {
        public byte[]? JournalEntryId { get; set; }
        public string CorporationID { get; set; }
        public string? PCID { get; set; }
        public string AccountId { get; set; }
        public string LineName { get; set; }
        public string LineId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string ReceiptId { get; set; }
        public byte[] AttachmentId { get; set; }
        public DateTime SaleDate { get; set; }
    }

    public class ApprovalPolicyUserDetails
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public int ApprovalOrder { get; set; }
        public int ApprovalType { get; set; }
        public bool IsCureentUser { get; set; }
    }

    public class GetApprovalOrderType
    {
        public string UserID { get; set; }
        public int ApproalOder { get; set; }
        public int ApprovalType { get; set; }
    }

    public class GetAssinedToUser
    {
        public string AssignedTo { get; set; }
        public int ApprovingOrder { get; set; }
        public string ApprovedBy { get; set; }
        public int ApprovalStatus { get; set; }
        
    }
}

