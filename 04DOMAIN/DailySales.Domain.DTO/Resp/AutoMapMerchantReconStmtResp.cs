using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Resp
{
    public class AutoMapMerchantReconStmtResp :StatusDTO
    {
    }

    public class SaveTranactionsResponse : StatusDTO 
    { 
        public  string ChargeBackJeid {  get; set; }    
        public string feeadjustJeid {  get; set; }  
        public string ExcessMerchantJeid {  get; set; } 
    }

    public class UndoValidationResponse: StatusDTO
    {

    }

    public class UndoImportDbResponse
    {
        public Int32 StatusCode { get;set; }
        public string Message {  get; set; }    
        public string JournalEntryId {  get; set; } 
    }
}
