using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Resp
{
    // Feed Import
    
    public class ImportResponse : StatusDTO
    {
        public ImportResponse()
        {
            base.Status = Constants.MSG_NO_DATA_FOUND;
            base.StatusCode = StatusCodes.Status204NoContent;
        }
        public long ImportID { get; set; }
        public long FeedAccountID { get; set; }
        public List<long> TransactionIDs { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ImportLoadResponse: FeedImportDTO,IStatusDTO
    {
        public ImportLoadResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public long ImportID { get; set; }
        public long TransactionsCount { get; set; }
        public bool IsMultipleImported { get; set; }
        /// <summary>
        /// Transactions From
        /// </summary>
        public DateTime? FromDate { get; set;}
        /// <summary>
        /// Transactions To
        /// </summary>
        public DateTime? ToDate { get; set; }
        /// <summary>
        /// FeedAccount LastSyncDate
        /// </summary>
        public DateTime? LastSyncDate { get; set; } = null;
    }
    public class NimCOAccountCheckResponse : FeedImportDTO, IStatusDTO
    {
        public NimCOAccountCheckResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }

        //format setting
        public class FormatResponse : StatusDTO
    {
        public FormatResponse()
        {
            this.Status = Constants.MSG_NO_DATA_FOUND;
            this.StatusCode = StatusCodes.Status204NoContent;
        }
        public long FormatID { get; set; }
        public string FormatRefId { get; set; }
    }
    public class FormatSettingLoadResponse : FormatSettingDTO, IStatusDTO
    {
        public FormatSettingLoadResponse()
        {
            this.Status = Constants.MSG_NO_DATA_FOUND;
            this.StatusCode = StatusCodes.Status204NoContent;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class FormatSettingsListResponse:StatusDTO
    {
        public FormatSettingsListResponse()
        {
            this.Status = Constants.MSG_NO_DATA_FOUND;
            this.StatusCode = StatusCodes.Status204NoContent;
        }
        public List<ImportSettingsDTO> ImportSettings { get; set; }
        public PageDTO Page { get; set; }=new PageDTO();
    }
    
}
