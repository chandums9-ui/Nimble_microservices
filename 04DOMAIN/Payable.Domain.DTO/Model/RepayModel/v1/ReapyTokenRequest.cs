using Payable.Domain.DTO.Model.RepayModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model.RepayModel.v1
{
    public class RepayTokenRequest:RepayBaseRequestModel
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }

        public string scope { get; set; }

  

        public string loginName { get; set; }
    }

    public class RepayTokenResponse:RepayBaseResponseModel
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }

        public RepayErrorResponse error { get; set; }
    }
    public class RepayApproveStatus:RepayBaseRequestModel
    {

    }
    public class RepayPaymentgroupStatus : RepayBaseRequestModel
    {

    }
    public class RepayPaidRequest : RepayBaseRequestModel
    {

    }

    public class RepayFundRequest : RepayBaseRequestModel
    {

    }
    public class WebhookReq:RepayBaseRequestModel
    {
        public string id { get; set; }
        public string resourceEvent { get; set; }
        public string description { get; set; }

        public string url { get; set; }

        public string dateCreated { get; set; }
        public string dateModified { get; set; }
        public string account { get; set; }
    }
}
