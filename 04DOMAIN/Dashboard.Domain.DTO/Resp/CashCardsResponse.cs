using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class CashCardsResponse : StatusDTO
    {
        public CashCardsResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<KeyValuePairObject<string, decimal>> CashAndCards { get; set; } = new List<KeyValuePairObject<string, decimal>>();
    }

    public class CashCardMonthlyResponse : StatusDTO
    {
        public CashCardMonthlyResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<KeyValuePairObject<string, CashCardsResponse>> CashAndCards { get; set; }
    }
    public class CashCardDbResponse
    {
        public string CardName { get; set; }
        public decimal CardAmount { get; set; }
    }

    public class CashCardTypesRes : StatusDTO
    {
        public List<CashCardDbResponse> CashCardsTypes { get; set; }
    }

    public class CashCardGridList : StatusDTO
    {
        public List<CashCardGridRes> CashCardsGrid { get; set; } = new List<CashCardGridRes>();

        public List<string> GridColNames { get; set; } = new List<string>();
    }


    public class CashCardGridRes : StatusDTO
    {
        public string Date { get; set; }
        public List<CCColResList> ColumnTypes { get; set; } = new List<CCColResList>();
        public decimal Total { get; set; }
        public decimal OutstandingBalance { get; set; }
        public decimal RunningBalance { get; set; }
        public string DepositDate { get; set; }
        public string ReconorClearedDate { get; set; }
        public string Comments { get; set; }

    }

    public class CCColResList
    {

        public string ColName { get; set; }

        public decimal ColAmount { get; set; }
    }
    public class CCGridList : StatusDTO
    {
        public List<CCGridDataResponse> GridData { get; set; } = new List<CCGridDataResponse>();
        public List<CashCardGridLineResponse> CashCardsGridLine { get; set; } = new List<CashCardGridLineResponse>();
    }
    public class CCGridDataResponse : StatusDTO
    {
        public DateTime? SaleDate { get; set; }
        public string Visa_Status { get; set; }
        public string Amex_Status { get; set; }
        public string CashChecks_Status { get; set; }
        public decimal OpeningBalance { get; set; }
        public string Mastercard_Status { get; set; }
        public string VisaMastercard_Status { get; set; }
        public string Discover_Status { get; set; }
        public decimal OtherCards { get; set; }
        public string OtherCards_Status { get; set; }
        public string MiscCreditCard_Status { get; set; }

        // public decimal RoomsAmount { get; set; }
        //public decimal FoodndBevaragesAmount { get; set; }

        public Dictionary<string, decimal> AmountsByLineName { get; set; } = new();
        public Dictionary<string, string> StatusByLineName { get; set; } = new();
        public decimal Visa { get; set; }
        public decimal Mastercard { get; set; }
        public decimal VisaMastercard { get; set; }
        public decimal Discover { get; set; }
        public decimal Amex { get; set; }
        public decimal MiscCreditCard { get; set; }
        public decimal CashChecks { get; set; }
        //  public decimal CardTotalAmount { get; set; }
        //  public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal RunningBalance { get; set; }
        public DateTime? ClearDate { get; set; }
        public DateTime? ReconDate { get; set; }
        public string DailySaleID { get; set; }
        public string LineName { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalOutStandingAmount { get; set; }
        public DateTime? DepositDate { get; set; }
        public string Comments { get; set; }
    }

    public class CashCardGridLineResponse
    {
        public DateTime? SaleDate { get; set; }

        public string Comments { get; set; }
        public string AttachmentID { get; set; }
        public bool ISAttachment { get; set; }

        public Dictionary<string, decimal> AmountsByLineName { get; set; } = new();
        public Dictionary<string, string> StatusByLineName { get; set; } = new();

        public decimal Visa_Rooms { get; set; }
        public decimal Visa_FB { get; set; }
        public string Visa_FB_Status { get; set; }
        public string Visa_Rooms_Status { get; set; }


        public decimal Amex_Rooms { get; set; }
        public decimal Amex_FB { get; set; }
        public string Amex_FB_Status { get; set; }
        public string Amex_Rooms_Status { get; set; }

        public List<DateTime> ReconDates { get; set; }
        public List<DateTime> ClearedDates { get; set; }
        public List<DateTime> DepositDates { get; set; }

        public decimal Master_Rooms { get; set; }
        public decimal Master_FB { get; set; }
        public string Master_FB_Status { get; set; }
        public string Master_Rooms_Status { get; set; }

        public decimal Discover_Rooms { get; set; }
        public decimal Discover_FB { get; set; }
        public string Discover_FB_Status { get; set; }
        public string Discover_Rooms_Status { get; set; }

        public decimal DebitCard { get; set; }
        public decimal MiscCCPayment { get; set; }
        public string DebitCard_Status { get; set; }
        public string MiscCCPayment_Status { get; set; }
        public decimal Visa { get; set; }
        public string Visa_Status { get; set; }
        public decimal VisaMastercard { get; set; }
        public string VisaMastercard_Status { get; set; }
        public decimal Discover { get; set; }
        public string Discover_Status { get; set; }
        public decimal Mastercard { get; set; }
        public string Mastercard_Status { get; set; }
        public decimal Amex { get; set; }
        public string Amex_Status { get; set; }
        public decimal OtherCards { get; set; }
        public string OtherCards_Status { get; set; }
        public decimal CashChecks { get; set; }
        public string CashChecks_Status { get; set; }
        public decimal Cash { get; set; }

        public decimal Checks { get; set; }

        public decimal PaidOuts { get; set; }

        public decimal EFTWirePayments { get; set; }

        public decimal POS_Cash { get; set; }

        public string Cash_Status { get; set; }

        public string Check_Status { get; set; }

        public string PaidOut_Status { get; set; }

        public string EFTWirePayments_status { get; set; }


        public string POS_Cash_Status { get; set; }
        //  public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal TotalOutStandingAmount { get; set; }
        public decimal RunningBalance { get; set; }
        public DateTime? ClearDate { get; set; }
        public DateTime? ReconDate { get; set; }
        public DateTime? DepositDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public string DailySaleID { get; set; }
    }

    //public class TableRow
    //{
    //    public string Date { get; set; }
    //    public decimal Visa { get; set; }
    //    public decimal Mastercard { get; set; }
    //    public decimal Discover { get; set; }
    //    public decimal Amex { get; set; }
    //    public decimal MiscCreditCard { get; set; }
    //    public decimal CashAndChecks { get; set; }
    //    public decimal Total { get; set; }
    //    public decimal OutstandingBalance { get; set; }
    //    public decimal RunningBalance { get; set; }
    //}

    //public class CashndChecksGridColumns
    //{
    //    public string Date { get; set; }
    //    public decimal Cash { get; set; }
    //    public decimal Checks { get; set; }
    //    public decimal PaidOuts { get; set; }
    //    public decimal EFTorWirePayments { get; set; }
    //    public decimal Total { get; set; }
    //    public decimal OutstandingBalance { get; set; }
    //    public decimal RunningBalance { get; set; }
    //    public string DepositDate { get; set; }
    //    public string ReconorClearedDate { get; set; }
    //    public string Comments { get; set; }

    //}

    //public class CashndCardGridDto
    //{
    //    public string Date { get; set; }
    //    public decimal RoomsAmount { get; set; }
    //    public decimal FoodndBevaragesAmount { get; set; }
    //    public decimal Total { get; set; }
    //    public decimal OutstandingBalance { get; set; }
    //    public decimal RunningBalance { get; set; }
    //    public string ReconorClearedDate { get; set; }

    //}

    public class CashAndCardSummaryRespo
    {
        public string CardName { get; set; }
        public int CardType { get; set; }
        public decimal Balance { get; set; }
    }
    public class CashAndCardDetailsRespo
    {
        public string DailyConfigInputEntryID { get; set; }
        public DateTime SaleDate { get; set; }
        public string lineName { get; set; }
        public Decimal Amount { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public Decimal TotalOutStandingAmount { get; set; }
        public Decimal RunningBalance { get; set; }
        public int lineOrder { get; set; }
        public DateTime? ClearedDate { get; set; }
        public DateTime? ReconDate { get; set; }
        public DateTime? DepositDate { get; set; }
        public string Comments { get; set; }
        public int ISAttachment { get; set; }
        public string AttachmentID { get; set; }
        public bool ISConfigExist { get; set; }
    }

    public class CashCardExportDto
    {
        public string SaleDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal RunningBalance { get; set; }
        public string ReconDate { get; set; }
        public string ClearDate { get; set; }
        public string Visa_Status { get; set; }
        public decimal Visa { get; set; }
        public string Mastercard_Status { get; set; }
        public decimal Mastercard { get; set; }
        public string VisaMastercard_Status { get; set; }
        public decimal VisaMastercard { get; set; }
        public string Discover_Status { get; set; }
        public decimal Discover { get; set; }
        public string Amex_Status { get; set; }
        public decimal Amex { get; set; }
        public string OtherCards_Status { get; set; }
        public decimal OtherCards { get; set; }
        public string CashChecks_Status { get; set; }
        public decimal CashChecks { get; set; }
    }

    public class ExcelExportDTO
    {
        public string SaleDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal RunningBalance { get; set; }
        public string ReconDate { get; set; }
        public string ClearDate { get; set; }

        public decimal Visa { get; set; }
        public string Visa_Status { get; set; }

        public decimal Mastercard { get; set; }
        public string Mastercard_Status { get; set; }

        public decimal VisaMastercard { get; set; }
        public string VisaMastercard_Status { get; set; }

        public decimal Discover { get; set; }
        public string Discover_Status { get; set; }

        public decimal Amex { get; set; }
        public string Amex_Status { get; set; }

        public decimal OtherCards { get; set; }
        public string OtherCards_Status { get; set; }

        public decimal CashChecks { get; set; }
        public string CashChecks_Status { get; set; }
    }

    public class CashCardExportRequest
    {
        public string CorpID { get; set; }
        public string StoreID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int BasedOn { get; set; }

        public List<CCGridDataResponse> ExportData { get; set; }

        public string FileName { get; set; }
    }



}
