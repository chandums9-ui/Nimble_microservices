using BankFeed.App.Contracts;
using BankFeed.Domain.DTO.Model;
using Common.App.Contracts;
using Common.Domain.DTO.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Infra.ProviderServices
{
    public class MeldProviderCalls : IMeldProvider
    {
        #region Properties
        private readonly ProviderBaseURLInfo providerBaseUrl;
        private readonly MeldKeyInfo meldKeyInfo;
        private readonly MeldEndPoints meldEndPoints;
        private ILoggerService logger;
        private bool disposedValue;
        private string fileName = string.Empty;
        private string folderName = "Meld";
        #endregion

        #region Ctor
        public MeldProviderCalls(IOptions<ProviderBaseURLInfo> _providerBaseUrl, IOptions<MeldKeyInfo> _melddKeyInfo, IOptions<MeldEndPoints> _meldEndPoints, ILoggerService _logger)
        {
            this.providerBaseUrl = _providerBaseUrl.Value;
            this.meldKeyInfo = _melddKeyInfo.Value;
            this.meldEndPoints = _meldEndPoints.Value;
            this.logger = _logger;
            this.fileName = fileName = "{0}-{1}-" + DateTime.Now.ToString("yyyy-MM-dd");
        }
        #endregion

        #region Connect

        /// <summary>
        /// Use this endpoint to obtain a connect token, which can be used to initialize the Meld widget and start a bank linking connection.
        /// </summary>
        /// <param name="ConnReq">Request object to connect to Meld</param>
        /// <returns>Return Connection details</returns>
        public async Task<MeldConnectResponse> CreateConnection(MeldConnectRequest ConnReq)
        {
            //https://api-sb.meld.io/bank-linking/connect/start
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.StartConnection);
            string jsonReq = JsonConvert.SerializeObject(ConnReq);
            MeldConnectResponse meldRes = new MeldConnectResponse();
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                RestRequest request = new RestRequest();
                MeldErrorMessages errorMessages = null;
                try
                {
                    fileName = string.Format(fileName, "CreateConnection", "NEW");

                    request.AddJsonBody(jsonReq);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);
                    resp = await client.ExecutePostAsync(request);
                    if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    {
                        errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                        if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes = JsonConvert.DeserializeObject<MeldConnectResponse>(resp.Content);
                            meldRes.StatusCode = StatusCodes.Status200OK;
                            logger.LogInfo(string.Concat("CreateConnection ID: ", "-->Req ID:", meldRes.id, " customerId: ", meldRes.customerId, ", externalCustomerId: ", meldRes.externalCustomerId));
                        }
                        else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                            meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            logger.LogError(string.Concat("Meld CreateConnection failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                        }
                    }
                    else
                        logger.LogTrace("Meld CreateConnection -> NO RESPONSE CONTENT");
                }
                catch (Exception ex)
                {
                    meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(string.Concat("Error @CreateConnection Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                    logger.LogFile(String.Concat(Environment.NewLine, "Meld CreateConnection Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    throw;
                }
                finally
                {
                    try
                    {
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld CreateConnection Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld CreateConnection Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : ""), Environment.NewLine, "----------------------"), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    }
                    catch { }
                    resp = null; request = null; errorMessages = null;
                }
            }
            return meldRes;
        }

        /// <summary>
        /// A Service Provider's connection to the institution may sometimes degrade or new financial account(s) may be discovered for the connection. In order to fix the connection or add these newly discovered account(s), the customer must reconnect. This endpoint generates a new connect token for the connection to enable this reconnection process. This connect token can be used to invoke a new bank linking flow where the user can enter the service provider's UI to fix the connection
        /// </summary>
        /// <param name="ConnReq">Request object to Re-connect to Meld</param>
        /// <returns>Return Connection details</returns>
        public async Task<MeldConnectResponse> RepairConnection(MeldReConnectRequest ConnReq)
        {
            //https://api-sb.meld.io/bank-linking/connect/repair
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.RepairConnection);
            string jsonReq = JsonConvert.SerializeObject(ConnReq);
            MeldConnectResponse meldRes = new MeldConnectResponse();
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                RestRequest request = new RestRequest();
                MeldErrorMessages errorMessages = null;
                try
                {
                    fileName = string.Format(fileName, "RepairConnection", ConnReq.connectionId);

                    request.AddJsonBody(jsonReq);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);
                    resp = await client.ExecutePostAsync(request);
                    if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    {
                        errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                        if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes = JsonConvert.DeserializeObject<MeldConnectResponse>(resp.Content);
                            meldRes.StatusCode = StatusCodes.Status200OK;
                            logger.LogTrace(string.Concat("RepairConnection ID: ", "-->ID:", meldRes.id));
                        }
                        else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                            meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            logger.LogError(string.Concat("Meld RepairConnection failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                        }
                    }
                    else
                        logger.LogTrace("Meld RepairConnection -> NO RESPONSE CONTENT");
                }
                catch (Exception ex)
                {
                    meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(string.Concat("Error @RepairConnection Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                    logger.LogFile(String.Concat(Environment.NewLine, "Meld RepairConnection Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    throw;
                }
                finally
                {
                    try
                    {
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld RepairConnection Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld RepairConnection Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : ""), Environment.NewLine, "----------------------"), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    }
                    catch { }
                    resp = null; request = null; errorMessages = null;
                }
            }
            return meldRes;
        }

        /// <summary>
        /// Once you delete a connection, it will be deleted on both Meld and the Service Provider's ends, and you will no longer receive any updates (through webhooks) regarding it, as well as no longer be charged for it. You can only delete a connection, not the link to any individual financial accounts associated with the connection.
        /// </summary>
        /// <param name="ConnReq">Connection Id tobe deleted as Path param</param>
        /// <returns>Returns error if any</returns>
        public async Task<MeldConnectResponse> DeleteConnection(MeldReConnectRequest ConnReq)
        {
            //https://api-sb.meld.io/bank-linking/connections/{connectionId}
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.DeleteConnection).Replace("{connectionId}", ConnReq.connectionId);
            MeldConnectResponse meldRes = new MeldConnectResponse();
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                RestRequest request = new RestRequest();
                MeldErrorMessages errorMessages = null;
                try
                {
                    this.logger.LogTrace("Processing Delete connection for connectionId: " + url);

                    fileName = string.Format(fileName, "DeleteConnection", ConnReq.connectionId);

                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);
                    resp = await client.ExecuteDeleteAsync(request);
                    if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    {
                        errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                        if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes = JsonConvert.DeserializeObject<MeldConnectResponse>(resp.Content);
                            meldRes = meldRes ?? new MeldConnectResponse();
                            meldRes.StatusCode = StatusCodes.Status200OK;
                            logger.LogTrace(string.Concat("DeleteConnection ID: ", "-->ID:", meldRes.id));
                        }
                        else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                            meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            logger.LogError(string.Concat("Meld DeleteConnection failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                        }
                    }
                    else
                        logger.LogTrace("Meld DeleteConnection -> NO RESPONSE CONTENT");
                }
                catch (Exception ex)
                {
                    meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(string.Concat("Error @DeleteConnection Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                    logger.LogFile(String.Concat(Environment.NewLine, "Meld DeleteConnection Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    throw;
                }
                finally
                {
                    try
                    {
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld DeleteConnection Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld DeleteConnection Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : ""), Environment.NewLine, "----------------------"), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    }
                    catch { }
                    resp = null; request = null; errorMessages = null;
                }
            }
            return meldRes;
        }

        #endregion Connect

        #region Connections

        /// <summary>
        /// Retrieve details of a specific institution connection.
        /// </summary>
        /// <param name="ConnReq">Connection id as Path param</param>
        /// <returns>Return Institution connection details</returns>
        public async Task<MeldGetConnectionResponse> GetInstitutionConnection(MeldReConnectRequest ConnReq)
        {
            //https://api-sb.meld.io/bank-linking/connections/{connectionId}
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.GetInstitutionConnection).Replace("{connectionId}", ConnReq.connectionId);
            MeldGetConnectionResponse meldRes = new MeldGetConnectionResponse();
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                RestRequest request = new RestRequest();
                MeldErrorMessages errorMessages = null;
                try
                {
                    fileName = string.Format(fileName, "GetInstitutionConnection", ConnReq.connectionId);

                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);
                    resp = await client.ExecuteGetAsync(request);
                    if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    {
                        errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                        if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes = JsonConvert.DeserializeObject<MeldGetConnectionResponse>(resp.Content);
                            meldRes.StatusCode = StatusCodes.Status200OK;
                            logger.LogTrace(string.Concat("Connection ID: ", "-->ID:", meldRes.id));
                        }
                        else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                            meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            logger.LogError(string.Concat("Meld GetInstitutionConnection failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                        }
                    }
                    else
                        logger.LogTrace("Meld GetInstitutionConnection -> NO RESPONSE CONTENT");
                }
                catch (Exception ex)
                {
                    meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(string.Concat("Error @GetInstitutionConnection Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                    logger.LogFile(String.Concat(Environment.NewLine, "Meld GetInstitutionConnection Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    throw;
                }
                finally
                {
                    try
                    {
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld GetInstitutionConnection Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld GetInstitutionConnection Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : ""), Environment.NewLine, "----------------------"), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    }
                    catch { }
                    resp = null; request = null; errorMessages = null;
                }
            }
            return meldRes;
        }

        /// <summary>
        /// Retrieve institution connections filtered by various parameters.
        /// </summary>
        /// <param name="AccountSearchRequest">Search filters</param>
        /// <returns>Returns institution connections</returns>
        public async Task<MeldSearchConnectionResponse> SearchConnections(MeldSearchConnectionsRequest AccountSearchRequest)
        {
            //https://api-sb.meld.io/bank-linking/accounts   
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.SearchConnections);
            MeldSearchConnectionResponse meldRes = new MeldSearchConnectionResponse();
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                RestRequest request = new RestRequest();
                MeldErrorMessages errorMessages = null;
                try
                {
                    fileName = string.Format(fileName, "SearchConnections", (AccountSearchRequest.customerId + "-" + AccountSearchRequest.externalCustomerId).Trim('-'));

                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);

                    if (!string.IsNullOrEmpty(AccountSearchRequest.customerId))
                        request.AddQueryParameter("customerId", AccountSearchRequest.customerId);
                    if (!string.IsNullOrEmpty(AccountSearchRequest.externalCustomerId))
                        request.AddQueryParameter("externalCustomerId", AccountSearchRequest.externalCustomerId);
                    if (!string.IsNullOrEmpty(AccountSearchRequest.institutionId))
                        request.AddQueryParameter("institutionId", AccountSearchRequest.institutionId);
                    if (!string.IsNullOrEmpty(AccountSearchRequest.institutionName))
                        request.AddQueryParameter("institutionName", AccountSearchRequest.institutionName);

                    if (!string.IsNullOrEmpty(AccountSearchRequest.statuses))
                        request.AddQueryParameter("statuses", AccountSearchRequest.statuses);
                    if (!string.IsNullOrEmpty(AccountSearchRequest.serviceProviders))
                        request.AddQueryParameter("serviceProviders", AccountSearchRequest.serviceProviders);
                    if (!string.IsNullOrEmpty(AccountSearchRequest.startDate))
                        request.AddQueryParameter("startDate", AccountSearchRequest.startDate);
                    if (!string.IsNullOrEmpty(AccountSearchRequest.endDate))
                        request.AddQueryParameter("endDate", AccountSearchRequest.endDate);

                    if (!string.IsNullOrEmpty(AccountSearchRequest.before))
                        request.AddQueryParameter("before", AccountSearchRequest.before);
                    if (!string.IsNullOrEmpty(AccountSearchRequest.after))
                        request.AddQueryParameter("after", AccountSearchRequest.after);

                    if (AccountSearchRequest.limit > 0)
                        request.AddQueryParameter("limit", AccountSearchRequest.limit);

                    resp = await client.ExecuteGetAsync(request);
                    if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    {
                        errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                        if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes = JsonConvert.DeserializeObject<MeldSearchConnectionResponse>(resp.Content);
                            meldRes.StatusCode = StatusCodes.Status200OK;
                            logger.LogTrace(string.Concat("SearchConnections ID: ", "-->ID:", (meldRes.connections != null) ? meldRes.connections.Count() : 0));
                        }
                        else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                            meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            logger.LogError(string.Concat("Meld SearchConnections failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                        }
                    }
                    else
                        logger.LogTrace("Meld SearchConnections -> NO RESPONSE CONTENT");
                }
                catch (Exception ex)
                {
                    meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(string.Concat("Error @SearchConnections Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                    logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchConnections Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    throw;
                }
                finally
                {
                    try
                    {
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchConnections filters: ", Environment.NewLine, (AccountSearchRequest != null ? JsonConvert.SerializeObject(AccountSearchRequest) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchConnections Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchConnections Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : ""), Environment.NewLine, "----------------------"), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    }
                    catch { }
                    resp = null; request = null; errorMessages = null;
                }
            }
            return meldRes;
        }

        /// <summary>
        /// Update a connection and its products. Adds new products retroactively to an existing connection and triggers a refresh with this updated set of products. The products provided must not already be present on the connection and be supported by the underlying service provider and institution in order to be successful.
        /// </summary>
        /// <param name="ConnReq">Connection id as Path param</param>
        /// <returns>Adds new products retroactively to an existing connection</returns>
        public async Task<MeldConnectResponse> UpdateConnectionProducts(MeldReConnectRequest ConnReq)
        {
            //https://api-sb.meld.io/bank-linking/connections/{connectionId}/update
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.UpdateConnections).Replace("{connectionId}", ConnReq.connectionId);

            var req = new MeldConnectRequest();
            if (!string.IsNullOrEmpty(this.meldKeyInfo.Products) && this.meldKeyInfo.Products.Split(',').Any())
                req.products = this.meldKeyInfo.Products.Split(',').ToList();

            if (!string.IsNullOrEmpty(this.meldKeyInfo.OptionalProducts) && this.meldKeyInfo.OptionalProducts.Split(',').Any())
                req.optionalProducts = this.meldKeyInfo.OptionalProducts.Split(',').ToList();

            string jsonReq = JsonConvert.SerializeObject(req);
            MeldConnectResponse meldRes = new MeldConnectResponse();
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                RestRequest request = new RestRequest();
                MeldErrorMessages errorMessages = null;
                try
                {
                    fileName = string.Format(fileName, "UpdateConnections", ConnReq.connectionId);

                    request.AddJsonBody(jsonReq);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);
                    resp = await client.ExecutePostAsync(request);
                    if (resp != null && string.IsNullOrEmpty(resp.Content))
                    {
                        errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                        if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes = JsonConvert.DeserializeObject<MeldConnectResponse>(resp.Content);
                            meldRes.StatusCode = StatusCodes.Status200OK;
                            logger.LogTrace(string.Concat("UpdateConnections ID: ", "-->ID:", meldRes.id));
                        }
                        else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                            meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            logger.LogError(string.Concat("Meld UpdateConnections failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                        }
                    }
                    else
                        logger.LogTrace("Meld GetInstitutionConnection -> NO RESPONSE CONTENT");
                }
                catch (Exception ex)
                {
                    meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(string.Concat("Error @UpdateConnections Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                    logger.LogFile(String.Concat(Environment.NewLine, "Meld UpdateConnections Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    throw;
                }
                finally
                {
                    try
                    {
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld UpdateConnections Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld UpdateConnections Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : ""), Environment.NewLine, "----------------------"), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    }
                    catch { }
                    resp = null; request = null; errorMessages = null;
                }
            }
            return meldRes;
        }

        /// <summary>
        /// Trigger a refresh of all financial accounts for a given institution connection with the latest information from the service provider. This is a premium service for most service providers and should be used sparingly for requesting up-to-date data as of the time of the call to this endpoint. Transaction and balance data is updated daily by service providers, and Meld's webhooks will notify of these updates so that product data does not become stale.
        /// </summary>
        /// <param name="ConnReq">Connection id as Path param</param>
        /// <returns>Returns all refreshed financial accounts for a given institution connection</returns>
        public async Task<MeldConnectResponse> RefreshConnectionAndAccounts(MeldReConnectRequest ConnReq)
        {
            //https://api-sb.meld.io/bank-linking/connections/{connectionId}/refresh
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.RefreshConnections).Replace("{connectionId}", ConnReq.connectionId);

            //var req = new MeldConnectRequest();
         
            //if (!string.IsNullOrEmpty(this.meldKeyInfo.Products) && this.meldKeyInfo.Products.Split(',').Any())
            //    req.products = this.meldKeyInfo.Products.Split(',').ToList();

            //if (!string.IsNullOrEmpty(this.meldKeyInfo.OptionalProducts) && this.meldKeyInfo.OptionalProducts.Split(',').Any())
            //    req.optionalProducts = this.meldKeyInfo.OptionalProducts.Split(',').ToList();

            string jsonReq = JsonConvert.SerializeObject(ConnReq);
            MeldConnectResponse meldRes = new MeldConnectResponse();
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                RestRequest request = new RestRequest();
                MeldErrorMessages errorMessages = null;
                try
                {
                    fileName = string.Format(fileName, "RefresheConnections", "");

                    request.AddJsonBody(jsonReq);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);
                    resp = await client.ExecutePostAsync(request);
                    if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    {
                        errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                        if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes = JsonConvert.DeserializeObject<MeldConnectResponse>(resp.Content);
                            meldRes.StatusCode = StatusCodes.Status200OK;
                            logger.LogTrace(string.Concat("RefresheConnections ID: ", "-->ID:", meldRes.id));
                            logger.LogFile($"Meld RefreshConnection done for : {ConnReq.connectionId} - StatusCode:{meldRes.StatusCode}", fileName, folderName);

                        }
                        else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                            meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            logger.LogError(string.Concat("Meld RefresheConnections failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                            logger.LogFile($"Meld RefreshConnection failed for : {ConnReq.connectionId} - StatusCode:{meldRes.StatusCode}", (fileName + "-ERROR"), folderName);
                        }
                    }
                    else
                        logger.LogTrace("Meld RefresheConnections -> NO RESPONSE CONTENT");
                }
                catch (Exception ex)
                {
                    meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(string.Concat("Error @RefresheConnections Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                    logger.LogFile(String.Concat(Environment.NewLine, "Meld RefresheConnections Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    throw;
                }
                finally
                {
                    try
                    {
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld RefresheConnections Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld RefresheConnections Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : ""), Environment.NewLine, "----------------------"), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    }
                    catch { }
                    resp = null; request = null; errorMessages = null;
                }
            }
            return meldRes;
        }

        #endregion Connections

        #region Accounts

        /// <summary>
        /// Use this endpoint to retrieve account information of a specific financial account.
        /// </summary>
        /// <param name="FinancialAccountId">Financial AccountId to get the information</param>
        /// <returns>Returns Financial account details</returns>
        public async Task<FinancialAccountResponse> GetFinancialAccount(string FinancialAccountId)
        {
            //https://api-sb.meld.io/bank-linking/accounts/{financialAccountId}
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.GetFinancialAccount).Replace("{financialAccountId}", FinancialAccountId);
            FinancialAccountResponse meldRes = new FinancialAccountResponse();
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                RestRequest request = new RestRequest();
                MeldErrorMessages errorMessages = null;
                try
                {
                    fileName = string.Format(fileName, "GetFinancialAccount", FinancialAccountId);

                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);
                    resp = await client.ExecuteGetAsync(request);
                    if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    {
                        errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                        if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes = JsonConvert.DeserializeObject<FinancialAccountResponse>(resp.Content);
                            meldRes.StatusCode = StatusCodes.Status200OK;
                            logger.LogTrace(string.Concat("GetFinancialAccount ID: ", "-->ID:", meldRes.id));
                        }
                        else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                            meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            logger.LogError(string.Concat("Meld GetFinancialAccount failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                        }
                    }
                    else
                        logger.LogTrace("Meld GetFinancialAccount -> NO RESPONSE CONTENT");
                }
                catch (Exception ex)
                {
                    meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(string.Concat("Error @GetFinancialAccount Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                    logger.LogFile(String.Concat(Environment.NewLine, "Meld GetFinancialAccount Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    throw;
                }
                finally
                {
                    try
                    {
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld GetFinancialAccount Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld GetFinancialAccount Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    }
                    catch { }
                    resp = null; request = null; errorMessages = null;
                }
            }
            return meldRes;
        }

        /// <summary>
        /// Use this endpoint to filter financial accounts by various parameters.
        /// </summary>
        /// <param name="AccountSearchRequest">Financial account search filters</param>
        /// <returns>Returns financial account details</returns>
        public async Task<FinancialAccountSearchResponse> SearchFinancialAccount(MeldFinancialAccountSearchRequest AccountSearchRequest)
        {
            //https://api-sb.meld.io/bank-linking/accounts   
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.SearchFinancialAccount);
            FinancialAccountSearchResponse meldFinalRes = new FinancialAccountSearchResponse();
            FinancialAccountSearchResponse meldRes = new FinancialAccountSearchResponse();
            using (var client = new RestClient(url))
            {
                do
                {
                    meldRes = new FinancialAccountSearchResponse();
                    RestResponse resp = null;
                    RestRequest request = new RestRequest();
                    MeldErrorMessages errorMessages = null;
                    try
                    {
                        fileName = string.Format(fileName, "SearchFinancialAccount", (AccountSearchRequest.connectionId + "-" + AccountSearchRequest.institutionId).Trim('-'));

                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);

                        if (!string.IsNullOrEmpty(AccountSearchRequest.customerId))
                            request.AddQueryParameter("customerId", AccountSearchRequest.customerId);
                        if (!string.IsNullOrEmpty(AccountSearchRequest.externalCustomerId))
                            request.AddQueryParameter("externalCustomerId", AccountSearchRequest.externalCustomerId);
                        if (!string.IsNullOrEmpty(AccountSearchRequest.institutionId))
                            request.AddQueryParameter("institutionId", AccountSearchRequest.institutionId);
                        if (!string.IsNullOrEmpty(AccountSearchRequest.before))
                            request.AddQueryParameter("before", AccountSearchRequest.before);
                        if (!string.IsNullOrEmpty(AccountSearchRequest.after))
                            request.AddQueryParameter("after", AccountSearchRequest.after);
                        if (!string.IsNullOrEmpty(AccountSearchRequest.connectionId))
                            request.AddQueryParameter("connectionId", AccountSearchRequest.connectionId);
                        if (AccountSearchRequest.limit > 0)
                            request.AddQueryParameter("limit", AccountSearchRequest.limit);

                        resp = await client.ExecuteGetAsync(request);
                        if (resp != null && !string.IsNullOrEmpty(resp.Content))
                        {
                            errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                            if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                            {
                                meldRes = JsonConvert.DeserializeObject<FinancialAccountSearchResponse>(resp.Content);
                                meldFinalRes.StatusCode = meldRes.StatusCode = StatusCodes.Status200OK;
                                logger.LogTrace(string.Concat("SearchFinancialAccount ID: ", "-->ID:", (meldRes.financialAccounts != null) ? meldRes.financialAccounts.Count() : 0));
                                if (meldRes.financialAccounts != null && meldRes.financialAccounts.Any())
                                {
                                    meldFinalRes.financialAccounts.AddRange(meldRes.financialAccounts);
                                    AccountSearchRequest.after = (meldRes.remaining > 0) ? meldRes.financialAccounts.LastOrDefault().key : string.Empty;
                                }
                                else
                                    AccountSearchRequest.after = string.Empty;
                            }
                            else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                            {
                                meldFinalRes.StatusCode = meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                                meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                                logger.LogError(string.Concat("Meld SearchFinancialAccount failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                            }
                        }
                        else
                            logger.LogTrace("Meld SearchFinancialAccount -> NO RESPONSE CONTENT");
                    }
                    catch (Exception ex)
                    {
                        meldFinalRes.StatusCode = meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                        logger.LogError(string.Concat("Error @SearchFinancialAccount Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchFinancialAccount Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                        throw;
                    }
                    finally
                    {
                        try
                        {
                            logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchFinancialAccount filters: ", Environment.NewLine, (AccountSearchRequest != null ? JsonConvert.SerializeObject(AccountSearchRequest) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                            logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchFinancialAccount Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                            logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchFinancialAccount Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : ""), Environment.NewLine, "----------------------"), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                        }
                        catch { }
                        resp = null; request = null; errorMessages = null;
                    }

                } while (meldRes != null && meldRes.remaining > 0);
            }
            return meldFinalRes;
        }

        #endregion Accounts

        #region Transactions
        /// <summary>
        /// Use this endpoint to retrieve transaction information of a specific financial account transaction.
        /// </summary>
        /// <param name="TransactionId">Transaction id</param>
        /// <returns>Returns transaction information of a specific financial account transaction.</returns>
        public async Task<MeldTransactionResponse> GetFinancialAccountTransactions(string TransactionId)
        {
            //https://api-sb.meld.io/bank-linking/transactions/{transactionId}
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.GetTransaction).Replace("{transactionId}", TransactionId);
            MeldTransactionResponse meldRes = new MeldTransactionResponse();
            using (var client = new RestClient(url))
            {
                RestResponse resp = null;
                RestRequest request = new RestRequest();
                MeldErrorMessages errorMessages = null;
                try
                {
                    fileName = string.Format(fileName, "GetFinancialAccountTransactions", TransactionId);

                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);
                    resp = await client.ExecuteGetAsync(request);
                    if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    {
                        errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                        if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes = JsonConvert.DeserializeObject<MeldTransactionResponse>(resp.Content);
                            meldRes.StatusCode = StatusCodes.Status200OK;
                            logger.LogTrace(string.Concat("GetFinancialAccountTransactions ID: ", "-->ID:", meldRes.id));
                        }
                        else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                        {
                            meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            logger.LogError(string.Concat("Meld GetFinancialAccountTransactions failed: ", errorMessages.code, "-", errorMessages.message, "-", errorMessages.requestId));
                        }
                    }
                    else
                        logger.LogTrace("Meld GetFinancialAccountTransactions -> NO RESPONSE CONTENT");
                }
                catch (Exception ex)
                {
                    meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(string.Concat("Error @GetFinancialAccountTransactions Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                    logger.LogFile(String.Concat(Environment.NewLine, "Meld GetFinancialAccountTransactions Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    throw;
                }
                finally
                {
                    try
                    {
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld GetFinancialAccountTransactions Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                        logger.LogFile(String.Concat(Environment.NewLine, "Meld GetFinancialAccountTransactions Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : ""), Environment.NewLine, "----------------------"), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                    }
                    catch { }
                    resp = null; request = null; errorMessages = null;
                }
            }
            return meldRes;
        }

        /// <summary>
        /// Use this endpoint to filter financial account transactions by various parameters.
        /// </summary>
        /// <param name="TransSearchRequest">Search filters for getting transactions for Financial account</param>
        /// <returns>Returns transactions for a finanacial account</returns>
        public async Task<MeldTransactionSearchResponse> SearchFinancialAccountTransactions(MeldTransactionSearchRequest TransSearchRequest)
        {
            //https://api-sb.meld.io/bank-linking/transactions
            string url = string.Concat(providerBaseUrl.Meld, meldEndPoints.SearchTransactions);
            MeldTransactionSearchResponse meldFinalRes = new MeldTransactionSearchResponse();
            MeldTransactionSearchResponse meldRes = new MeldTransactionSearchResponse();
            using (var client = new RestClient(url))
            {
                do
                {
                    meldRes = new MeldTransactionSearchResponse();
                    RestResponse resp = null;
                    RestRequest request = new RestRequest();
                    MeldErrorMessages errorMessages = null;
                    try
                    {
                        fileName = "SearchFinancialAccountTransactions Call";
                        fileName = string.Format(fileName, "SearchFinancialAccountTransactions", (TransSearchRequest.connectionId + "-" + TransSearchRequest.financialAccountId).Trim('-'));

                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "BASIC " + meldKeyInfo.APIKey);

                        if (!string.IsNullOrEmpty(TransSearchRequest.startDate))
                            request.AddQueryParameter("startDate", TransSearchRequest.startDate);
                        if (!string.IsNullOrEmpty(TransSearchRequest.endDate))
                            request.AddQueryParameter("endDate", TransSearchRequest.endDate);
                        if (!string.IsNullOrEmpty(TransSearchRequest.customerId))
                            request.AddQueryParameter("customerId", TransSearchRequest.customerId);
                        if (!string.IsNullOrEmpty(TransSearchRequest.externalCustomerId))
                            request.AddQueryParameter("externalCustomerId", TransSearchRequest.externalCustomerId);
                        if (!string.IsNullOrEmpty(TransSearchRequest.financialAccountId))
                            request.AddQueryParameter("financialAccountId", TransSearchRequest.financialAccountId);
                        if (!string.IsNullOrEmpty(TransSearchRequest.status))
                            request.AddQueryParameter("status", TransSearchRequest.status);
                        if (!string.IsNullOrEmpty(TransSearchRequest.before))
                            request.AddQueryParameter("before", TransSearchRequest.before);
                        if (!string.IsNullOrEmpty(TransSearchRequest.after))
                            request.AddQueryParameter("after", TransSearchRequest.after);
                        if (!string.IsNullOrEmpty(TransSearchRequest.category))
                            request.AddQueryParameter("category", TransSearchRequest.category);
                        if (!string.IsNullOrEmpty(TransSearchRequest.connectionId))
                            request.AddQueryParameter("connectionId", TransSearchRequest.connectionId);
                        if (TransSearchRequest.limit > 0)
                            request.AddQueryParameter("limit", TransSearchRequest.limit);

                        resp = await client.ExecuteGetAsync(request);
                        if (resp != null && !string.IsNullOrEmpty(resp.Content))
                        {
                            errorMessages = JsonConvert.DeserializeObject<MeldErrorMessages>(resp.Content);
                            if (errorMessages == null || string.IsNullOrEmpty(errorMessages.code))
                            {
                                meldRes = JsonConvert.DeserializeObject<MeldTransactionSearchResponse>(resp.Content);
                               
                                meldFinalRes.StatusCode = meldRes.StatusCode = StatusCodes.Status200OK;                                
                                
                                if (meldRes.financialAccountTransactions != null && meldRes.financialAccountTransactions.Any())
                                {
                                    meldFinalRes.financialAccountTransactions.AddRange(meldRes.financialAccountTransactions);
                                    TransSearchRequest.after = (meldRes.remaining > 0) ? meldRes.financialAccountTransactions.LastOrDefault().key : string.Empty;
                                }
                                else
                                    TransSearchRequest.after = string.Empty;
                            }
                            else if (errorMessages != null && !string.IsNullOrEmpty(errorMessages.code))
                            {
                                meldFinalRes.StatusCode = meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                                meldRes.Status = $"{errorMessages.code} - {errorMessages.message}";
                            }
                        }
                        else
                            logger.LogTrace("Meld SearchFinancialAccountTransactions -> NO RESPONSE CONTENT");
                    }
                    catch (Exception ex)
                    {
                        meldFinalRes.StatusCode = meldRes.StatusCode = StatusCodes.Status500InternalServerError;
                        logger.LogError(string.Concat("Error @SearchFinancialAccountTransactions Meld> ExMess:", ex.Message, Environment.NewLine + "Content: ", resp != null ? resp.Content : string.Empty));
                        logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchFinancialAccountTransactions Error: ", Environment.NewLine, ex.DeepParseMessage()), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                        throw;
                    }
                    finally
                    {
                        try
                        {
                            logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchFinancialAccountTransactions Req: ", Environment.NewLine, (request != null ? JsonConvert.SerializeObject(request) : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);

                            logger.LogFile(String.Concat(Environment.NewLine, "Meld SearchFinancialAccountTransactions Res: ", Environment.NewLine, ((resp != null && !string.IsNullOrEmpty(resp.Content)) ? resp.Content : "")), (meldRes.StatusCode == StatusCodes.Status200OK) ? fileName : (fileName + "-ERROR"), folderName);
                        }
                        catch 
                        {
                        }
                        resp = null; request = null; errorMessages = null;
                    }

                } while (meldRes != null && meldRes.remaining > 0);
            }
            return meldFinalRes;
        }

        #endregion Transactions

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
