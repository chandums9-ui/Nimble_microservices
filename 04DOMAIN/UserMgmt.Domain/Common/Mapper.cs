using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Req;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.Domain.DataModel;
using UserMgmt.Domain.DTO.Req;

namespace UserMgmt.Domain.Common
{
    /// <summary>
    /// Mapper class for UserManagement module
    /// </summary>
    public static class Mapper
    {
        public static UserPrevileges MapUserPrivilege(UserPrivilegesRequest Entity, long UserLinkId, long? GivenBy, UserPrevileges? OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.MenuId = Entity.MenuID;
                OrigEntity.Update = Entity.Update;
                OrigEntity.Create = Entity.Create;
                OrigEntity.Delete = Entity.Delete;
                if (Entity.Create == true || Entity.Update == true || Entity.Delete == true)
                { OrigEntity.View = true; }
                else { OrigEntity.View = Entity.View; }
                OrigEntity.UserLinkId = UserLinkId;
                OrigEntity.GivenBy = GivenBy;
                return OrigEntity;
            }
            else
            {
                OrigEntity = new UserPrevileges();
                OrigEntity.MenuId = Entity.MenuID;
                OrigEntity.Create = Entity.Create;
                OrigEntity.Update = Entity.Update;
                OrigEntity.Delete = Entity.Delete;
                if (Entity.Create == true || Entity.Update == true || Entity.Delete == true)
                { OrigEntity.View = true; }
                else { OrigEntity.View = Entity.View; }
                OrigEntity.UserLinkId = UserLinkId;
                OrigEntity.GivenBy = GivenBy;
                return OrigEntity;
            }
        }

        public static RolePrevileges MapRolePrivilege(RolePrivilegesRequest Entity, long RoleID, long? GivenBy, RolePrevileges? OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.MenuId = Entity.MenuID;
                OrigEntity.Update = Entity.Update;
                OrigEntity.Create = Entity.Create;
                OrigEntity.Delete = Entity.Delete;
                OrigEntity.RoleId = RoleID;
                if (Entity.Create == true || Entity.Update == true || Entity.Delete == true)
                { OrigEntity.View = true; }
                else { OrigEntity.View = Entity.View; }
                OrigEntity.GivenBy = GivenBy;
                return OrigEntity;
            }
            else
            {
                OrigEntity = new RolePrevileges();
                OrigEntity.MenuId = Entity.MenuID;
                OrigEntity.Create = Entity.Create;
                OrigEntity.Update = Entity.Update;
                OrigEntity.Delete = Entity.Delete;
                OrigEntity.RoleId = RoleID;
                if (Entity.Create == true || Entity.Update == true || Entity.Delete == true)
                { OrigEntity.View = true; }
                else { OrigEntity.View = Entity.View; }
                OrigEntity.GivenBy = GivenBy;
                return OrigEntity;
            }
        }

        public static Role MapRole(RoleSaveUpdateRequest Entity, Role? OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.Name = Entity.Name; 
                if(Entity.ParentID!=0)
                {
                    OrigEntity.ParentId = Entity.ParentID;
                }             
                OrigEntity.Status = Entity.Status;           
            }
            else
            {
                OrigEntity=new Role();
                OrigEntity.Name = Entity.Name;
                if (Entity.ParentID != 0)
                {
                    OrigEntity.ParentId = Entity.ParentID;
                }
                //OrigEntity.ParentId = Entity.ParentID != 0 ? Entity.ParentID : null;
                OrigEntity.Status = (short)Status.Active;            
            }
            return OrigEntity;
        }

        public static UserInfo MapUserInfo(UserInfoRequest userInfoReq, UserInfo userInfo)
        {
            if (userInfo != null)
            {
                userInfo.FirstName = userInfoReq.FirstName;
                userInfo.LastName = userInfoReq.LastName;
                userInfo.MiddleName = userInfoReq.MiddleName;
                //userInfo.Password = userInfoReq.Password;    
            }
            else
            {
                userInfo = new UserInfo();
                userInfo.UserId = new PFAID( userInfoReq.UserID).UID;
                userInfo.FirstName = userInfoReq.FirstName;
                userInfo.LastName = userInfoReq.LastName;
                userInfo.MiddleName = userInfoReq.MiddleName;
                userInfo.UserName = userInfoReq.UserName;
                userInfo.Password = userInfoReq.Password;
                userInfo.Status=(short)Status.Active;
            }
            return userInfo;
        }

        
        public static ClientInfo MapClientInfo(byte[] ID,string clientID,string clientSecret,ClientInfo clientInfo,long userInfoID,long?UrlInfoID)
        {
            if (clientInfo == null)
            {
                clientInfo = new ClientInfo();
                clientInfo.UserInfoId = userInfoID;
                clientInfo.UserId = ID;
                clientInfo.ClientId = clientID;
                clientInfo.ClientSecret = clientSecret;

                clientInfo.Type = 0;
                clientInfo.UrlinfoId = UrlInfoID;
                clientInfo.Status = (short)Status.Active;
            }
            return clientInfo;
        }
        public static Urlinfo MapUrlInfo(UrlInfoRequest data, Urlinfo urlInfo)
        {
            if (urlInfo != null)
            {
                urlInfo.Status = (short)Status.Active;
            }
            else 
            {
                urlInfo = new Urlinfo();
                urlInfo.Url = data.url;
                urlInfo.Name = data.Name;
                urlInfo.Status = (short)Status.Active;
            }
            return urlInfo;
        }
        public static UserClientLink MapUserClientLink(long? userInfoID,long? clientInfoID, string userLevel,byte[] userID, long? reportTo=null, UserClientLink? OrigEntity = null, long? roleID = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.ReportTo = reportTo;
                OrigEntity.RoleId = roleID;
            }
            else
            {
                OrigEntity = new UserClientLink();
                OrigEntity.ClientInfoId = clientInfoID;
                OrigEntity.UserInfoId = userInfoID;
                OrigEntity.UserId = userID;
                OrigEntity.UserLevel = Convert.ToInt64(userLevel ?? "0");
                OrigEntity.ReportTo = reportTo ??0;
                OrigEntity.RoleId = roleID;
            }
            return OrigEntity;
        }   
    }
}
