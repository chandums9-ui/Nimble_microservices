using Common.App.AppResponse;
using Common.App.Contracts;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.API.ExceptionHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;
        public ExceptionMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                string clientName = (string)httpContext.Items["ClientName"];
                _logger.LogError(Environment.NewLine + $"Something went wrong from {clientName}: {ex}" + Environment.NewLine);
                await HandleException(httpContext, ex);
            }
        }


      
        private async Task HandleException(HttpContext context, Exception exception)
        {

            context.Response.ContentType = "application/json";
            if (exception.HResult == -404) context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var response = new ApiResponse(StatusCodes.Status500InternalServerError);// context.Response.StatusCode);
            var jsonResp = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(jsonResp);

        }
    }
}
