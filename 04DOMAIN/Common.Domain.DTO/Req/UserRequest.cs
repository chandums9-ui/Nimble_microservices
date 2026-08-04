using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{
    public class UserPreferencesRequest : ModelBaseUserID
    {
        public short? Type { get; set; }
    }


    public class ValidateTokenRequest
    {
        public string Token { get; set; }
    }
    public class ExchangeAuth
    {
        public string AuthenticationID { get; set; }
    }

    public class UserInfoRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string UserID { get; set; }
        public string ReportTo { get; set; }
        public string ClientID { get; set; }
        public string UserLevel { get; set; }

    }
    public class ClientRegisterRequest : UserInfoRequest
    {
        public string urlName { get; set; }
        public string url { get; set; }

    }
    public class MigrationUsersInfo : UserInfoRequest
    {
        public int RoleID { get; set; }
        public int Level { get; set; }

    }
    public class MigrationUserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string UserID { get; set; }
        public string ReportTo { get; set; }
        public string ClientID { get; set; }
        public int UserLevel { get; set; }
        public string RoleID { get; set; }
        public int Level { get; set; }
    }
    public class CorpAssignedUserName
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public bool IsCurrentUser { get; set; }
        public string FirstNameLastName { get; set; }
    }

    public class UserApprovalDetailsRequest()
    {
        public List<string> CorpIDs { get; set; }
        //public string CurrentUserID { get; set; }
        /// <summary>
        /// JournalSourceTypes
        /// </summary>
        public short ScreenType { get; set; }

    }
}