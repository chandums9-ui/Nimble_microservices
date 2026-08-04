using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using UserMgmt.App.Services;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using UserMgmt.Infra.DBCon;
using CoreAccounting.Infra.DBCon;
using UserMgmt.Infra.DataRepos;
using CoreAccounting.Infra.DataRepos;
using AutoRecurrings;
using CoreAccounting.App.Service;
using AutoRecurrings;
using static Org.BouncyCastle.Math.EC.ECCurve;
using RestSharp;
using Common.App.Contracts;
using Common.Infra.Logger;
using NLog;
using Newtonsoft.Json;
using BankFeed.Domain.DTO.Model;
using Common.Domain.DTO.App;
using System.Security.Cryptography;
using static AutoRecurrings.AutoRecurringDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Linq;


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

      

        string clientAdminId= configuration.GetSection("ClinetAdminID").Value;
        string clientSecrectId= configuration.GetSection("ClinetSecretID").Value;

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

                    string coreDBConnection = configuration.GetConnectionString(dbName+ "Connection");
                   


                    string storedPName = "AutoRecurTransactions_UpDated";

                    List<string> jIds = new List<string>();

                    // Call the method to execute the stored procedure
                    jIds = await ExecuteStoredProcedure(coreDBConnection, storedPName);

                    if (jIds != null && jIds.Count > 0)
                    {
                        #region Token Generation && SyncMoreJournals
                        try
                        {
                            string? baseURL = configuration.GetSection("UserMgtBaseUrl").Value;
                            string? tokenGenarationURL = configuration.GetSection("UserTokenGenerationAPI").Value;

                            string token = await EndpointCalling(baseURL, tokenGenarationURL, clientAdminId, clientSecrectId, null,null,dbName);

                            if (token != null)
                            {
                                string? corebaseURL = configuration.GetSection("CoreBaseUrl").Value;
                                string? syncMoreJournalsURL = configuration.GetSection("SyncMoreJournalsAPI").Value;

                                string response = await EndpointCalling(corebaseURL, syncMoreJournalsURL, null, null, jIds, token, dbName);
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
        }
        catch (Exception ex)
        { throw; }
    }

    static async Task<List<string>> ExecuteStoredProcedure(string connectionString, string storedPName)
    {
        // Establish a connection to the database
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            List<string> journalIds = new List<string>();
            List<byte> journalByteIds = new List<byte>();
            string[] JIDs = null;
            try
            {
                // Open the connection
                connection.Open();

                

                //OpenAPIAuthentication userMgtService = new OpenAPIAuthentication(UserMgmt.App.Contracts.IUnitOfWork unitOfWork,);

                // Create a SqlCommand object to execute the stored procedure
                using (SqlCommand command = new SqlCommand(storedPName, connection))
                {
                    // Specify that the command is a stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    DataSet ds = new DataSet();
                    var dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string jId = row[0].ToString();

                            if (!string.IsNullOrEmpty(jId))
                            {                                
                                JIDs = jId.Split(',');
                                if(JIDs.Length>0&&  JIDs.Length==1)
                                {
                                    if (JIDs[0] == "Failed")
                                        JIDs = null;
                                }
                            }
                        }
                    }
                    

                }

                // Close the connection
                connection.Close();

                if (JIDs != null && JIDs.Count() > 0)
                {

                    foreach (var id in JIDs)
                    {
                        journalIds.Add(id);
                    }
                   
                }

                return journalIds;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }
    }
   
    static async Task<string> EndpointCalling(string? baseUrl, string? endPointURl, string clientAdminID,string clientSecrectID,List<string> journalIDs,string token,string urlName)
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
                             jsonReq = JsonConvert.SerializeObject(new { clientID = clientAdminID, clientSecret = clientSecrectID, urlName = urlName });

                        }
                        else
                        {
                            string jourIDs = string.Join(",",journalIDs);
                            
                             
                            
                            jsonReq = JsonConvert.SerializeObject(new { ID = jourIDs, CorpID= "000000000000000000000000000000000000", IsUpdatePrevious = false });
                            
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
                                
                                if(tokenRes != null && !string.IsNullOrEmpty(tokenRes.Token))
                                res = tokenRes.Token;

                            }else
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

   
}