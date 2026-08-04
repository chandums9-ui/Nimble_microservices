using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class RepayPaymentAndVendorDetails
    {

        public string CorpID { get; set; }

        public string JournalEntryID { get; set; }

        public string TransactionID { get; set;}
        public string VendorID { get; set; }
        public string GroupName {  get; set; }
        public string CustID {  get; set; }
        public string InvNum { get; set; }

        public decimal NetAmount { get; set; }  
        public decimal TotalAmount { get; set; }

        public DateTime? InvDate { get; set; }

        public DateTime? DueDate { get; set; }

        public long BatchNo { get; set; }

        public string PaymentNumber { get; set; }

        public string SSN { get; set; }
        public string FederalID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }

        public string StateCode { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }
        public string CountryCode { get; set; }

        public string overNightCheck {  get; set; }

        public string VID { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }

        //public string Phone { get; set; }

        //public string Email { get; set; }

    }
}
