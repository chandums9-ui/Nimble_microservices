
using Azure.Core;
using Common.App.Contracts;
using Common.Domain.DTO.Resp;
using Common.Infra.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Xml;
using static WarehouseDumps.WarehouseDumpDTO;


public class Program
{
    public static async Task Main()
    {
        #region Configuration
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
                    #region Token Generation & Warehouse Dump Process
                    try
                    {
                        string? baseURL = configuration.GetSection("UserMgtBaseUrl").Value;
                        string? tokenGenarationURL = configuration.GetSection("UserTokenGenerationAPI").Value;
                        string token = await EndpointCalling(baseURL, tokenGenarationURL, clientAdminId, clientSecrectId, null, dbName);

                        if (token != null)
                        {
                            var warehouseBase = configuration["warehousebaseUrl"];

                            var endpointKeys = new[]
                                                 {
                                                    "SynchCorpAPI",
                                                    "SyncAccrualAPI",
                                                    "SynchAdjustmentsAPI",
                                                    "SyncSalesAPI",
                                                    "SyncBudgetAPI",
                                                    "SyncForecastAPI"

                                                };

                            foreach (var key in endpointKeys)
                            {
                                string? apiPath = configuration[key];
                                if (!string.IsNullOrEmpty(apiPath))
                                {
                                    logger.LogInfo($"Calling warehouse API: {apiPath}");
                                    var resp = await CallWarehouseEndpoint(configuration, warehouseBase, apiPath, token, dbName, key);
                                    logger.LogInfo($"Response: {resp?.Id ?? "No ID"}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Error during sync for DB {dbName}: {ex.Message}");
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
    private static async Task<GenericStringResponse> CallWarehouseEndpoint(IConfiguration configuration, string baseUrl, string endpoint, string token, string corpId, string apiKey)
    {
        var result = new GenericStringResponse();

        try
        {
            if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(endpoint))
                return result;

            var getApis = configuration.GetSection("WarehouseEndpoints:GetApis").Get<string[]>();
            var postApis = configuration.GetSection("WarehouseEndpoints:PostApis").Get<string[]>();

            bool isGet = getApis?.Contains(apiKey) ?? false;

            var options = new RestClientOptions($"{baseUrl}{endpoint}")
            {
                ConfigureMessageHandler = handler =>
                {// Configure XML reader settings
                    var settings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Prohibit, // or DtdProcessing.Parse with proper configuration
                        XmlResolver = null, // Disable external entity resolution
                        MaxCharactersFromEntities = 0
                    };
                    return new HttpClientHandler();
                },
                Timeout = TimeSpan.FromMinutes(100),
            };

            var client = new RestClient(options);

            var request = new RestRequest()
                .AddHeader("Authorization", $"Bearer {token}")
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Accept", "application/json");
            string jsonReq = string.Empty;
            if (isGet)
            {
                request.Method = Method.Get;
                request.AddQueryParameter("CorpID", corpId);
                var resp = await client.ExecuteGetAsync<GenericStringResponse>(request);
                result = resp.Data;
            }
            else
            {
                request.Method = Method.Post;
                request.AddJsonBody(new { });
                var resp = await client.ExecutePostAsync<GenericStringResponse>(request);
                result=resp.Data;
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in CallWarehouseEndpoint: {ex.Message}");
            return new GenericStringResponse { Id = $"Error: {ex.Message}" };
        }
    }
}