using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Domain.Models
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public QueueNames Queues { get; set; }  
        public AIUnapprovedQueueNames AIUnapprovedQNames { get; set; }
        public class QueueNames
        {
            public string MainQueue { get; set; }
            public string DeadLetterQueue { get; set; }
            public string MainExchange { get; set; }
            public string DeadLetterExchange { get; set; }
        }
        public class AIUnapprovedQueueNames
        {
            public string AIUnapprovedMainQueue { get; set; }
            public string AIUnapprovedDeadLetterQueue { get; set; }
            public string AIUnapprovedMainExchange { get; set; }
            public string AIUnapprovedDeadLetterExchange { get; set; }
        }
        #region Corporation

        public CorporationSettings corporationSettings { get; set; }
        public class CorporationSettings
        {
            public CorporationQueues corporationQueues { get; set; }
            public CorporationExchanges corporationExchanges { get; set; }
        }

        public class CorporationQueues
        {
            public string Corporation_MainQueue { get; set; }
            public string Corporation_ReattemptQueue { get; set; }
            public string Corporation_DeadLetterQueue { get; set; }
        }

        public class CorporationExchanges
        {
            public string Corporation_MainExchange { get; set; }
            public string Corporation_ReattemptExchange { get; set; }
            public string Corporation_DeadLetterExchange { get; set; }
        }

        #endregion

        #region PC

        public PCSettings pcSettings { get; set; }
        public class PCSettings
        {
            public PCQueues pcQueues { get; set; }
            public PCExchanges pcExchanges { get; set; }
        }

        public class PCQueues
        {
            public string PC_MainQueue { get; set; }
            public string PC_ReattemptQueue { get; set; }
            public string PC_DeadLetterQueue { get; set; }
        }

        public class PCExchanges
        {
            public string PC_MainExchange { get; set; }
            public string PC_ReattemptExchange { get; set; }
            public string PC_DeadLetterExchange { get; set; }
        }
        #endregion

        #region Vendor

        public VendorSettings vendorSettings { get; set; }
        public class VendorSettings
        {
            public VendorQueues vendorQueues { get; set; }
            public VendorExchanges vendorExchanges { get; set; }
        }
        public class VendorQueues
        {
            public string Vendor_MainQueue { get; set; }
            public string Vendor_ReattemptQueue { get; set; }
            public string Vendor_DeadLetterQueue { get; set; }
        }

        public class VendorExchanges
        {
            public string Vendor_MainExchange { get; set; }
            public string Vendor_ReattemptExchange { get; set; }
            public string Vendor_DeadLetterExchange { get; set; }
        }
        #endregion

        #region Contract

        public ContractSettings contractSettings { get; set; }
        public class ContractSettings
        {
            public ContractQueues contractQueues { get; set; }
            public ContractExchanges contractExchanges { get; set; }
        }
        public class ContractQueues
        {
            public string Contract_MainQueue { get; set; }
            public string Contract_ReattemptQueue { get; set; }
            public string Contract_DeadLetterQueue { get; set; }
        }

        public class ContractExchanges
        {
            public string Contract_MainExchange { get; set; }
            public string Contract_ReattemptExchange { get; set; }
            public string Contract_DeadLetterExchange { get; set; }
        }
        #endregion

        #region Account
        public AccountSettings accountSettings { get; set; }
        public class AccountSettings
        {
            public AccountQueues accountQueues { get; set; }
            public AccountExchanges accountExchanges { get; set; }
        }

        public class AccountQueues
        {
            public string Account_MainQueue { get; set; }
            public string Account_ReattemptQueue { get; set; }
            public string Account_DeadLetterQueue { get; set; }
        }

        public class AccountExchanges
        {
            public string Account_MainExchange { get; set; }
            public string Account_ReattemptExchange { get; set; }
            public string Account_DeadLetterExchange { get; set; }
        }

        #endregion
       
    }

    public class WebhookInfo {
        public string ExternalAPI_SecretKey { get; set; }
        public string Webhook_CacheKey { get; set; }
        
    } 
}
