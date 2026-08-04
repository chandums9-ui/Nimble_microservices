using Amazon.Runtime.Internal;
using Azure;
using Azure.Core;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model;
using Dashboard.App.Contracts;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Dashboard.Analytics.Infra.ProviderServices
{
    public class STRApiCalls : ISTRExternalAPIService, IDisposable
    {
        #region Properties
        //private readonly ProviderBaseURLInfo providerBaseUrl;
        //private readonly PlaidKeyInfo plaidKeyInfo;
        //private readonly PlaidEndPoints plaidEndPoints;
        private ILoggerService logger;
        private bool disposedValue;
        #endregion

        #region Ctor
        public STRApiCalls(ILoggerService _logger)
        {

            this.logger = _logger;
            
        }
        #endregion

        #region MultiCorporationSTRData

        /// <summary>
        /// Retrieves data for a specified week for the Multi Corporation.
        /// </summary>
        /// <param name="request">Details of the week or date range.</param>
        /// <returns>Returns STR week data for the Multi Corporation.</returns>
        public async Task<MultiCorpExternalApiSTRResponse> GetMultiCorpWeekData(STRWeekORRangeRequest request)
        {
            MultiCorpExternalApiSTRResponse? response = null;
            try
            {
                return await Post<MultiCorpExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Retrieves weekly data for the Multi Corporation based on the specified request parameters.
        /// </summary>
        /// <param name="request">Details of the week for which data is requested.</param>
        /// <returns>Returns STR weekly data for the Multi Corporation.</returns>
        public async Task<MultiCorpExternalApiSTRResponse> GetMultiCorpWeeklyData(STRWeeklyRequest request)
        {
            MultiCorpExternalApiSTRResponse? response = null;
            try
            {
                return await Post<MultiCorpExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Retrieves monthly data for the Multi Corporation.
        /// </summary>
        /// <param name="request">Details of the month for which data is requested.</param>
        /// <returns>Returns STR monthly data for the Multi Corporation.</returns>
        public async Task<MultiCorpExternalApiSTRResponse> GetMultiCorpMonthlyData(STRMontlyRequest request)
        {
            MultiCorpExternalApiSTRResponse? response = null;
            try
            {
                return await Post<MultiCorpExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Retrieves yearly data for the Multi Corporation.
        /// </summary>
        /// <param name="request">Details of the year for which data is requested.</param>
        /// <returns>Returns STR yearly data for the Multi Corporation.</returns>
        public async Task<MultiCorpExternalApiSTRResponse> GetMultiCorpYearlyData(STRYearlyRequest request)
        {
            MultiCorpExternalApiSTRResponse? response = null;
            try
            {
                return await Post<MultiCorpExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }


		/// <summary>
		/// Retrieves data for a specific date range for the Multi Corporation.
		/// </summary>
		/// <param name="request">Details of the start and end date for the range.</param>
		/// <returns>Returns STR range data for the Multi Corporation.</returns>
		public async Task<MultiCorpExternalApiSTRResponse> GetMultiCorpRangeData(STRWeekORRangeRequest request)
        {
            MultiCorpExternalApiSTRResponse? response = null;
            try
            {
                return await Post<MultiCorpExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        #endregion

        #region SingleCorporationSTRData

        /// <summary>
        /// Retrieves latest available data from the external api.
        /// </summary>
        public async Task<LatestAvailableDateResponse> GetLatestAvailableDate(LatestAvailableDateRequest request)
        {
            LatestAvailableDateResponse response = null;
            try
            {
                return await Post<LatestAvailableDateResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves week data from the external API.
        /// </summary>
        /// <returns>Returns week data from the external API</returns>
        public async Task<ExternalApiSTRResponse> GetSTRWeekData(STRWeekORRangeRequest request)
        {
            ExternalApiSTRResponse? response = null;
            try
            {
                return await Post<ExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Retrieves weekly data from the external API.
        /// </summary>
        /// <returns></returns>
        public async Task<ExternalApiSTRResponse> GetSTRWeeklyData(STRWeeklyRequest request)
        {
            ExternalApiSTRResponse? response = null;
            try
            {
                return await Post<ExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Retrieves montly data from the external API.
        /// </summary>
        /// <returns></returns>
        public async Task<ExternalApiSTRResponse> GetSTRMontlyData(STRMontlyRequest request)
        {
            ExternalApiSTRResponse? response = null;
            try
            {
                return await Post<ExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Retrieves yearly data from the external API.
        /// </summary>
        /// <returns></returns>
        public async Task<ExternalApiSTRResponse> GetSTRYearlyData(STRYearlyRequest request)
        {
            ExternalApiSTRResponse? response = null;
            try
            {
                return await Post<ExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Retrieves range data from the external API.
        /// </summary>
        /// <returns></returns>
        public async Task<ExternalApiSTRResponse> GetSTRRangeData(STRWeekORRangeRequest request)
        {
            ExternalApiSTRResponse? response = null;
            try
            {
                return await Post<ExternalApiSTRResponse>(request.UrlRequest, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

		#region STR OverView
		
		public async Task<ExternalApiSTRResponse> GetSTROverview(STRWeekORRangeRequest request)
		{
			ExternalApiSTRResponse? response = null;
			try
			{
				return await Post<ExternalApiSTRResponse>(request.UrlRequest, request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				response = null;
			}
		}
		#endregion



		#endregion

		#region STRImportScreen&GraphicalView



		/// <summary>
		/// Retrieves STR Month Data based on the provided request and URL.
		/// </summary>
		/// <param name="request">The request object containing the parameters for the data retrieval.</param>
		/// <param name="url">The URL endpoint to which the request is sent.</param>
		/// <returns>A task representing the asynchronous operation, containing the STRMonthDataResponse.</returns>
		/// <exception cref="Exception">Throws an exception if an error occurs during the request.</exception>
		public async Task<STRMonthDataResponse> GetSTRMonthData(STRMonthDataRequest request, string url)
        {
            STRMonthDataResponse response = new STRMonthDataResponse();
            try
            {
                response = await Post<STRMonthDataResponse>(url, request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Retrieves STR Grid Data based on the provided request and URL.
        /// </summary>
        /// <param name="request">The request object containing the parameters for the data retrieval.</param>
        /// <param name="url">The URL endpoint to which the request is sent.</param>
        /// <returns>A task representing the asynchronous operation, containing the STRGridDataResponse.</returns>
        public async Task<STRGridDataResponse> STRGridData(STRGridDataRequest request, string url)
        {
            STRGridDataResponse response = new STRGridDataResponse();
            try
            {
                response = await Post<STRGridDataResponse>(url, request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
           
        }

        /// <summary>
        /// Retrieves STR Day Wise Month Data based on the provided request and URL.
        /// </summary>
        /// <param name="request">The request object containing the parameters for the data retrieval.</param>
        /// <param name="url">The URL endpoint to which the request is sent.</param>
        /// <returns>A task representing the asynchronous operation, containing the STRDayWiseMonthData.</returns>
        public async Task<STRDayWiseMonthData> STRDayWiseMonthData(STRDayWiseMonthRequest request, string url)
        {
            STRDayWiseMonthData response = new STRDayWiseMonthData();
            try
            {
                response = await Post<STRDayWiseMonthData>(url, request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }

        }

        /// <summary>
        /// Retrieves ADR Multi Corp Month Data based on the provided request and URL.
        /// </summary>
        /// <param name="request">The request object containing the parameters for the data retrieval.</param>
        /// <param name="url">The URL endpoint to which the request is sent.</param>
        /// <returns>A task representing the asynchronous operation, containing the AdrMultiCorpData.</returns>
        public async Task<AdrMultiCorpData> GetAdrMultiCorpMonthData(STRMutliCorpMonthRequest request, string url)
        {
            AdrMultiCorpData response = new AdrMultiCorpData();
            try
            {
                response = await Post<AdrMultiCorpData>(url, request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }

        }

        /// <summary>
        /// Retrieves OCC Multi Corp Month Data based on the provided request and URL.
        /// </summary>
        /// <param name="request">The request object containing the parameters for the data retrieval.</param>
        /// <param name="url">The URL endpoint to which the request is sent.</param>
        /// <returns>A task representing the asynchronous operation, containing the OccMultiCorpData.</returns>
        public async Task<OccMultiCorpData> GetOccMultiCorpMonthData(STRMutliCorpMonthRequest request, string url)
        {
            OccMultiCorpData response = new OccMultiCorpData();
            try
            {
                response = await Post<OccMultiCorpData>(url, request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        /// <summary>
        /// Retrieves RevPAR Multi Corp Month Data based on the provided request and URL.
        /// </summary>
        /// <param name="request">The request object containing the parameters for the data retrieval.</param>
        /// <param name="url">The URL endpoint to which the request is sent.</param>
        /// <returns>A task representing the asynchronous operation, containing the RevparMultiCorpData.</returns>
        public async Task<RevparMultiCorpData> GetRevparMultiCorpMonthData(STRMutliCorpMonthRequest request, string url)
        {
            RevparMultiCorpData response = new RevparMultiCorpData();
            try
            {
                response = await Post<RevparMultiCorpData>(url, request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }

        }

        
        /// <summary>
        /// Uploads a file using multipart form data to the specified URL.
        /// </summary>
        /// <param name="request">The multipart form data content containing the file and other data to be uploaded.</param>
        /// <param name="url">The URL endpoint to which the file is uploaded.</param>
        /// <returns>A task representing the asynchronous operation, containing the STRFileUploadResponse.</returns>
        /// <exception cref="HttpRequestException">Thrown when an error occurs during the HTTP request.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public async Task<STRFileUploadResponse> STRFileUpload(MultipartFormDataContent request, string url)
        {
            List<STRFileUploadResponse> response1 = new List<STRFileUploadResponse>();
            STRFileUploadResponse response = new STRFileUploadResponse();

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var data = await httpClient.PostAsync(url, request);
                    if (data.IsSuccessStatusCode)
                    {
                        var content = await data.Content.ReadAsStringAsync();
                        response1 = JsonSerializer.Deserialize<List<STRFileUploadResponse>>(content);
                        response = response1.FirstOrDefault();    
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                // Log specific HttpRequestException details
                Console.WriteLine($"Request error: {httpRequestException.Message}");
                // Re-throw if needed or handle accordingly
                throw;
            }
            catch (Exception ex)
            {
                // Log general exception details
                Console.WriteLine($"Unexpected error: {ex.Message}");
                // Re-throw if needed or handle accordingly
                throw;
            }

            return response ?? new STRFileUploadResponse();
        }


        public async Task<DeleteSTRFileResponse> DeleteSTRFile(DeleteSTRFileRequest request,string url)
        {
            DeleteSTRFileResponse response = new DeleteSTRFileResponse();
            try
            {
                response = await Post<DeleteSTRFileResponse>(url, request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
        }

        #endregion

        #region GSS
        public async Task<GSSFileUploadResponse> GSSFileUpload(MultipartFormDataContent request, string url)
        {

            GSSFileUploadResponse response = new GSSFileUploadResponse();

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var data = await httpClient.PostAsync(url, request);
                    if (data.IsSuccessStatusCode)
                    {
                        var content = await data.Content.ReadAsStringAsync();
                        response = JsonSerializer.Deserialize<GSSFileUploadResponse>(content);
                        return response;
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                // Log specific HttpRequestException details
                Console.WriteLine($"Request error: {httpRequestException.Message}");
                // Re-throw if needed or handle accordingly
                throw;
            }
            catch (Exception ex)
            {
                // Log general exception details
                Console.WriteLine($"Unexpected error: {ex.Message}");
                // Re-throw if needed or handle accordingly
                throw;
            }

            return response ?? new GSSFileUploadResponse();
        }

        #endregion


        #region STRReportData
        public async Task<STRDayOrMonthOrWeekReport> GetSTRDayAndMonthReportData(STRReportRequest request, string url)
        {

            STRDayOrMonthOrWeekReport response = new STRDayOrMonthOrWeekReport();
            try
            {
                response = await Post<STRDayOrMonthOrWeekReport>(url, request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }
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

        #region HttpServices

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            //if (uri.ToLower().Contains("plaidconnect"))
            //   _httpClient.Timeout = TimeSpan.FromSeconds(300);

            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var content = JsonSerializer.Serialize(value);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return await sendRequest<T>(request);
        }
        public async Task<T> Put<T>(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            var content = JsonSerializer.Serialize(value);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return await sendRequest<T>(request);
        }

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            string ResContent = string.Empty;
            var options = new JsonSerializerOptions();
            try
            {
                //using var response = await _httpClient.SendAsync(request);
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("Unauthorized");
                        throw new UnauthorizedAccessException();
                    }
                    else if (!response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var res = await response.Content.ReadFromJsonAsync<ValidateTokenRespDTO>();
                            throw new Exception(res.Status);
                        }
                        catch
                        {
                            ResContent = await response.Content.ReadAsStringAsync();
                            throw new Exception(ResContent);
                        }
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        ResContent = await response.Content.ReadAsStringAsync();
                        return (T)Convert.ChangeType(ResContent, typeof(string));
                    }
                    else
                        return await response.Content.ReadFromJsonAsync<T>();
                }
            }
            catch (HttpRequestException rEx)
            {
                if (rEx.Message.ToLower().Contains("failed to fetch"))
                {
                    Console.WriteLine(rEx.Message);
                    //await _localStorageService.RemoveItemsAsync(new List<string>() { "token", "ups" });
                    //_toastService.ShowToast(_sharedResx.GetResourceValue("vld_SessionTimeOut"), ToastLevel.Warning);//"Session timeout"
                    //_toastService.ShowToast("Something went wrong!!", ToastLevel.Warning);
                    // ((AuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
                    //_navigationManager.NavigateTo("/error");
                }
                throw new Exception(rEx.GetMessage());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetMessage());
            }
        }

        #endregion
    }

    public static class ExceptionHelper
    {
        public static string GetMessage(this Exception ex)
        {
            if (ex.InnerException == null)
            {
                if (ex.Message.Contains("DELETE statement conflicted"))
                    return "Can not delete this record. It is being used in one or more records";

                return ex.Message;
            }


            return ex.InnerException.GetMessage();
        }
    }
}