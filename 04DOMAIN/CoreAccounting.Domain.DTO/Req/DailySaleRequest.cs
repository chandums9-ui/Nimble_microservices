using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: Should Move this file to DailySale module.
namespace CoreAccounting.Domain.DTO.Req
{
    public class DailySaleRequest:ModelBaseCorporationID
    {
        public string? ProfitCenterID { get; set; }
    }
}
