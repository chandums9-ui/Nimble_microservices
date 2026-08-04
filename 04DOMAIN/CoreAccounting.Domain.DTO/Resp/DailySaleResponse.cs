using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using CoreAccounting.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: Should Move this file to DailySale module.
namespace CoreAccounting.Domain.DTO.Resp
{

    public class DailySaleLinesResponse : ModelBaseCorporationID, IStatusDTO
    {
        public int TotalCount { get; set; }
        public List<DailySaleLinesDTO>? Lines { get; set; }
        public string? ProfitCenterID { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }


}
