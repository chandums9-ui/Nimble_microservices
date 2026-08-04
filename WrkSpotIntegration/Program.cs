using RestSharp;
using Common.App.Contracts;
using Common.Infra.Logger;
using NLog;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using static WrkSpotIntegration.wrkSpotIntegrationDTO;


public class Program
{
    static async Task Main()
    {
        #region Configuration

        // Define connection strings
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, true);
        IConfiguration configuration = builder.Build();

        LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        ILoggerService logger = new LoggerService();



        string clientAdminId = configuration.GetSection("ClinetAdminID").Value;
        string clientSecrectId = configuration.GetSection("ClinetSecretID").Value;

        


        #endregion


        try
        {
            #region Token Generation && WrkSpotIntegrationDataCalling
            try
            {
                string? baseURL = configuration.GetSection("UserMgtBaseUrl").Value;
                string? tokenGenarationURL = configuration.GetSection("UserTokenGenerationAPI").Value;

                string token = await EndpointCalling(baseURL, tokenGenarationURL, clientAdminId, clientSecrectId, null);

                if (token != null)
                {
                    string? corebaseURL = configuration.GetSection("CoreBaseUrl").Value;
                    string? WrkSpotIntegrationURL = configuration.GetSection("WrkSpotIntegrationAPI").Value;

                    string response = await EndpointCalling(corebaseURL, WrkSpotIntegrationURL, null, null,token);
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
            }
            #endregion
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    static async Task<string> EndpointCalling(string? baseUrl, string? endPointURl, string clientAdminID, string clientSecrectID, string token)
    {
        string res = string.Empty;
        if (!string.IsNullOrEmpty(baseUrl) && !string.IsNullOrEmpty(endPointURl))
        {

            string url = string.Concat(baseUrl, endPointURl);
            var restOptions = new RestClientOptions($"{url}");
            if (restOptions != null)
            {

                using (var client = new RestClient(restOptions))
                {
                    try
                    {
                        string jsonReq = string.Empty;
                        var request = new RestRequest();
                        restOptions.Timeout = TimeSpan.FromMinutes(59);
                        if (!string.IsNullOrEmpty(clientAdminID))
                        {
                            jsonReq = JsonConvert.SerializeObject(new { clientID = clientAdminID, clientSecret = clientSecrectID, urlName = "navika" });

                        }
                        else
                        {
                            //jsonReq
                            request.AddHeader("Authorization", "Bearer " + token);
                        }
                        request.AddHeader("Content-Type", "application/json");
                        request.AddJsonBody(jsonReq);

                        RestResponse resp = await client.ExecutePostAsync(request);

                        if (resp != null && resp.IsSuccessful)
                        {
                            if (token == null)
                            {
                                var tokenRes = JsonConvert.DeserializeObject<AuthToken>(resp.Content);

                                if (tokenRes != null && !string.IsNullOrEmpty(tokenRes.Token))
                                    res = tokenRes.Token;

                            }
                            else
                            {
                                res = resp.StatusDescription;
                            }

                        }
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return res;
    }

    
}
