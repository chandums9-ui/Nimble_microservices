using Common.Domain.DTO.Model;
using Microsoft.Extensions.Options;
using Payable.App.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payable.Domain.DTO.Resp;
using Microsoft.Identity.Client;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Common.Domain.DTO.Enums;
using Microsoft.VisualBasic;
using Common.Domain.DTO.App;
using System.Net;

namespace Payable.Infra.PaymentGateWay
{
    public class PaymentGatewayCalls : IPaymentGateway
    {
        #region Fields
        private readonly PaymentGateWayDetails paymenturls;

        #endregion

        #region ctor
        public PaymentGatewayCalls(IOptions<PaymentGateWayDetails> _payurl)
        {
            this.paymenturls = _payurl.Value;
        }
        #endregion

        #region Public 
        public async Task<CreateACHVendorSuccessResponse> CreateVendor(string InputString, string PGAccountID, string TransactionID, short ProviderType)
        {
            CreateACHVendorSuccessResponse mainResponse = new();
            try
            {
                var paramurl = "pgAccountId=" + PGAccountID + "&txnAcId=" + TransactionID;
                string apiurl = string.Concat(paymenturls.CreateVendor, paramurl);
                string url = string.Concat(paymenturls.MainURL, apiurl);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddBody(InputString);
                    RestResponse resp = await client.ExecutePostAsync(request);
                    if (ProviderType == (short)ACHProviderTypeEnum.Forte && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Forte = JsonConvert.DeserializeObject<CreateForteVendorSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Forte.message;
                    }
                    else if (ProviderType == (short)ACHProviderTypeEnum.SwirePay && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Swirepay = JsonConvert.DeserializeObject<CreateSwirePayVendorSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Swirepay.message;
                    }
                    else
                    {
                        mainResponse.StatusCode = StatusCodes.Status500InternalServerError;
                        mainResponse.Status = "Error Occurred";
                    }
                }
                return mainResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mainResponse = null;
            }
        }

