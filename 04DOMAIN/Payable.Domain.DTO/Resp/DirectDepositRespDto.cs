using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class DirectDepositRespDto
    {
           //public string Checked { get; set; }
            public List<DDTable> DDTableData = new List<DDTable>();

    }

    public class DDTable
    {
        public string JID { get; set; }
        public string BillDate { get; set; }
        public string PayeeName { get; set; }
        public string BankAccount { get; set; }
        public string DirectDepositNum { get; set; }
        public string VendorId { get; set; }
        public string AccountId { get; set; }
        public string JournalEntryId { get; set; }
        public Decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public bool chkStatus { get; set; }

        public string EFTREFNumber { get; set; }
    }

    public class DirectDepositBillPayments
    {
        public DateTime TransactionDate { get; set; }
        public string Name { get; set; }
        public string AccountName { get; set; }
        public decimal PaidAmount { get; set; }
        public byte[] DDBPJID { get; set; }
        public string DirectDepositNum { get; set; }
        public byte[] JournalEntryId { get; set; }
        public byte[] AccountId { get; set; }
        public string Status { get; set; }
        public byte[] VendorId { get; set; }
        public string PaymentStatus { get; set; }
        public string EFTREFNumber { get; set; }
    }

    public class DirectDepositSaveRequest
    {
        public RequestHeader RequestHeader { get; set; }
        public MergeInfo MergeInfo { get; set; }
        public Result? Result { get; set; }
    }


    public class RequestHeader
    {
        //public byte[] UserID { get; set; }
        public string MenuID { get; set; }
    }

    public class MergeInfo
    {
        public List<string> TransactionID { get; set; }
        public List<string> JournalEntryId { get; set; }
        public string CreationNo { get; set; }
        public string CreationDate { get; set; }
    }

    public class Result
    {
        public List<DDCreationNumberDivision> DDCreationNumberDivision { get; set; }
    }

    public class DDCreationNumberDivision
    {
        public string TID { get; set; }
        public string JID { get; set; }
        public string CorporationID { get; set; }
        public string AccountID { get; set; }
        public string CreationNum { get; set; }
        public string CreationDate { get; set; }
    }

    

    public class DirectDepositDetailsResult
    {
        public string BID { get; set; }
        public NamedReference CorporationID { get; set; }
        public NamedReference AccountType { get; set; }
        public sbyte BankAndPaymentsIn { get; set; }
        public string CorpLegalName { get; set; }
        public string CorpShortName { get; set; }
        public string DDBankName { get; set; }
        public string DDOriginatorNo { get; set; }
        public string DDInstitutionNo { get; set; }
        public string DDRoutingNo { get; set; }
        public string DDAccountNo { get; set; }
        public string DataCentreNo { get; set; }
        public string ResFieldsForOrg { get; set; }
        public string EleIdentification { get; set; }
        public string SettlementCode { get; set; }
        public string PayableTransCode { get; set; }
        public string DDBankTemplate { get; set; }
        public string Status { get; set; }
        public string CorpName { get; set; }
    }

    public class NamedReference
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class DirectDepositDetailsLoadResult
    {
        public DirectDepositDetailsResult Result { get; set; }
    }

    public class DirectDepositVendorDetailsResult
    {
        public string BID { get; set; }
        public string DDInstitutionNo { get; set; }
        public string DDAccountNo { get; set; }
        public string DataCentreNo { get; set; }
        public string CorpName { get; set; }
        public string SettlementCode { get; set; }
    }

    public class DirectDepositVendorDetailsLoadResult
    {
        public DirectDepositVendorDetailsResult Result { get; set; }
    }


    public class LoadByCorporationIDAccountIDParams
    {
        public string CorporationID { get; set; }
        public string AccountID { get; set; }
        //public RequestHeaderforDDExpoSave RequestHeader { get; set; }
    }

    //public class DirectDepositExportDetail
    //{
    //    public string? ID { get; set; }
    //    public string? JournalEntryId { get; set; }
    //    public string? TransactionId { get; set; }
    //    public string? CorporationId { get; set; }
    //    public string? AccountId { get; set; }
    //    public string? CreatedBy { get; set; }
    //    public string? ExportedBy { get; set; }
    //    public string? LastExportedBy { get; set; }

    //    // Nullable decimal
    //    public decimal? BillAmt { get; set; }

    //    // Nullable strings
    //    public string? CreationNum { get; set; }
    //    public string? MultipleCreationNoDisplay { get; set; }
    //    public string? Eftrefnumber { get; set; }

    //    // Nullable integers and long
    //    public int? ExportedCnt { get; set; }
    //    public long? BID { get; set; }

    //    // Nullable DateTime
    //    public DateTime? CreatedDate { get; set; }
    //    public DateTime? LatestCreatedDate { get; set; }

    //    // Nullable boolean (if IsFromReport is actually intended as a bool)
    //    public int? IsFromReport { get; set; }
    //}


    

    //public class RequestHeaderforDDExpoSave
    //{
    //    public string ClientID { get; set; }
    //    public string UserID { get; set; }
    //}



    //public class LoadByIDRequest
    //{
    //    public string ID { get; set; }
    //}

}
