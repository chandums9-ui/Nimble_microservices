using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class SaveOrEditFundTransferResponse:StatusDTO
    {
        public string FundMainTransactionId { get; set; }
        public string FromJournalEntryId { get; set; }
        public string EntryNumber { get; set; }
    }
}
