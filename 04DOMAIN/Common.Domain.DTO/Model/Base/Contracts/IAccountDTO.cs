using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base.Contracts
{
    public interface IBalanceDTO
    {
        decimal Balance { get; set; }
    }

    public interface IModelBaseHeaderDTO
    {
        string CorpName { get; set; }
        DateTime? GeneratedTime { get; set; }
        int TotalCount { get; set; }
    }

    public interface IStatusDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
}
