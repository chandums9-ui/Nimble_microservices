using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
  

    public class EPayVendorList : StatusDTO
    {
        public List<EPayVendorNames> Vendors { get; set; } = new List<EPayVendorNames>();
    }

    public class EPayVendorNames
    {
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string Corporation { get; set; }
        public string CorporationID { get; set; }
        public string CorporationLegalName { get; set; }
        public decimal BillsAmount { get; set; }
        public decimal DebitMemos { get; set; }
        public decimal Balance { get; set; }
        public long BillsCount { get; set; }
        public long DebitMemoCount { get; set; }
    }
}
