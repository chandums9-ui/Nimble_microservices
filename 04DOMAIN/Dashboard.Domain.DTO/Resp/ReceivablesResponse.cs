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
    public class ReceivablesResponse : StatusDTO
    {
        public ReceivablesResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<Receivable> Recievables { get; set; } = new List<Receivable>();
    }
    public class Receivable : CorpNames
    {
        public Int64 CorpKey { get; set; }
        public decimal Balance { get; set; }
        public string LedgerName { get; set; }
        public Int16 ARLedgerType { get; set; }
        public Int16 AdjustLedgerType { get; set; }


    }
}
