using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Payable.Domain.DTO.Req
{
    public class MiscInfoLoadRequest
    {
        public MiscInfoLoadRequest()
        {
            MiscInfoSourceType = new List<int>();
        }
        public List<int> MiscInfoSourceType { get; set; }
    }

    public class MiscInfoSaveRequest : MiscInfoDTO
    {

    }
    public class CreditDaysSaveRequest : FrequencyDTO
    {

    }
    public class VendorAddressSaveRequest : VendorAdress
    {
        [DefaultValue(false)]
        public bool UpdateAllTransaction { get; set; }
        [DefaultValue(true)]
        public bool IsSave{ get; set; } = true;
    }

    public class VendorContractSaveRequest : VendorContractDTO
    {
        [DefaultValue(true)]
        public bool IsSave { get; set; } = true;
        public bool IsFromBill { get; set; } = false;

    }
    public class CloneVendorsRequest
    {
        public string FromCorporation { get; set; }
        public string ToCorporation { get; set; }
    }
    public class VendorSaveRequest : BusinessDTO
    {
        public decimal OpeningBalance { get; set; }
        public DateTime AsOfDate { get; set; }
        public List<VendorAddressSaveRequest> VendorAdress { get; set; } = new List<VendorAddressSaveRequest>();
        public List<VendorContractSaveRequest> VendorContract { get; set; } = new List<VendorContractSaveRequest>();
        [DefaultValue(true)]
        public bool IsSave { get; set; }
        public bool IsCloning { get; set; }
        public List<string> CorpIDs { get; set; }=new List<string>();
        public bool IsNew { get; set; } = true;
        public bool IsLockValidated {  get; set; }
        public bool IsRestoreDeleted { get; set; }
        public bool IsImport { get; set; }
    }
    public class ACHSaveRequest : ACHDetailsDTO
    {

    }
    public class VendorImportRequest
    {
        public string CorporationID { get; set; }
        public List<VendorImportDTO> ImportData { get; set; }
    }

    public class GetUseTaxTransactionsList
    {
        public List<UseTaxTransactionDTO> Transactions { get; set; } = new List<UseTaxTransactionDTO>();
    }
    public class VendorSummaryGridRequest
    {
        public string CorpID { get; set; }
        public int PageNumber { get; set; }
        public string UserID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public SearchFilterDetails VendorFilter { get; set; } =null;
        public SearchFilterDetails CorporationFilter { get; set; } = null;
        [DefaultValue(-1)]
        public int VendorStatusFilter { get; set; } = -1;
    }
    public class LoadVendorMasterDetailsRequest
    {
        public SearchFilterDetails CorporationFilter {  get; set; }=new SearchFilterDetails();
        public SearchFilterDetails VendorFilter { get; set; } = new SearchFilterDetails();
        public SearchFilterDetails FederalFilter { get; set; } = new SearchFilterDetails();
        public SearchFilterDetails SSNFilter { get; set; } = new SearchFilterDetails();
        public SearchFilterDetails MobileNumFilter { get; set; } = new SearchFilterDetails();
        [DefaultValue(-1)]
        public int VendorStatusFilter { get; set; } = 1;
        [DefaultValue(-1)]
        public int VendorStatusFilterSort { get; set; } = -1;
		public int PageNumber { get; set; }
    }
    public class SearchFilterDetails
    {
        [DefaultValue(null)]
        public string ID {  get; set; }
        [DefaultValue(null)]
        public string SearchString { get; set; }
        public int SearchFilterType {  get; set; }
        public int SearchFilterOption { get; set; } = -1;

        public int SortOrder { get; set; } = -1;
    }
    public class DeleteVendorRequest
    {
        public string CorpID { get;set; }
        public string VendorID { get; set;}
    }
    
}
