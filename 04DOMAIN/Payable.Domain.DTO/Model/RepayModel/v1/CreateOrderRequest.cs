
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Payable.Domain.DTO.Model.RepayModel.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Payable.Domain.DTO.Model.RepayModel.v1
{
    public class RepayCreateOrderRequest :RepayBaseRequestModel
    {
      
       public Repaygroup group { get; set; }

        [Required]
        public string custId { get; set; }

        [MaxLength(200)]
        public string comments { get; set; }

       // public decimal MyProperty { get; set; }
        public string paymentNumber { get; set; }

        public string misc1 { get; set; }
      
        public RepayVendor vendor { get; set; }

        public string overNightCheck { get; set; }

       

        public List<VenorInvoices> invoices {  get; set; }   
    }

    public class Repaygroup 
    {
        [Required]
        public string name { get; set; }
    }
    public class RepayVendor
    {
        public string vendorNumber { get; set; }

       
        public string locationCode { get; set; }
        public string vendorName1 { get; set; }


        public string vendorPhone { get; set; }

        public string contactEmail { get; set; }
        public RepayVendorAddress address { get; set; } 
    }
    public class RepayPaidVendor
    {
        public string vendorNumber { get; set; }


        public string locationCode { get; set; }
        public string vendorName1 { get; set; }

        public string vendorPhone { get; set; }

        public string email { get; set; }
        public RepayVendorAddress address { get; set; }
    }
    public class RepayVendorAddress
    {
        public string address1 { get; set; }

        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string countryCode { get; set; }
    }

    public class VenorInvoices
    {
        public decimal totalAmount { get; set; }
        public decimal adjustAmount { get; set; }
        public decimal netAmount { get; set; }

        public string invoiceNumber { get; set; }

        public string invoiceDate { get; set; }

        public string dueDate { get; set; }

        public string misc1 { get; set; }
    }
    public class VenorPaidInvoices
    {
        public string netAmount { get; set; }
    

        public string invoiceNumber { get; set; }

        public string invoiceDate { get; set; }
    }

    public class RepayOrderList
    {
        public string PaymentId { get; set; }

        public string PaymentGroupId { get; set; }

        public string PaymentGroupName { get; set; }

        public string ClientName { get; set; }

        public string JsonBody { get; set; }

        public bool IsOrderCreate { get; set; } = true;

    }

    public class RepayOrderFailReq
    {
        public string Request { get; set; }

        public string Response { get; set; }
    }

}
