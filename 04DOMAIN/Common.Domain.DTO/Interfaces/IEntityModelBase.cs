using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Interfaces
{
    public interface EntityModelAuditBase
    {
        byte[] CreatedBy { get; set; }

        DateTime CreatedDate { get; set; }

        byte[] ModifiedBy { get; set; }

        DateTime ModifiedDate { get; set; }

    }

    public interface EntityModelId
    {
        byte[] Id { get; set; }
    }

    public interface EntityModelClientId
    {
        byte[] ClientId { get; set; }
    }

    public interface EntityModelCorporationId
    {
        byte[] CorporationId { get; set; }
    }
}
