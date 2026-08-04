using Dashboard.Domain.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class CashandCardWidgetReq
    {
        public string CorpID { get; set; } = string.Empty;

        public int BasedOnStatusOf { get; set; } = (int)CashCardFilter.ReconciledDate;


        public string ProfitCenterID { get; set; }

        public int WidgetBalanceType { get; set; } = 0;

        public decimal year { get; set; } = DateTime.Now.Year;

        public int Month { get; set; } = DateTime.Now.Month;

        public int SelectedCardType { get; set; } = -1;
        public bool IsCardOrLineType { get; set; }

        public long UrlKey { get; set; }

    }

    public class CashandCardSummaryReq
    {
        public string CorpID { get; set; } = string.Empty;
        public string StoreID { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public short BasedOn { get; set; }
        public long UrlKey { get; set; }
        public int? Month { get; set; } = DateTime.Now.Month;
        public decimal? year { get; set; } = DateTime.Now.Year;

    }
    public class CashandCardDetailReq
    {
        public string CorpID { get; set; } = string.Empty;

        public int BasedOn { get; set; } = (int)CashCardFilter.ReconciledDate;

        public string StoreID { get; set; }

        public int WidgetBalanceType { get; set; } = 0;

        public decimal year { get; set; } = DateTime.Now.Year;

        public int Month { get; set; } = DateTime.Now.Month;

        public int CardType { get; set; } = -1;
        public bool IsCardType { get; set; }

        public long UrlKey { get; set; }
        public bool ISConfigExist { get; set; }
        public int IsReconsiled { get; set; } = 0;
        public DateTime? AsOfDate { get; set; }

    }

    public class CardClickInfo
    {
        public bool IsClicked { get; set; } = false;
        public DateTime AsOfDate { get; set; } = DateTime.MinValue;
        public string CardName { get; set; } = string.Empty;
    }

    public class FinancialAnalysisReportReq
    {
        public string CorpID { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Type { get; set; }
        public long UrlKey { get; set; }
    }

}
