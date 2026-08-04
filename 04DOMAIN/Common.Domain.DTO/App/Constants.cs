using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.App
{
    public class Constants
    {
        //MSG_, RQD_, VLD_, LBL_
        #region Common Messages
        public const string MSG_NO_DATA_FOUND = "No data found";
        public const string MSG_UNAUTH = "UnAuthorized token";
        public const string MSG_BADREQ = "There is a problem in request";
        public const string MSG_UNPROCESSCONTENT = "Validation exception(Request or response can not be processed";
        public const string MSG_DATA_FOUND = "Data found";
        public const string MSG_SUCCESS = "Success";
        public const string MSG_ERROR = "Error occurred";
        public const string MSG_ENDPOINT_ERROR = "There is problem executing current end point";
        public const string MSG_JE_COM_STAT = "Something went wrong please check the information.";
        public const string MSG_PaidBE_AmountReduce = "Bill/DebitMemo amount can't be reduced below the paid/adjusted value.";
        public const string MSG_PaidBE_PayeeNameChange = "Payee name can't be changed once it is paid/adjusted.";


        public const string MSG_FAILED_WENT_WRONG = "Something went wrong";
        public const string MSG_FAILED = "Failed";
        public const string MSG_CORP_REQ = "CorporationID is Required";
        public const string MSG_Exclude_DEL_SUC = "Exclude Setting deleted successfully";
        public const string MSG_DATA_DEL_SUC = "Data deleted successfully";//There is no existing import to remove
        public const string MSG_Vendor_DEL_SUC = "Vendor deleted successfully";
        public const string MSG_DATA_DEL_EXC = "Failed to delete data";
        public const string MSG_DATA_UNDOLAST_UPDATE = "There is no existing import to remove";

        public const string MSG_DATA_LOAD_SUC = "Data fetched successfully";
        public const string MSG_DATA_LOAD_FAIL = "Failed to fetch data";
        public const string MSG_Exclude_ADD_SUC = "Exclude Setting added successfully";
        public const string MSG_Thres_ADD_SUC = "Threshold added successfully";
        public const string MSG_DATA_ADD_SUC = "Data added successfully";
        public const string MSG_DATA_ADD_FAIL = "Failed to add data";
        public const string MSG_DATA_CLONE_SUC = "Data Cloned successfully";
        public const string MSG_DATA_CLONE_FAIL = "Failed to Clone data";
        public const string MSG_Exclude_UPDATE_SUC = "Exclude Setting updated successfully";
        public const string MSG_Thres_UPDATE_SUC = "Threshold updated successfully";
        public const string MSG_DATA_UPDATE_SUC = "Data updated successfully";
        public const string MSG_EFTDATA_UPDATE_SUC = "Format updated successfully";
        public const string MSG_EFTDATA_Add_SUC = "Format added successfully";
        public const string MSG_DATA_UPDATE_FAIL = "Failed to update data";

        public const string MSG_DAYS_EXCEED = "Number of days cannot be greater than 365";
        public const string MSG_ACC_ARCH = "Account archived ";
        public const string MSG_ACC_ARCH_FAIL = "Failed to archive the account ";
        public const string MSG_ARC_ACT_SUC = "Account moved to active ";
        public const string MSG_ARC_ACC_FAIL = "Failed to move account to active ";
        public const string MSG_DATACLEARPOSTED_FAIL = "Clear posted failed due to data delete from books";
        public const string MSG_DATACLEARPOSTED_FAILFORRECONCIL = "Clear posted failed due to entry reconciled/resume reconciled from books";
        public const string MSG_BANKCONNECTION = "This bank has been connected in other property or connection, please check";
        public const string MSG_CHECKING_CONNECTIONMAPPING = "“Do you want to change the provider for this connection";
        public const string MSG_CHECKING_IMPORTMAPPING = "Do you want to switch to connection mode?";
        public const string MSG_CHANGING_NEWCOAMAPPING = "Do you want to change to selected COA, system will refresh all open feeds and Posted transaction will be loaded as per selected COA";
        public const string MSG_CHECKING_ACCMAPPING = "The selected COA is already in use for another session, please select different COA!";
        public const string MSG_SUCCESS_ACCMAPPING = "Account mapping success";
        public const string MSG_SUCCESS_ACC_UNMAPPING = "Unmapping success";
        public const string MSG_CHECKING_ACCCODEEXISTS = "Account code already exists!";
        public const string MSG_CHECKING_SubACCCODTREXISTS = "As there are transactions posted under this account, you cannot create a sub-account for it.";
        public const string MSG_SAVEACC_SUCCESS = "Account saved successfully";
        public const string MSG_SAVEACC_Fail = "Failed to save the account";
        public const string Masked_Balance = "*****.**";
        public const string MSG_INVALID_REQUEST = " The request is invalid or missing required parameters.";
        public const string MSG_AutoSyncFeeds_Success = "Auto sync bank feeds completed successfully.";
        public const string MSG_NO_Pay_Id = "Payment method not found";
        #endregion

        #region Reccuring
        public const string MSG_BL_SAVE = "BillEntry details Saved Successfully";
        public const string MSG_BL_UPD = "BillEntry details Updated Successfully";
        public const string MSG_BL_DEL = "BillEntry details deleted Successfully";
        public const string MSG_DM_SAVE = "Debit Memo Entry details Saved successfully.";
        public const string MSG_DM_UPD = "Debit Memo Entry details Updated successfully.";
        public const string MSG_DM_DEL = "Debit Memo Entry details deleted successfully.";
        #endregion ReccuringEnd
        #region JournalEntry
        // get JournalEntry
        public const string MSG_JE_GET = "Not found journal ID";
        //create or update
        public const string MSG_JE_DCC = "Debit Total and Credit Total is not matched";
        public const string MSG_JE_APC = "APAccounts need to select Vendor from Name List";
        public const string MSG_JE_ARC = "ARAccounts need to select Customer from Name List";
        public const string MSG_JE_CRE = "Saved Successfully ";
        public const string MSG_JE_VOIDJE = "Void Of Journal ";
        public const string MSG_CUSNAME_EXS = "Customer already exists";
        public const string MSG_ONAME_EXS = "Others Name already exists.";
        public const string MSG_ENAME_EXS = "Employee Name already exists.";
        public const string MSG_Title_Req = "Title is Required";
        public const string MSG_Title_Exits = "Title already exists";
        // delete
        public const string MSG_JE_DEL_STAT = "Journal entry Deleted successfully.";
        public const string MSG_JE_DEL_STAT2 = "There are some divisions with reconciled or in resume reconcile mode.So can not delete";
        public const string MSG_JE_CORP_LOCKED = "Alert! Journal Entry is locked for this corporation";
        public const string MSG_JEL_COMMENT_SAVED = "Journal entry comment Saved";
        public const string MSG_JE_APPROVE_SUCESS = "Journal entry approved Successfully";
        public const string MSG_JE_VERIFY_SUCESS = "Journal entry verified Successfully";
        public const string MSG_JE_REJECT_SUCESS = "Journal entry rejected Successfully";
        public const string MSG_JE_CRE_SUCCESS = "Journal entry details saved successfully";
        public const string MSG_JE_UP_SUCCESS = "Journal entry details updated successfully";
        public const string MSG_JE_NOTFOUND = "Journal entry Not Found";

        public const string MSG_JE_Approved = "Journal Entry Already Approved";
        public const string MSG_JE_Verified = "Journal Entry Already Verified";
        #endregion

        #region check
        public const string MSG_AMOUNTS_EQUAL = "Sum of amount fields in transaction details should match total payment amount.";
        public const string MSG_TOBEPRINTED = "To be Printed";
        public const string MSG_TOBEGenerated = "To be Approved";
        public const string MSG_TOBEPaid = "To be Paid";
        public const string MSG_Void = "Void of Check";
        public const string MSG_DUP_CHECKNO = "Check No already exists, Please enter new  Check Num.";
        public const string MSG_Edit_Contract_Check = "Select bills have different Contracts, we should not allow to update multiple contract bills.";
        #endregion

        #region Receipt
        public const string MSG_AMOUNTS_FORRECEIPT_EQUAL = "Sum of amount fields in transaction details should match total receipt amount.";
        public const string MSG_RC_Void = "Void of Reciept";
        #endregion

        #region CreditCard
        public const string MSG_CC_Void = "Void of CreditCard";
        #endregion

        #region BillEntry
        public const string MSG_BLL_FOUND = "Bill List Found";
        public const string MSG_BLL_NOTFOUND = "Not Found Bill List";
        public const string MSG_BLL_OUTSTANDLESS = "Some one already completed this payment; please re-issue the payment after refreshing the bills.";
        public const string MSG_BL_FOUND = "Found Bill";
        public const string MSG_BL_NOTFOUND = "Bill Not Found";
        public const string MSG_BL_VOID = "Void of Bill ";
        public const string MSG_BL_CORP_LOCKED = "Alert! Bill Entry is locked for this corporation";
        public const string MSG_BL_DDLBD = "Due Date cannot be less than Bill Date";
        public const string MSG_BL_COA_VAL = "Select Account from the list at row ";
        public const string MSG_BL_COA_MIN_VAL = "Select Account/Purpose in atleast one row.";
        public const string MSG_BL_DEL_SUCESS = "Bill deleted Successfully";
        public const string MSG_BL_VOID_SUCESS = "Bill voided Successfully";
        public const string MSG_BL_REJECT_SUCESS = "Bill rejected Successfully";
        public const string MSG_DM_REJECT_SUCESS = "DebitMemo rejected Successfully";
        public const string MSG_BL_ALReadyPaid = "A few transaction statuses were changed from a different session. please refresh and try again.";
        public const string MSG_BL_AlreadyUsedBPLink = "Some of the selected bills are not available. they may have been paid, voided, deleted, or does not exist with the amount.";
        public const string MSG_BL_COMMENT_SAVED = "Bill comment Saved";
        public const string MSG_BL_APPROVE_SUCESS = "Bill approved Successfully";
        public const string MSG_BL_VERIFY_SUCESS = "Bill verified Successfully";
        public const string MSG_DM_VERIFY_SUCESS = "DebitMemo verified Successfully";
        public const string MSG_BPLINKSUCCESS_SINGLE= "Bill Payment posted successfully";
        public const string MSG_BPLINKSUCCESS_Multiple = "Bill Payments posted successfully";

        public const string MSG_BL_Filter_Failed = "Failed to apply filters";


        public const string MSG_BL_Approved = "Bill Already Approved";
        public const string MSG_BL_Verified = "Bill Already Verified";

        public const string MSG_BL_VenNotFound = "Vendor Not Found";
        public const string MSG_AMOUNT_NotEqual = "Head Amount and Split distribution total should be equal.";

        #endregion

        #region BillPayment

        public const string MSG_BP_CORP_LOCKED = "Alert! Bill Payment is locked for this corporation";
        public const string MSG_BP_Reconciled = "Reconciled transaction cannot be voided/deleted.";
        public const string MSG_BP_AdjustedFailed = "Alert! Bill Payment is used as adjustment in other bill payments";
        public const string MSG_BP_DEL_SUCESS = "Bill Payment deleted Successfully";
        public const string MSG_BP_VOID_SUCESS = "Bill Payment voided Successfully";
        public const string MSG_BP_JE_NOT_FOUND = "Bill Payment JournalEntry not found";
        public const string MSG_BP_DMAMT_GT_BEAMT = "Sum of Debit Memo Amount cannot be greater or equal than Sum of Bill Amount";
        public const string MSG_BP_NotVendor_Bills = "Vendor Bills Not Found";
        public const string MSG_BP_MismatchOutStand = "Outstanding Amount Mismatched";
        public const string MSG_BP_MultiVendors = "You Can’t make this payment, as multiple vendors were selected.";
        public const string MSG_BP_MultiCorp = "You Can’t make this payment, as multiple payment methods were selected.";
        public const string MSG_EP_Exist_MSG = "Unable to update this transaction, already used in Epayments.";
        public const string MSG_BL_Void_Fail = "Unable to void this transaction, try again.";
        public const string MSG_BBP_REJECT_SUCESS = "Payments rejected Successfully";
        public const string MSG_BBP_REJECT_FAILED = "Payments rejection Failed";
        public const string MSG_BP_REJECT_SUCESS = "Payment rejected Successfully";
        public const string MSG_BP_REJECT_FAILED = "Payment rejection Failed";
        public const string MSG_BBP_APP_SUCESS = "Payments Approved Successfully";
        public const string MSG_BBP_APP_FAILED = "Payments Approval Failed";
        public const string MSG_EPay_APP_FAILED = "This payment already approved in Epayments.Unable to update this transaction.";
        #endregion
        #region DebitMemo
        public const string MSG_DML_FND = "Found DebitMemo List";
        public const string MSG_DML_NFND = "Not Found DebitMemo List";
        public const string MSG_DM_FND = "Found DebitMemo";
        public const string MSG_DM_NFND = "Not Found DebitMemo";
        public const string MSG_DM_VOID = "Void of Debit Memo ";
        public const string MSG_DM_CORP_LOCKED = "Alert! Debit Memo is locked for this corporation";
        #endregion

        #region FileUpload
        public const string RQD_FILE_NAME_TO_DELETE = "Please specify the file name to be deleted";
        public const string RQD_FILE_NAME_TO_DOWNLOAD = "Please specify the file name to be downloaded";
        //upload
        public const string MSG_FILE_UPLOAD_SUCCESS = "File/s uploaded successfully.";
        public const string MSG_FILE_UPLOAD_FAILED = "Failed to upload files";
        public const string MSG_FILE_EXISTS = "{0}";
        //download
        public const string MSG_FILE_DOWNLOAD_SUCCESS = "File/s downloaded successfully.";
        public const string MSG_FILE_DOWNLOAD_FAILED = "Failed to download file/s.";
        //delete
        public const string MSG_FILE_DELETE_SUCCESS = "File/s deleted successfully.";
        public const string MSG_FILE_DELETE_FAILED = "Failed to delete file/s";
        public const string MSG_FILE_DELETE_ERROR = "Error while deleting the file '{0}'.";
        public const string MSG_FILE_UPDATE_DB_ERROR = "Error while updating file in DB.";
        //view
        public const string MSG_FILE_GET_ERROR = "Error in extracting files from AWS.";

        public const string MSG_FILE_DB_UPDTE_ERROR = "Error while saving file '{0}' in DB.";

        public const string MSG_FILE_MAXLIMIT_EXCEEDED = "Total files size exceeded 20 MB (max.limit).";
        public const string MSG_FILE_NO_DATA_FOUND = "No files found.";

        #endregion

        #region FeedRules

        public const string MSG_NAME_REQ = "Name ID is Required";
        public const string MSG_TRANTYP_REQ = "Transaction Type is required.";
        public const string MSG_QUERY_VAL = "Select valid QueryMatchType";
        public const string MSG_PRI_VAL = "Select valid Priority";
        public const string MSG_AMNT_VAL = "Select valid AmountType";
        public const string MSG_ACC_VAL = "Select valid BankAccount";
        public const string MSG_SETUP_REQ = "Setup For is required";
        public const string MSG_DUP_RULE_SETUP = "Setup For already exists for the same account.";
        public const string MSG_DUP_RULE_PRIORITY = "Please change priority, already occupied..!";
        public const string MSG_RULE_SAVE_EXCEPTION = "Failed to {0} rule";
        public const string MSG_RULE_CLONE = " feedule cloned ";
        public const string MSG_RULE_CLONE_FAIL = "Failed to clone feedrule ";


        #endregion

        #region Feed Auto-Sync
        public const string MSG_DEF_SYNCHOUR = "05:00 AM";
        public const int MSG_DEF_TIMEZONE = 3;//EST
        public const string MSG_SYNC_FAIL = "Failed to set up sync time";
        public const string MSG_SYNC_UPDATE = "Sync time updated successfully";
        public const string MSG_SYNC_SAVE = "Sync time set up successful";
        public const string MSG_CONNECTION_FAILED = "Sync failed!!! Please reconnect. (Possible Reasons - Password Change, MFA enabled, Bank connection error, Feeds Fetching error etc..)";

        #endregion

        #region FeedImport

        public const string MSG_ACCNUM_MINLENGTH = "please enter valid account number (min. 4 digits)";
        public const string MSG_DUP_BANK_NAME = "Bank name already exists";
        public const string MSG_EX_ACC = "Account already selected.";
        public const string MSG_DUP_NAME = "Format name already exists";
        public const string MSG_CONN_IMP_SUCCESS = "Account successfully converted to import and new statement imported";
        public const string MSG_IMP_SUC = "Import successful";
        public const string MSG_IMP_FAIL = "Import failed";
        public const string MSG_IMP_INVALID_PROV = "Client initially not registered for import provider";
        public const string MSG_IMP_FAILS_DUETOSAMEFILE = "Import failed! Due to same file getting duplicate transactions.";

        #endregion

        #region FeedAccount

        public const string MSG_IMP = "Import";
        public const string MSG_AUT = "Auto";
        #endregion

        #region FeedTransactions

        public const string MSG_RULE_SUC = "Rules applied successfully";
        public const string MSG_RULE_NULL = "No rules found to apply";
        public const string MSG_TRANS_NULL = "No transactions found to apply rules";
        public const string MSG_RULE_FAIL = "Failed to apply rules";
        public const string MSG_RULE_NOM = "No rules matched";
        public const string MSG_MAP_EX = "Mapping already exists for the feed transaction";
        public const string MSG_MAP_SUC = "Transaction posted successfully";
        #endregion

        #region FeedTransaction Filter messages

        public const string MSG_POST_SUC = "Transaction posted successfully";
        public const string MSG_POST_FAIL = "Failed to post the data";
        public const string MSG_TRAN_IGN = "Transaction has been moved to ignored";
        public const string MSG_TRAN_CLR_MAT = "Nimble transaction cleared successfully";
        public const string MSG_TRAN_CLR_POS = "Posted Nimble Transaction with match reference cleared successfully and Feeds are moved Open";
        public const string MSG_TRAN_COMMON = "Applied common data";
        public const string MSG_TRAN_MOV_OPEN = "Moved to open feeds";
        public const string MSG_TRAN_DEL = "Transaction deleted successfully";


        #endregion

        #region UserMgmt
        public const string MSG_USER_Create = "User create suceesfully";
        public const string MSG_UM_PwdUpdate = "Password update successfully";
        public const string MSG_PWDNotUpdate = "Check your password.";
        public const string MSG_OldPwd = "Password is incorrect please enter valid password";
        public const string MSG_UserDel = "User deleted successfully";
        public const string MSG_UserNotAvailble = "User is not available";
        public const string MSG_UrlNotAvailble = "Url is not available";
        public const string MSG_ClientNotAvailble = "Client is not available";
        public const string MSG_NotValidEmail = "Please enter valid Email";

        #endregion

        #region SendEmail
        public const string MSG_Email_Create = "Email created suceesfully";
        public const string MSG_Email_Send = "Email sent successfully";
        public const string MSG_Email_Create_Fail = "Failed to create email";
        public const string MSG_Email_Send_Fail = "Failed to send email";


        #endregion

        #region UserPrivileges
        public const string MSG_UP_SINGLEMENU = "Privileges saved successfully ";
        public const string MSG_UP_SINGLEMENU_Update = "Privileges update successfully ";
        public const string MSG_UP_NOTADD = "No new privileges to add";
        public const string MSG_UP_MP = "Multiple menus saved successfully";
        public const string MSG_UP_FULL = "Full privileges assigned";
        public const string MSG_UP_NOTAvailable = "User privileges not available";
        public const string MSG_UP_List = "Parent user doesn't have privileges ";
        public const string MSG_UP_HASROLE = "User has a role";
        #endregion

        #region Role
        public const string MSG_RP_CREATE = "User role created successfully";
        public const string MSG_RP_Exists = " Already role exists for the user";
        public const string MSG_RP_UPDATE = "User role updated successfully";
        public const string MSG_RP_NOTUPDATED = "User role not updated";
        public const string MSG_RP_DELETE = "User role deleted successfully";
        public const string MSG_RP_NOTDELETE = "Vendor details cannot be deleted as it is being used.";
        public const string MSG_RP_SINGLEMENU = "User role privileges assigned successfully";
        public const string MSG_RP_SINGLEMENU_UPDATE = "User role privileges updated successfully";
        public const string MSG_RP_MULTIPLEMENU = "Added multiple Privileges";
        public const string MSG_RP_MULTIPLEMENU_ERROR = "No new privileges to add";
        public const string MSG_RP_FULL_ACCESS = "Full privileges assigned";
        public const string MSG_RP_PRIVILEGS = "List of role privileges";
        public const string MSG_RP_FULL_NOT = "User role privileges not available";
        public const string MSG_RP_DONT_PRIVILEGE = "User role don't have privileges";
        public const string MSG_RP_NOT_FOUND = "User role not Found";
        #endregion

        #region UserCorporation and UserPcLink
        public const string MSG_UC_Create = "Corporation saved successfully";
        public const string MSG_UC_NOTCreated = "Corporation not created";
        public const string MSG_UC_MISMATCH = "Corporation is mismatched";
        public const string MSG_UC_Update = "Corporation updated successfully";
        public const string MSG_UC_Delete = "Corporation deleted successfully";
        public const string MSG_UC_Error = "Profit center details cannot be deleted as it is being in use.";

        public const string MSG_PC_Create = "UserPC link saved successfully";
        public const string MSG_PC_Not_Create = "User PC links not saved";
        public const string MSG_PC_Update = "PcID updates successfully";
        public const string MSG_PC_NOTUpdate = "Failed PcID not update";
        public const string MSG_PC_Delete = "PCID deleted successfully";
        public const string MSG_PC_Error = "PCID not deleted ";
        #endregion

        #region FeedTransaction Matching

        public const string MSG_AMOUNT_MISMATCH = " Amounts mismatched";
        public const string MSG_MATCH_SUC = " Transaction/s match success";
        public const string MSG_MATCH_FAIL = " Failed to match some data";
        public const string MSG_DUP_MATCH = " Transaction is already matched";
        #endregion

        #region DailySale
        //create
        public const string MSG_APPROVAL_STATUS = "User Don't have Policy";
        public const string MSG_SALE_EXISTS = "The SaleDate Already Exists";
        public const string MSG_NOT_SAVE = "Unable to create the Daily Sale";
        public const string MSG_NOT_PMSSAVE = "PMS report details saved successfully!!";
        public const string MSG_NOT_PMSUPDATE = "PMS report details updated successfully!!";
        public const string MSG_NOT_PMSNOTSAVE = "Unable to Create the PMS Details";
        //update
        public const string MSG_USER_DONT_HAVE_POLICY = "User Don't have Approval Policy";
        public const string MSG_FAILED_TO_UPDATE = "Failed to update Something went wrong";

        //Delete
        public const string MSG_DELETE_SUCCESS = "Daily Sale Delete Succesfully";
        public const string MSG_OTB_DELETE_SUCCESS = "OTB File Deleted Succesfully";
        public const string MSG_NOT_DELETE = "Not delete something went wrong";
        public const string MSG_ENTRY_NOTAVAILABLE = "The Entry not available";
        //Approve
        public const string MSG_CANNOT_Approve = "Unapproved sales exist for the Previous sale date, you can’t approve !!";
        public const string MSG_USER_NOT_VALID = "Please Provide  valid user";
        //For Private
        public const string MSG_CREATE_DAILYSALE_SUCCESS = "Daily sale created successfully!!";
        public const string MSG_UPDATE_DAILYSALE_SUCCESS = "Daily Sale Updated Successfully";
        public const string MSG_FILES_INFORMATION_SAVE_FAILED = "Files Information Insert Data Failed";
        public const string MSG_FILES_DOWNLOAD_SUCCESS = "Fille Download Successfull";
        //Validate
        public const string MSG_FACILITY_CORPORATION_EXISTS = "Facility Id Not Matching with CorporationID";
        public const string MSG_FACILITY_MATCH = "Facility Id  Existing";
        public const string MSG_FACILITY_CORPORATION_MATCH = "CorporationID Already have FacilityID";
        public const string MSG_SALEDATE_EXISTS = "Daily sale already exists for the selected date. Please check.";
        public const string MSG_APPROVAL_LEVEL_EXISTS = "DailySale Created in Entry or Verification or Approval Level";
        public const string MSG_IMPORT_OTB_SUCCESS = "OTB details imported Successfully  ";
        public const string MSG_IMPORT_OTB_PARTIAL_SUCCESS = "Partial success: Some SaleDate(s) already exist and were skipped: ";
        public const string MSG_ALREADY_IMPORT_MSG = "As already Exists, No OTB Forecast details were saved.";
        public const string MSG_OTB_MEMORIZE_SUCCESS = "OTB Report Memorized Succesfully";
        public const string MSG_OTB_MEMORIZE_FAILURE = "OTB File Memorization Failed";
        public const string MSG_OTB_RESET_SUCCESS = "OTB Report Reset Succesfully";
        public const string MSG_OTB_RESET_FAILURE = "OTB File Reset Failed";
        #endregion

        #region WidgetPrivileges
        public const string MSG_WID_PRIV_NOT_AVAILABLE = "Widgets privileges not available";

        public const string MSG_WID_PRIV_SUCCESS = "Widget privileges saved successFully";
        public const string MSG_WID_PRIV_FAILED = "Widget privileges not Saved ";
        public const string MSG_WID_PARENT_PRIV_NOT_AVAILABLE = "Privileges not available for parent";

        public const string MSG_Dashboard_Privileges = "Dash board privileges updated successfully";

        #endregion

        #region ConfigurationPoup
        public const string MSG_FORMULA_ADDED = "Formula validated successfully";
        public const string MSG_FORMULA_DELETED = "Formula deleted successfully";
        public const string MSG_FORMULA_LOADED = "Formula loaded succesfully";
        #endregion

        #region WidgetFilters
        public const string MSG_WID_DEL_SUCCESS = "Widget deleted successfully";
        public const string MSG_WID_DEL_FAIL = "Failed to delete the widget";
        public const string MSG_WID_NULL = "Widget Id or User Id null in request";
        #endregion

        #region VendorMaster
        public const string MSG_Ven = "Vendor";
        public const string MSG_Exists = "Already exists";
        public const string MSG_NAME = "Name is required";
        public const string MSG_CD_INT_REQ = "CreditDays interval should be valid";
        public const string MSG_BUS_REQ = "Business ID is required";
        public const string ACC_NUM_EXS = "Account number already exists for this vendor";
        public const string MSG_NAME_EXS = "Vendor already exists";
        public const string MSG_SSNFEDID_REQ = "As per the preference selected, Federal ID or SSN is mandatory";
        public const string check = "check";
        public const string whitecheck = "white check";
        public const string PrePrinted = "Pre-Printed";
        public const string PrintCheck = "print check";
        public const string MSG_ACCNum = "Has same account numbers for contract";
        public const string MSG_LOAD_Data_SUC = "Data Found and Audit Entries Saved Successfully";
        public const string MSG_LOAD_Save_Fail = "Data Found but Auidt Entries Failed to Save";
        public const string MSG_VENDOR_UPDATE_SUC = "Vendor information updated successfully";
        public const string MSG_VENDORCON_UPDATE_SUC = "Contract details Updated successfully";
        public const string MSG_PS_EXTS = "Purpose Already Exists";
        public const string MSG_VENDORCON_NOTDELETE = "Vendor Contract details cannot be deleted as it is being used";
        #endregion

        #region VendorACH
        public const string MSG_ACHNEXT = "ACH is not Configured for the Provided Corporation or User";
        public const string MSG_CreateDetailsReq = "CorpID,VendorName,EmailID are Required";
        public const string MSG_ADDNAMEREQ = "Address Name is Required";
        public const string MSG_ADDREQ = "Address is Required";
        public const string MSG_CityREQ = "City is Required";
        public const string MSG_STREQ = "State is Required";
        public const string MSG_CONREQ = "Country is Required";
        public const string MSG_ZPREQ = "Zipcode is Required";
        public const string MSG_CONTREQ = "Contact Number  is Required";
        public const string MSG_ACCTXNREQ = "PgAccountID, TransactionID are Required";
        public const string MSG_LOCORGREQ = "LocationID, OrganizationID are Required";
        public const string MSG_CSTREQ = "Customer Token is Required";
        public const string MSG_ACCNAMEREQ = "Account Holder Name is Required";
        public const string MSG_ACCNUMREQ = "Account Number is Required";
        public const string MSG_RTNUMREQ = "Routing Number is Required";
        public const string MSG_CVVREQ = "CVV is Required";
        public const string MSG_EXPREQ = "Expiry Date is Required";
        public const string MSG_EXPERR = "Expiry date is not correct";
        public const string MSG_SPCC = "SwirePay doesn't have CC Details ";
        public const string MSG_ATREQ = " Adress Token is Required";
        public const string MSG_PMREQ = "Paymethod Token is Required";
        public const string MSG_ACTYPEREQ = "Account Type is Required";
        #endregion

        #region E Payments
        public const string MSG_IDREQ = "ID is required";
        public const string MSG_FIELDS_REQ = "All fields are mandatory";
        public const string MSG_EFT_REQ = "Can't load EFT configurations";
        public const string MSG_ACC_REQ = "Account ID is Required";
        public const string MSG_PT_REQ = "Provider Type is Required";
        public const string MSG_CLName_REQ = "Corp Legal Name is Required";
        public const string MSG_CSName_REQ = "Corp Short Name is Required";
        public const string MSG_BKTEMP_REQ = "Bank Template is Required";
        public const string MSG_BName_REQ = "Bank Name is Required";
        public const string MSG_OrigNo_REQ = "Originator Number is Required";
        public const string MSG_DataCNo_REQ = "Data Center Number is Required";
        public const string MSG_DDtype_REQ = "Service Type is Required";
        public const string MSG_CLID_REQ = "Client ID is Required";
        public const string MSG_CLSECRET_REQ = "Client Secret ID is Required";
        
        public const string MSG_CONFIG_ALERT = "Already Another Provider Type is Configured for the given Corporation and Account";
        public const string MSG_EFTFID_REQ = "Format is Required";
        public const string MSG_SAVE_Data_SUC = "Data and Audit Entries Saved Successfully";
        public const string MSG_SAVE_Data_Fail = "Data and Audit Entries Failed to Save";
        public const string MSG_Audit_Save_Fail = "Data Saved but Auidt Entries Failed to Save";
        public const string MSG_EFTFName_REQ = "Format Name is Required";
        public const string MSG_EFTFIELD_REQ = "Minimum Fields should be included ";
        public const string MSG_NAME_EXISTS = "Format Name Already Exists";
        public const string MSG_FORMAT_USED = "Cannot delete the format as it has already been configured.";
        public const string MSG_CONFIG_UPDATE = "This configuration cannot be edited/updated because there are transactions under processing associated with it";
        public const string MSG_CONFIG_DELETE = "This configuration cannot be deleted because there are pending transactions associated with it";
        public const string MSG_DD_UPDATE = "There are still some bill or payments pending in system you can't switch now. Please check!";
        public const string MSG_DDActive_UPDATE = "This configuration cannot be set to inactive because there are pending transactions associated with it.";
        public const string MSG_DD_EPayErrorConfig = "There are still some payments that are under process and you can't switch now.";
        public const string MSG_Repay_Acc_Validation = "The Account ID is unique for the client and has already been used in another client's configuration. Please choose a different Account ID.";
        public const string MSG_Repay_DuplicateAccId = "Corporation can't have two different account Id's";
        public const string MSG_Repay_DuplicateCustId = "The Customer ID should be unique for configuration and has already been used in another configuration. Please choose a different Customer ID.";
        public const string MSG_Epay_HeadAccountMsg = "You can't pick the Head acoount if there are sub-accounts under it.Please change your section and continue. ";
        #endregion

        #region Dashboard Graphs
        public const int Daily_Department_Graph = 12;
        public const int QTD_Department_Graph = 18;
        #endregion

        #region DefaultValues
        public const string BinaryEmptyID = "000000000000000000000000000000000000";
        #endregion


        #region SystemTokenValues
        public const string ClID = "202420341231005959.sys01.app.nimbleproperty.net";
        public const string ClSecID = "0x453839353846313341414244463030423546";
        public const string SysUserID = "202420340059000000000000000000000000";
        #endregion

        #region FlexandFlowThroughMessages
        public const string MSG_POSITIVE_FLOW = "Positive Flow Through";
        public const string MSG_NEGATIVE_FLOW = "Negative Flow Through";
        public const string MSG_POSITIVE_FLEX = "Positive Flex";
        public const string MSG_NEGATIVE_FLEX = "Negative Flex";
        #endregion

        #region RedisCacheMessages
        public const string MSG_REDISCACHE_DELETE_SUCCESS = "Data successfully deleted from Redis cache.";
        public const string MSG_REDISCACHE_ADD_SUCCESS = "Data successfully added to Redis cache.";
        public const string MSG_REDISCACHE_DELETE_FAIL = "Failed to delete data from Redis cache.";
        public const string MSG_REDISCACHE_ADD_FAIL = "Failed to add data to Redis cache.";
        public const string MSG_REDISCACHE_KEY_EXIST = "Redis cache key already exist.";
        public const string MSG_REDISCACHE_ConnectionLost = "Redis cache connection lost.";

        #endregion

        public const string CHAT_ROOMNAME_REQ = "Room Name is required";
        public const string CHAT_APP_NP = "User doesn't have approval Policy for this Corporation";
        public const string APP_USER_NP = "Approval Policy Users are not Present";
        public const string CHATDATA_SAVE = "Room Created Error Saving Room Data";
        public const string CHAT_GRP_VALIDATION = "Minimum 2 users are required for Room Creation";
        public const string SystemUser = "000000000000000000000000000000000000";

        #region DepartmentMaster

        public const string MSG_DEPTNAME_REQ = "Department Name is Required";
        public const string MSG_DEPTNAME_EXIST = "Department Name Already Exists";
        public const string MSG_DEPARTMENT_EXIST = "Department already deleted.";
        public const string MSG_DEPT_NOTFOUND = "Department not found.";
        public const string MSG_DEPT_ERROR = "An error occurred while processing the request.";
        public const string MSG_DEPT_LEVEL = "Only one level of department hierarchy is allowed.";
        public const string MSG_DEPT_NOTVALID = "Invalid parent department selected.";
        public const string MSG_DEPT_ADD_PASS = "Department added successfully";
        public const string MSG_DEPT_ADD_FAIL = "Failed to add Department";
        public const string MSG_DEPT_DEL_PASS = "Department deleted successfully";
        public const string MSG_DEPT_DEL_FAIL = "Department delete failed";
        public const string MSG_DEPT_UPDATE_PASS = "Department updated successfully";
        public const string MSG_DEPT_UPDATE_FAIL = "Failed to Update Department";
        public const string MSG_DEPT_USED_GRP = "Department is used in group/report config. Delete not allowed.";

        #endregion DepartmentMaster

        #region GroupConfig 
        public const string MSG_GROUPNAME_REQ = "Group Name is Required";
        public const string MSG_GROUPNAME_EXIST = "Group Name Already Exists";
        public const string MSG_GroupConfig_Exceed = "Group nesting cannot exceed 5 levels.";
        public const string MSG_GroupConfig_COA = "COA settings are not allowed for sub-groups.";
        public const string MSG_GroupConfig_CoaSub = "Selected head group has configured sub-group COA(s), not allowed to save.";
        public const string MSG_GroupConfig_CoaRange = "COA ranges overlap within the request.";
        public const string MSG_GroupConfig_Deleted = "GroupConfig already deleted";
        public const string MSG_GroupConfig_NotFound = "GroupConfig not found";
        public const string MSG_GroupConfig_Error = "An error occurred while processing the request.";
        public const string MSG_GroupConfig_NotAllow = "Selected head group has configured with COA's, not allowed to select";
        public const string MSG_GRP_DEL_PASS = "Group deleted successfully";
        public const string MSG_GRP_DEL_FAIL = "Group delete failed";
        public const string MSG_GRP_UPDATE_PASS = "Group updated successfully";
        public const string MSG_GRP_UPDATE_FAIL = "Failed to Update Group";
        public const string MSG_GRP_ADD_PASS = "Group added successfully";
        public const string MSG_GRP_ADD_FAIL = "Failed to add Group";
        public const string MSG_GRP_USED_INREPORT = "Group is used in ReportConfigLines . Delete not allowed.";
        public const string MSG_GRP_AccRange_Required = "AccountRanges are required.";
        public const string MSG_GRP_AccRange = "FromAccountNumber  range must be less than or equalto ToAccountNumber Range";
        public const string MSG_GRP_ParentGrpDel = "For this GroupId has subgroups. Deletion is not allowed when subgroups are present.";
        public const string MSG_GRP_AccNumFrom = "From Account Number should be greater than 0.";
        public const string MSG_GRP_AccNumTo = "To Account Number should be greater than 0.";
        public const string MSG_GRP_AccNumRange = "From Account Number cannot be greater than To Account Number.";

        #endregion
        #region AutoCloneCOA
        public const string MSG_COA_ADD_PASS = "COA added successfully";
        public const string MSG_COA_ADD_FAIL = "Failed to add COA";
        public const string MSG_COA_Update_PASS = "COA updated successfully";
        public const string MSG_COA_Update_Fail = "Faield to update COA";
        public const string MSG_COA_Autoclone_Enabled = "AutoCloneEnable is true, not required  ranges  for clone.";
        public const string MSG_COA_Range_Required = "COA range must be specified when AutoCloneEnable checkbox is disabled.";
        public const string MSG_COA_Duplicate_AccType = "Cannot select the same account type in multiple rows when 'ApplyTo' is All.";
        public const string MSG_COA_Range_AlreadyExists = "Entered COA range is already existing, Please check!";
        public const string MSG_COA_CustomRange = "Both 'Allowed COA From' and 'Allowed COA To' must be provided for custom ranges.";
        public const string MSG_COA_Range = "'Allowed COA From' must be less than or equal to 'Allowed COA To'.";
        public const string MSG_COA_AccTypeMatch = "The Selected COA code doesn't match with the type selected, Please check!";
        public const string MSG_COA_Overlap = "Entered COA range is overlap existing ranges, Please check!";
        public const string MSG_COA_ApplyTO = "'Allowed COA From' and 'Allowed COA To' must be zero when ApplyTo is 'All' selected.";
        public const string MSG_COA_Already_Del = "COA Already deleted";
        public const string MSG_CORP_NotFound = "No corporations found for the selected management group.";
        public const string MSG_MGMT_COAAlert = "As per the selected Group, auto clone rules will be updated and applied on the new COA created henceforth";
        public const string MSG_COA_NotSelect= "Cannot select 'All' with other configurations.";
        #endregion

        #region Report Layout
        public const string MSG_ReportConfig_LayoutDuplicate = "Layout name already exists.";
        public const string MSG_ReportConfig_Duplicate = "Report name already exists.";
        public const string MSG_ReportConfig_Save = "Report configuration saved successfully.";
        public const string MSG_ReportConfig_Update = "Report configuration updated successfully.";
        public const string MSG_ReportConfig_NotFound = "Report config  details not found.";
        public const string MSG_ReportConfig_DeActivate = "The layout has been deactivated.";
        public const string MSG_ReportConfig_Already_Del = "Report Config already deleted";
        public const string MSG_ReportConfig_Budget = "Report Config used in budget,can't be update.";

        public const string MSG_IncStmtSettings_ReportName = "Report Name is mandatory";
        public const string MSG_IncStmtSettings_ReportId = "Please Select Main Report from List";
        public const string MSG_IncStmtSettings_ReportName_Duplicate = "ReportName already exists";
        public const string MSG_IncStmtSettings_Save = "Income Statement customization settings saved successfully.";
        public const string MSG_IncStmtSettings_SaveF = "Income Statement customization settings save failed.";
        public const string MSG_IncStmtSettings_Update = "Income Statement customization settings updated successfully.";
        public const string MSG_IncStmtSettings_Delete = "Income Statement customization settings deleted successfully.";
        public const string MSG_IncStmtSettings_UpdateF = "Income Statement customization settings update failed.";
        public const string MSG_IncStmtSettings_DeleteF = "Income Statement customization settings delete failed.";
        public const string MSG_IncStmtSettings_MainReportF = "Selected main report doesn't exist";
        //public const string MSG_ReportConfig_NotFound = "Report config saved successfully.";
        public const string MSG_ReportGen_Sucess = "Report Generation Layout Loaded Successfully";
        #endregion

        #region WebHook Subscription

        public const string MSG_Subscription_UPDATE = "Subscription updated successfully";
        public const string MSG_Subscription_NOTFOUND = "Subscription not found";
        public const string MSG_Subscription_REGISTERED = "Subscription registered successfully";
        public const string MSG_Subscription_UNREGISTERED = "Subscription unregistered successfully";
        public const string MSG_Subscription_FAILED = "Failed to unregister subscription";
        public const string MSG_Subscription_INVALID = "Invalid Subscription ID";

        #endregion

        #region FundTransfer
        public const string Msg_FundTransfer_Suc = "Fund transfer details saved successfully";
        public const string Msg_ReturnTransfer_Suc = "Return transfer details saved successfully";
        public const string Msg_FundTransfer_Upd = "Journal details updated successfully";
        public const string Msg_ReturnTransfer_Upd = "Journal details updated successfully";
        public const string Msg_FundorReturn_Del= "Journal details deleted successfully";
        public const string Msg_FundTransfer_Void = "Journal details voided successfully";
        public const string Msg_ReturnTransfer_Void = "Journal details voided successfully";








        #endregion
    }
}
