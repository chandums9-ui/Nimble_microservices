using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{
    public class MicroSysRequest
    {
        public List<PaymentStatusDTO> ListInfo { get; set; } = new List<PaymentStatusDTO>();
    }
    public class PaymentStatusDTO
    {
        public string BATCHNO { get; set; }
        public string PAYMENT_ENTRY_ID { get; set; }
        public string? DESCRIPTION { get; set; }        
        public string EFTDATE { get; set; }
        public string PAYMENT_STATUS { get; set; }
    }
    public class PaymentStatusRequest
    {
        public string CorporationID { get; set; }
        [DefaultValue("000000000000000000000000000000000000")]
        public string VendorID { get; set; }
        [DefaultValue("000000000000000000000000000000000000")]
        public string AccountID { get; set; }        
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}
