using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class ViewGridResp : StatusDTO
    {
        //public decimal TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int RowsInPage { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public List<FundTransferDetails> Entries { get; set; } = new();

    }


    public class FundTransferDetails
    {
        public string JEID { get; set; }
        //public string FromCorporationID { get; set; }
        //public string ToCorporationIDs { get; set; }
        public string TransferDate { get; set; }
        public string EntryNumber { get; set; }
        public int TransferType { get; set; }
        public string EntryType { get; set; }
        public string FromCorporation { get; set; }
        public string ToCorporation { get; set; }
        public string Mode { get; set; }
        public decimal Amount { get; set; }
        //public string? AttachmentId { get; set; }
        public bool HasAttachments { get; set; }
        public bool IsReconciliation { get; set; } = false;
        public bool IsVoid { get; set; }
        public bool IsLock { get; set; }
        public bool IsEditAccess { get; set; } = true;
        //public decimal TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int RowsInPage { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }


    }
 
}
