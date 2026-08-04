using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Interfaces
{
    public interface IDtoModelIDString
    {
        string ID { get; set; }
    }
    public interface IDtoModelIDInt64
    {
        long ID { get; set; }
    }
    public interface IDtoModelID
    {
        byte ID { get; set; }
    }

    public interface IDtoModelCorporation
    {
        string CorpId { get; set; }

        string? CorpName { get; set; }
    }
    public interface DtoModelClientId
    {
        string ClientId { get; set; }
    }
}
