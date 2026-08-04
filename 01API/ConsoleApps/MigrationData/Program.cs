using Common.App.Contracts;
using Common.Infra.Logger;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using System.Data;
using System.Xml.Linq;
using static MigrationData.migrationDTO;

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

        #region Input from  User

        Console.WriteLine("Enter Your Url");

        string urlName = Console.ReadLine();

        Console.WriteLine("$ Url Name: {urlName}");

        #endregion

        #region Token Generation

        try
        {
            if (commaSepartedDBs != null && commaSepartedDBs.Count() > 0)
            {
                foreach (var db in commaSepartedDBs)
                {
                    string dbName = db.Trim();

                    string coreDBConnection = configuration.GetConnectionString(dbName + "Connection");


                    string? baseURL = configuration.GetSection("UserMgtBaseUrl").Value;
                    string? tokenGenarationURL = configuration.GetSection("UserTokenGenerationAPI").Value;
                    string token = await EndpointCalling(baseURL, tokenGenarationURL, clientAdminId, clientSecrectId, null, urlName,false);

                    if (token != null)
                    {
                        #region MigrationClients && MigrationUsers

                        string? coreURL = configuration.GetSection("CoreBaseUrl").Value;
                        string? migrationClientsURL = configuration.GetSection("MigrationClientsAPI").Value;
                        string? migrationUsersURL = configuration.GetSection("MigrationUsersAPI").Value;
                        string? sharingMigrationUsesURL = configuration.GetSection("SharingMigrationUsersAPI").Value;

                        var migrationClientsRes = await EndpointCalling(coreURL, migrationClientsURL, null, null, token, urlName,true);

                        var migrationUsersRes = await EndpointCalling(coreURL, migrationUsersURL, null, null, token, urlName,true);

                        //sharing MigrationUsers Info
                        var sharingMUsersRes = await EndpointCalling(coreURL, sharingMigrationUsesURL, null, null, token, urlName,false);

                        if (sharingMUsersRes != null)
                        {
                            string storedPName = configuration.GetSection("MigrationUsersSP").Value;
                            await ExecuteStoredProcedure(UMDBConnection, storedPName, urlName);
                        }

                    }
                }
                #endregion
            }

        }
        catch (Exception ex)
        {
            throw;
        }

      #endregion
    }

    static async Task<string> EndpointCalling(string? baseUrl, string? endPointURl, string clientId, string clientScrectId, string token, string urlName,bool isGetReq)
    {
        string res = string.Empty;
        RestResponse resp = new RestResponse();
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
                            //jsonReq = JsonConvert.SerializeObject(new { clientID = clientId });
                            request.AddHeader("Authorization", "Bearer " + token);
                            //request.AddJsonBody(jsonReq);
                        }



                        request.AddHeader("Content-Type", "application/json");

                        if(isGetReq == true)
                        {
                            resp = await client.ExecuteGetAsync(request);
                        }
                        else 
                        {
                            resp = await client.ExecutePostAsync(request);
                        }
                         

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

    static async Task<string> ExecuteStoredProcedure(string connectionString, string storedPName,string urlName)
    {
        string res=string.Empty;
        // Establish a connection to the database
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
           
            try
            {
                // Open the connection
                connection.Open();

                // Create a SqlCommand object to execute the stored procedure
                using (SqlCommand command = new SqlCommand(storedPName, connection))
                {
                    // Specify that the command is a stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters (if required by the stored procedure)
                    command.Parameters.Add(new SqlParameter("@ClientName", SqlDbType.VarChar)).Value = urlName;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //res = reader.GetString(0);
                        Console.WriteLine("Stored procedure executed successfully.");
                    }
                    
                
                }
                // Close the connection
                connection.Close();

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}