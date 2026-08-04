using Common.Domain.DTO.Model.Base;
using Payable.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class BillEntrySummaryResponse
    {
        public int Type { get; set; }
        public DateTime? Date { get; set; }
        public string Corporation { get; set; }
        public string CorporationName {  get; set; }
        public string Number { get; set; }
        public string PayeeName { get; set; }
        public string AccountNumber { get; set; }
        public string PayMethodName { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; } = 0;
        public DateTime DueDate { get; set; }
    }

    public class BillSummaryCardResp
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }

        public int TransCount { get; set; }
    }
}