        public async Task<CreateFundingSourceSuccessResponse> CreateFundingSource(string InputString, string PGAccountID, string TransactionID, short ProviderType)
        {
            CreateFundingSourceSuccessResponse mainResponse = new();
            try
            {
                var paramurl = "pgAccountId=" + PGAccountID + "&txnAcId=" + TransactionID;
                string apiurl = string.Concat(paymenturls.CreateFundingSource, paramurl);
                string url = string.Concat(paymenturls.MainURL, apiurl);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddBody(InputString);
                    RestResponse resp = await client.ExecutePostAsync(request);
                    if (ProviderType == (short)ACHProviderTypeEnum.Forte && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Forte = JsonConvert.DeserializeObject<CreateForteAccSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Forte.message;
                    }
                    else if (ProviderType == (short)ACHProviderTypeEnum.SwirePay && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Swirepay = JsonConvert.DeserializeObject<CreateSwirePayAccSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Swirepay.message;
                    }
                    else
                    {
                        mainResponse.StatusCode = StatusCodes.Status500InternalServerError;
                        mainResponse.Status = "Error Occurred";
                    }
                }
                return mainResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mainResponse = null;
            }
        }

        public async Task<CreateCCFundingSourceSuccessResponse> CreateCCFundingSource(string InputString, string PGAccountID, string TransactionID, short ProviderType)
        {
            CreateCCFundingSourceSuccessResponse mainResponse = new();
            try
            {
                var paramurl = "pgAccountId=" + PGAccountID + "&txnAcId=" + TransactionID;
                string apiurl = string.Concat(paymenturls.CreateCCFundingSource, paramurl);
                string url = string.Concat(paymenturls.MainURL, apiurl);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddBody(InputString);
                    RestResponse resp = await client.ExecutePostAsync(request);
                    if (ProviderType == (short)ACHProviderTypeEnum.Forte && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Forte = JsonConvert.DeserializeObject<CreateForteCCSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Forte.message;
                    }
                    else
                    {
                        mainResponse.StatusCode = StatusCodes.Status500InternalServerError;
                        mainResponse.Status = "Error Occurred";
                    }
                }
                return mainResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mainResponse = null;
            }
        }

        public async Task<UpdateACHAdressSuccessResponse> UpdateACHAddress(string InputString, string PGAccountID, string TransactionID, short ProviderType)
        {
            UpdateACHAdressSuccessResponse mainResponse = new();
            try
            {
                var paramurl = "pgAccountId=" + PGAccountID + "&txnAcId=" + TransactionID;
                string apiurl = string.Concat(paymenturls.UpdateAddress, paramurl);
                string url = string.Concat(paymenturls.MainURL, apiurl);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddBody(InputString);
                    RestResponse resp = await client.ExecutePostAsync(request);
                    if (ProviderType == (short)ACHProviderTypeEnum.Forte && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Forte = JsonConvert.DeserializeObject<UpdateForteAddressSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Forte.message;
                    }
                    else if (ProviderType == (short)ACHProviderTypeEnum.SwirePay && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.SwirePay = JsonConvert.DeserializeObject<UpdateSwirePayAddressSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.SwirePay.message;
                    }
                    else
                    {
                        mainResponse.StatusCode = StatusCodes.Status500InternalServerError;
                        mainResponse.Status = "Error Occurred";
                    }
                }
                return mainResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mainResponse = null;
            }
        }

        public async Task<UpdateACHVendorSuccessResponse> UpdateACHVendor(string InputString, string PGAccountID, string TransactionID, short ProviderType)
        {
            UpdateACHVendorSuccessResponse mainResponse = new();
            try
            {
                var paramurl = "pgAccountId=" + PGAccountID + "&txnAcId=" + TransactionID;
                string apiurl = string.Concat(paymenturls.UpdateVendor, paramurl);
                string url = string.Concat(paymenturls.MainURL, apiurl);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddBody(InputString);
                    RestResponse resp = await client.ExecutePostAsync(request);
                    if (ProviderType == (short)ACHProviderTypeEnum.Forte && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Forte = JsonConvert.DeserializeObject<UpdateForteVendorSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Forte.message;
                    }
                    else if (ProviderType == (short)ACHProviderTypeEnum.SwirePay && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Swirepay = JsonConvert.DeserializeObject<UpdateSwirePayVendorSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Swirepay.message;
                    }
                    else
                    {
                        mainResponse.StatusCode = StatusCodes.Status500InternalServerError;
                        mainResponse.Status = "Error Occurred";
                    }
                }
                return mainResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mainResponse = null;
            }
        }
        public async Task<GetFundingSourceSuccessResponse> GetFundingSource(string InputString, string PGAccountID, string TransactionID, short ProviderType)
        {
            GetFundingSourceSuccessResponse mainResponse = new();
            try
            {
                var paramurl = "pgAccountId=" + PGAccountID + "&txnAcId=" + TransactionID;
                string apiurl = string.Concat(paymenturls.GetFundingSource, paramurl);
                string url = string.Concat(paymenturls.MainURL, apiurl);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddBody(InputString);
                    RestResponse resp = await client.ExecutePostAsync(request);
                    if (ProviderType == (short)ACHProviderTypeEnum.Forte && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Forte = JsonConvert.DeserializeObject<GetForteFundingSourceSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Forte.message;
                    }
                    else if (ProviderType == (short)ACHProviderTypeEnum.SwirePay && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Swirepay = JsonConvert.DeserializeObject<GetSwirePayFundingSourceSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Swirepay.message;
                    }
                    else
                    {
                        mainResponse.StatusCode = StatusCodes.Status500InternalServerError;
                        mainResponse.Status = "Error Occurred";
                    }
                }
                return mainResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mainResponse = null;
            }
        }
        public async Task<DeleteFundingSourceSuccessResponse> DeleteFundingSource(string InputString, string PGAccountID, string TransactionID, short ProviderType)
        {
            DeleteFundingSourceSuccessResponse mainResponse = new();
            try
            {
                var paramurl = "pgAccountId=" + PGAccountID + "&txnAcId=" + TransactionID;
                string apiurl = string.Concat(paymenturls.DeleteFundingSource, paramurl);
                string url = string.Concat(paymenturls.MainURL, apiurl);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddBody(InputString);
                    RestResponse resp = await client.ExecutePostAsync(request);
                    if (ProviderType == (short)ACHProviderTypeEnum.Forte && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Forte = JsonConvert.DeserializeObject<DeleteForteFundingSourceSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Forte.message;
                    }
                    else if (ProviderType == (short)ACHProviderTypeEnum.SwirePay && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Swirepay = JsonConvert.DeserializeObject<DeleteSwirePayFundingSourceSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Swirepay.message;
                    }
                    else
                    {
                        mainResponse.StatusCode = StatusCodes.Status500InternalServerError;
                        mainResponse.Status = "Error Occurred";
                    }
                }
                return mainResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mainResponse = null;
            }
        }
        public async Task<DeleteVendorSuccessResponse> DeleteVendor(string InputString, string PGAccountID, string TransactionID, short ProviderType)
        {
            DeleteVendorSuccessResponse mainResponse = new();
            try
            {
                var paramurl = "pgAccountId=" + PGAccountID + "&txnAcId=" + TransactionID;
                string apiurl = string.Concat(paymenturls.DeleteVendor, paramurl);
                string url = string.Concat(paymenturls.MainURL, apiurl);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddBody(InputString);
                    RestResponse resp = await client.ExecutePostAsync(request);
                    if (ProviderType == (short)ACHProviderTypeEnum.Forte && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Forte = JsonConvert.DeserializeObject<DeleteForteVendorSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Forte.message;
                    }
                    else if (ProviderType == (short)ACHProviderTypeEnum.SwirePay && resp.StatusCode == HttpStatusCode.OK)
                    {
                        mainResponse.Swirepay = JsonConvert.DeserializeObject<DeleteSwirePayVendorSuccessResponse>(resp.Content);
                        mainResponse.StatusCode = StatusCodes.Status200OK;
                        mainResponse.Status = mainResponse.Swirepay.message;
                    }
                    else
                    {
                        mainResponse.StatusCode = StatusCodes.Status500InternalServerError;
                        mainResponse.Status = "Error Occurred";
                    }
                }
                return mainResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mainResponse = null;
            }
        }

        #endregion
    }
}
