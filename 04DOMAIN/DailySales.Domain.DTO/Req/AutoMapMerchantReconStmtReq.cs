using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public class AutoMapMerchantReconStmtReq
    {      
            public string ReconUniqId { get; set; }
            public string UserID {  get; set; } 
            public string CorporationId { get; set; }
            public string PCID { get; set; }
            public string AccountId { get; set; }
            public string MerchantNumber { get; set; }
            public string MerchantName { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime ModifiedDate { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public short MatchType {  get; set; }    // 1-automapp ,2- manual mapp
            public List<MerchantSettlement> MerchantSettlements { get; set; }
    }
    public class MerchantSettlement
    {
            public string TransUniqId { get; set; }
            public DateTime SettlementDate { get; set; }
            public string CardTypes { get; set; }
            public string NimbleCardType {  get; set; } 
           // public decimal SettlementAmount { get; set; }
            public decimal GrossSaleAmount { get; set; }
            public decimal NetSaleAmount { get; set; }

            public decimal ChargebacksOrReturns { get; set; }
            public decimal ChargebackAmount { get; set; }
            
            public decimal AdjustmentAmount { get; set; }

            public DateTime TransferDate { get; set; }
            public List<ReconciledSale> ReconciledSales { get; set; }
    }

        public class ReconciledSale
        {
            public string RecId { get; set; }
            public string RecDetId { get; set; }
            public string SaleId { get; set; }
            public string TransId { get; set; }
            public decimal Amount {  get; set; }    

        }



    public class AutoMatchOrManulaMatchRequest
    {
        public bool IsManualMatch {  get; set; }    
        public List<AutoMatchingLine> AutoMatchingLines { get; set; }
    }

    public class AutoMatchingLine
    {
       
        public string MatchingId { get; set; }
        public string AccountID { get; set; }
        public string ReconUniqId { get; set; }

        public string TransactionUniqID { get; set; }
        public string UserID { get; set; }
        public string CorporationId { get; set; }
        public string PCID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<MerchantSettlementDetails> MerchantSettlements { get; set; }
        public List<DailySaleLinesInfo> DailySales { get; set; }
    }

    public class MerchantSettlementDetails
    {
        public string TransUniqId { get; set; }
        public string CardType { get; set; }
        public string NimbleCardType { get; set; }
        public DateTime SettlementDate { get; set; }
        public decimal SettlementAmount { get; set; }
        public decimal ChargebacksOrReturns { get; set; }
        public decimal FinancialAdjustments { get; set; }
       // public  decimal MerchantExcessAmount {  get; set; } 
        public DateTime TransferDate { get; set; }
    }

    public class DailySaleLinesInfo
    {
        public DateTime SaleDate { get; set; }
        public string LineName { get; set; }
        public string LineId { get; set; }
        public string CardType { get; set; }
        public string RecID { get; set; }
        public string RecDetID { get; set; }
        public string Description { get; set; }
        public string SaleID { get; set; }
        public string TransID { get; set; }
        public decimal Amount { get; set; }
    }



    public class AutoMatchOrManualMatchInfoSaveRequest
    {
        public string ReconUniqId { get; set; }
        public string CorporationId { get; set; }
        public string PCId { get; set; }
        public string AccountId { get; set; }
        public string ProviderName { get; set; }
        public string MerchantNumber { get; set; }
        public string MerchantName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public List<MerchantSettlement> MerchantSettlements { get; set; }
    }

    public class MerchantSettlementInfo
    {
        public string TransUniqId { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CardTypes { get; set; }
        public string NimbleCardType { get; set; }
        public decimal SettlementAmount { get; set; }
        public decimal ChargebacksOrReturns { get; set; }   
        public decimal ChargebackAmount { get; set; }       
        public decimal FinancialAdjustments { get; set; }   
        public decimal AdjustmentAmount { get; set; }      

        public DateTime TransferDate { get; set; }
    }

    public class SplitTransactionRequest
    {  
        public string CorporationId {  get; set; }  
        public string PCID {  get; set; }   
        public string UserId {  get; set; } 
        public short ApprovalType {  get; set; }        
        public List<MerchantSettlementDetails> MerchantSettlements { get; set; }
        public List<DailySaleLinesInfo> DailySales { get; set; }
    }

    public class SplitTransactionResponse:StatusDTO
    {
        public List<string> SplitJeIds { get; set; } = new List<string>();

    }

    public class ImportReconStmtRequest
    {
        public string ReconUniqId { get; set; }
        public string CorporationId { get; set; }
        public string PCID { get; set; }
        public string AccountId { get; set; }
        public string ProviderName { get; set; }
        public string MerchantNumber { get; set; }
        public string MerchantName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string UserID {  get; set; } 
        public List<ImporStatementDetails> MerchantSettlements { get; set; }
    }

    public class ImporStatementDetails
    {
        public string TransUniqId { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CardTypes { get; set; }
        public string NimblecardType { get; set; }
        public decimal SettlementAmount { get; set; }
        public decimal? ChargebacksOrReturns { get; set; }
        public decimal? ChargebackAmount { get; set; }
        public decimal? FinancialAdjustments { get; set; }
        public decimal? AdjustmentAmount { get; set; }
        public decimal? NetSaleAmount { get; set; }
        public DateTime TransferDate { get; set; }
    }


}

