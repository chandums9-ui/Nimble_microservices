using Common.Domain.DTO.App;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    /// <summary>
    /// Cash and card due balaces respose for single corporations
    /// </summary>
    public class DueBalancesResponse :StatusDTO
    {
        public DueBalancesResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<CashandCardBalance> CashandcardDues { get; set; }=new List<CashandCardBalance>();
    }
    //public class DueBalances
    //{
    //    public string AccountID { get; set; }
    //    public string AccountName { get; set; }
    //    public int AccountType { get; set; }
    //    public DateTime LastSyncDate { get; set; }
    //    public DateTime LastReconDate { get; set; }
    //    public decimal BankBalance { get; set; }
    //    public decimal NimbleBalance { get; set; }
    //    public decimal EndingBalance { get; set; }
    //    public int OpenFeeds { get; set; }
    //    public int OpenPayments { get; set; }
    //    public int OpenReciepts { get; set; }


    //}

    /// <summary>
    /// Cash and card due balaces respose for single corporations
    /// </summary>
    public class CashandCardBalance
    {
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public Int16 AccountType { get; set; }
        public DateTime? LastReconDate {  set; get; }
        public string? ReconciliationID { set; get; }
        public decimal OpenPayments { get; set; }   
        public decimal OpenReceipts { get; set; }   
        public decimal EndingBalance { get; set; }
    }

    /// <summary>
    /// Cash and card due balaces respose for all corporations
    /// </summary>
    public class CashandCardDuesResponse : StatusDTO
    {
        public CashandCardDuesResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<CashandCardDues> CashandcardDues { get; set; } = new List<CashandCardDues>();
    }

    /// <summary>
    /// Cash and card due balaces respose for all corporations
    /// </summary>
    public class CashandCardDues : CorpNames
    {
        public decimal BankOpenPayments { get; set; }
        public decimal BankOpenReceipts { get; set; }
        public decimal BankEndingBalance { get; set; }

        public decimal CCOpenPayments { get; set; }
        public decimal CCOpenReceipts { get; set; }
        public decimal CCEndingBalance { get; set; }
    }
    public class PendingReceiptsDbResponse
    {
        public string CorporationID {  get; set; }  
        public string CorpDBAName { get; set; }
        public string CorpLegalName {  get; set; }  
        public string CardName {  get; set; }   
        public Int32 CardType { get; set; }    
        public decimal Balance {  get; set; }       
    }

    public class CardSummaryResponse
    {
        public string CardName { get; set; }
        public int CardType { get; set; }
        public decimal Balance { get; set; }
    }

    public class CorporationCardsResponse
    {
        public string CorporationId { get; set; }
        public string CorporationName { get; set; }
        public string CorpLegalName { get;set; }
        public string LegalName {  get; set; }  
        public List<CardSummaryResponse> Cards { get; set; }
    }
}
