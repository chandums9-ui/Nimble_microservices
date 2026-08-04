using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class InterCompanyReportResponse : StatusDTO
    {
        public InterCompanyReportHeader Header { get; set; }
        public List<InterCompanyReportEntry> Entries { get; set; } = new List<InterCompanyReportEntry>();
    }

    public class InterCompanyReportHeader
    {
        public string Reportname { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public string FromCorporationId { get; set; }
        public string FromCorporationName { get; set; }
        public string ToCorporationId { get; set; }
        public string ToCorporationName { get; set; }
        public string FromAccountId { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountId { get; set; }
        public string ToAccountName { get; set; }
    }

    public class InterCompanyReportEntry
    {
        public DateTime? TransferDate { get; set; }
        public string EntryNumber { get; set; }
        public string Type { get; set; }
        public string Memo { get; set; }
        public decimal FromCorpAmount { get; set; }
        public decimal ToCorpAmount { get; set; }
        public string FromCorpStatus { get; set; }
        public string ToCorpStatus { get; set; }
        public string FromCorpSplitAccountId { get; set; }
        public string FromCorpSplitAccountName { get; set; }
        public string ToCorpSplitAccountId { get; set; }
        public string ToCorpSplitAccountName { get; set; }

        public string FromJournalEntryId { get; set; }
    }

    public class InterCompanyReportDbResponse
    {
        public string Reportname { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string FromCorporationId { get; set; }
        public string FromCorporationName { get; set; }
        public string FromAccountId { get; set; }
        public string FromAccountName { get; set; }
        public string ToCorporationId { get; set; }
        public string ToCorporationName { get; set; }
        public string ToAccountId { get; set; }
        public string ToAccountName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public string Type { get; set; }
        public DateTime? TransferDate { get; set; }
        public string EntryNumber { get; set; }
        public string Memo { get; set; }
        public string FromCorpSplitAccountId { get; set; }
        public string FromCorpSplitAccountName { get; set; }
        public decimal FromCorpAmount { get; set; }
        public string fromCorpStatus { get; set; }
        public string ToCorpSplitAccountId { get; set; }
        public string ToCorpSplitAccountName { get; set; }
        public decimal ToCorpAmount { get; set; }
        public string ToCorpStatus { get; set; }
        public string FromJournalEntryId { get; set; }
    }
}
