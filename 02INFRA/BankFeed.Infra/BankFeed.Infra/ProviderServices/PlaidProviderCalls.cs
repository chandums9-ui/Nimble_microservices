using Azure.Core;
using BankFeed.App.Contracts;
using BankFeed.Domain.DTO.Model;

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using NLog;
using Common.App.Contracts;
using Microsoft.Identity.Client;
using System.Reflection;
using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Http;
using Common.Domain.DTO.App;
using Azure;
namespace BankFeed.Infra.ProviderServices
{
    public class PlaidProviderCalls : IPlaidProvider, IDisposable
    {


        /// <summary>
        /// Method to create public token for Plaid
        /// </summary>
        /// <param name="Params"></param>
        /// <returns>public token</returns>
        /// 

        #region Properties
        private readonly ProviderBaseURLInfo providerBaseUrl;
        private readonly PlaidKeyInfo plaidKeyInfo;
        private readonly PlaidEndPoints plaidEndPoints;
        private ILoggerService logger;
        private bool disposedValue;
        private string[] msgsToCompareRelogin = { "ITEM_LOGIN_REQUIRED", "MFA_NOT_SUPPORTED", "ACCESS_NOT_GRANTED", "INSUFFICIENT_CREDENTIALS", "INVALID_CREDENTIALS", "INVALID_MFA", "INVALID_SEND_METHOD",
            "INVALID_OTP", "ITEM_LOCKED", "NO_AUTH_ACCOUNTS","PRODUCT_NOT_ENABLED","USER_INPUT_TIMEOUT","USER_SETUP_REQUIRED"};
        #endregion

        #region Ctor
        public PlaidProviderCalls(IOptions<ProviderBaseURLInfo> _providerBaseUrl, IOptions<PlaidKeyInfo> _plaidKeyInfo, IOptions<PlaidEndPoints> _plaidEndPoints, ILoggerService _logger)
        {
            this.providerBaseUrl = _providerBaseUrl.Value;
            this.plaidKeyInfo = _plaidKeyInfo.Value;
            this.plaidEndPoints = _plaidEndPoints.Value;
            this.logger = _logger;
        }
        #endregion

        #region PlaidTokenActions
        public async Task<PlaidTokenRespInfo> CreatePublicToken(AccessTokenDTO Params)
        {
            string url = String.Concat(providerBaseUrl.Plaid, plaidEndPoints.publicToken);
            string jsonReq = "{ \"client_id\": \"" + plaidKeyInfo.clientID + "\",\"secret\": \"" + plaidKeyInfo.clientSecret + "\", \"access_token\": \"" + Params.access_token + "\" }";
            PlaidTokenRespInfo tokenInfo = null;
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {
                    var request = new RestRequest();
                    request.AddJsonBody(jsonReq);
                    request.AddHeader("Content-Type", "application/json");
                    resp = await client.ExecutePostAsync(request);
                    PlaidErrorMessages errorMessages = JsonConvert.DeserializeObject<PlaidErrorMessages>(resp.Content);
                    if (errorMessages == null || string.IsNullOrEmpty(errorMessages.error_type))
                    {
                        tokenInfo = JsonConvert.DeserializeObject<PlaidTokenRespInfo>(resp.Content);
                        logger.LogTrace(String.Concat("Public Token: ", "-->Req ID:", tokenInfo.request_id));
                    }
                    else
                        logger.LogTrace(String.Concat("Public Token: ", Params.access_token, "-->Req ID:", tokenInfo.request_id));
                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @CreatePublicToken Plaid: ", Params.access_token, "-->ExMess:", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
            }
            return tokenInfo;
        }
        public async Task<ExchangeToken> GetExchangeToken(string public_token)
        {
            string url = String.Concat(providerBaseUrl.Plaid, plaidEndPoints.exchangeToken);
            string jsonReq = "{ \"client_id\": \"" + plaidKeyInfo.clientID + "\",\"secret\": \"" + plaidKeyInfo.clientSecret + "\", \"public_token\": \"" + public_token + "\" }";
            ExchangeToken exchangeTokenResp = null;
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {
                    var request = new RestRequest();
                    request.AddJsonBody(jsonReq);
                    request.AddHeader("Content-Type", "application/json");
                    resp = await client.ExecutePostAsync(request);
                    PlaidErrorMessages errorMessages = JsonConvert.DeserializeObject<PlaidErrorMessages>(resp.Content);
                    if (errorMessages == null || string.IsNullOrEmpty(errorMessages.error_type))
                    {
                        exchangeTokenResp = JsonConvert.DeserializeObject<ExchangeToken>(resp.Content);
                        logger.LogTrace(String.Concat("Exchange Token: ", "-->Req ID:", exchangeTokenResp.request_id));
                    }
                    else
                        logger.LogTrace(String.Concat("Exchange Token:: ", public_token, "-->Req ID:", exchangeTokenResp.request_id));
                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @GetExchangeToken Plaid: ", public_token, "-->ExMess:", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
                return exchangeTokenResp;
            }
        }
        #endregion

