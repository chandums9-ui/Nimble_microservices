using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public class CorpFacilityDTOMain
    {
        public string id { get; set; }
        public string name { get; set; }
        public string legalName { get; set; }
        public string facilityID { get; set; }
        public string AliasFacilityID { get; set; }
        public int pmsType { get; set; }
        public string pmsName { get; set; }
        public string clientName { get; set; }
        public short status { get; set; }
        public string bookkeepingMail { get; set; }
        [JsonIgnore]
        public string PCID { get; set; }
        [JsonIgnore]
        public string pcName { get; set; }
         

    }
    public class CorpFacilityDTO
    {

        public string id { get; set; }
        public string name { get; set; }
        public string legalName { get; set; }
        public string facilityID { get; set; }
        public string AliasFacilityID { get; set; }
        public int pmsType { get; set; }
        public string pmsName { get; set; }
        public string clientName { get; set; }
        public short status { get; set; }
        public string bookkeepingMail { get; set; }
        [JsonIgnore]
        public string PCID { get; set; }
        [JsonIgnore]
        public string pcName { get; set; }
        public List<PCDetailsDTO> PCDetails { get; set; }
        
    }
    public class PCDetailsDTO
    {
        public string pcId { get; set; }
        public string pcName { get; set; }
        public string facilityID { get; set; }
        public string AliasFacilityID { get; set; }
        public int pmsType { get; set; }
        public string pmsName { get; set; }
        public string clientName { get; set; }
        public short status { get; set; }
        public string bookkeepingMail { get; set; }
    }
}
