using Common.Domain.DTO.Model;
using Common.Domain.DTO.Resp;
using CoreAccounting.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class SaveOrEditFundTransferReq : FundAndReturnTransferDTO
    {
        public string? FundMainTransactionId { get; set; }
        public string? FromJournalEntryId { get; set; }
        public bool HasAttachments { get; set; } = false;

        public bool IsVoid { get; set; } = false;
        public bool IsButtonVoid { get; set; } = false;
        public DateTime? VoidDate { get; set; }
        public string VoidRemarks { get; set; }
        public bool IsValidate { get; set; } = false;
        public bool IsFundTransfer { get; set; }    
        public List<FundOrReturnTransferReconciliationDetails> ReconcileDetails { get; set; } = new List<FundOrReturnTransferReconciliationDetails>();
        public List<APToolTipDetails>? APToolTipDetails { get; set; }
    }

    
    public class FundAndReturnTransferDTO
    {
        public string CorpID { get; set; }  
        public string Jeid { get; set; }    
        public DateTime? TransactionDate { get; set; }
        public DateTime? ClearedDate { get; set; }
        public string EntryNum { get; set; }
        public string RefNumber { get; set; }
        public short FundTransferType { get; set; }
        public bool InterCompanies { get; set; }
        public bool InterAccounts { get; set; }
        public string Memo { get; set; }
        public bool IsFromTransfer { get; set; }
        public List<fundTransferDivisionDTO> FromfundAndReturnTransferDiv { get; set; } = new List<fundTransferDivisionDTO>();//1/0
        public List<fundTransferDivisionDTO> TofundAndReturnTransferDiv { get; set; } = new List<fundTransferDivisionDTO>();//1,2,3,4,5-Any true-5
    }
    
    public class fundTransferDivisionDTO
    {
        public string LabelCorpName { get; set; }   
        public string ParentCorpID { get; set; }
        public string ParentCorporationName { get; set; }

        public string ParentAccountID { get; set; }
        public string ParentAccountName { get; set; }
        public string ParentAccountType { get; set; }   
        public decimal ParentAccountBalance { get; set; } = 0.00m;

        public string ParentPayeeId { get; set; }
        public string ParentPayeeName { get; set; }
        public short ParentPayeeSourceType { get; set; }
        
        public decimal ParentAmount { get; set; } = 0.00m;
        public string ParentDisplayAmount { get; set; } = "0.00";

        public string ParentReconciliationID { get; set; }
        public string ParentReconStatus { get; set; }
        public bool IsNewlyAdded { get; set; }

        public bool BankTransactionRefID { get; set; }
        public string BillPaymentStatus { get; set; }



        public List<MultipleSplitRows> MultipleSplitRows { get; set; }=new List<MultipleSplitRows>();//Empty->0
    }

    public class MultipleSplitRows
    {
        public string ChildJournalEntryId { get; set; }
        public string TransactionId { get; set; }
        public string ParentTransactionId { get; set; }

        public string ProfitcenterID { get; set; }
        public string ProfitCenterName { get; set; }

        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; } 
        public string Memo { get; set; }
      //  public string ToCorpID { get; set; }
       // public string ToCorpName { get; set; }
        public decimal SplitAmount { get; set; }
       // public string ParentID { get; set; }

        public string PayeeId { get; set; }
        public string PayeeName { get; set; }
        public short PayeeSourceType { get; set; }

        public string? ReconciliationID { get; set; }
        public string? ReconStatus { get; set; }
        public bool BankTransactionRefID { get; set; }
    }

    public class APToolTipDetails
    {
        public string BillPaymentJEID { get; set; }
        public string CheckNumber { get; set; }
        public string? PaymentType { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime BillPaymentDate { get; set; }

    }
}
