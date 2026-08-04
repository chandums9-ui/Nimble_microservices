using BankFeed.App.Contracts;
using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mime;
using Newtonsoft.Json;

using DataModel.Domain.DataModel;
using Azure.Core;
using Microsoft.Identity.Client;
using Common.Domain.DTO.Enums;
using BankFeed.Domain.Enums;

using UserMgmt.Domain.DTO.Req;
using System.ComponentModel;
using BankFeed.Domain.DataModel;
using Common.Domain.DTO.App;
using System.Net;
using System.Xml.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using System.Linq.Expressions;
using Common.App.Contracts;
using RestSharp;
using static Azure.Core.HttpHeader;
using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using Azure;
using System.Data;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace BankFeed.Infra.Services
{
    public class YodleeProviderCalls : IYodleeProvider
    {
        #region Fields

        private readonly ProviderBaseURLInfo providerBaseUrl;
        private readonly YodleeKeyInfo yodleeKeyInfo;
        private readonly YodleeEndPoints yodleeEndPoints;
        private bool disposedValue;
        private ILoggerService logger;
        #endregion

        public YodleeProviderCalls(IOptions<ProviderBaseURLInfo> _providerBaseUrl, IOptions<YodleeKeyInfo> _yodleeKeyInfo, IOptions<YodleeEndPoints> _yodleeEndPoints, ILoggerService _logger)
        {
            this.providerBaseUrl = _providerBaseUrl.Value;
            this.yodleeKeyInfo = _yodleeKeyInfo.Value;
            this.yodleeEndPoints = _yodleeEndPoints.Value;
            this.logger = _logger;
        }
        #region Yodlee

        /// <summary>
        /// To get Authorization token from Yodlee
        /// </summary>
        /// <returns> token </returns>
        public async Task<YodleeTokenInfo> GetAuthToken(string clientID, string clientName, bool isAppToken = false)
        {
            string name = string.Empty;
            if (isAppToken)
                name = yodleeKeyInfo.AdminKey;
            else
                name = clientID + "_" + clientName;
            YodleeTokenInfo tokenInfo = null;
            RestResponse resp = null;
            try
            {
                string url = String.Concat(providerBaseUrl.Yodlee, yodleeEndPoints.authToken);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    request.AddParameter("clientId", yodleeKeyInfo.clientID);
                    request.AddParameter("secret", yodleeKeyInfo.clientSecret);
                    request.AddHeader("loginName", name);
                    request.AddHeader("Api-Version", "1.1");
                    resp = await client.ExecutePostAsync(request);
                    tokenInfo = JsonConvert.DeserializeObject<YodleeTokenInfo>(resp.Content);
                    logger.LogTrace(String.Concat("Auth Token: ", "-->Name :", name));
                }

            }
            catch (Exception ex)
            {
                logger.LogError(String.Concat("Error @GetAuthToken Yodlee: ", "-->Name :", name, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                throw;
            }
            return tokenInfo;
        }

        public async Task<bool> RegisterUser(string ClientID, string ClientName, string token)
        {
            string url = String.Concat(providerBaseUrl.Yodlee, yodleeEndPoints.userRegister);
            string loginName = ClientID + "_" + ClientName;
            string jsonReq = JsonConvert.SerializeObject(new { user = new { loginName = loginName } });
            bool isReady = false;
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {
                    var request = new RestRequest();
                    request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                    request.AddHeader("Api-Version", "1.1");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddJsonBody(jsonReq);
                    resp = await client.ExecutePostAsync(request);
                    YodleeTokenError errorMessages = JsonConvert.DeserializeObject<YodleeTokenError>(resp.Content);
                    isReady = (errorMessages == null);
                    logger.LogTrace(String.Concat("Reg User: ", "-->Name :", loginName));
                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @RegisterUser Yodlee: ", "-->Name :", loginName, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
            }
            //    UriBuilder uri = new UriBuilder(providerBaseUrl.Yodlee);
            //var res = await uri.Uri.AppendPathSegment(yodleeEndPoints.userRegister)
            //     .WithHeaders(new { ContentType = "application/json;charset=UTF-8" })
            //.WithHeader("Api-Version", "1.1").WithHeader("Authorization", "Bearer " + token).WithHeader("cache-control", "no-cache")
            //.PostJsonAsync(new { user = new { loginName = loginName } }).ReceiveString();
            return isReady;
        }

        public async Task<bool> UnRegisterUser(string token)
        {
            string url = String.Concat(providerBaseUrl.Yodlee, yodleeEndPoints.userUnregister);
            bool isReady = false;
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {
                    var request = new RestRequest();
                    request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                    request.AddHeader("Api-Version", "1.1");
                    request.AddHeader("Authorization", "Bearer " + token);
                    resp = await client.DeleteAsync(request);
                    string response = JsonConvert.DeserializeObject<string>(resp.Content);
                    logger.LogTrace(String.Concat("Un Reg User: ", "-->Token :", token));
                    isReady = true;

                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @UnRegisterUser Yodlee: ", "-->Token :", token, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
            }
            //    UriBuilder uri = new UriBuilder(providerBaseUrl.Yodlee);
            //var res = await uri.Uri.AppendPathSegment(yodleeEndPoints.userUnregister)
            //    .WithHeaders(new { ContentType = "application/json;charset=UTF-8" })
            //    .WithHeader("Api-Version", "1.1").WithHeader("Authorization", "Bearer " + token).WithHeader("cache-control", "no-cache")
            //    .DeleteAsync().ReceiveString();
            return isReady;
        }


        // Get Methods

        public async Task<YodleeAccountListDTO> GetAccountsByProviderAccountID(string providerAccountID, string token)
        {
            YodleeAccountListDTO accountInfo = new YodleeAccountListDTO() { StatusCode = StatusCodes.Status500InternalServerError };

            var yStatus = await this.CheckProviderAccountStatus(token, providerAccountID);
            if (yStatus != null && yStatus.StatusCode != StatusCodes.Status200OK)
            {
                if (!string.IsNullOrEmpty(yStatus.Status))
                    accountInfo.Status = yStatus.Status;
                accountInfo.StatusCode = yStatus.StatusCode;
            }
            else if (yStatus != null && yStatus.StatusCode == StatusCodes.Status200OK)
            {
                string url = String.Concat(providerBaseUrl.Yodlee, yodleeEndPoints.account);

                using (var client = new RestClient(url))
                {
                    RestResponse resp = null;
                    try
                    {
                        var request = new RestRequest();
                        request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                        request.AddHeader("Api-Version", "1.1");
                        request.AddHeader("Authorization", "Bearer " + token);
                        request.AddQueryParameter("providerAccountId", providerAccountID);

                        resp = await client.GetAsync(request);
                        accountInfo = JsonConvert.DeserializeObject<YodleeAccountListDTO>(resp.Content);
                        if (accountInfo != null && accountInfo.account.Any())
                        {
                            accountInfo.account = accountInfo.account.Where(t => t.accountStatus.ToUpper() == "ACTIVE").ToArray();
                        }
                        accountInfo.StatusCode = StatusCodes.Status200OK;

                        logger.LogTrace(String.Concat("Accounts Provider ID: ", "-->Prov Acc ID :", providerAccountID));
                        //if (accountInfo != null && accountInfo.account != null)
                        //{
                        //    try
                        //    {
                        //        foreach (var bankAccount in accountInfo.account.ToList())
                        //        {
                        //            logger.LogInfo($"Yodlee Acc ID:{bankAccount.id},Acc.Name {bankAccount.accountName}({bankAccount.accountNumber}), Pro.Name: {bankAccount.providerName}({bankAccount.providerId})- Pro.ACC.ID: {bankAccount.providerAccountId}, Bal: {(bankAccount.balance != null ? bankAccount.balance.amount : 0)}, Aval.Bal: {(bankAccount.availableBalance != null ? bankAccount.availableBalance.amount : 0)}, Curr.Bal: {(bankAccount.currentBalance != null ? bankAccount.currentBalance.amount : 0)}" + Environment.NewLine);
                        //        }
                        //    }
                        //    catch { }
                        //}

                    }
                    catch (Exception ex)
                    {
                        logger.LogError(String.Concat("Error @GetAccountsByProviderAccountID Yodlee: ", "-->Prov Acc ID :", providerAccountID, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                        throw;
                    }
                }
            }

            return accountInfo;
        }

        public async Task<YodleeAccountListDTO> GetAccountDetailsByAccountID(string accountID, string token)
        {
            YodleeAccountListDTO accountInfo = null;
            string url = String.Concat(providerBaseUrl.Yodlee, yodleeEndPoints.account);

            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {
                    var request = new RestRequest();
                    request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                    request.AddHeader("Api-Version", "1.1");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddQueryParameter("accountId", accountID);
                    resp = await client.GetAsync(request);
                    accountInfo = JsonConvert.DeserializeObject<YodleeAccountListDTO>(resp.Content);
                    logger.LogTrace(String.Concat("Accounts Provider ID: ", "-->Acc ID :", accountID));

                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @GetAccountDetailsByAccountID Yodlee: ", "-->Acc ID :", accountID, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
            }

            return accountInfo;

            //try
            //{
            //    UriBuilder uri = new UriBuilder(providerBaseUrl.Yodlee);

            //    accountInfo = await uri.Uri.AppendPathSegment(yodleeEndPoints.account)
            //       .WithHeader("Api-Version", "1.1").WithHeader("Authorization", "Bearer " + token).SetQueryParam("accountId", accountID)
            //       .GetAsync().ReceiveJson<YodleeAccountListDTO>();

            //    return accountInfo;
            //}
            //catch (Exception ex)
            //{
            //    // return null;
            //    throw;
            //}
        }

        private async Task<StatusDTO> CheckProviderAccountStatus(string token, string providerAccountID)
        {
            StatusDTO response = new StatusDTO() { StatusCode = StatusCodes.Status401Unauthorized };
            YodleeProviderAccountStatusDTO providerAccountStatus = null;

            //string endPoint = yodleeEndPoints.providerAccounts + providerAccountID;
            string url = String.Concat(providerBaseUrl.Yodlee, yodleeEndPoints.providerAccounts);
            string[] msgsToCompareRelogin = { "CREDENTIALS_UPDATE_NEEDED", "INCORRECT_CREDENTIALS", "ACCOUNT_LOCKED", "ADDL_AUTHENTICATION_REQUIRED", "CONSENT_REQUIRED", "USER_ACTION_NEEDED_AT_SITE", "NEW_AUTHENTICATION_REQUIRED", "CONSENT_REQUIRED", "CONSENT_EXPIRED", "CONSENT_REVOKED", "MIGRATION_IN_PROGRESS" };
            //"DATA_RETRIEVAL_FAILED", "DATA_NOT_AVAILABLE", "REQUEST_TIME_OUT","DATASET_NOT_SUPPORTED","ENROLLMENT_REQUIRED_FOR_DATASET"
            //"SITE_BLOCKING_ERROR","UNEXPECTED_SITE_ERROR", "SITE_UNAVAILABLE","SITE_NOT_SUPPORTED","SITE_SESSION_INVALIDATED"

            string msgsToCheckLoginProgress = "LOGIN_IN_PROGRESS";
            string errorMessage = string.Empty;
            bool isAccountActive = false;

            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {
                    var request = new RestRequest();
                    request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                    request.AddHeader("Api-Version", "1.1");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddQueryParameter("providerAccountIds", providerAccountID);

                    resp = await client.GetAsync(request);

                    providerAccountStatus = JsonConvert.DeserializeObject<YodleeProviderAccountStatusDTO>(resp.Content);
                    if (providerAccountStatus != null && providerAccountStatus.providerAccount != null && providerAccountStatus.providerAccount.Any())
                    {
                        if (providerAccountStatus.providerAccount.Where(t => t.id.ToString() == providerAccountID).Any())
                        {
                            ProviderAccountDTO proAcc = providerAccountStatus.providerAccount.Where(t => t.id.ToString() == providerAccountID).FirstOrDefault();
                            isAccountActive = proAcc.status.ToUpper() == "FAILED" || proAcc.status.ToUpper() == "USER_INPUT_REQUIRED" ? false : true;
                            if (proAcc.dataset != null && proAcc.dataset.Any())
                                errorMessage = proAcc.dataset.FirstOrDefault().additionalStatus;
                        }
                        else
                        {
                            isAccountActive = false;
                            if (providerAccountStatus.providerAccount.FirstOrDefault().dataset != null && providerAccountStatus.providerAccount.FirstOrDefault().dataset.Any())
                                errorMessage = providerAccountStatus.providerAccount.FirstOrDefault().dataset.FirstOrDefault().additionalStatus;
                        }
                        //isAccountActive = (!string.IsNullOrEmpty(errorMessage) && msgsToCompareRelogin.Contains(errorMessage)) ? false : isAccountActive;// errorMessage == msgsToCheckLoginProgress ? 3 : 1);
                    }
                    if (!isAccountActive)
                    {
                        logger.LogError(String.Concat("Yodlee CheckProviderAccountStatus Req: ", JsonConvert.SerializeObject(request), Environment.NewLine,
                            "Yodlee CheckProviderAccountStatus resp: ", resp.Content, Environment.NewLine, "-------------", Environment.NewLine));
                    }

                    response.StatusCode = isAccountActive ? StatusCodes.Status200OK : StatusCodes.Status401Unauthorized;
                    response.Status = this.GetErrorMsg(errorMessage);

                    logger.LogTrace(String.Concat("Yodlee CheckProviderAccountStatus: ", "-->Prov Acc ID :", providerAccountID, ", Status: ", response.Status));

                    if (response.StatusCode == StatusCodes.Status401Unauthorized && string.IsNullOrEmpty(response.Status))
                        response.Status = Constants.MSG_CONNECTION_FAILED;

                    return response;
                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @CheckProviderAccountStatus: ", "-->Prov Acc ID :", providerAccountID, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
            }

            //UriBuilder uri = new UriBuilder(providerBaseUrl.Yodlee);

            //ProviderAccountDTO providerAccountStatus = await uri.Uri.AppendPathSegment(endPoint)
            //   .WithHeaders(new { ContentType = "application/json;charset=UTF-8" })
            //    .WithHeader("Api-Version", "1.1").WithHeader("Authorization", "Bearer " + token).WithHeader("cache-control", "no-cache")
            //    .GetAsync().ReceiveJson<ProviderAccountDTO>();

            return response;
        }
        private string GetErrorMsg(string errorStatus)
        {
            if(string.IsNullOrEmpty(errorStatus)) 
                return string.Empty;

            switch (errorStatus.Trim())
            {
                //success cases
                case "AVAILABLE_DATA_RETRIEVED":
                    return "All the data available at the provider site is retrieved for the provided dataset.";
                case "PARTIAL_DATA_RETRIEVED":
                    return "Partial data is retrieved for the dataset.";

                //Failure cases
                case "DATA_RETRIEVAL_FAILED":
                    return "Failed to retrieve the data due to unexpected issues.";
                case "DATA_NOT_AVAILABLE":
                    return "The requested data or document is not available at the provider site.";
                case "ACCOUNT_LOCKED":
                    return "The account is locked at the provider site. The user has exceeded the maximum number of incorrect login attempts resulting in the account getting locked.";
                case "ADDL_AUTHENTICATION_REQUIRED":
                    return "Additional MFA information is needed at the provider site or to download the document. Additional verification is required.";
                case "CREDENTIALS_UPDATE_NEEDED":
                    return "Unable to log in to the provider site due to outdated credentials. The site may be prompting the user to change or verify the credentials.";
                case "INCORRECT_CREDENTIALS":
                    return "Unable to log in to the provider site due to incorrect credentials. The credentials that the user has provided are incorrect.";
                case "REQUEST_TIME_OUT":
                    return "The request has timed out due to technical reasons.";
                case "SITE_BLOCKING_ERROR":
                    return "The Yodlee IP is blocked by the provider site.";
                case "UNEXPECTED_SITE_ERROR":
                    return "All errors indicating issues at the provider site, such as the site being down for maintenance.";
                case "SITE_NOT_SUPPORTED":
                    return "Site does not support the requested data or support is not available to complete the requested action. For example, the site is not available. Document download is not supported at the site, etc.";
                case "SITE_UNAVAILABLE":
                    return "The provider site is unavailable due to issues such as the site being down for maintenance.";
                case "TECH_ERROR":
                    return "There is a technical error. ";
                case "USER_ACTION_NEEDED_AT_SITE":
                    return "The errors that require users to take action at the provider site, for example, accept T&C, etc.";
                case "SITE_SESSION_INVALIDATED":
                    return "Multiple sessions or a session is terminated by the provider site.";
                case "NEW_AUTHENTICATION_REQUIRED":
                    return "The site has requested OAuth authentication.";
                case "DATASET_NOT_SUPPORTED":
                    return "The requested datasets are not supported.";
                case "ENROLLMENT_REQUIRED_FOR_DATASET":
                    return "The dataset cannot be retrieved as the user has not enrolled for it.";
                case "CONSENT_REQUIRED":
                    return "Consent is required as the account information is migrated from the credential-based provider to the Open Banking provider site.";
                case "CONSENT_EXPIRED":
                    return "The consent provided by the user to access the account information through Open Banking has expired.";
                case "CONSENT_REVOKED":
                    return "The user has revoked the consent to access the account information through Open Banking.";
                case "MIGRATION_IN_PROGRESS":
                    return "The account information is being migrated from the credential-based provider to the Open Banking provider site.";

                default:
                    return string.Empty;
            }

        }
        public async Task<ProviderAccountDTO> GetproviderAccountsbyProviderId(string providerID, string token)
        {
            ProviderAccountDTO providerAccountStatus = null;

            string url = String.Concat(providerBaseUrl.Yodlee, yodleeEndPoints.providerAccounts);

            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {
                    var request = new RestRequest();
                    request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                    request.AddHeader("Api-Version", "1.1");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddQueryParameter("providerAccountIds", providerID);
                    resp = await client.GetAsync(request);
                    providerAccountStatus = JsonConvert.DeserializeObject<ProviderAccountDTO>(resp.Content);
                    logger.LogTrace(String.Concat("Accounts Provider ID: ", "-->Prov Acc ID :", providerID));
                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Accounts Provider ID: ", "-->Prov Acc ID :", providerID, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
            }
            //string endPoint = yodleeEndPoints.providerAccounts + providerID;
            //UriBuilder uri = new UriBuilder(providerBaseUrl.Yodlee);
            //ProviderAccountDTO providerAccountStatus = await uri.Uri.AppendPathSegment(endPoint)
            // .WithHeaders(new { ContentType = "application/json;charset=UTF-8" })
            //  .WithHeader("Api-Version", "1.1").WithHeader("Authorization", "Bearer " + token).WithHeader("cache-control", "no-cache").SetQueryParam("providerAccountIds", providerID)
            //  .GetAsync().ReceiveJson<ProviderAccountDTO>();
            return providerAccountStatus;
        }
        public async Task<List<YodleeTransaction>> GetTransactions(YodleeAccountDTO bankAccount, string fromDate, string toDate, int? transactiontype, string token)
        {
            string container = transactiontype == Convert.ToInt32(YodleeContainerTypes.bank) ?
                Convert.ToString(YodleeContainerTypes.bank) :
                (transactiontype == Convert.ToInt32(YodleeContainerTypes.creditCard) ?
                Convert.ToString(YodleeContainerTypes.creditCard) :
                (transactiontype == Convert.ToInt32(YodleeContainerTypes.insurance) ?
                Convert.ToString(YodleeContainerTypes.insurance) :
                (Convert.ToString(YodleeContainerTypes.loan))));

            int transCount = await GetTransactionsCount(bankAccount, fromDate, toDate, transactiontype, token);

            decimal totalAllowedCount = 500;
            decimal totCount = Convert.ToDecimal(transCount);
            int pageCount = Convert.ToInt32(Math.Ceiling(totCount / totalAllowedCount));
            //totCount = Convert.ToDecimal(transCount);
            //pageCount = Convert.ToInt32(Math.Ceiling(totCount / totalAllowedCount));
            
            string url = String.Concat(providerBaseUrl.Yodlee, yodleeEndPoints.transactions);
            List<YodleeTransaction> trans = new List<YodleeTransaction>();

            if (pageCount > 0)
            {
                string fileName = String.Concat("Yodlee-", bankAccount.accountName.Replace(" ", "-"), "-", bankAccount.id, "-", Convert.ToDateTime(fromDate).ToString("MM-dd-yyyy"), "_", Convert.ToDateTime(toDate).ToString("MM-dd-yyyy"), "-", DateTime.Now.Ticks);

                RestRequest request = null;
                YodleeTransactionlist transactionsList = null;
                RestResponse resp = null;
                int topRecords = 0;
                string skipRecords = "0";
                long transListCount = 0;

                for (int i = 0; i < pageCount; i++)
                {
                    topRecords = Convert.ToInt32((totCount - (i * totalAllowedCount)) >= totalAllowedCount ? totalAllowedCount : (totCount - (i * totalAllowedCount)));
                    skipRecords = Convert.ToInt32((i * totalAllowedCount)).ToString();

                    using (var client = new RestClient(url))
                    {
                        transactionsList = null;
                        resp = null;
                        request = new RestRequest();

                        try
                        {
                            request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                            request.AddHeader("Api-Version", "1.1");
                            request.AddHeader("Authorization", "Bearer " + token);
                            request.AddQueryParameter("accountId", bankAccount.id);
                            request.AddQueryParameter("fromDate", fromDate);
                            request.AddQueryParameter("toDate", toDate);
                            request.AddQueryParameter("container", container);
                            request.AddQueryParameter("skip", skipRecords);
                            request.AddQueryParameter("top", topRecords);

                            resp = await client.GetAsync(request);
                            if (resp != null && !string.IsNullOrEmpty(resp.Content))
                            {
                                transactionsList = JsonConvert.DeserializeObject<YodleeTransactionlist>(resp.Content);
                                if (transactionsList != null && transactionsList.transaction != null)
                                {
                                    transListCount = transactionsList.transaction.Count;
                                    if (transListCount > 0)
                                        trans.AddRange(transactionsList.transaction);
                                }
                            }
                            logger.LogInfo(String.Concat("Yodlee GetTransactions: ", "-->Acc ID :", bankAccount.id, "(" + bankAccount.accountName + "-" + bankAccount.accountNumber + ")", "Container: ", container, " from date: ", fromDate, " to date: ", toDate, "skip ", skipRecords, "top ", topRecords, "-->Trans Count:", transListCount));

                            {
                                if (bankAccount != null)
                                {
                                    logger.LogFile($"Yodlee Transactions Acc ID: {bankAccount.id}, ACCName: {bankAccount.accountName}({bankAccount.accountNumber}) ,Container: {container}, Type: {bankAccount.accountType}, Pro.ACC.ID: {bankAccount.providerAccountId}, Pro.ID: {bankAccount.providerId}, Pro.Name: {bankAccount.providerName},Container: {container}, CurBal:  {((bankAccount.currentBalance != null) ? bankAccount.currentBalance.amount : 0)}, AvalBal: {((bankAccount.availableBalance != null) ? bankAccount.availableBalance.amount : 0)}, BalLimit: {((bankAccount.balance != null) ? bankAccount.balance.amount : 0)}" + Environment.NewLine, (transListCount > 0) ? fileName : string.Concat(fileName, "-NOCOUNT"));
                                }

                                logger.LogFile(String.Concat(Environment.NewLine, "Yodlee GetTransactions Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (transListCount > 0) ? fileName : (fileName + "-NOCOUNT"));

                                logger.LogFile(String.Concat(Environment.NewLine, "Yodlee GetTransactions Res: ", "Trans Count: ", transListCount, Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : "")), (transListCount > 0) ? fileName : (fileName + "-NOCOUNT"));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(String.Concat("Error @GetTransactions Yodlee: ", "-->Acc ID :", bankAccount.id, "(" + bankAccount.accountName + ")", "Container: ", container, " from date: ", fromDate, " to date: ", toDate, "skip ", skipRecords, "top ", topRecords, "excess msg--->", ex.Message,
                                Environment.NewLine, "Req: ", (request != null ? JsonConvert.SerializeObject(request) : ""), Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));

                            //Log req & res into file
                            logger.LogFile(String.Concat(Environment.NewLine, "Yodlee GetTransactions Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (fileName + "-ERROR"));

                            logger.LogFile(String.Concat(Environment.NewLine, "Yodlee GetTransactions Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : "")), (fileName + "-ERROR"));
                            throw;
                        }
                    }
                }
            }
            return trans;
        }

        public async Task<int> GetTransactionsCount(YodleeAccountDTO bankAccount, string fromDate, string toDate, int? transactiontype, string token)
        {

            int count = 0;
            string container = transactiontype == Convert.ToInt32(YodleeContainerTypes.bank) ?
                Convert.ToString(YodleeContainerTypes.bank) :
                (transactiontype == Convert.ToInt32(YodleeContainerTypes.creditCard) ?
                Convert.ToString(YodleeContainerTypes.creditCard) :
                (transactiontype == Convert.ToInt32(YodleeContainerTypes.insurance) ?
                Convert.ToString(YodleeContainerTypes.insurance) :
                (Convert.ToString(YodleeContainerTypes.loan))));

            string url = String.Concat(providerBaseUrl.Yodlee, yodleeEndPoints.transactionsCount);
            TransactionsCount transactionCount = null;
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                var request = new RestRequest();

                string fileName = String.Concat("Yodlee-", bankAccount.accountName.Replace(" ", "-"), "-", bankAccount.id, "-", Convert.ToDateTime(fromDate).ToString("MM-dd-yyyy"), "_", Convert.ToDateTime(toDate).ToString("MM-dd-yyyy"), "-", DateTime.Now.Ticks);
                try
                {
                   
                    request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                    request.AddHeader("Api-Version", "1.1");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddQueryParameter("accountId", bankAccount.id);
                    request.AddQueryParameter("fromDate", fromDate);
                    request.AddQueryParameter("toDate", toDate);
                    request.AddQueryParameter("container", container);

                    //logger.LogInfo(String.Concat(Environment.NewLine + "Yodlee GetTransactionsCount request: ", JsonConvert.SerializeObject(request)) + Environment.NewLine);
                    resp = await client.GetAsync(request);
                    if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    {
                        transactionCount = JsonConvert.DeserializeObject<TransactionsCount>(resp.Content);
                        if (transactionCount != null && transactionCount.transaction != null && transactionCount.transaction.TOTAL != null && transactionCount.transaction.TOTAL.count == 0)
                        {
                            if (bankAccount != null)
                            {
                                logger.LogFile($"Yodlee Transactions Acc ID: {bankAccount.id}, ACCName: {bankAccount.accountName}, Type: {bankAccount.accountType}, Pro.ACC.ID: {bankAccount.providerAccountId}, Pro.ID: {bankAccount.providerId},, Pro.Name: {bankAccount.providerName}, container: {container}, CurBal:  {((bankAccount.currentBalance != null) ? bankAccount.currentBalance.amount : 0)}, AvalBal: {((bankAccount.availableBalance != null) ? bankAccount.availableBalance.amount : 0)}, BalLimit: {((bankAccount.balance != null) ? bankAccount.balance.amount : 0)}" + Environment.NewLine, (transactionCount.transaction.TOTAL.count > 0) ? fileName : (fileName+"-NOCOUNT"));
                            }

                            logger.LogFile(String.Concat(Environment.NewLine, "Yodlee GetTransactionsCount Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (transactionCount.transaction.TOTAL.count > 0) ? fileName : (fileName+"-NOCOUNT"));

                            logger.LogFile(String.Concat(Environment.NewLine, "Yodlee GetTransactionsCount Res: ", "Trans Count: ", 0, Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : "")), (transactionCount.transaction.TOTAL.count > 0) ? fileName : (fileName+"-NOCOUNT"));
                        }
                    }
                    logger.LogInfo(String.Concat("Yodlee GetTransactionsCount: ", "-->Acc ID :", bankAccount.id, "("+ bankAccount.accountName+ ")", "container: ", container, " From date: ", fromDate, " To date: ", toDate, " Trans Count: ", (transactionCount != null && transactionCount.transaction != null && transactionCount.transaction.TOTAL != null && transactionCount.transaction.TOTAL.count > 0) ? transactionCount.transaction.TOTAL.count : 0));
                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @GetTransactionsCount Yodlee: ", "-->Acc ID :", bankAccount.id, "(" + bankAccount.accountName + ")", "container: ", container, " from date: ", fromDate, " to date: ", toDate, "excess msg--->", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));

                    logger.LogFile(String.Concat(Environment.NewLine, "Yodlee GetTransactionsCount Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (fileName+"-ERROR"));

                    logger.LogFile(String.Concat(Environment.NewLine, "Yodlee GetTransactionsCount Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : "")), (fileName+"-ERROR"));

                    throw;
                }
            }
            if (transactionCount != null && transactionCount.transaction != null && transactionCount.transaction.TOTAL != null && transactionCount.transaction.TOTAL.count > 0)
                count = transactionCount.transaction.TOTAL.count;

            return count;

            /*try
            {
                UriBuilder uri = new UriBuilder(providerBaseUrl.Yodlee);
                // string jsonReq = "{ \"accountId\": \"" + bankAccountID + "\",\"fromDate\": \"" + fromDate + "\", \"toDate\": \"" + toDate + "\",\"container\": " + container + " }";
                //var transactionCount1 = await uri.Uri.AppendPathSegment(yodleeEndPoints.transactionsCount).WithHeader("Api-Version", "1.1").WithHeader("Authorization", "Bearer " + token).SetQueryParam("accountId", bankAccountID).SetQueryParam("fromDate", fromDate).
                //    SetQueryParam("toDate", toDate).SetQueryParam("container", container).GetAsync().ReceiveString();

                //TransactionsCount transactionCount = JsonConvert.DeserializeObject<TransactionsCount>(transactionCount1);
                //var transactionCount = await uri.Uri.AppendPathSegment(yodleeEndPoints.transactionsCount)
                    .WithHeader("Api-Version", "1.1").WithHeader("Authorization", "Bearer " + token).SetQueryParams(new
                    {
                        accountId = bankAccountID,
                        fromDate = fromDate,
                        toDate = toDate,
                        container = container

                    })
                    .GetAsync().ReceiveJson<TransactionsCount>();

                if (transactionCount != null && transactionCount.transaction.TOTAL != null && transactionCount.transaction.TOTAL.count > 0)
                    count = transactionCount.transaction.TOTAL.count;
                return count;
            }
            catch (Exception ex)
            {
                // return 0;
                throw;
            }*/

        }

        #endregion

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CheckService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}


