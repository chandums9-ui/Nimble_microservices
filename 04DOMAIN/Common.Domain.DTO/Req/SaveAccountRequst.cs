using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{
    public class SaveAccountRequst
    {
        public string CorporationID { get; set; }
        public string AccountTypeID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string ParentAccId { get; set; }
        public Decimal OpeningBalance { get; set; }
        public DateTime? AsOf { get; set; }
    }
}
