using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public class PMSMappingRedisCacheDTO
    {
        public long PMSCorpMappingID { get; set; }
        public long? PMSMappingID { get; set; }
        public byte[] lineID { get; set; }
        public long? Order { get;set; }
        public long AccountID { get; set; }
        public string  AccountDescription {  get; set; }
        public string PMSCurrencyType { get; set; }
        public int Status { get; set; }
        public string SEQ { get; set; }
        public int DebitCreditMapping { get; set; }
        public string TransactionType { get; set; }
        public bool DebitType { get; set; }

    }
}
