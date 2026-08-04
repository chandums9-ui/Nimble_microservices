// See https://aka.ms/new-console-template for more information
#region Packages
using Common.App.Contracts;
using Common.Infra.Logger;
using Microsoft.Extensions.Configuration;
using NLog;
using RestSharp;
#endregion

#region Configuration
var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, true);
IConfiguration configuration = builder.Build();
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
ILoggerService logger = new LoggerService();
#endregion


#region AutoSynchFeeds
string? baseURL = configuration.GetSection("UserMgmtMigration:").Value;
string? autoSynchFeedURL = configuration.GetSection("BankFeedAutoFeedSynchAPI").Value;

if (!string.IsNullOrEmpty(baseURL) && !string.IsNullOrEmpty(autoSynchFeedURL))
{
    ;
    string url=string.Concat(baseURL, autoSynchFeedURL);
    var restOptions = new RestClientOptions($"{url}");
    if (restOptions != null)
    {
        restOptions.Timeout = TimeSpan.FromMinutes(59);

        using (var client = new RestClient(restOptions))
        {
            try
            {
                var request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                RestResponse resp = await client.ExecuteGetAsync(request);

            }
            catch (Exception ex)
            {
                logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
            }
        }
    }
}
#endregion

#region InActiveAccountSynch

string? inActiveAccSyncURL = configuration.GetSection("BankFeedInActiveSynchAPI").Value;
if (!string.IsNullOrEmpty(baseURL) && !string.IsNullOrEmpty(inActiveAccSyncURL))
{
    
    string url = string.Concat(baseURL, inActiveAccSyncURL);
    var restInActiveOptions = new RestClientOptions($"{url}");
    if (restInActiveOptions != null)
    {
        restInActiveOptions.Timeout = TimeSpan.FromMinutes(59);
        using (var client = new RestClient(restInActiveOptions))
        {
            try
            {
                var request = new RestRequest();
                request.Timeout = TimeSpan.FromMinutes(55);
                request.AddHeader("Content-Type", "application/json");
                RestResponse resp = await client.ExecuteGetAsync(request);

            }
            catch (Exception ex)
            {
                logger.LogError($"some thing went wrong in auto sync api call with {ex.Message}");
            }
        }
    }
}
#endregion