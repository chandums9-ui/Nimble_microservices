using System.Net.Http;

namespace Payable.Domain.DTO.Model.RepayModel.Common
{
    public class APIRequestModel<T>
    {
        public APIRequestModel()
        {
            Params = Params;
            RequestType = RequestTypeConstants.Json;
            Method = "POST";
        }

        public string Url { get; set; }
        public string Method { get; set; }
        public T Params { get; set; }
        public string RequestType { get; set; }
        public bool IsMultipartFormData { get; set; }
        public string Token { get; set; }
        public MultipartFormDataContent MultipartContent { get; set; }

        public string GetMessageToLog()
        {
            return $"Url: {Url}, Method: {Method}, RequestType: {RequestType}, IsMultipartFormData: {IsMultipartFormData}";
        }
    }
}
