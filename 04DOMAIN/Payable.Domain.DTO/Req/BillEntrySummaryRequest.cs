using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    public class BillEntrySummaryRequest
    {
       public string UserId {  get; set; }
        public string CorpId {  get; set; }
        public int Type { get; set;}
        
    }

    public class BillSummaryCardReq
    {
        public string UserId { get; set; }
        public string CorpId { get; set; }
    }

    public class PurposeReq
    {
        public string corpId { get; set; }

        public string purposeName { get; set; }

        public string accountId { get; set; }
    }
    public class BillFile
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
    public class UploadBillReq
    {
        public List<BillFile> files { get; set; }
        public string CorpId { get; set; }
        public string Memo {  get; set; }
        public string FileName { get; set; }
        public string clientURL { get; set; }
    }

    public class BillsDataWidgetReq
    {
        public string CorpId { get; set; }
        public string UserID {  get; set; } 
        public DateTime AsOfDate {  get; set; } 
    }

}
