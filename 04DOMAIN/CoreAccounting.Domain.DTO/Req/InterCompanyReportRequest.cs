using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class InterCompanyReportRequest
    {
        public string FromCorporationId { get; set; }
        public string ToCorporationId { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; }

    }
}
