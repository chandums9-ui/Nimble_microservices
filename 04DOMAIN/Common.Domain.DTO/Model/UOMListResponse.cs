using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class UOMListResponse : StatusDTO
    { 
        public List<UOMDTO> UOMDTO { get; set; }
    }
    public class UOMDTO
    {
        public string ID { get; set; }
        public string ClientID { get; set; }
        public string UOMType { get; set; }
        public string UOMName { get; set;}
        public string ShortName { get; set;}
        public int Status { get; set;}
    }

    public class ShipViaListResponse : StatusDTO
    {
        public List<ShipViaDto> ShipViaDto { get; set; }
    }
    public class ShipViaDto
    {
        public string ID { get; set; }
        public string ShipViaName { get; set;}
        public string Description { get; set;}
        public short Status { get; set;}    
    }

}
