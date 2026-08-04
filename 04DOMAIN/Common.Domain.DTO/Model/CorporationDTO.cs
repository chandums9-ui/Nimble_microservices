using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class CorporationDTO
    {
        public string ID { get; set; }
        public string ProfitCenterID { get; set; }
        public string Name { get; set; }
        public string LegalName { get; set; }
        public long SortOrder { get; set; }
        public int PropertyType {  get; set; }  
        public long STRID { get; set; }
    }

    public class CorpNames
    {
        public string CorporationID { get; set; }
        public string CorpDBAName { get; set; }
        public string CorpLegalName { get; set; }
    }
    public class IncomestmntLayoutNamesDbresponse
    {
        public Int16 LayoutType { get; set; }
        public string LayoutName { get; set; }
        public Int16 LayoutStatus { get; set; }
        public string CorporationID { get; set; }
        public string CreatedBy {  get; set; }  

    }
    public class IncomestmtLayoutResponse : StatusDTO
    {
        public List<IncomestmntLayoutNamesDbresponse> layoutsList { get; set; } = new List<IncomestmntLayoutNamesDbresponse>();
    }
    public class CheckIncomeConfig
    {
        public string CorporationID {  set; get; }  
        public int ConfigurationCount {  get; set; }    
        public Int16 IsConfigurable {  get; set; }  
    }
    public class CorportionDropdownResp : StatusDTO
    {
        public List<GetcorporationsbyTypeDbResp> CorporationList { get; set; }
    }
    public class GetcorporationsbyTypeDbResp
    {
        public string CorporationId { get; set; }
        public string CorporationName { get; set; }
    }

    public class PCsDropdownResp : StatusDTO
    {
        public List<GetPCsbyTypeDbResp> PCList { get; set; }
    }
    public class GetPCsbyTypeDbResp
    {
        public string PCID { get; set; }
        public string PCName { get; set; }
    }

    public class LayoutCorporationsDbResponse
    {
        public string CorporationID {  get; set; }  
        public string CorporationName { get; set; }
        public Int64 ReportConfigId {  get; set; }  
    }
    public class LayoutCorporationsList : StatusDTO
    {
        public List<LayoutCorporationsDbResponse> LayoutCorporations { get; set; } =new List<LayoutCorporationsDbResponse>();   
    }
}
