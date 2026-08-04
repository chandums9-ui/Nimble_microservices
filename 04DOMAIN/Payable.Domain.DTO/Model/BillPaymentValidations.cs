using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillPaymentValidations :StatusDTO
    {
        public StatusDTO Status { get; set; }
        public bool isCheck {  get; set; }
        public bool isEPayment { get; set; }

        public bool isVoucher { get; set; }

        public short? PaymentMethodType { get; set; }
    }
}
