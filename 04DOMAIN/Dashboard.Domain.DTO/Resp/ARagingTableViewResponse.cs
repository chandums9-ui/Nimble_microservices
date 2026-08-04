using Common.Domain.DTO.App;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class ARagingTableViewResponse : StatusDTO
    {
        public List<ARagingResponse> AragingResponses { get; set; } = new List<ARagingResponse>();
    }
    public class ARagingResponse : CorpNames
    {
        //public string CorporationID { get; set; }
        //public string CorporationName { get; set; }
        public List<KeyValuePairObject<string, decimal>> LedgerAmounts { get; set; }
        /// <summary>
        /// coma separated ledger names for Corporation
        /// </summary>
        public string LedgerNames { get; set; }
        /// <summary>
        /// Sum of all ledger amount for the corporation
        /// </summary>
        public decimal TotalLedgerAmount { get; set; }
    }
    //public class ARagingResponse
    //{
    //    public string CorpName { get; set; }
    //    public decimal GuestLedger { get; set; }
    //    public decimal CityLedger { get; set; }
    //    public decimal AdvanceDepositLedger { get; set; }
    //    public decimal CompanyLedger { get; set; }
    //}
}
