using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base
{

    public class ModelBaseIDString
    {
        public string ID { get; set; }
    }
    
    public class ModelBaseIDInt64
    {
        public long ID { get; set; }
    }
    public class ModelBaseIDByte
    {
        public byte ID { get; set; }
    }
    public class ModelBaseCorporationID
    {
        public string CorpID { get; set; }
        //public string? CorpName { get; set; }
    }
    public class ModelBaseCorporation : ModelBaseCorporationID
    {
        public string? CorpName { get; set; }
        public long CorpSortOrder { get; set; }
    }
    public class ModelBaseClientID
    {
        public string ClientID { get; set; }
    }
    public class ModelBaseRoleID
    {
        public string RoleID { get; set; }
    }
    public class ModelBaseUserID: ModelBaseRoleID
    {
        public string UserID { get; set; }
    }
    public class ModelBaseHeaderDTO
    {
        public string CorpName { get; set; }
        public DateTime? GeneratedTime { get; set; }
        public int TotalCount { get; set; }
    }

    public class ModelBaseIDNameDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public int BrandName { get; set; }
        //public int SortOrder {  get; set; }
        public int? PaymentMethodType { get; set; }

        public string? PurposeAccountID { get; set; }
        public string? PurposeAccountName { get; set; }
        public int CreditDays { get;set; } 
        public short Status { get; set; }       
    }

    public class ModelBaseIDNameSortOrderDTO : ModelBaseIDNameDTO
    {
        public long SortOrder { get; set; }
        public string CorporationID { get; set; }
    }
    public class ModelBaseIDIntervalDTO
    {
        public string ID { get; set; }
        public string Interval { get; set; }

    }
   
    public class ModelBaseStatusString
    {
        public string Status { get; set; }
    }
    public class ModelBaseStatusBoolean
    {
        public bool? Status { get; set; }
    }
    public class ModelBaseStatusShort
    {
        public short? Status { get; set; }
    }

}
