using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class VendorMasterDetails
    {
        public string CorporationId { get; set; }
        public string CorporationName { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string AddressID { get; set; }
        public long AddressIDLong { get; set; }
        public string Address {  get; set; }
        public string BusinessType { get; set; }
        public string AccountNumber { get; set; }
        public string FederalID { get; set; }
        public string SSN { get; set; }
        [JsonPropertyName("Is1099")]
        public bool Is1099 { get; set; }
        public bool IsEditAccess { get; set; } = true;
        public decimal TotalDue { get; set; }
        public string PayMethod { get; set; }
        public bool Status { get; set; }
        public long TotalRecords { get; set; }
        public bool HasAttachments { get; set; }
        public int PageCount { get; set; }
    }
}
