
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.App;
//using static DataModel.Domain.DataModel.Corporation;

namespace Common.Domain.DTO.Resp
{
    public class CorporationsResponse
    {
        public List<CorporationDTO> Corporations { get; set; }
    }
    public class IOCorporationsResponse:StatusDTO
    {
        public List<CorporationData> CorpList { get; set; }= new List<CorporationData>();
    }
    public class IOVendorsResponse:StatusDTO
    {
        public List<VendorData> VendorList { get; set; }=new List<VendorData>();
    }
    public class IOContractResponse:StatusDTO
    {
        public List<ContractsData> ContractList { get; set; } = new List<ContractsData>();
    }
    public class ContractsData
    {
        public string ID { get; set; }
        public string AccountNumber { get; set; }
        public string Adress { get; set; }
        public bool IsDefault { get; set; }
        public short Status { get; set; }
    }
    public class VendorData
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DefaultAddress { get; set; }
        public short Status { get; set; }
    }
    public class CorporationData
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string LegalName { get; set; }
        public string BookKeepingDiretory { get; set; }
        public short Status { get; set; }
        public string ClientID { get; set; }

        public string ClientUserName { get; set; }
        public List<PcData> PCList { get; set; }=new List<PcData>();
    }
    public class PcData
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string BookKeepingDiretory { get; set; }
        public short Status { get; set; }
    }


    public class JournalLookupRespose 
    {
        public List<AccountDTO> Accounts { get; set; }
        public List<KeyValuePairObject<string, string>> ProfitCenters { get; set; }
        public List<ModelBaseIDNameSortOrderDTO> NamesInfo { get; set; }
        public List<ModelBaseIDNameDTO> Pourposes { get; set; }
        public List<GenericLongListDTO> TaxLines { get; set; }
        
    }
    public class RecurringLookupRespose
    {
        public List<KeyValuePairObject<string, string>> Frequencies { get; set; }
        public List<KeyValuePairObject<string, string>> Remainders { get; set; }
    }
}
