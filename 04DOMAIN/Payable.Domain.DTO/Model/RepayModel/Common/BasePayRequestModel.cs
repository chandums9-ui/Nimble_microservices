

namespace Payable.Domain.DTO.Model.RepayModel.Common
{
    public class BasePayRequestModel<T> : RepayBaseRequestModel
    {
        
        public string OrderToken { get; set; }


        public T PaymentMethod { get; set; }
    }
}
