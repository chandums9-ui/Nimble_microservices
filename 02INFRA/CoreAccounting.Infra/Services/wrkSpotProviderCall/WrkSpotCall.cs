
using Common.App.Contracts;
using Common.Domain.DTO.Req;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Infra.Services.wrkSpotProviderCall
{
    public class WrkSpotCall: IwrkSpotCall
    {
        #region Fields
        private readonly WrkSpotProviderInfo providerBaseUrl;
        private readonly WrkSpotKeyInfo wrkSpotKeyInfo;
        private readonly WrkSpotEndPoints wrkSpotEndPoints;
        private bool disposedValue;
        private ILoggerService logger;
        #endregion

        public WrkSpotCall(IOptions<WrkSpotProviderInfo> _providerBaseUrl, IOptions<WrkSpotKeyInfo> _wrkSpotKeyInfo, IOptions<WrkSpotEndPoints> _wrkSpotEndPoints, ILoggerService _logger)
        {
            this.providerBaseUrl = _providerBaseUrl.Value;
            this.wrkSpotKeyInfo = _wrkSpotKeyInfo.Value;
            this.wrkSpotEndPoints = _wrkSpotEndPoints.Value;
            this.logger = _logger;
        }

        #region WrkSpot

        public async Task<token> GetAuthToken(string clientID, bool isAppToken = false)//, string clientName
        {
            string name = string.Empty;
            token tokenInfo = new token();
            RestResponse resp = null;
            string jsonReq = string.Empty;
            try
            {
                string url = String.Concat(providerBaseUrl.WrkSpotURL, wrkSpotEndPoints.AuthToken);
                using (var client = new RestClient(url))
                {

                    var request = new RestRequest();
                    //jsonReq = JsonConvert.SerializeObject(new { vendorCode = wrkSpotKeyInfo.VendorCode, secretKey = wrkSpotKeyInfo.SecretKey });
                     jsonReq = "{ \"vendorCode\": \"" + wrkSpotKeyInfo.VendorCode + "\",\"secretKey\": \"" + wrkSpotKeyInfo.SecretKey + "\" }";

                    request.AddHeader("Content-Type", "application/json");
                    //request.CachePolicy=
                    //request.AddHeader("loginName", name);
                    //request.AddHeader("Api-Version", "1.1");
                    request.AddJsonBody(jsonReq);
                    request.Timeout = TimeSpan.FromMinutes(10);
                    
                    resp = await client.ExecutePostAsync(request);
                    tokenInfo = JsonConvert.DeserializeObject<token>(resp.Content);
                    logger.LogTrace(String.Concat("Auth Token: ", tokenInfo.access_token, "-->Name :", name));
                }

            }
            catch (Exception ex)
            {
                logger.LogError(String.Concat("Error @GetAuthToken WrkSpot: ", "-->Name :", name, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                throw;
            }
            return tokenInfo;
        }

        public async Task<Summary> SummaryDetails(string token,List<string> siteCodes, CorpSearchRequest req)
        {
            string name = string.Empty;

            Summary summaryDetails = new Summary();
            RestResponse resp = null;
            string jsonReq = string.Empty;
            try
            {
                string url = String.Concat(providerBaseUrl.WrkSpotURL, wrkSpotEndPoints.Summary);
                using (var client = new RestClient(url))
                {

                    var request = new RestRequest();
                    jsonReq = JsonConvert.SerializeObject(new { siteCodes = siteCodes, startDate = req.FromDate, endDate = req.ToDate });

                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Api-Version", "1.1");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("loginName", name);
                    request.AddJsonBody(jsonReq);
                    resp = await client.ExecutePostAsync(request);
                    summaryDetails = JsonConvert.DeserializeObject<Summary>(resp.Content);
                    logger.LogTrace(String.Concat("SummaryDetails: ", summaryDetails.propertyName, "-->Name :", name));
                }

            }
            catch (Exception ex)
            {
                logger.LogError(String.Concat("Error @SummaryDetails WrkSpot: ", "-->Name :", name, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                throw;
            }
            return summaryDetails;
        }

        public async Task<PropetiesList> GetProperties(string token)
        {
            PropetiesList listOfProperties = new PropetiesList();
            string name = string.Empty;
            RestResponse resp = null;
            try
            {
                string url = String.Concat(providerBaseUrl.WrkSpotURL, wrkSpotEndPoints.Properties);
                using (var client = new RestClient(url))
                {

                    var request = new RestRequest();

                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Api-Version", "1.1");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("loginName", name);
                    resp = await client.ExecuteGetAsync(request);
                    listOfProperties = JsonConvert.DeserializeObject<PropetiesList>(resp.Content);
                    logger.LogTrace(String.Concat("Properites Count: ", listOfProperties.properties.Count));
                }

            }
            catch (Exception ex)
            {
                logger.LogError(String.Concat("Error @GetProperties WrkSpot: ", "-->Name :", name, "-->exc mess :", ex.Message, Environment.NewLine + "Content: ", (resp != null ? resp.Content : string.Empty)));
                throw;
            }
            return listOfProperties;
        }

        #endregion
    }
}
