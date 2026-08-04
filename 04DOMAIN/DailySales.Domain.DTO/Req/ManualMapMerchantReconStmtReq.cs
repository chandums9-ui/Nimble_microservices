using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Req
{
    public class ManualMapMerchantReconStmtReq
    {
       
            public string ReconUniqId { get; set; }           
            public string ModifiedBy { get; set; }
            public DateTime ModifiedDate { get; set; }          
            public List<MerchantSettlements> MerchantSettlements { get; set; }
    }
        public class MerchantSettlements
    {
            public string TransUniqId { get; set; }
            public DateTime SettlementDate { get; set; }
            public string CardTypes { get; set; }
        //  public decimal SettlementAmount { get; set; }

        public decimal GrossSaleAmount { get; set; }
        public decimal NetSaleAmount { get; set; }

        public decimal ChargebacksOrReturns { get; set; }
          
            public decimal AdjustmentAmount { get; set; }

            public DateTime TransferDate { get; set; }
            public List<ReconciledSales> ReconciledSales { get; set; }
        }

        public class ReconciledSales
        {
            public string RecId { get; set; }
            public string RecDetId { get; set; }
            public string SaleId { get; set; }
            public string TransId { get; set; }

            public decimal Amount { get; set; }
        }


}

