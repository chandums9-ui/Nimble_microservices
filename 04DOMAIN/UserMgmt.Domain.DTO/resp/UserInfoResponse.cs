using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgmt.Domain.DTO.Resp
{
    public class UserInfoResponse : StatusDTO
    {

    }
    public class UserInfoPassResponse : StatusDTO
    {
        public string UserID { get; set; }
        public string Password { get; set; }
    }
    public class UserPwdResp : StatusDTO
    {
       public string Password { get; set; }
    }
    public class ReportUsersResponse: StatusDTO
    {    
        public List<ReportUsersDetails> users { get; set; }
    }
    public class RolesResponse:StatusDTO
    {
        public List<RolesDetails> roles { get; set; }
    }
    public class RolesDetails
    {
        public long ID { get; set; }
        public string Name { get; set; }  
        public long? ParentID { get; set; }
    }

    public class ReportUsersDetails
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
    }
    public class ReportUsers
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public long UserLevel { get; set; }
        public long RoleID { get; set; }
        public long ParentID { get; set; }
    }

    public class UserInfoResp : StatusDTO
    {
        public long UserInfoID { get; set; }

        public long ClientInfoID { get; set; }

        public long UrlInfoID { get; set; }
    }
    public class ClientInfoResponse : StatusDTO
    {
        public List<CLientData> ClientData { get; set; }=new List<CLientData>();
    }
    public class CLientData
    {
        public long ClientInfoID { get; set; }
        public string ClientID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public short? Status { get; set; }
    }
    public class UserDetails
    {
        public string UserID {  get; set; } 
        public string UserEmail { get; set; }       
        public string FirstName { get; set; }   
        public string LastName { get; set; } 
        public string MiddleName { get; set; }  
    }
    public class URLInfoResponse : StatusDTO
    {
        public List<URLData> URLs { get; set; }=new List<URLData>();
    }
    public class URLData
    {
        public long URLID { get; set; }
        public string URL { get; set; }
        public string URLShortName { get; set; }
        public short? Status { get; set; }
    }
    public class ClientIdRes:StatusDTO
    {
        public List<ClientIdList> ClientIds { get; set; } =new List<ClientIdList>();
    }
    public class ClientIdList
    {
        public string ClientId { get; set; }
    }
}