        #region PlaidAccountActions
        public async Task<PlaidAccounts> GetAccounts(string access_token)
        {
            string url = String.Concat(providerBaseUrl.Plaid, plaidEndPoints.Accounts);
            string jsonReq = "{ \"client_id\": \"" + plaidKeyInfo.clientID + "\",\"secret\": \"" + plaidKeyInfo.clientSecret + "\", \"access_token\": \"" + access_token + "\" }";
            PlaidAccounts plaidAccounts = new PlaidAccounts() { StatusCode = StatusCodes.Status401Unauthorized };
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {

                    var request = new RestRequest();
                    request.AddJsonBody(jsonReq);
                    request.AddHeader("Content-Type", "application/json");
                    resp = await client.ExecutePostAsync(request);
                    PlaidErrorMessages errorMessages = JsonConvert.DeserializeObject<PlaidErrorMessages>(resp.Content);

                    if (errorMessages == null || string.IsNullOrEmpty(errorMessages.error_type))
                    {
                        plaidAccounts = JsonConvert.DeserializeObject<PlaidAccounts>(resp.Content);
                        if (plaidAccounts != null)
                            plaidAccounts.StatusCode = StatusCodes.Status200OK;

                        logger.LogInfo(String.Concat("Get Accounts:: ", "-->Acc Count:", (plaidAccounts != null && plaidAccounts.accounts != null) ? plaidAccounts.accounts.Count : 0));
                    }
                    else if (errorMessages != null)
                    {
                        if (msgsToCompareRelogin.Contains(errorMessages.error_code))// == "ITEM_LOGIN_REQUIRED")
                            plaidAccounts.StatusCode = StatusCodes.Status401Unauthorized;

                        plaidAccounts.Status = !string.IsNullOrEmpty(errorMessages.error_message) ? errorMessages.error_message : errorMessages.error_code;
                        logger.LogTrace(String.Concat("Get Accounts: ", access_token, "-->Req ID:", errorMessages.request_id, ", Error: ", errorMessages.error_message));
                    }

                    if (plaidAccounts.StatusCode == StatusCodes.Status401Unauthorized && !string.IsNullOrEmpty(plaidAccounts.Status))
                        plaidAccounts.Status = Constants.MSG_CONNECTION_FAILED;
                }

                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @GetAccounts Paild: ", access_token, "-->ExMess:", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
            }
            return plaidAccounts;
        }

