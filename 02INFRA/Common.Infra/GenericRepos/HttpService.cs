using Common.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common.App.Contracts;
using System.Net.Http.Headers;

namespace Common.Infra.GenericRepos
{
    public class HttpService : IHttpService
    {
        #region HttpServices

        public async Task<T> Get<T>(string uri, string authnToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request, authnToken);
        }

        public async Task<T> Post<T>(string uri, object value, string authnToken)
        {
            //if (uri.ToLower().Contains("plaidconnect"))
            //   _httpClient.Timeout = TimeSpan.FromSeconds(300);

            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var content = JsonSerializer.Serialize(value);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return await sendRequest<T>(request, authnToken);
        }
        public async Task<T> Put<T>(string uri, object value, string authnToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            var content = JsonSerializer.Serialize(value);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return await sendRequest<T>(request, authnToken);
        }

        private async Task<T> sendRequest<T>(HttpRequestMessage request, string authnToken)
        {
            string ResContent = string.Empty;
            var options = new JsonSerializerOptions();
            try
            {
                if (!string.IsNullOrEmpty(authnToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", !string.IsNullOrEmpty(authnToken) ? authnToken : "BearerToken");

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
