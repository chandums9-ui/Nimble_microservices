using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using FluentValidation;
using System.ComponentModel;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Enums;


namespace Common.Domain.DTO.Req
{

    public class CorpIDRequest : ModelBaseCorporationID
    {
    }
    /// <summary>
    /// List of Corporation ids
    /// </summary>
    public class CorpIDsRequest : PageDTO
    {
        public List<string> CorpIDs { get; set; }
       
       //public SearchRuleRequest? SearchRequest { get; set; }
    }

    public class SearchRuleRequest
    {
        public string? CorpID { get; set; }
        public long? FeedAccID { get; set; }
        public string? BankOrCreditAccountID { get; set; }

        public string? NimbleAccID { get; set; }
        public string? SetUpFor { get; set; }
        public int Priority { get; set; }
        public short? RuleType { get; set; }
        public short? CreatedFrom { get; set; }
    }

    public class CorpIDAndAccTypeIDRequest : CorpIDRequest
    {
        [DefaultValue("000000000000000000000000000000000000")]
        public string AccountTypeID { get; set; }
        [DefaultValue(false)]
        public bool IsInactive { get; set; }=false;
    }
    public class SavePurposeRequest
    {
        public string CorporationID { get; set; }
        public string PurposeName { get; set; }
        public string? AccountID { get; set; }
        public string  AccountName { get; set; }
    }
    public class SavePurposeResponse:StatusDTO
    {
        public string PurposeID { get; set; }
        public string PurposeName { get; set; }
        public string? AccountID { get; set; }
        public string? AccountName { get; set; }
    }
    public class IOVendorRequest
    {
        public string CorpID { get; set; }
        /// <summary>
        /// 0- Inactive
        /// 1-Active
        /// 2-Active & Inactive
        /// </summary>
        [DefaultValue(1)]
        public short Status { get; set; } = (short)TransactionStatus.Void;
    }
    public class CorpIDAndAccBalancesRequest : CorpIDRequest
    {
        public List<string> AccountTypeIDs { get; set; } = new List<string>() { "0x000000000000000000000000000000000000" };
    }
    public class AccIDAndAccBalancesRequest 
    {
        public List<string> AccountIDs { get; set; } = new List<string>() { "0x000000000000000000000000000000000000" };
        public bool IsBalanceRequired { get; set; }=false;
    }
    public class CorpIDAndTypeRequest : CorpIDRequest
    {
        //[DefaultValue()]
        public string? Type { get; set; }
    }
    public class CorpSearchRequest : CorpIDRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class CorporationLockRequest()
    {
        public string CorpID { get; set; }
        public string BooksDate { get; set; }
        public short Type { get; set; }
    }
    public class ClientOrUserIDRequest
    {
        public string ClientID { get; set; }
        public string UserID { get; set; }
    }

    public class ListofCorpIdReq
    {
        public List<string> CorpID { get; set; } = new List<string>();
    }
    public class PurposeNamesRes:StatusDTO
    {
        public List<string> PurposeNames { get; set; } = new List<string>();
    }

    public class PurposeAccountDTO
    {

        public string Name { get; set; }
    }

    public class CorporationSelectionDetailsResp : StatusDTO
    {
        public List<CorporationSelectionDto> Corporations { get; set; } = new();
    }


    public class CorporationSelectionDto
    {
        public string CorporationName { get; set; }
        public string Legalname { get; set; }
        public string PropertyType { get; set; }
        public string ServiceTypename { get; set; }
        public string BrandName { get; set; }
        public string PMSName { get; set; }
    }

    public class ListOfDepartmentsRes : StatusDTO
    {
        public List<string> DepartmentId { get; set; } = new List<string>();
        public List<string> DepartmentNames { get; set; } = new List<string>();
    }


    public class ListOfDistinctAccountNamesReq
    {
        public List<string> CorpID { get; set; } = new List<string>();
        public string AccountTypeID { get; set; }
        public string DepartmentID { get; set; }
        public bool IsInactive { get; set; } = false;

    }
    public class ListOfDistinctAccountNamesRes : StatusDTO
    {
        public List<AccountNameModel> Accounts { get; set; } = new();
    }

    public class AccountNameModel
    {
        public string AccountName { get; set; }
        public string AccountType { get; set; }
    }

    public class ListOfDistinctAccountNamesDBRes
    {
        public string AccountName { get; set; }
        public string AccountType { get; set; }
    }
    public class PurposeAccountNameReq
    {
        public List<string> CorpId { get; set; } = new List<string>();
        public string PurposeName { get; set; }

    }

    public class PurposeAccountNameRes : StatusDTO
    {
        public string AccountName { get; set; }
    }
    
}
