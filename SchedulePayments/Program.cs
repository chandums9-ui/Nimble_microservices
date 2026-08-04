using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Infra.Logger;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using System.Data;
using System.Data.Common;
using UserMgmt.Domain.DTO.Resp;


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



        string UMDBConnection = configuration.GetSection("ConnectionStrings:UMDBConnection").Value; ;
        string clientAdminId = configuration.GetSection("ClinetAdminID").Value;
        string clientSecrectId = configuration.GetSection("ClinetSecretID").Value;

        string ClientDBs = configuration.GetSection("ClientDBs:DBs").Value;
        string[] commaSepartedDBs = null;

        if (!string.IsNullOrEmpty(ClientDBs))
            commaSepartedDBs = ClientDBs.Split(',');


        #endregion

        try
        {
            if (commaSepartedDBs != null && commaSepartedDBs.Count() > 0)
            {
                foreach (var db in commaSepartedDBs)
                {
                    string dbName = db.Trim();

                    string coreDBConnection = configuration.GetConnectionString(dbName + "Connection");




                    #region Token Generation & GetClientIds && Paid Process
                    try
                    {
                        string? baseURL = configuration.GetSection("UserMgtBaseUrl").Value;
                        string? tokenGenarationURL = configuration.GetSection("UserTokenGenerationAPI").Value;
                        string token = await EndpointCalling(baseURL, tokenGenarationURL, clientAdminId, clientSecrectId, null, dbName);

                        if (token != null)
                        {
                            string? GetClientIdsURL = configuration.GetSection("GetClientIDsAPI").Value;
                            var clientIdsRes = await EndpointCallingForListOfClients(baseURL, GetClientIdsURL, null, null, token, dbName);
                            if (clientIdsRes != null && clientIdsRes.Count > 0)
                            {
                                foreach (var clientId in clientIdsRes)
                                {
                                    string clientID = clientId.ClientId.Replace("0x", "").Trim();
                                    string? payableURL = configuration.GetSection("PayableUrl").Value;
                                    string? RepayPaymentURL = configuration.GetSection("RepayPaymentAPI").Value;

                                    string response = await EndpointCalling(payableURL, RepayPaymentURL, clientID, null, token, dbName);

                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
                    }
                    #endregion
                }

            }
        }
        catch (Exception ex)
        { throw; }
    }



    static async Task<string> EndpointCalling(string? baseUrl, string? endPointURl, string clientId, string clientScrectId, string token, string urlName)
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

                        if (!string.IsNullOrEmpty(clientScrectId))
                        {
                            jsonReq = JsonConvert.SerializeObject(new { clientID = clientId, clientSecret = clientScrectId, urlName = urlName });//, urlName = "qapayable"
                            request.AddJsonBody(jsonReq);
                        }
                        else
                        {
                            jsonReq = JsonConvert.SerializeObject(new { clientID = clientId });
                            request.AddHeader("Authorization", "Bearer " + token);
                            request.AddJsonBody(jsonReq);
                        }



                        request.AddHeader("Content-Type", "application/json");


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
                    catch (Exception ex)
                    {
                        //logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
                        throw;
                    }
                }

            }

        }
        return res;
    }
    static async Task<List<ClientIdList>> EndpointCallingForListOfClients(string? baseUrl, string? endPointURl, string clientId, string clientScrectId, string token, string urlName)
    {
        List<ClientIdList> res = new List<ClientIdList>();
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

                        request.AddHeader("Authorization", "Bearer " + token);


                        request.AddHeader("Content-Type", "application/json");


                        RestResponse resp = await client.ExecutePostAsync(request);

                        if (resp != null && resp.IsSuccessful)
                        {
                            res = JsonConvert.DeserializeObject<List<ClientIdList>>(resp.Content);
                        }

                    }
                    catch (Exception ex)
                    {
                        //logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
                        throw;
                    }
                }

            }

        }
        return res;
    }
}