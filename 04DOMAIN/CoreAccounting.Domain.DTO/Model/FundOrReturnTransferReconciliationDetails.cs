using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Model
{
    

    public class FundOrReturnTransferReconciliationDetails
    {
        public string TransactionID { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }

        public string ReconciliationID { get; set; }
        public string ReconciledDate { get; set; }
        public string RecType { get; set; }
        public bool IsReconciled { get; set; }
        public bool IsResumed { get; set; }
    }
}
