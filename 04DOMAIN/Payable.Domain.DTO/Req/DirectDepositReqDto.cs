using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    public class DirectDepositReqDto
    {
        [Required(ErrorMessage = "Select Corporation from the list.")]
        public string Corporation { get; set; }

        public string? CorporationName { get; set; }

        public string VendorCurrencyLocation { get; set; }

        public string Vendor { get; set; }

        public string? VendorName { get; set; }

        public string BankFormat { get; set; }

        public string? ViewTransactions { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
