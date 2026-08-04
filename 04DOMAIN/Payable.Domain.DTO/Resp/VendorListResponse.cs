using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using Payable.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    /// <summary>
    /// Represents the response containing a list of vendors.
    /// </summary>
    public class VendorListResponse: StatusDTO
    {
        public List<VendorDTO> Vendors { get; set; } = new List<VendorDTO>();
    }

    public class VendorNameList
    {
        public string Name { get; set; } = string.Empty;
    }

    public class BillPayLinkVendorsList
    {
        public string VendorID { get; set; } = string.Empty;    
        public string VendorName { get; set; } = string.Empty;  
    }
}
