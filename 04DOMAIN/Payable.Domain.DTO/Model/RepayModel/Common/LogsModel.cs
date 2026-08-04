using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Payable.Domain.DTO.Model.RepayModel.Common
{
    public class LogsModel
    {

        public List<TraceModel> Trace { get; set; }
        public string RawRequest { get; set; }
        public string RawResponse { get; set; }
        public HttpHeaders RequestHeaders { get; set; }
        public HttpHeaders ResponseHeaders { get; set; }
        public Exception Exception { get; set; }

        public void LogMessage(string message)
        {
            Trace = Trace ?? new List<TraceModel>();
            Trace.Add(new TraceModel()
            {
                TimeStamp = DateTime.UtcNow,
                Message = message
            });
        }

        public static LogsModel GetLogsBasedOnRequest(LogsModel logs, List<LogLevel> logTypes)
        {
            if (logTypes == null)
            {
                return new LogsModel
                {
                    Exception = logs.Exception,
                };
            }
            else
            {
                return new LogsModel
                {
                    Exception = logs.Exception,
                    Trace = logTypes.Contains(LogLevel.Trace) ? logs.Trace : null,
                    RawRequest = logTypes.Contains(LogLevel.RawRequest) ? logs.RawRequest :(logs.RawRequest!=null)? logs.RawRequest: null,
                    RawResponse = logTypes.Contains(LogLevel.RawResponse) ? logs.RawResponse :( logs.RawResponse!=null)? logs.RawResponse: null,
                    RequestHeaders = logTypes.Contains(LogLevel.RequestHeaders) ? logs.RequestHeaders : null,
                    ResponseHeaders = logTypes.Contains(LogLevel.ResponseHeaders) ? logs.ResponseHeaders : null,
                };
            }

        }
    }

    public class TraceModel
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }

    }
}
