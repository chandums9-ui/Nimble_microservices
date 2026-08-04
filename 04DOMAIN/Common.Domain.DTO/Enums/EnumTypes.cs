using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common.Domain.DTO.Enums
{
    public enum APBillPaymentAdjust
    {
        BillEntrySaveUpdate = 0,
        BillEntryVoidDelete,
        BillPaymentSaveUpdate,
        BillPaymentVoidDelete,
        DebitMemoSaveUpdate,
        DebitMemoVoidDelete
    }
    public enum PMS
    {
        ONQ = 1,
        Opera = 2,
        Fosse = 3,
        Synxis = 4,
        NiteVision = 5,
        ChoiceAdvantage = 6,
        VisualMatrix = 7,
        DigitalMatrix = 9,
        LightSpeed = 10,
        HotelKey = 12,
        Redistay = 15,
        AnandSystems = 16,
        MarriotFS = 17,
        NewBook = 18,
        AutoClerk = 19,
        DailyImport = 20,
        NewBookPMS = 21,
        Webrezpro = 23,
        CloudBed = 25,
        MSI = 27,
        Roomkey = 29,
        PEP = 30,
        ONQDesktop = 31,
        StayNtouch = 32,
        RoomMaster = 33,
        InnforHMS = 34,
        FiveStar = 35,
        Innroad = 36,
        Mews = 37,
        RDP = 38,
        Gearco = 39,
        Toast = 40,
        CrunchTime = 41,
        StayPMS = 42,
        Clover = 43

    }
    public enum PMSNotRequired
    {
        Required=0,
        NotRequired=1,
    }
    public enum Status
    {
        InActivatedBySuperAdmin = -1,
        InActive = 0,
        Active,
        Approved,
        Delete,
        Denied,
        UnApproved,
        New,
        Accepted,
        Read,
        UnRead,
        ManualMessage = 10,//for messages
        AutomatedMessage,//for automated messages
        CustVendorAccount,
        Misc,
        BySuperAdmin,
        AlreadyRequested,
        Paid,
        [Display(Name = "0x0FA700000000000000000000000000000024")]
        DefaultAccount,
        Trial,
        RegisteredActive,
        FTICFAAccount = 20, //Account status 20 for FT to Other Corps
        Resume = 21,
        Pending, //for email status starts at 22
        Processing,
        Sent,
        NotDelivered,
        Obsolete,
        ICFAStatus = 27,
        DefaultWithDunkin,
        Delete1
    }
    public enum JournalSourceTypes
    {
        BankFeedCheck = 0,
        BankFeedReceipt = 1,
        BankFeedCreditCard = 2,
        GenPayment = 11,
        LoanRePayment,
        EftrpPayment,
        DCPPayment,
        PayBill,
        Paycheck,
        AdHocPayment,
        TaxPayment,
        PayrollAgencyPayment,
        Journal,//20
        Bill,
        Invoice,
        GasPurchase,
        //start--pay roll module do not change wage cost start value 25
        WageCost,
        WageCostGrossWages,
        WageCostGrossDedAgency,
        WageCostTax,
        WageCostNetGrossDedAgency,
        WageCostEmployerTax,
        WageCostGrossPay,
        WageCostNetPay,
        //end--pay roll module ending value 31
        Receipt = 32,
        InvoiceReceipt,
        BankDeposit,
        PFTransfer,
        PFReceived,
        CreditMemo = 37,
        DebitMemo,
        ReceiptDeposit,
        FundTransferTransferredTo = 41,
        PaymentFundTransfer,
        SalesFundsTransfer,
        GasBill = 44,
        BusinessCreditCard = 45,
        BillAdjustment = 46,
        BillDiscount = 47,
        InvoiceReceiptAdjustment = 48,
        InvoiceReceiptDiscount = 49,
        ICAFunds,
        StockAdjust,
        InventoryAdjust,
        GasBillAdjustment = 54,
        GasBillDiscount = 55,
        DailyBankDeposit = 56,
        PeriodicBankDeposit = 57,
        DailyBankDepositShortage = 58,
        CCAdjustment = 59,
        GasPurchaseJournal = 60,
        GPProduct,
        GPTax,
        GPNetAmount,
        SalesDunkInJournalEntry,
        SalesGasJournalEntry,
        SalesJournalPurpose,
        DailyCardSalesJournal,
        SalesJournalMotel1,
        SalesJournalMotel2,
        StoreSettings = 70, //This is just used for opening storesettings user control as popup in transactionpopup
        SalesBKJournalEntry = 71,
        CustOrVendorCentral = 72,
        YearEndProcess = 73,
        CCFee,
        StoredValue,
        ReconcileAlterEntry,
        DCPReceipt,
        MoneyInAndOut,
        DCPPaymentImport,
        // Below mention 100 to 104 are related Recurring.
        RecGenPayment = 100,
        RecJournal,
        RecBill,
        RecInvoice,
        RecReceipt,
        JournalImport,
        DCRImport,
        FundTransferCommon,
        TaxAdjustment,
        BillTransfer,
        JJPurchaseImport = 110,
        JournalFundTransfer,
        JournalFundTransferTo,
        AccrualWageCost,
        AccrualTaxes,
        Accrual = 115,
        AccrualReverse,
        GlConfiguration,
        CreditCardCharges,
        CombinedBillTransfer,
        CombinedBillTransferTo = 120,
        DebitMemoTransfer,
        DebitMemoTransferTo,
        ReceiptTransfer,
        ReceiptTransferTo,
        FranchiseeBill,
        BKInvoiceBill,
        FundTransfer,
        FundTransferTo,
        ReturnTransfer,
        ReturnTransferTo,
        BillPaymentTransfer,
        BillPaymenttransferTo,
        ConsultingWageCost = 133,
        ConsultingEarnings,
        ConsultingAdditions,
        ConsultingDeductions,
        ConsultingEmployeWithHolding,
        ConsultingContribution,
        PayLiabilities,
        IssueCheck,
        CommissionPaybles = 141,
        Timesheet,
        Expense,
        InterCompany,
        Deposit,
        BillImportOrApproval = 148,
        RevJournal = 149,
        DailySales,
        PO,
        BankFeedMatch = 152,
        BankFeed,
        UnApproveJournal = 154,
        UnAppGenPayment,
        UnAppReceipt,
        DSApproved = 157,
        DSJournal,
        DSReceipts,
        LoanScheduleLinkPayments = 160,
        LoanSchedules,
        BillVoid,
        BillPaymentVoid,
        DebitMemoVoid,
        InvoiceVoid = 165,
        CustomerReceiptVoid,
        CreditMemoVoid,
        CheckVoid,
        ReceiptVoid,
        CreditCardVoid,
        JournalVoid,
        FundTransferVoid,
        ReturnTransferVoid,
        FundTransferToVoid,
        ReturnTransferToVoid,
        ProfitCenter = 176,
        Corporation,
        IncStmtCustRptSave,
        JEPayroll,
        JEPayrollVoid,
        AccountsEdit,
        DSCashAndChecks = 181,
        EftPayment = 182,
        DebitMemoApproval,
        ChargebackJournal = 184,
        FeeadjustmentJournal = 185,

    }
    public enum TransactionSourceTypeAddNew
    {
        Customer = 0,
        Vendor,
        Employee,
        Others = 13,
    }
    public enum TransactionSourceType
    {
        Customer = 0,
        Vendor,
        Employee,
        Agency,
        Tax,
        Products,
        Lookups,
        Cards,
        GasTax,
        GasSettingsProducts,
        GasSettingsLookups,
        WholeSalesProduct,
        OverRingProduct,
        Others = 13,
        ImportBankDeposit,
        OtherRevenue,
        ConvenientSales,
        Taxes,
        MoneyInAndOut,
        CashTypes,
        //For temperory  these are starting from 40
        Purpose = 80,
        WageCostPaychecksTotal,
        WageCostAdhocTotal,
        WageCostTaxTotal,
        Adjustment,
        Discount,
        ReconcileAlterEntry,
        Default,
        SalesAccrual,
        Project,
        InvoiceJournal,
        Timesheet,
        Expense,
        //using for Null Source Type ReplaceMent in Stock Adjust  And InventoryAdjust     
        StockAdjust,
        InventoryAdjust,
        ShiftNames,
        PayLiabilityCustom,
        InvoiceAdjustmentAmount,
        PayrollDepartment = 98

    }
    public enum TransactionStatus
    {
        InActive = 0,
        Active,
        Void,
        Delete,
        Pending,
        Completed,
        [Display(Name ="Reconciled")]
        Reconciled,
        Locked,
        RecurringReconciled,
        RecurringLocked,
        ReconcileLocked = 10,
        RecurringReconcileLocked,
        CCFee,
        Paid,
        Unpaid,
        WithoutCC,
        NormalCC,
        ReverseCC,
        GasWithoutAmount,
        [Display(Name = "Resume Reconciled")]
        ResumeReconcile,
        Adjusted = 20,
        Open
    }
    public enum IntercompanyTransferMode
    {
        InterAccounts = 1,
        InterCompanies = 2
    }
    public enum TransactionDebitCredit
    {
        Debit = 0,
        Credit
    }

    public enum AccountTypesEnum
    {
        [DescriptionAttribute("0FA700000000000000000000000000000003")]
        Bank,
        [DescriptionAttribute("0FA700000000000000000000000000000004")]
        AccountsReceiveble,
        [DescriptionAttribute("0FA700000000000000000000000000000005")]
        Inventory,
        [DescriptionAttribute("0FA700000000000000000000000000000006")]
        OtherCurrentAsset,
        [DescriptionAttribute("0FA700000000000000000000000000000007")]
        FixedAsset,
        [DescriptionAttribute("0FA700000000000000000000000000000009")]
        OtherAsset,
        [DescriptionAttribute("0FA700000000000000000000000000000012")]
        AccountsPayable,
        [DescriptionAttribute("0FA700000000000000000000000000000013")]
        CurrentPortionLongTermLiability,
        [DescriptionAttribute("0FA700000000000000000000000000000014")]
        OtherCurrentLiability,
        [DescriptionAttribute("0FA700000000000000000000000000000015")]
        LoanFromShareHolders,
        [DescriptionAttribute("0FA700000000000000000000000000000016")]
        LongTermLiability,
        [DescriptionAttribute("0FA700000000000000000000000000000017")]
        OtherLiability,
        [DescriptionAttribute("0FA700000000000000000000000000000018")]
        Equity,
        [DescriptionAttribute("0FA700000000000000000000000000000020")]
        Income,
        [DescriptionAttribute("0FA700000000000000000000000000000021")]
        COGS,
        [DescriptionAttribute("0FA700000000000000000000000000000022")]
        Expense,
        [DescriptionAttribute("0FA700000000000000000000000000000023")]
        OtherIncome,
        [DescriptionAttribute("0FA700000000000000000000000000000025")]
        CreditCard,
        [DescriptionAttribute("0FA700000000000000000000000000000024")]
        AskAnAccountant,
        [DescriptionAttribute("0FA700000000000000000000000000000008")]
        OtherIntangible
    }
    public enum JournalReferenceTypes
    {
        AccountOpeningBalance = 0
    }
    public static class EnumUtils
    {
        public static string stringValueOf(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static object enumValueOf(string value, Type enumType)
        {
            try
            {
                string[] names = Enum.GetNames(enumType);
                foreach (string name in names)
                {
                    if (stringValueOf((Enum)Enum.Parse(enumType, name)).Equals(value))
                    {
                        return Enum.Parse(enumType, name);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            throw new ArgumentException("The string is not a description or value of the specified enum.");
        }
    }

    public enum UserPrefType
    {
        None = 0,
        IsDailySales,
        IsBill,
        IsPO,
        ViewGridPageSize,
        ViewGridDefault,
        ViewGridRange,
        EnableStatistics,
        EableIncDepWise,
        EableIncGopPer,
        EableIncNOI,
        EnableIncStats,
        IBNOE,
        UnApproveAlerts,
        MissingDailySales,
        GenLedPref,
        ReportRenderType = 16,
        WhiteCheckPrefenceType = 17,
        LegalNamePreferenceType = 18,
        EnableUnclassified,
        IsSSNFederalID,
        IsPLStatsEnable,
        EablePerIncPayroll,
        ShowType,
        ShowNetIncome,
        ShowLayDepTot,
        IsBillDebitMemoDate = 25,
        ShowDisIncExpTot,
        //IsFederalID,
        IsVoid,
        IsAllowVoidDel,
        ShowLegalDBAName,
        AutoMemo
    }

    public enum DailyConfigDepartmentTypeEnum
    {
        [Display(Name = "Cash Receipts")]
        CashReceipts = 7,

        [Display(Name = "Credit Card Receipts")]
        CCReceipts = 8
    }
    public enum BankFeedTransTypeEnum
    {
        Receipt = 0,
        Payments = 1
    }
    public enum TransactionMode
    {
        Cash = 1,
        Check,
        ACHDebit,
        DirectDeposit,
        ACHReceipt,
        CreditCardCredit,
        LoanCheck,
        WireTransfer
    }
    public enum DateViewTypeEnum
    {
        [Display(Name = "Daily")]
        Daily = 0,
        [Display(Name = "Monthly")]
        Montly = 1,
        [Display(Name = "Quarterly")]
        Quarterly = 2,
        [Display(Name = "Yearly")]
        Yearly = 3
    }
    public enum ViewGridPreferenceDateTypes
    {
        ThisWeek = 1,
        Today,
        Range,
        ThisMonthToDate,
        ThisMonth,
        ThisYear,
        LastWeek,
        LastYear,
        Since30days,
        ThisQuarterToDate,
        ThisYearToDate,
        Recent,
        LastWeekToDate,
        LastQuarterToDate,
        LastMonth,
        ThisQuarter,
        LastQuarter,
        LastMonthToDate,
        Yesterday,
        ThisWeekToDate,
        LastYearToDate,
        NextWeek,
        NextMonth,
        NextFourWeeks,
        NextQuarter,
        NextYear

    }

    public enum DateFilterTypeEnum
    {
        [Order(3)]
        [Display(Name = "Range")]
        Range = 3,

        [Order(2)]
        [Display(Name = "Today")]
        Today = 2,

        [Order(19)]
        [Display(Name = "Yesterday")]
        Yesterday = 19,

        [Order(12)]
        [Display(Name = "Recent")]
        Recent = 12,

        [Order(9)]
        [Display(Name = "Past 30 Days")]
        Past30days = 9,

        [Order(1)]
        [Display(Name = "This Week")]
        ThisWeek = 1,

        [Order(20)]
        [Display(Name = "This Week-to-Date", ShortName = "WTD")]
        ThisWeektoDate = 20,

        [Order(7)]
        [Display(Name = "Last Week")]
        LastWeek = 7,

        [Order(13)]
        [Display(Name = "Last Week-to-Date")]
        LastWeektoDate = 13,

        [Order(22)]
        [Display(Name = "Next Week")]
        NextWeek = 22,

        [Order(24)]
        [Display(Name = "Next Four Weeks")]
        NextFourWeeks = 24,

        [Order(5)]
        [Display(Name = "This Month")]
        ThisMonth = 5,

        [Order(4)]
        [Display(Name = "This Month-to-Date", ShortName = "MTD")]
        ThisMonthtoDate = 4,

        [Order(15)]
        [Display(Name = "Last Month")]
        LastMonth = 15,

        [Order(18)]
        [Display(Name = "Last Month-to-Date")]
        LastMonthtoDate = 18,

        [Order(23)]
        [Display(Name = "Next Month")]
        NextMonth = 23,

        [Order(16)]
        [Display(Name = "This Quarter")]
        ThisQuarter = 16,

        [Order(10)]
        [Display(Name = "This Quarter-to-Date", ShortName = "QTD")]
        ThisQuartertoDate = 10,

        [Order(17)]
        [Display(Name = "Last Quarter")]
        LastQuarter = 17,

        [Order(14)]
        [Display(Name = "Last Quarter-to-Date")]
        LastQuartertoDate = 14,

        [Order(25)]
        [Display(Name = "Next Quarter")]
        NextQuarter = 25,

        [Order(6)]
        [Display(Name = "This Year")]
        ThisYear = 6,

        [Order(11)]
        [Display(Name = "This Year-to-Date", ShortName = "YTD")]
        ThisYeartoDate = 11,

        [Order(8)]
        [Display(Name = "Last Year")]
        LastYear = 8,

        [Order(21)]
        [Display(Name = "Last Year-to-Date")]
        LastYeartoDate = 21,

        [Order(26)]
        [Display(Name = "Next Year")]
        NextYear = 26
    }



    public enum DateFilterEnum
    {
        [Order(0)]
        [Display(Name = "Range")]
        Range = 0,

        [Order(1)]
        [Display(Name = "Today")]
        Today = 1,

        [Order(2)]
        [Display(Name = "Yesterday")]
        Yesterday,

        [Order(3)]
        [Display(Name = "Recent")]
        Recent,

        [Order(4)]
        [Display(Name = "Past 30days")]
        Past30days,

        [Order(5)]
        [Display(Name = "This Week")]
        ThisWeek,

        [Order(6)]
        [Display(Name = "This Week-to-date", ShortName = "WTD")] //"WTD")]
        ThisWeektoDate,

        [Order(7)]
        [Display(Name = "Last Week")]
        LastWeek,

        [Order(8)]
        [Display(Name = "Last Week-to-Date")]
        LastWeektoDate,

        [Order(9)]
        [Display(Name = "Next Week")] //new
        NextWeek,

        [Order(10)]
        [Display(Name = "Next Four Weeks")] //new
        NextFourWeeks,

        [Order(11)]
        [Display(Name = "This Month")]
        ThisMonth,

        [Order(12)]
        [Display(Name = "This Month-to-date", ShortName = "MTD")] //"MTD")]
        ThisMonthtoDate,

        [Order(13)]
        [Display(Name = "Last Month")]
        LastMonth,

        [Order(14)]
        [Display(Name = "Last Month-to-Date")]
        LastMonthtoDate,

        [Order(15)]
        [Display(Name = "Next Month")] //new
        NextMonth,

        [Order(16)]
        [Display(Name = "This Quarter")]
        ThisQuarter,

        [Order(17)]
        [Display(Name = "This Quarter-to-date", ShortName = "QTD")]//"QTD")]
        ThisQuartertoDate,

        [Order(18)]
        [Display(Name = "Last Quarter")]
        LastQuarter,

        [Order(19)]
        [Display(Name = "Last Quarter-to-Date")]
        LastQuartertoDate,

        [Order(20)]
        [Display(Name = "Next Quarter")]
        NextQuarter,

        [Order(21)]
        [Display(Name = "This Year")]
        ThisYear,

        [Order(22)]
        [Display(Name = "This Year-to-date", ShortName = "YTD")]//"YTD")]
        ThisYeartoDate,

        [Order(23)]
        [Display(Name = "Last Year")]
        LastYear,

        [Order(24)]
        [Display(Name = "Last Year-to-Date")]
        LastYeartoDate,

        [Order(25)]
        [Display(Name = "Next Year")] //new
        NextYear,
    }

    public enum FutureDateFilterEnum
    {
        [Order(0)]
        [Display(Name = "Range")]
        Range = 0,
        // Short-Term Predictions
        [Order(1)]
        [Display(Name = "Next 7 Days")]
        Next7Days = 1,

        [Order(2)]
        [Display(Name = "Next 14 Days")]
        Next14Days = 2,

        [Order(3)]
        [Display(Name = "Next 30 Days")]
        Next30Days = 3, // Default

        [Order(4)]
        [Display(Name = "Next 45 Days")]
        Next45Days = 4,

        [Order(5)]
        [Display(Name = "Next 60 Days")]
        Next60Days = 5,


        // Medium-Term Predictions
        [Order(6)]
        [Display(Name = "Next 90 Days", ShortName = "Next Quarter")]
        Next90Days = 6,

        [Order(7)]
        [Display(Name = "Next 120 Days")]
        Next120Days = 7,

        [Order(8)]
        [Display(Name = "Next 6 Months")]
        Next6Months = 8,


        // Long-Term Predictions
        [Order(9)]
        [Display(Name = "Next Financial Quarter")]
        NextFinancialQuarter = 9,

        [Order(10)]
        [Display(Name = "Next 9 Months")]
        Next9Months = 10,

        [Order(11)]
        [Display(Name = "Next 12 Months", ShortName = "1 Year Forecast")]
        Next12Months = 11
    }


    public enum MiscMasterType
    {
        Job = 1,
        Relation,
        Division,
        VisaStatus,
        [Display(Name = "SendMethod ")]
        SendMethod,
        ShipVia,
        PaymentCard,
        PaymentBank,
        [Display(Name = "BusinessType ")]
        BusinessType,
        AutoPayment,
        Online,
        DirectDeposit,
        Repay
    }
    public enum FrequencyType
    {
        /// <summary>
        /// used for Payments,Receipts
        /// </summary>
        General = 1,
        /// <summary>
        /// Used for Payroll related
        /// </summary>
        Payroll,
        [Display(Name = "CreditDays ")]
        /// <summary>
        /// Used for credit days in Customer,Vendor,Bill Entry,Invoice
        /// </summary>
        CreditDays,
        Frequency
    }

    public enum FundTransferAccountType
    {
        [Display(Name = "Debit Account")]
        DebitAccount = 1,
        [Display(Name = "Credit Account")]
        CreditAccount
    }   
    public enum PrintCheckTypes
    {
        WhiteCheck = 1,
        PrePrinted = 2
    }

    public enum PaymentMethodType
    {
        Check,
        DebitCrad,
        Online,
        NimbleACH,
        NimbleCardPay,
        ManualACH,
        ManualCARDPAY,
        [Display(Name = "Direct Deposit")]
        DirectDeposit,
        [Display(Name = "Repay")]
        Repay,
        [Display(Name = "RepayACH")]
        RepayACH,
        [Display(Name = "RepayVCC")]
        RepayVCC,
        [Display(Name = "RepayCheck")]
        RepayCheck,
        [Display(Name = "EFT")]
        EFT,
    }
    public enum PaymentTypes
    {
        [Display(Name = "Check")]
        Check,
        [Display(Name = "VCard")]
        VCC,
        [Display(Name = "ACH")]
        ACH
    }
    public enum ACHProviderTypeEnum
    {
        Forte=1,
        SwirePay=2
    }
    public enum ACHTypeEnum
    {
        Deactivate = 0,
        BothPayments,
        VendorPayments,
        CustomerPayments,
    }
    public enum SortByEnum
    {
        DefaultSort=-1,
        [Display(Name = "Payee Name")]
        PayeeName,
        [Display(Name = "Books Date")]
        BooksDate,
        [Display(Name = "Due Date")]
        DueDate,
        [Display(Name = "Due Amount")]
        DueAmount,
        [Display(Name = "Bill Date")]
        BillDate
    }
    public enum ACHAccountTypeEnum
    {
        Card = 0,
        FundingSource
    }

    public enum ACHCardTypesEnum
    {
        [Display(Name = "amex")]
        //Amex = 1,
        Amex = 37,
        [Display(Name = "diners")]
        Diners,
        //Discovery,
        [Display(Name = "disc")]
        Discovery = 3,
        [Display(Name = "jcb")]
        JCB,
        //Master,
        [Display(Name = "mast")]
        Master = 5,
        [Display(Name = "visa")]
        Visa = 6,
        Checkings,
        Savings,
    }
    public enum SearchFilterByEnum
    {
        [Display(Name = "Type")]
        Type = 0,
        [Display(Name = "Number")]
        Number = 1,
        [Display(Name = "Payee Name")]
        VendorName = 2,
        [Display(Name = "Account Number")]
        AccountNum = 3,
        [Display(Name = "Pay Method")]
        PayMethod = 4,
        [Display(Name = "Memo")]
        Memo = 5,
        [Display(Name = "Amount")]
        Amount = 6,
        [Display(Name = "Status")]
        Status = 7,
        [Display(Name = "Recieved Via")]
        RecievedVia = 8,
        [Display(Name = "Created by")]
        Createdby = 9,
        [Display(Name = "Assigned To")]
        AssignedTo = 10,
        [Display(Name = "Bank Account")]
        BankAccount = 11,
        [Display(Name = "Pay Mode")]
        PayMode = 12,
        [Display(Name = "Corporation")]
        Corporation = 13,
        [Display(Name = "Journal Type")]
        JournalType = 14,
        [Display(Name = "Description")]
        Description = 15,
    }
    public enum BillSearchFilterOptionsEnum
    {
        [Display(Name ="Starts With")]
        BeginsWith,
        [Display(Name = "Ends With")]
        EndsWith,
        [Display(Name = "Contains")]
        Contains,
        [Display(Name = "Equals")]
        TextEquals,
        [Display(Name = ">=")]
        greaterthan,
        [Display(Name = "<=")]
        Lessthan,
        [Display(Name = "=")]
        IntEquals,
        IDEquals,
        SortAsec,
        SordDesc
    }
    public enum SearchFilterTypeEnum
    {
        All=1,
        Blanks,
        NonBlanks
    }

    public enum PurposeType
    {
        PaymentsOrReceipts = 1,
        DCPDivisionCode,
        Sales,
        ServiceInfo,
        CCExpensesMapping,
        EFTPayment,
        LoanPayment,
        Global,
        BKDivisionCode,
        Inventory
        

    }
    public enum DefPurpose
    {

        [Display(Name = "Misc Purpose")]
        DefPurpose
    }

    public enum DefaultReferenceType
    {
        [DescriptionAttribute("0FC700000000000000000000000000000052")]
        DefaultRefId,
    }

    public enum ImportDocumentTypeNamesEnum
    {
        [Display(Name = "Vendor")]
        Vendor =1,
        [Display(Name = "VendorContract")]
        VendorContract,
        [Display(Name = "Bill")]
        Bill,
        [Display(Name = "BillPay")]
        BillPay,
        [Display(Name = "DirectDeposit")]
        DirectDeposit,
        [Display(Name = "JournalEntry")]
        Journal,
        [Display(Name = "DSCashAndChecks")]
        DSCashAndChecks,
        [Display(Name = "FundTransfer")]
        FundTransfer,
        [Display(Name = "ReturnTransfer")]
        ReturnTransfer
    }

    public enum JournalFilterbyEnum
    {
        [Display(Name = "Entry Number")]
        EntryNumber,
        [Display(Name = "Amount")]
        Amount,
        [Display(Name = "Account")]
        Account,
        [Display(Name = "Description")]
        Description,
        [Display(Name = "Transcation Type")]
        TranscationType,
        [Display(Name = "Status")]
        Status,

    }

    public enum JournalStatusTypesForGrid
    {
        [Display(Name = "All")]
        All = -1,
        [Display(Name = "In Verification")]
        InVerification,
        [Display(Name = "In Approval")]
        InApproval,
        [Display(Name = "Rejected")]
        Rejected,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Voided")]
        Voided,

    }

    /// <summary>
    ///  0---InActive,
    ///  1---Active,
    ///  2---Void,
    ///  3---Delete,
    /// 1000----wagecost(PayrollJournalEntryStatus),
    /// 1100-----paychecks(PayrollJournalEntryStatus),
    /// 1010-----adhoc payment(PayrollJournalEntryStatus),
    /// 1001-----tax payment(PayrollJournalEntryStatus),
    /// 1111-----agency payment(paychecks,adhoc,tax),(PayrollJournalEntryStatus),
    /// As of Null is aslo Active
    /// </summary>
    public enum JournalEntryStatus
    {
        [Display(Name = "In Active")]
        InActive = 0,
        [Display(Name = "Active")]
        Active = 1,
        [Display(Name = "Voided")]
        Void = 2,
        [Display(Name = "Deleted")]
        Delete = 3,
        //[Display(Name = "Wage Cost")]
        //wagecost = 1000,
    }



    public enum EntryApprovalType
    {
        Entry = 1,
        Verification = 2,
        Approval = 3,
    }
    public enum EntryApprovalStatus
    {
        Approved = 0,
        Entry = 1,
        Verification = 2,
        InApproval = 3,
        Rejected = 4,
        UnApproved = 5,
    }
    public enum EntryType
    {
        [Display(Name = "Manual")]
        Manual = 0,
        [Display(Name = "Automatic")]
        Auto = 1,
        Import,
        Migration
    }
    public enum SalutationsEnum
    {
        [Display(Name = "Mr.")]
        Mr = 1,
        [Display(Name = "Ms.")]
        Ms,
        [Display(Name = "Mrs.")]
        Mrs,
        [Display(Name = "Dr.")]
        Dr,
        [Display(Name = "Prof.")]
        Prof,
        [Display(Name = "Rev.")]
        Rev

    }
    public enum WebhookEventTypes
    {
        ClientEvent = 1,
        CorporationEvent,
        PCEvent,
        VendorEvent,
        ContractEvent,
        COAAddEvent,//this event used in ReportConfiguration
        GroupMasterEvent,//this event used in ReportConfiguration
        ChangeMgmtGroupEvent,//this event used in ReportConfiguration
        ReportColumnEvent//this event used in ReportConfiguration
    }

    public enum MerchantReconCardTypes
    {

        Amex = 1,
        Visa = 2,
        Mastercard = 3,
        Discover = 4,
        [Display(Name = "Other Cards")]
        OtherCards = 5,
        [Display(Name = "Visa & Mastercard")]
        VisaMastercard = 6,

    }

    public enum TransactionType
    {
        FeeAdjustment = 1,
        ChargeBack = 2,
        ExcessMerchant = 3,
        ChargeBackReversal = 4,
    }
    public enum SynchEventType : short
    {


        /// <summary>
        /// Event for creating a corporation.
        /// </summary>
        CorporationCreateSynch = 0,

        /// <summary>
        /// Event for updating a corporation.
        /// </summary>
        CorporationUpdateSynch = 1,

        /// <summary>
        /// Event for PC synchronization.
        /// </summary>
        PCSynch = 2,

        /// <summary>
        /// Event for vendor synchronization.
        /// </summary>
        VendorSynch = 3,

        /// <summary>
        /// Event for customer synchronization.
        /// </summary>
        CustomerSynch = 4,

        /// <summary>
        /// Event for employee synchronization.
        /// </summary>
        EmployeeSynch = 5,

        /// <summary>
        /// Event for other synchronization.
        /// </summary>
        OtherSynch = 6,

        /// <summary>
        /// Event for payroll department synchronization.
        /// </summary>
        PayrollDepartmentSynch = 7,

        /// <summary>
        /// Event for income statement group synchronization.
        /// </summary>
        IncomeStmtGroupSynch = 8,

        /// <summary>
        /// Event for income statement department synchronization.
        /// </summary>
        IncomeStmtDeptSynch = 9,

        /// <summary>
        /// Event for income statement synchronization.
        /// </summary>
        IncomeStmtSynch = 10,

        /// <summary>
        /// Event for daily sale line synchronization.
        /// </summary>
        DailySaleLineSynch = 11,

        /// <summary>
        /// Event for accrual synchronization.
        /// </summary>
        AccrualSynch = 12,

        /// <summary>
        /// Event for budget synchronization.
        /// </summary>
        BudgetSynch = 13,

        /// <summary>
        /// Event for forecast synchronization.
        /// </summary>
        ForecastSynch = 14,

        /// <summary>
        /// Event for income department cloning.
        /// </summary>
        SynchIncDeptCloning = 15,

        /// <summary>
        /// Event for income config cloning.
        /// </summary>
        SynchIncConfigCloning = 16,

        /// <summary>
        /// Event for income group cloning.
        /// </summary>
        SynchIncGroupCloning = 17,

        /// <summary>
        /// Event for account cloning.
        /// </summary>
        SynchAccountCloning = 18,

        /// <summary>
        /// Event for daily sale lines cloning.
        /// </summary>
        SynchDailySaleLinesCloning = 19,

        /// <summary>
        /// Event for synch cloning.
        /// </summary>
        SynchCloning = 20,

        /// <summary>
        /// Event for OAccrualSynch
        /// </summary>
        OAccrualSynch = 21,

        ///<summary>
        /// Event for Account Synch
        /// </summary>
        AccountSynch = 22,

        /// <summary>
        /// Event for synchronizing sales data.
        /// </summary>
        SynchSales = 23,

        /// <summary>
        /// Event for synch Accural Bulk.
        /// </summary>
        SynchAccrualBulk = 24,

        /// <summary>
        /// Event for synch More Journals.
        /// </summary>
        SynchMoreJournals = 25,

        /// <summary>
        /// Event for synch cash reconciliation.
        /// </summary>
        SynchCashReconciliation = 26,

        /// <summary>
        /// Event for synch Daily sales AR.
        /// </summary>
        SynchDailySalesAR = 27,

        /// <summary>
        /// Event for synch cash reconciliation.
        /// </summary>
        SynchDailySaleReceipts = 28,

        /// <summary>
        /// Event for vendor cloning to all corporations.
        /// </summary>
        VendorCloningSynch = 29,

        /// <summary>
        /// Event for OTB
        /// </summary>
        OTBSynch = 30,

        /// <summary>
        /// Event for OTB
        /// </summary>
       // OTBDetailsSynch = 31,

        /// <summary>
        /// Event for CRAB
        /// </summary>
        //CorporationBudRoomAvailability_NewSynch = 32,

        /// <summary>
        /// Event for SynchAccrualUnApproved
        /// </summary>
        /// 
        SynchAccrualUnApproved = 33
    }

}
