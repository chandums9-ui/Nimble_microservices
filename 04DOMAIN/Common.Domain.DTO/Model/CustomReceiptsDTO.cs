using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base
{
    public class CustomReceiptsDTO
    {
        public string Id { get; set; }
        public string CororationID { get; set; }
        public DateTime CreatedDate { get; set; } // entry date
        public string EntryNumber { get; set; }
        public string Memo { get; set; }
        //public string CreatedBy { get; set; }
        public string ReceivedFrom { get; set; }  //AR
        public string BillID { get; set; }
        public string AccountID { get; set; }
        public decimal Amount { get; set; }
        public string ReferenceNumber { get; set; }
    }
}
