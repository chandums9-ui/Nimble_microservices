using Common.Domain.DTO.Model.Base;
using CoreAccounting.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class GetFundTransferDetailsResp : StatusDTO

    {

        public SaveOrEditFundTransferReq Data { get; set; }

    }
    public class AccountBalanceRes:StatusDTO
    {
        public decimal AccountBalance { get; set; }
    }

}
