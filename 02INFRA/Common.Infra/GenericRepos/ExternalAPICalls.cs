using Common.App.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Common.Domain.DTO.Model;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Common.Domain.DTO.Req;
using Common.Infra.GenericRepos;
using Common.Domain.DTO.Resp;
using Common.Domain.DTO.Model.Base;

namespace Common.Infra.GenericRepos
{
    public class ExternalAPICalls : IExternalAPICalls
    {
        #region Properties
        private readonly ExternalApiURLs extApiUrl;
        private ILoggerService logger;
        private readonly IHttpService httpService;
        private bool disposedValue;

        #endregion Properties

        #region Ctor
        public ExternalAPICalls(IOptions<ExternalApiURLs> _extApiUrl, IHttpService _httpService, ILoggerService _logger)
        {
            this.extApiUrl = _extApiUrl.Value;
            this.logger = _logger;
            this.httpService = _httpService;
        }
        #endregion

        public async Task<TResponse> CoreAPICall<TRequest, TResponse>(TRequest Request, string AuthToken, string endpoint)
        {
            try
            {
                return await httpService.Post<TResponse>(Path.Combine(extApiUrl.CoreApiUrl, endpoint), Request, AuthToken);
            }
            catch { throw; }
        }

        public async Task<TResponse> UserMngtAPICall<TRequest, TResponse>(TRequest Request, string AuthToken, string endpoint)
        {
            try
            {
                return await httpService.Post<TResponse>(Path.Combine(extApiUrl.UserMngtApiUrl, endpoint), Request, AuthToken);
            }
            catch { throw; }
        }

        public async Task<TResponse> STRAPICall<TRequest, TResponse>(TRequest Request, string AuthToken, string endpoint)
        {
            try
            {
                return await httpService.Post<TResponse>(Path.Combine(extApiUrl.STRApiUrl, endpoint), Request, AuthToken);
            }
            catch { throw; }
        }

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }




        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}
