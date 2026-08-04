#region Packages
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Common.Infra.Logger;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using UserMgmt.Domain.DTO.Resp;
#endregion
#region Configuration
var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, true);
IConfiguration configuration = builder.Build();
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
ILoggerService logger = new LoggerService();
#endregion
UserMigrationsResp userResp = null;
ClientMigrationsResp clientResp = null;
string baseMigrationURL = configuration.GetSection("UserMgmtMigration:baseurl").Value;
string baseGetURL = configuration.GetSection("UserMigrationInfo:baseurl").Value;
string pwdRefKey = configuration.GetSection("pwdrefKey").Value;
#region ClientMigration

if (!string.IsNullOrEmpty(baseGetURL) && !string.IsNullOrEmpty(baseMigrationURL))
{
    string clientsUrls = configuration.GetSection("UserMigrationInfo:clientsinfo").Value;
    ClientMigrationsResp clientMigrationresp = null;
    if (!string.IsNullOrEmpty(clientsUrls))
    {
        string clurl = string.Concat(baseGetURL, clientsUrls);
       
        using (var client = new RestClient())
        {
            try
            {
                var request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                RestResponse resp = await client.ExecuteGetAsync(request);
               
                if (resp != null && !string.IsNullOrEmpty(resp.Content))
                  clientMigrationresp = JsonConvert.DeserializeObject<ClientMigrationsResp>(resp.Content);
                else
                    clientMigrationresp=new ClientMigrationsResp();
            }
            catch (Exception ex)
            {
                logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
            }

        }
        string clientMigrURL = configuration.GetSection("UserMgmtMigration:clientMigration").Value;
   
        if (!string.IsNullOrEmpty(clientMigrURL) && clientMigrationresp!=null && clientMigrationresp.Clients.Count()>0)
        {
            string clMigurl = string.Concat(baseMigrationURL, clientMigrURL);
            
            foreach (ClientRegisterRequest cr in clientMigrationresp.Clients)
            {
                string pwd= Cryptography.DecryptData(cr.Password,new PFAID(cr.UserID).UID);
                cr.Password= Cryptography.EncryptStringData(cr.Password, pwdRefKey);
               
            }

            using (var client = new RestClient())
            {
                try
                {
                    string jsonBody=JsonConvert.SerializeObject(clientMigrationresp.Clients);
                    var request = new RestRequest();
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(jsonBody);
                    RestResponse resp = await client.PostAsync(request);

                }
                catch (Exception ex)
                {
                    logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
                }

            }
        }
    }
    #endregion
    #region UserMigration
    string usersUrls = configuration.GetSection("UserMigrationInfo:userinfo").Value;
    UserMigrationsResp userMigrationresp = null;
    if (!string.IsNullOrEmpty(usersUrls) )
    {
        string userurl = string.Concat(baseGetURL, usersUrls);
       
        using (var client = new RestClient())
        {
            try
            {
                var request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                RestResponse resp = await client.ExecuteGetAsync(request);

                if (resp != null && !string.IsNullOrEmpty(resp.Content))
                    userMigrationresp = JsonConvert.DeserializeObject<UserMigrationsResp>(resp.Content);
                else
                    userMigrationresp = new UserMigrationsResp();
            }
            catch (Exception ex)
            {
                logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
            }

        }
    }
    string userMigrURL = configuration.GetSection("UserMgmtMigration:userMigration").Value;
    if (!string.IsNullOrEmpty(userMigrURL) && userMigrationresp != null && userMigrationresp.Users.Count() > 0)
    {
        string userMigurl = string.Concat(userMigrURL, userMigrURL);
       
        foreach (UserInfoRequest cr in userMigrationresp.Users)
        {
            string pwd = Cryptography.DecryptData(cr.Password, new PFAID(cr.UserID).UID);
            cr.Password = Cryptography.EncryptStringData(cr.Password, pwdRefKey);

        }
        using (var client = new RestClient())
        {
            try
            {
                string jsonBody = JsonConvert.SerializeObject(userMigrationresp.Users);
                var request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(jsonBody);
                RestResponse resp = await client.PostAsync(request);

            }
            catch (Exception ex)
            {
                logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
            }

        }
    }
    #endregion
}

