using Common.Domain.DTO.Model.Base;
using DailySales.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class DsCashChecksResp : StatusDTO
    {
        public long? DepositeId { get; set; }

        public string CorporationID { get; set; }

        public string PCID { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime DepositeDate { get; set; }

        public string? Memo { get; set; }

        public decimal? Totalamount { get; set; }

        public decimal? TotalDeposite { get; set; }

        public decimal? difference { get; set; }

        public bool? IsAttachment { get; set; }
        public string? AttachmentId { get; set; }

        //public string? ActivityComments { get; set; }
        public string AssignedTo { get; set; }
        public bool IsSplitEnabled { get; set; }
        public List<Depositeinfo> Depositesinfo { get; set; }
        public List<ReconToolTipDetails> ReconToolTipDetails { get; set; }
        public List<string>? DailyConfigInputIds { get; set; }
        public bool HasMultipleAccounts { get; set; }


        public DateTime? LatestFromDate { get; set; }
        public DateTime? LatestToDate { get; set; }
        public string LatestAccountID { get; set; }

    }
    public class ReconToolTipDetails
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string ReconType { get; set; }
        public DateTime StatementDate { get; set; }
    }

    public class GetDailySalesDetailsDbResp
    {
        public string LineID { set; get; }
        public string LineName { set; get; }
        public long LineOrder { get; set; }
        public string ConfigInputId { set; get; }
        public DateTime SaleDate { set; get; }
        public string ReceiptID { set; get; }
        public decimal Amount { set; get; }
        public bool EnableMultiple { set; get; }
        public bool EnableNegative { set; get; }
        public DateTime? LatestFromDate { get; set; }
        public DateTime? LatestToDate { get; set; }
        public string LatestAccountID { get; set; }

    }
    public class SaveOrEditResp : StatusDTO
    {
        public string? AttachmentId { get; set; }
        public List<string> DailyConfigInputIds { get; set; } = new List<string>();
    }

  
}

