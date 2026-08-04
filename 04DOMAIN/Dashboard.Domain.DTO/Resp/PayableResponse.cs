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
    public class PayableResponse : StatusDTO
    {
        public PayableResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<Payables> Payables { get; set; } = new List<Payables>();
    }
    public class Payables : CorpNames
    {

        public decimal Current { get; set; }
        public decimal OneToThirty { get; set; }
        public decimal ThirtyOneToSixy { get; set; }
        public decimal SixtyOneToNinty { get; set; }
        public decimal AboveNinty { get; set; }
    }
    public class PayableAgingDbResponse : Payables
    {
        public int BillCount { get; set; }
        public int CurrentBillCount { get; set; }
        public int OneToThirtyBillCount { get; set; }
        public int ThirtyOneToSixtyBillCount { get; set; }
        public int SixtyOneToNinetyBillCount { get; set; }
        public int AboveNinetyBillCount { get; set; }
    }
    public class PayablesAgingResponse : StatusDTO
    {
        public PayablesAgingResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<PayableAgingDbResponse> AgingDbResponse { get; set; } = new List<PayableAgingDbResponse> { };
    }
    public class NonOperatingARagingResponse : PayableResponse
    {

    }
    public class ARagingTotalsResponse:StatusDTO
    {
        public List<LedgersandAgingTotal> ledgersandAgingTotals { get; set; } = new List<LedgersandAgingTotal>();    

    }
    public class LedgersandAgingTotal: CorpNames
    {
        public decimal TotalReceivableValue {  get; set; }    
    }
}
