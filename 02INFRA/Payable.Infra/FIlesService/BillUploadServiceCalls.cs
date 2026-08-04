using Azure;
using Common.App.Contracts;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Payable.App.Contracts;
using Payable.Domain.DTO.Req;
using Payable.Domain.DTO.Resp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Infra.FIlesService
{
    public class BillUploadServiceCalls : IBillUploadService
    {
        #region Fields

        private readonly BillUploadURLs billUploadURLs;

        #endregion Fields End

        #region Ctor
        public BillUploadServiceCalls(IOptions<BillUploadURLs> _billUploadurls)
        {
            this.billUploadURLs = _billUploadurls.Value;
        }
        #endregion ctor

        #region methods
        public async Task<BillUploadResponse> BillFilesUpload(MultipartFormDataContent formData,string clientURL)
        {
            BillUploadResponse response = new BillUploadResponse();
            //externalAPI url
            string url = string.Concat(billUploadURLs.MainURL, billUploadURLs.upload_url);
            formData.Add(new StringContent(clientURL), "client_url");
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    //Request call for external API
                    var data = await httpClient.PostAsync(url, formData);
                    if (data.IsSuccessStatusCode)
                    {
                        response.Status = await data.Content.ReadAsStringAsync();
                        response.StatusCode = (int)data.StatusCode;
                    }
                    else
                    {
                        string contentString = await data.Content.ReadAsStringAsync();
                        if (!(contentString.StartsWith("{") && contentString.EndsWith("}")) || 
                            (contentString.StartsWith("[") && contentString.EndsWith("]")))
                        {
                            response.Status = contentString.ToString();
                            response.StatusCode = StatusCodes.Status500InternalServerError;
                        }
                        else
                        {
                            var errorResponse = JsonConvert.DeserializeObject<UploadErrorResponse>(contentString);
                            response.Status = errorResponse?.detail?.FirstOrDefault()?.msg ?? null;
                            response.StatusCode = StatusCodes.Status500InternalServerError;
                        }
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                Console.WriteLine($"Request error: {httpRequestException.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
            return response;
        }
        #endregion methods
    }
}
