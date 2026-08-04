using BankFeed.Domain.DTO.Model;
using BankFeed.Domain.DTO.Resp;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
    // Format Setting
    public class ImportFormatRequest: FormatSettingDTO
    {
        public int ColumnCount { get; set; }
    }

    // Feed Import
    public class FeedImportRequest: FeedImportDTO, IModelBaseClientID
    {
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        
        public string AccountName { get; set; }
        [Required]
        public decimal EndingBalance { get; set; }
        /// <summary>
        /// Transactions max date or FeedAccount LastSyncDate
        /// </summary>
        public DateTime? LastSyncDate { get; set; } = null;
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Single Line-1 / Double Line-2
        /// </summary>
        public short AmountFrom  { get; set; }
        /// <summary>
        /// With +/-(1) / DR/CR(2)
        /// </summary>
        public short AmountMode { get; set; }
        public List<FeedTransactionRequest> ImportTransactions { get; set; }
       
    }

    public class FeedImportNimbleAccRequest: ModelBaseIDString
    {
        public string CorpID { get; set; }
        public string BankName { get; set; }
        public string FormatName { get; set; }
        public string ClientID { get; set; }
    }
    public class FeedTransactionRequest
    {
        public decimal Amount { get; set; }
        //for single line amount description CR/DR/Credit
        public string DRCR { get; set; }
        /// <summary>
        /// For double line amount CR- Credit / DR - Debit
        /// </summary>
        public string CR { get; set; }
        public string DR { get; set; }
        public string CheckNo { get; set; }
        public DateTime Date { get; set; }
        public string Memo { get; set; }

    }
}
