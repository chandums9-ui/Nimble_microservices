using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace BankFeed.Domain.Enums
{
    public enum NimbleProvidersEnum
    {
        Plaid = 1,
        Yodlee = 2,
        Import = 3,
        Meld = 4,
    }
    public enum ProvidersConnectTypeEnum
    {
        Connect = 1,
        Reconnect = 2,
        Sync = 3,
        ChangeProvider = 4,
        ConvertToImport = 5,
        AutoSync = 6,
        Import,
        Disconnect,
        Archive,
        ArchiveToActive,
        MergeSettings,
        AccountSettings,
        LinkRequired,
        EditImport

    }

    public enum ConnectionLogStatusEnum
    {
        Old = 0,
        Active = 1,
        Deleted = 3
    }
    public enum BankFeedActualtrTypeEnum
    {
        [Display(Name = "Receipts")]
        Receipts = 0,
        [Display(Name = "Payments")]
        Payments = 1
    }
    public enum BankAccountTypeEnum
    {
        [Display(Name = "Bank")]
        Depository = 1,
        [Display(Name = "Card")]
        Credit = 2,
        [Display(Name = "Loan")]
        Loan = 3,
        [Display(Name = "Other")]
        Other = 4
    }

    public enum BankAccountStatusEnum
    {
        Pending = 0,
        Active = 1,
        LinkRequired = 2,
        Deleted = 3,

        /// <summary>
        /// transactions not found opt for relogin
        /// </summary>
        OptForRelogin = 4,
        /// <summary>
        /// Account archived means not in active
        /// </summary>
        AccountArchived = 5,
        /// <summary>
        /// Relogin Successful but transactions not found
        /// </summary>
        ReloginSuccessfulButNoFeeds = 6,
        /// <summary>
        /// Problem with account opt for relogin from account status
        /// </summary>
        IssueWithAccountOptReloginFromAccountStatus = 7,

        /// <summary>
        /// New account available
        /// </summary>
        ReconnectAvailable = 8,
        DeletedByProvider = 9,
        UnRecoverable = 10,
    }
    public enum AccountStatusFilterEnum
    {
        Active = 1,
        InProgressandFailed = 2,
        Archived = 3,
        ReadyToLink = 4
    }

    public enum FeedTransStatusEnum
    {
        [Display(Name = "Pending")]
        Pending = 0,

        [Display(Name = "Post")]
        NewFeed = 1,

        [Display(Name = "Posted")]
        Posted = 2,

        [Display(Name = "Posted")]
        Matched = 3,

        [Display(Name = "Possible Match")]
        PossibleMatch = 4,

        [Display(Name = "Ignored")]
        Ignored = 5,

        [Display(Name = "BillMatch")]
        BillMatch = 6,

        [Display(Name = "Posted")]
        AutoMatched = 7,

        [Display(Name = "By Possible Match")]
        PostedByPossibleMatch = 8,

        [Display(Name = "By Bill Match")]
        PostedByBillMatch = 9,

        [Display(Name = "RuleModified")]
        RuleModified = 10,

        [Display(Name = "RuleModified")]
        PostedButRuleModified = 11,

        [Display(Name = "Possible Match")]
        DSCashPossibleMatch = 12,

        [Display(Name = "Possible Match")]
        MergePossibleMatch = 13,

        [Display(Name = "Expired")]
        Expired = 14,
    }
    //public enum FeedTransMatchStatusEnum
    //{
    //    [Display(Name = "Pending")]
    //    Pending = 0,

    //    [Display(Name = "Unassinged")]
    //    Unassinged = 1,

    //    [Display(Name = "Match Found")]
    //    MatchFound = 2,

    //    [Display(Name = "Match Found")]
    //    MatchFound1 = 3,

    //    [Display(Name = "Possible Match")]
    //    PossibleMatch = 4,

    //    [Display(Name = "Ignored")]
    //    Ignored = 5,

    //    [Display(Name = "BillMatch Foound")]
    //    BillMatch = 6,

    //    [Display(Name = "Match Found")]
    //    MatchFound2 = 7,
    //}

    #region Account Types
    public enum NimbleAccountTypesEnum
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
    #endregion

    #region FeedTransactionEnums
    public enum StatusCountEnum
    {
        Status1 = -1,
        Status0 = 0,
        Statuss1 = 1,
        Status2 = 2,
        Status3 = 3,
        Status4 = 4,
        Status5 = 5,
        Status6 = 6,
        Status7 = 7,
        Status8 = 8,
        Status9 = 9,
        Status10 = 10,
        Status11 = 11,
        Status12 = 12,
        Status13 = 13,
        Status14 = 14

    }
    public enum StatusTypeFilterEnum
    {
        [Display(Name = "All Open Transaction")]
        openfeeds = 0,

        [Display(Name = "All Transactions With Posted")]
        AllWithPosted = 1,

        [Display(Name = "Possible Matches")]
        PossibleMatches = 2,

        [Display(Name = "Only Posted")]
        OnlyPosted = 3,

        [Display(Name = "Bills Matched")]
        BillsMatch = 4,

        [Display(Name = "Rule Applied")]
        RuleApplied = 5,

        [Display(Name = "Unassigned")]
        UnAssigned = 7,

        [Display(Name = "Ignored")]
        Ignored = 8,
    }

    public enum FeedStatusFilterEnum
    {
        ConfirmFeeds = 0,
        PendingFeeds = 1,
        BothFeeds = 2
    }
    public enum PostTypeFilterEnum
    {
        [Display(Name = "All ")]
        All = 0,

        [Display(Name = "Payments")]
        Payments = 1,

        [Display(Name = "Receipts")]
        Receipts = 2
    }
    
    public enum TransMapTypeEnum
    {
        [Display(Name = "Check ")]
        Check = 11,

        [Display(Name = "Journal ")]
        Journal = 20,

        [Display(Name = "Bill Payment ")]
        BillPay = 15,

        [Display(Name = "Receipt ")]
        Receipt = 32,

        [Display(Name = "Customer Receipt ")]
        CustReceipt = 33,

        [Display(Name = "Credit Card ")]
        CC = 118,       

        [Display(Name = "Fund Transfer ")]
        FundTransfer = 127,

        [Display(Name = "Return Transfer ")]
        ReturnTransfer = 129,

        [Display(Name = "Journal Payroll ")]
        JournalPayroll = 133,

        [Display(Name = "DS Journal")]
        DSJournal = 158,

        [Display(Name = "DS Receipt")]
        DSReceipt = 159,
    }

    /// <summary>
    /// Rule should match AllLines conditions/SingleLine - any one condition
    /// </summary>
    public enum FeedruleQueryMatchTypeEnum
    {
        AllLines = 1,
        SingleLine = 2
    }

    /// <summary>
    /// Rule applied on Description/Amount
    /// </summary>
    public enum FeedruleTypeEnum
    {
        [Display(Name = "Description")]
        Description = 1,
        [Display(Name = "Amount")]
        Amount = 2
    }

    /// <summary>
    /// Rule condition on Description
    /// </summary>
    public enum FeedruleDescriptionFilterTypeEnum
    {
        [Display(Name = "Contains")]
        Contains = 1,
        [Display(Name = "Equal to")]
        Equal = 2,
        [Display(Name = "Doesnt contain")]
        DoesntContain = 3,
        [Display(Name = "Starts with")]
        StartsWith = 4,
        [Display(Name = "Ends with")]
        EndsWith = 5
    }

    /// <summary>
    /// Rule condition on Amount
    /// </summary>
    public enum FeedruleAmountFilterTypeEnum
    {
        [Display(Name = "=")]//Equal to
        Equal = 1,
        [Display(Name = ">=")]//GreaterOrEqual to
        GreaterOrEqual = 2,
        [Display(Name = "<=")]//LessOrEqual to
        LessOrEqual = 3
    }
    #endregion

    /// <summary>
    /// To select amount from Import
    /// </summary>
    public enum ImportAmountFromEnum
    {
        SingleLine = 1,
        DoubleLIne = 2
    }
    public enum TransactionPostType
    {
        [Display(Name = "DEBIT")]
        DEBIT = 0,
        [Display(Name = "CREDIT")]
        CREDIT = 1
    }

    public enum PlaidAccountType
    {
        [Display(Name = "Depository")]
        Depository = 0,
        [Display(Name = "Credit")]
        Credit = 1,
        [Display(Name = "Loan")]
        Loan,
        [Display(Name = "Other")]
        Other

    }
    /// <summary>
    /// To check/read amount data in Import
    /// </summary>
    public enum ImportAmountModeEnum
    {
        WithSigns = 1,
        WithCRDR = 2
    }

    public enum BulkTranOperationEnum
    {
        Post = 1,
        Ignore = 2,
        SelectFromCommonData = 3,
        ClearMatch = 4,
        ClearPosted = 5,
        MoveIgnoreToOpenFeeds = 6,
        Delete = 7
    }

    public enum ProviderStatusEnum
    {
        [Display(Name = "Connected successfully,But No accounts for given provider AccountID")]
        Noaccounts = 0,

        [Display(Name = "Connected successfully,But no transactions to link account")]
        NoTransactions = 1,

        [Display(Name = "No Matching account Id for selected account")]
        NoMatchingaccounts = 2,
        [Display(Name = "Connected successfully, Transactions to link account")]
        Transactions = 3,
        [Display(Name = "Unauthorized")]
        Unauthorized = 4,
    }

    public enum ProviderEnum
    {
        [Display(Name = "Connect Process")]
        Connect = 1,

        [Display(Name = "Re-Connect Process")]
        Reconnect = 2,

        [Display(Name = "Sync Process")]
        Sync = 3,
        [Display(Name = "Auto Sync Process")]
        AutoSync = 4,
        [Display(Name = "Import To ConnectionProcess")]
        ImportToConnProcess = 5,

        [Display(Name = "Webhook")]
        Webhook = 6
    }

    public enum FeedStatusEnum
    {
        Active = 1,
        InprogressFailed = 2,
        Archive = 3,
        ReadyToLink = 4

    }

    public enum MeldConnectionStatusEnum
    {
        /// <summary>
        /// A connection is in progress following its initialization and the customer is in the widget flow. It will remain in this status until it becomes either EXPIRED or ACTIVE
        /// </summary>
        IN_PROGRESS,

        /// <summary>
        ///  A connection becomes expired if it was never completed in the allowed timeframe (4 hours) since its initialization.
        /// </summary>
        EXPIRED,

        /// <summary>
        /// A connection becomes active once the user successfully completes the widget flow and access to the underlying financial accounts is authorized
        /// </summary>
        ACTIVE,

        /// <summary>
        ///  The connection is experiencing temporary issues and cannot be aggregated at this time, but it should resolve on its own. This status may also occur if only some accounts belonging to the connection successfully aggregated, but not all that were requested.
        /// </summary>
        PARTIALLY_ACTIVE,

        /// <summary>
        /// The connection is active and routinely aggregating, but has the option to reconnect. This may occur for connections in which new accounts were detected and can be added (with customer consent), or when service provider access will expire soon, but the connection will still be operational until then.
        /// </summary>
        RECONNECT_AVAILABLE, //newAcc available

        /// <summary>
        /// Indicates that a once ACTIVE connection is now failing and needs to be repaired in order to capture the most up to date data
        /// </summary>
        RECONNECT_REQUIRED,

        /// <summary>
        /// The connection is permanently broken and can no longer be aggregated.
        /// </summary>
        UNRECOVERABLE,

        /// <summary>
        /// A connection that is marked deleted is in a final state. This indicates that it will no longer be updated nor actively billed. 
        /// </summary>
        DELETED,

        /// <summary>
        /// Indicates an unforeseen service provider error, or an internal Meld error. Refreshes and reconnects are allowed as the error's true cause is not yet known
        /// </summary>
        UNDETERMINED,

        /// <summary>
        /// BANK_LINKING_TRANSACTIONS_AGGREGATED
        /// </summary>
        TRANSACTIONS_AGGREGATED,

        /// <summary>
        /// BANK_LINKING_HISTORICAL_TRANSACTIONS_AGGREGATED
        /// </summary>
        HISTORICAL_TRANSACTIONS_AGGREGATED,

    }
}
