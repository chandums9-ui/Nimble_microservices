using Common.Domain.DTO.App;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class BalanceSheetTableResponse : StatusDTO
    {
        //public List<BalanceSheetTabledbResponse> BalancsheetDetails {  get; set; }
        public BalanceSheetTableResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
            BalanceReports = new List<BalanceSheet>();
        }
        public List<BalanceSheet> BalanceReports { get; set; }
    }
    public class BalanceSheet : CorpNames
    {
        public decimal CurrentCash { get; set; }
        public decimal CurrentReceivables { get; set; }
        public decimal CurrentTotalAssets { get; set; }
        public decimal CurrentPayables { get; set; }
        public decimal CurrentLongTermLiability { get; set; }
        public decimal CurrentTotalLiabilities { get; set; }
        public decimal CurrentTotalEquity { get; set; }
        public decimal CurrentNetIncome { get; set; }
        public decimal LYCash { get; set; }
        public decimal LYReceivables { get; set; }
        public decimal LYTotalAssets { get; set; }
        public decimal LYPayables { get; set; }
        public decimal LYLongTermLiability { get; set; }
        public decimal LYTotalLiabilities { get; set; }
        public decimal LYTotalEquity { get; set; }
        public decimal LYNetIncome { get; set; }

    }

    public class MonthlyBalanceSheetResponse
    {
        public decimal CashandBank { get; set; }
        public decimal Recievables { get; set; }
        public decimal Payables { get; set; }
        public decimal LongTermLiability { get; set; }
        public decimal NetIncome { get; set; }
        public decimal MonthlyTotalAssets { get; set; }
        public decimal MonthlyTotalLiabilities { get; set; }
        public decimal MonthlyTotalEquities { get; set; }
    }
    //public class MonthlyBalanceSheetResponse
    //{
    //    public BalanceItem CashandBank { get; set; }
    //    public BalanceItem Recievables { get; set; }
    //    public BalanceItem Payables { get; set; }
    //    public BalanceItem LongTermLiability { get; set; }
    //    public BalanceItem NetIncome { get; set; }
    //    public BalanceItem MonthlyTotalAssets { get; set; }
    //    public BalanceItem MonthlyTotalLiabilities { get; set; }
    //    public BalanceItem MonthlyTotalEquities { get; set; }
    //}
    public class BalanceItem
    {
        public decimal? Current { get; set; }
        public decimal? Budget { get; set; }
        public decimal? LY { get; set; }
        public decimal? SubValue { get; set; }
        public string SubDisplayValue
        {
            get
            {
                if (SubValue > 1000000)
                    return Math.Round(Convert.ToDecimal(SubValue / 1000000), 1) + "M";
                else if (SubValue > 1000)
                    return Math.Round(Convert.ToDecimal(SubValue / 1000), 1) + "K";
                else
                    return Convert.ToDouble(SubValue).ToString();
            }
        }
        public string DisplayValue
        {
            get
            {
                if (Current > 1000000)
                    return Math.Round(Convert.ToDecimal(Current / 1000000), 1) + "M";
                else if (Current > 1000)
                    return Math.Round(Convert.ToDecimal(Current / 1000), 1) + "K";
                else
                    return Convert.ToDouble(Current).ToString();
            }
        }
    }

    public class BalanceSheetGraphResponse : CorporationInfo, IStatusDTO
    {
        public BalanceSheetGraphResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public decimal TotalAssests { get; set; }
        public decimal TotalLiabilty { get; set; }
        public decimal TotalEquity { get; set; }
        public List<KeyValuePairObject<string, MonthlyReport>> BalanceReports { get; set; } = new List<KeyValuePairObject<string, MonthlyReport>>();
    }

    public class MonthlyReport
    {
        public decimal Assets { get; set; }
        public decimal Liability { get; set; }
        public decimal Equity { get; set; }

    }

    public class BalanceSheetyearDataResponse
    {
        public string Name { get; set; }
        public string MnthName { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAsset { get; set; }
        public decimal TotalLiabilities { get; set; }
        public decimal TotalEquity { get; set; }
    }
    public class BaldbResponse
    {
        public string MonthName { get; set; }
        public decimal Asset { get; set; }
        public decimal Liability { get; set; }
        public decimal Equity { get; set; }
        public int Month { get; set; }

        public int Year { get; set; }
    }
    public class BalanceSheetTabledbResponse
    {
        public string CorporationName { get; set; }
        public string CorpLegalName { get; set; }
        public decimal CashbankCur { get; set; }
        public decimal ReceivablesCur { get; set; }
        public decimal TotalAssetsCur { get; set; }
        public decimal PayablesCur { get; set; }
        public decimal LongtermLiabCur { get; set; }
        public decimal TotalLiabilitiesCur { get; set; }
        public decimal NetincomeCur { get; set; }
        public decimal CashbankBud { get; set; }
        public decimal ReceivablesBud { get; set; }
        public decimal TotalAssetsBud { get; set; }
        public decimal PayablesBud { get; set; }
        public decimal LongtermLiabBud { get; set; }
        public decimal TotalLiabilitiesBud { get; set; }
        public decimal NetincomeBud { get; set; }
        public decimal CashbankLY { get; set; }
        public decimal ReceivablesLY { get; set; }
        public decimal TotalAssetsLY { get; set; }
        public decimal PayablesLY { get; set; }
        public decimal LongtermLiabLY { get; set; }
        public decimal TotalLiabilitiesLY { get; set; }
        public decimal NetincomeLY { get; set; }

    }
}