        /// <summary>
        /// To get classified information of the accounts
        /// </summary>
        /// <param name="PRequest">access token</param>
        /// <returns>Accounts information</returns>
        public async Task<PlaidAccounts> GetAuth(AccessTokenDTO Params)
        {
            string url = String.Concat(providerBaseUrl.Plaid, plaidEndPoints.Auth);
            string jsonReq = "{ \"client_id\": \"" + plaidKeyInfo.clientID + "\",\"secret\": \"" + plaidKeyInfo.clientSecret + "\", \"access_token\": \"" + Params.access_token + "\" }";
            PlaidAccounts plaidAccounts = null;
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {
                    var request = new RestRequest();
                    request.AddJsonBody(jsonReq);
                    request.AddHeader("Content-Type", "application/json");
                    resp = await client.ExecutePostAsync(request);
                    PlaidErrorMessages errorMessages = JsonConvert.DeserializeObject<PlaidErrorMessages>(resp.Content);
                    if (errorMessages == null || string.IsNullOrEmpty(errorMessages.error_type))
                    {
                        plaidAccounts = JsonConvert.DeserializeObject<PlaidAccounts>(resp.Content);
                        logger.LogTrace(String.Concat("Auth: ", Params.access_token, "-->Acc Count:", plaidAccounts.accounts.Count));
                    }
                    else
                    {
                        logger.LogTrace(String.Concat("Auth: ", Params.access_token, "-->Error Type:", errorMessages.error_type, "Error Code:", errorMessages.error_code, "RequestID:", errorMessages.request_id));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @GetAuth Plaid: ", Params.access_token, "-->ExMess:", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
            }
            //PlaidAccounts plaidAccounts = JsonConvert.DeserializeObject<PlaidAccounts>(pr);
            return plaidAccounts;

        }


        #endregion



        #region PlaidTransactionActions
        public async Task<PlaidTransactionsResponse> GetTransactions(string access_token, List<string> accountIDs, string start_date, string end_date)
        {
            PlaidTransactionsResponse response = new PlaidTransactionsResponse();
            string url = String.Concat(providerBaseUrl.Plaid, plaidEndPoints.Transactions);
            List<PlaidTransactionDetails> transDetas = new List<PlaidTransactionDetails>();
            using (var client = new RestClient(url))
            {
                long totalPages = 1;
                long iter_Page = 0;
                var account_Ids = JsonConvert.SerializeObject(accountIDs).ToString();
                RestResponse resp = null;
                string fileName = String.Concat("Plaid-", "{0}", Convert.ToDateTime(start_date).ToString("MM-dd-yyyy"), "_", Convert.ToDateTime(end_date).ToString("MM-dd-yyyy"), "-", DateTime.Now.Ticks);

                long transCount = 0;
                string jsonReq = string.Empty;
                TransOptions op = null;
                RestRequest request = null;
                PlaidErrorMessages errorMessages = null;
                PlaidTransactionDetails trans = null;
                BankAccountDetails acc = null;
                while (iter_Page < totalPages)
                {
                    acc = null;
                    request = new RestRequest();
                    op = new TransOptions();
                    jsonReq = string.Empty;
                    trans = null;
                    errorMessages = null;

                    try
                    {
                        op.offset = (int)iter_Page * 499;
                        op.count = 499;
                        jsonReq = "{ \"client_id\": \"" + plaidKeyInfo.clientID + "\",\"secret\": \"" + plaidKeyInfo.clientSecret + "\", \"access_token\": \"" + access_token + "\",\"start_date\": \"" + start_date + "\",\"end_date\": \"" + end_date + "\",\"options\" : { \"account_ids\": " + account_Ids + " ,\"count\":" + op.count + " ,\"offset\": " + op.offset + " } }";

                        request.AddJsonBody(jsonReq);
                        request.AddHeader("Content-Type", "application/json");
                        resp = await client.ExecutePostAsync(request);

                        errorMessages = JsonConvert.DeserializeObject<PlaidErrorMessages>(resp.Content);
                        if (errorMessages == null || (errorMessages != null && string.IsNullOrEmpty(errorMessages.error_type)))
                        {
                            trans = JsonConvert.DeserializeObject<PlaidTransactionDetails>(resp.Content);
                            transCount = 0;
                            if (trans != null)
                            {
                                transCount = (trans.total_transactions != null && trans.total_transactions >= 0) ? trans.total_transactions : 0;
                                totalPages = (transCount > 0 ? Convert.ToInt64(transCount / 499) : 0) + 1;
                                transDetas.Add(trans);
                            }
                            response.transactions = transDetas;
                            response.StatusCode = StatusCodes.Status200OK;

                            logger.LogInfo(String.Concat("Plaid Transactions : ", account_Ids, "--> from date:", start_date, " to date: ", end_date, "Trans Count:", transCount));

                            //if (transCount > 0)
                            {
                                if (trans != null && trans.accounts != null)
                                    acc = trans.accounts.FirstOrDefault();
                                if (acc != null)
                                {
                                    fileName = string.Format(fileName, acc.name + "(" + acc.mask + ")" + "-");//acc.account_id
                                    logger.LogFile($"Plaid Transactions Acc ID: {acc.account_id}, OffName: {acc.official_name}, Name: {acc.name}-{acc.mask}, Type: {acc.type}, CurBal:  {((acc.balances != null && acc.balances.current.HasValue) ? acc.balances.current.Value : 0)}, AvalBal: {((acc.balances != null && acc.balances.available.HasValue) ? acc.balances.available.Value : 0)}, BalLimit: {((acc.balances != null && acc.balances.limit.HasValue) ? acc.balances.limit.Value : 0)}" + Environment.NewLine, (transCount > 0) ? fileName : (fileName + "-NOCOUNT"));

                                    logger.LogInfo($"Plaid Transactions Acc ID: {acc.account_id}, OffName: {acc.official_name}, Name: {acc.name}-{acc.mask}, Type: {acc.type}, CurBal: {((acc.balances != null && acc.balances.current.HasValue) ? acc.balances.current.Value : 0)}, AvalBal:  {((acc.balances != null && acc.balances.available.HasValue) ? acc.balances.available.Value : 0)}, BalLimit: {((acc.balances != null && acc.balances.limit.HasValue) ? acc.balances.limit.Value : 0)}" + Environment.NewLine);
                                }
                                logger.LogFile(String.Concat(Environment.NewLine, "Plaid Transactions Req: ", Environment.NewLine, (!string.IsNullOrEmpty(jsonReq) ? jsonReq : "")), (transCount > 0) ? fileName : (fileName + "-NOCOUNT"));

                                logger.LogFile(String.Concat(Environment.NewLine, "Plaid Transactions Res: ", "Trans Count: ", transCount, Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : "")), (transCount > 0) ? fileName : (fileName + "-NOCOUNT"));
                            }
                        }
                        else if (errorMessages != null)
                        {
                            if (msgsToCompareRelogin.Contains(errorMessages.error_code))// == "ITEM_LOGIN_REQUIRED")
                                response.StatusCode = StatusCodes.Status401Unauthorized;
                            response.Status = !string.IsNullOrEmpty(errorMessages.error_message) ? errorMessages.error_message : errorMessages.error_code;

                            if (response.StatusCode == StatusCodes.Status401Unauthorized && !string.IsNullOrEmpty(response.Status))
                                response.Status = Constants.MSG_CONNECTION_FAILED;

                            logger.LogError(String.Concat("Plaid Transactions : ", account_Ids, "-->Error Type:", errorMessages.error_type, "Error Code:", errorMessages.error_code, "RequestID:", errorMessages.request_id, Environment.NewLine, "Error_Message: ", errorMessages.error_message, Environment.NewLine, "Req: ", jsonReq));
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(String.Concat("Error @GetTransactions Plaid : ", account_Ids, "-->ExMess:", ex.Message, Environment.NewLine, "Req: ", jsonReq, Environment.NewLine, "Res Content: " + (resp != null ? resp.Content : string.Empty)));

                        //Log req & res into file
                        logger.LogFile(String.Concat(Environment.NewLine, "Plaid Transactions Req: ", Environment.NewLine, (!string.IsNullOrEmpty(jsonReq) ? jsonReq : "")), (fileName + "-ERROR"));

                        logger.LogFile(String.Concat(Environment.NewLine, "Plaid Transactions Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : "")), (fileName + "-ERROR"));
                        throw;
                    }
                    iter_Page++;

                }
            }
            return response;
        }
        #endregion


        #region PlaidUnRegisterActions

        /// <summary>
        /// To remove the Account connection
        /// </summary>
        /// <param name="PRequest">access token</param>
        /// <returns>request ID </returns>
        public async Task<PlaidRequestID> RemoveLogin(AccessTokenDTO Params)
        {
            string url = String.Concat(providerBaseUrl.Plaid, plaidEndPoints.Unregister);
            string jsonReq = "{ \"client_id\": \"" + plaidKeyInfo.clientID + "\",\"secret\": \"" + plaidKeyInfo.clientSecret + "\", \"access_token\": \"" + Params.access_token + "\" }";
            PlaidRequestID pr = null;
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                try
                {
                    var request = new RestRequest();
                    request.AddJsonBody(jsonReq);
                    request.AddHeader("Content-Type", "application/json");
                    resp = await client.ExecutePostAsync(request);
                    PlaidErrorMessages errorMessages = JsonConvert.DeserializeObject<PlaidErrorMessages>(resp.Content);
                    if (errorMessages == null || string.IsNullOrEmpty(errorMessages.error_type))
                    {
                        pr = JsonConvert.DeserializeObject<PlaidRequestID>(resp.Content);
                        logger.LogTrace(String.Concat("RemoveLogin: ", Params.access_token, "-->Req ID:", pr.request_id));
                    }

                    else
                    {
                        logger.LogTrace(String.Concat("RemoveLogin: ", Params.access_token, "-->Error Type:", errorMessages.error_type, "Error Code:", errorMessages.error_code, "RequestID:", errorMessages.request_id));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(String.Concat("Error @RemoveLogin Plaid: ", Params.access_token, "-->ExMess:", ex.Message + Environment.NewLine + "Content: " + (resp != null ? resp.Content : string.Empty)));
                    throw;
                }
            }


            return pr;
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




        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}
