namespace Payable.Domain.DTO.Model.RepayModel.Common
{
    public enum LogLevel
    {
        Trace,
        RawRequest,
        RawResponse,
        RequestHeaders,
        ResponseHeaders
    }
    public class RequestTypeConstants
    {
        public const string Json = "application/json";
        public const string FormData = "form-data";
    }

    public enum AppEnum
    {
        repay,
        phonepay,
        gpay
  
    }
}
