using Payable.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class TransactionDivisionDetails
    {
        public string TID { get; set; }
        public string PurposeID { get; set; }

        public string StoreID { get; set; }
        public string AccountID { get; set; }
        public string Description { get; set; }        
        public decimal Amount { get; set; }
        public decimal Statistics { get; set; }
        public int Order { get; set; }
        public short? UseTaxID { get; set; }
        public string UseTaxName { get; set; }

        public string AccountName { get; set; }

        public string PurposeName { get; set; }

        public string StoreName { get; set; }   
        public bool IsReconciled { get; set; }
        public List<BillEntryAIInfoDetails> GroupedBillTransactions { get; set; } = new List<BillEntryAIInfoDetails>();
        public bool IsAccountUpdateReq { get; set; } = false;
    }

    public class GetApprovalOrderType
    {
        public string UserID { get; set; }
        public int ApproalOder { get; set; }
        public int ApprovalType { get; set; }
    }
}
