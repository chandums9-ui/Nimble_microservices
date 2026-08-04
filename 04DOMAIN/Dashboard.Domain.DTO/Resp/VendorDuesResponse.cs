using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class VendorDuesResponse :StatusDTO
    {
        public VendorDuesResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public List<VendorDues> vendorDues { get; set; }=new List<VendorDues>() { };
    }
    public class VendorDues
    {
       
        public string VendorName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate {  get; set; }    
        public int? AgingDays { get; set; }
    }
}
