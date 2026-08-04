

namespace Payable.Domain.DTO.Model.RepayModel.Common
{
    public class APIResponseModel<T> : RepayBaseResponseModel
    {
        
        public string PaymentMethod { get; set; }

      
        public string Channel { get; set; }

     
        public string Action { get; set; }

    
        public T Data { get; set; }

    }
}
