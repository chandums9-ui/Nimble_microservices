using System.Net;

namespace Payable.Domain.DTO.Model.RepayModel.Common
{
    public class RepayBaseResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public LogsModel Logs { get; set; }
    }
}
