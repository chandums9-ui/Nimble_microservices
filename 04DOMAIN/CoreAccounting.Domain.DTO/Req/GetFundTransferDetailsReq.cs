using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class GetFundTransferDetailsReq

    {

        /// <summary> 

        /// Main fund transfer transaction id 

        /// (Same value used as FundMainTransactionId in SaveOrEdit) 

        /// </summary> 

        public string FundMainTransactionId { get; set; }

    }
}
