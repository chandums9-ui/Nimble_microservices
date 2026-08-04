using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
    public class Setup1099Res
    {
        public class UserPreferncesRes : StatusDTO
        {
            public bool IsLegalName { get; set; }
        }

        public class ExcludeSettingsListRes : StatusDTO
        {
            public List<ExcludeSetting> ExcludeSettingsList { get; set; } = new List<ExcludeSetting>();
        }

        public class ExcludeSetting
        {
            public Int32 ID { get; set; }

            public string Name { get; set; }
            public string ClientID { get; set; }

            public short Status { get; set; }
        }
        public class ExcludeSettingResponse : ModelBaseIDInt64, IStatusDTO
        {

            public int StatusCode { get; set; }
            public string Status { get; set; }

        }

        public class VendorDetailsOf1099Response : StatusDTO
        {
            public List<VendorDetailsByCorp> VendorDetails { get; set; } = new List<VendorDetailsByCorp>();
        }

        public class VendorDetailsByCorp
        {
            public string VendorID { get; set; }

            public string VendorName { get; set; }
            public string Company { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }

            public string Mobile { get; set; }

            public string Address { get; set; }


            public string FederalOrSSN { get; set; }

            public decimal TotalAmount { get; set; }

            public short Status { get; set; }
            public bool IsSelected { get; set; }

            public string CorpFederalID { get; set; }
        }


        public class BoxLinesRes : StatusDTO
        {
            public List<BoxLines> BoxlinesList { get; set; } = new List<BoxLines>();

        }
        public class BoxLines
        {
            public string Name { get; set; }
            public Int32 ID { get; set; }
            public short Type { get; set; }

            public short Status { get; set; }
            public Int32 OrderID { get; set; }

        }
        public class LoadChangeMappingsRes : StatusDTO
        {
            public List<COAAccounts> COAList { get; set; } = new List<COAAccounts>();
            public List<ParentVendorDetails> ParentVendorDet { get; set; } = new List<ParentVendorDetails>();
        }
        public class COAAccounts
        {
            public string VendorID { get; set; }
            public string AccountID { get; set; }
            public string AccountName { get; set; }
            public long BoxLineID { get; set; }
            public string AccountTypeName { get; set; }
            public int AccountTypeOrder { get; set; }
            public string BoxName { get; set; }
            public int BoxOrderID { get; set; }

            public decimal Amount { get; set; }
            public string JID { get; set; }
            public string ParentID { get; set; }


        }
        public class ParentVendorDetails
        {
            public string TranID { get; set; }

            public string ParentID { get; set; }

            public string VendorName { get; set; }
            public string VendorID { get; set; }
            public string AccountID { get; set; }

            // public decimal Amount { get; set; }
            // public decimal TotalAmount { get; set; }

        }
        public class ChangeMappings
        {
            public string AccountNmae { get; set; }
            public string AccountID { get; set; }
            public string AccountType { get; set; }

            public string BoxLineName { get; set; }
            public string BoxLineId { get; set; }
        }

        public class ThresholdRes : StatusDTO
        {
            public List<Thresholds1099> ThresholdList { get; set; } = new List<Thresholds1099>();
        }
        public class Thresholds1099
        {
            public string ID { get; set; }
            public int BoxLineID { get; set; }
            public string BoxLineName { get; set; }

            public decimal Amount { get; set; }
            public DateTime CreatedOn { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public string ModifiedBy { get; set; }

            public string ClientID { get; set; }
            public short Type { get; set; }

            public short Status { get; set; }
            public int BoxLineOrderID { get; set; }
        }

        public class ChangeMappingResponse : ExcludeSettingResponse
        {

            public string ReqID { get; set; }
        }
        public class UpdateThresholdResponse : ExcludeSettingResponse
        {


        }

        public class ChangeMappingTempRes : StatusDTO
        {
            public string RequestID { get; set; }
        }

        public class ExcelExportRes : StatusDTO
        {

            public byte[] ExcelFileBytes { get; set; }
            public string FileName { get; set; }
        }
    }
}

