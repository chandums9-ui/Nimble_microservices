using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    
    public class PandLResponse
    {
        public string CorpName { get; set; }
        public DateTime? GeneratedTime { get; set; }
        public int TotalCount { get; set; }
        public string AccountTypeName { get; set; }
        public long SortOrder { get; set; }
        public string ID { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string ParentAccountName { get; set; }
        public string ParentAccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string AccountTypeID { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal NetOperatingIncome { get; set; }
        public decimal NetOtherIncome { get; set; }
        public decimal NetIncome { get; set; }
        public long DetailOrder { get; set; }
    }

}

