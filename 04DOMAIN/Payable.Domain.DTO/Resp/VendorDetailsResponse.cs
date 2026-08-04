using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class VendorDetailsResponse
    {
        public string CreditDaysID { get; set; }
        public string PaymentMethodID { get; set; }
        public string DefaultContractID { get; set; }
        public string DefaultContractAccNum { get; set; }
        public long UseTaxID { get; set; }
        public string UseTaxName { get; set; }
        public long AddressID { get; set; }
        public string AddressString { get; set; }
        public bool IsActive { get; set; }
        public string VendorAccountID { get; set; }
        public decimal VendorBalance { get; set; } = 0.00m;

        public ContractDetailsResponse ContractDetailsResponse { get; set; }=new ContractDetailsResponse();
    }
}
