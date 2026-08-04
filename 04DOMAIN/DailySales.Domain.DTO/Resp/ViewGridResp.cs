using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class ViewGridResp : StatusDTO
    {
        public List<DepositsDetails> Deposits { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int RowsInPage { get; set; }
        public decimal TotalDifferenceSum { get; set; }
        public decimal TotalDepositedSum { get; set; }
        public decimal TotalAmountSum { get; set; }
    }


    public class DepositsDetails
    {
        public long DepositId { get; set; }
        public string CorporationName { get; set; }
        public string Corporationid { get; set; }
        public string PCID { get; set; }
        public string PCName { get; set; }
        public string DepositPeriod { get; set; }
        public string DepositeDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDeposite { get; set; }
        public decimal Difference { get; set; }
        public string createdby { get; set; }
        //public string status { get; set; }
        public string Status { get; set; }
        public bool IsEditAccess { get; set; }
        public bool IsUpdate { get; set; } = false;
        public string Pay { get; set; }
        public bool IsPayAccess { get; set; }
        public bool HasAttachments { get; set; }
        public string? AttachmentId { get; set; }

        public bool IsNewUser { get; set; }
        //public DateTime? minFromDate { get; set; }
        //public DateTime? maxToDate { get; set; }

    }
    public class ViewGridDbResp
    {
        public long DepositId { get; set; }

        public string Corporationid { get; set; }
        public string CorporationName { get; set; }
        public string PCID { get; set; }
        public string PCName { get; set; }
        public string DepositPeriod { get; set; }
        public string DepositDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDeposited { get; set; }
        public decimal Difference { get; set; }
        public string AssignedTo { get; set; }
        public string createdby { get; set; }
        //public string status { get; set; }
        public string Status { get; set; }       
        public int ApprovalLevel { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int RowsInPage { get; set; }
        public decimal TotalDifferenceSum { get; set; }
        public decimal TotalDepositedSum { get; set; }
        public decimal TotalAmountSum { get; set; }
        public short HasAttachments { get; set; }
        public string? AttachmentId { get; set; }
        //public DateTime? minFromDate { get; set; }
        //public DateTime? maxToDate { get; set; }

    }
}
