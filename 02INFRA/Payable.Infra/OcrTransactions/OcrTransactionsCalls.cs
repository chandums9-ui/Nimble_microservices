using Amazon.Runtime.Internal;
using Azure;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Payable.App.Contracts;
using Payable.Domain.DTO.Req;
using Payable.Domain.DTO.Resp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Infra.OcrTransactions
{
    public class OcrTransactionsCalls : IOcrTransactionService
    {
        #region Fields

        private readonly OCRTransctionsURLs ocrTransurl;
        private readonly ILoggerService logger;

        #endregion Fields End

        #region Ctor
        public OcrTransactionsCalls(IOptions<OCRTransctionsURLs> _ocrTransurl, ILoggerService _logger)
        {
            this.ocrTransurl = _ocrTransurl.Value;
            this.logger = _logger;
        }
        #endregion ctor

        #region Methods
        public async Task<OcrTransCountResponse> GetOcrTransactionsCount(string CorpId, string clientURL)
        {
            OcrTransCountResponse response = new OcrTransCountResponse();
            try
            {
              
                string url = string.Concat(ocrTransurl.MainURL, ocrTransurl.OcrTransactionsCountURL);
                using (var client = new RestClient(url))
                {
                    var request = new RestRequest();
                    request.AddQueryParameter("corporation_id", CorpId);
                    request.AddQueryParameter("client_url", clientURL);
                    RestResponse resp = await client.ExecuteGetAsync(request);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        response = JsonConvert.DeserializeObject<OcrTransCountResponse>(resp.Content);
                        response.StatusCode = StatusCodes.Status200OK;
                    }
                    else
                    {
                        var errorResposne = JsonConvert.DeserializeObject<ChatErrorResponse>(resp.Content);
                        response.Status = errorResposne?.detail?.FirstOrDefault()?.msg ?? null;
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = Constants.MSG_FAILED_WENT_WRONG;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                logger.LogError(Environment.NewLine + $"Something went wrong: {ex}" + Environment.NewLine);
            }

            return response;
        }
        #endregion Methods
    }
}
