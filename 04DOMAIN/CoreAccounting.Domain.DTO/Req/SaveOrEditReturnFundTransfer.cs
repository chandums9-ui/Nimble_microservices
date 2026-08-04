using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class SaveOrEditReturnFundTransferReq

    {

        public string FundMainTransactionId { get; set; }   // Null = New, Not Null = Edit 

       // public FundTransferType TransferType { get; set; }  // Return Transfer 

        public DateTime TransferDate { get; set; }

        public string EntryNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public string Memo { get; set; }

        public ReturnFundTransferFromReq TransferFrom { get; set; }

        public List<ReturnFundTransferToReq> TransferTo { get; set; }

    }
    public class ReturnFundTransferFromReq

    {

        public string CorporationId { get; set; }

        /// <summary> 

        /// DEBIT account (reversed) 

        /// </summary> 

        public string DebitAccountId { get; set; }

        public string Name { get; set; }

        /// <summary> 

        /// CREDIT lines (reversed) 

        /// </summary> 

        public List<ReturnFundTransferFromLineReq> CreditLines { get; set; }

        public decimal TotalAmount { get; set; }

    }
    public class ReturnFundTransferFromLineReq

    {

        public string CreditAccountId { get; set; }

        public string Name { get; set; }

        public string Memo { get; set; }

        public decimal Amount { get; set; }

    }
    public class ReturnFundTransferToReq

    {

        public string ToCorporationId { get; set; }

        /// <summary> 

        /// DEBIT account (reversed) 

        /// </summary> 

        public string DebitAccountId { get; set; }

        public string Name { get; set; }

        public decimal TotalAmount { get; set; }

        /// <summary> 

        /// CREDIT lines (reversed) 

        /// </summary> 

        public List<ReturnFundTransferToLineReq> CreditLines { get; set; }

    }

    public class ReturnFundTransferToLineReq

    {

        public string PCID { get; set; }

        public string CreditAccountId { get; set; }

        public string Name { get; set; }

        public string Memo { get; set; }

        public decimal Amount { get; set; }

    }
}
