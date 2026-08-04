using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class MicroSysDTO
    {
        public string CORPORATION_ID { get; set; }
        public string CORPORATION_NAME { get; set; }
        public string CORPORATION_ADDRESS { get; set; }
    }
    public class MicroSysAccountDTO
    {
        public string COA_ID { get; set; }
        public string CORPORATION_ID { get; set; }
        public string COA_NAME { get; set; }       
    }
    public class MicroSysAccountBalanceDTO : MicroSysAccountDTO, IBalanceDTO
    {
        public decimal Balance { get; set; }

    }

    public class MicroSysCorporationAccountBalanceDTO : MicroSysAccountBalanceDTO, IModelBaseHeaderDTO
    {
        public string CorpName { get; set; }
        public new DateTime? GeneratedTime { get; set; }
        public int TotalCount { get; set; }
    }
    public class MicroSysVendorNameDTO
    {
        public string CORPORATION_ID { get; set; }
        public string VENDOR_ID { get; set; }
        public string VENDOR_NAME { get; set; }
        public string VENDOR_ADDRESS_LINE1 { get; set; }
        public string VENDOR_CITY { get; set; }
        public string VENDOR_STATE { get; set; }
        public string VENDOR_ZIP { get; set; }
        public string VENDOR_COUNTRY { get; set; }
    }
    public class MicroSysPaymentDetailDTO
    {
        public string CORPORATION_ID { get; set; }
        public string BATCHNO { get; set; }
        public DateTime BATCH_DATE { get; set; }
        public string BANK_ID { get; set; }
        public string BANK_NAME { get; set; }
        public short PAYMENT_TYPE { get; set; }
        public string PAYMENT_ENTRY_ID { get; set; }
        public DateTime PAYMENT_DATE { get; set; }
        public string ERP_VENDOR_ID { get; set; }
        public string CHEQUENO { get; set; }        
        public string REMIT_TO { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public decimal INVOICE_AMOUNT { get; set; }
        public string INVOICE_DESCRIPTION { get; set; }
    }
}
