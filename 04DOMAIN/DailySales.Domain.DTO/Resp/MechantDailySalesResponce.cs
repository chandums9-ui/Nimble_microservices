using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class MechantDailySalesResponce : StatusDTO
    {

        public string CorporationID { get; set; }
        public string AccountID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public List<SaleReceipt> SaleReceipts { get; set; }
    }


    public class SaleReceipt
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

    public class CardTypes
    {
        public  string CardID {  get; set; }    
        public  string Name {  get; set; }   
        public  int DeptType{ get; set; }   
        public  int DeptTypeOrder {  get; set; }    

    }

    public class CardTypeResponse:StatusDTO
    {
        public List<CardTypes> CardsList {  get; set; }=new List<CardTypes>();  

    }

    public class CardsConfiguredAccounts:StatusDTO
    {
        public List <ConfiguredAccoutsDbResp> configuredAccounts { get; set; } =new List<ConfiguredAccoutsDbResp>();   
    }
    public class ConfiguredAccoutsDbResp
    {
        public string AccountID {  get; set; } 
        public string AccountType { get; set; }     
        public string AccountName { get; set; } 
        public string AccountTypeName {  get; set; }    
    }
    public class MerchentMulticorpDialysaleReceipts
    {
        public string CorporationID { get; set; }
        public string PcID { get; set; }
        public string AccountID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<SaleReceipt> SaleReceipts { get; set; }
    }

    public class DailysaleReceiptsMultiCorpResponse : StatusDTO
    {
        public List<MerchentMulticorpDialysaleReceipts> CorpwiseReceiptsResponse { get; set; }
    }

    public class ReceiptsMultiCorpDbRespobse : SaleReceipt
    {
        public string CorporationID { get; set; }
        public string AccountID { get; set; }
        public string StoreID { get; set; }
    }

}

