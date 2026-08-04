using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class UserParentDetailsResponse:StatusDTO
    {
        public List<ParentUserIDs> UserDetails {  get; set; }
    }
    public class ParentUserIDs
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int Level { get; set; }
        public string RoleID {  get; set; } 
        public string ParentRoleID {  get; set; }   
    }
    public class RoleIDsAndNameResponse : StatusDTO
    {
        public List<RoleIDsAndNames> RoleNamesDetails {  get; set; }    

    }
    public class RoleIDsAndNames
    {
        public string ID {  get; set; }      //UserID
        public string RoleID { get; set; }  
        public string Name { get; set; }

    }

    public class UserandReportingNames
    {
        public string UserID { get; set; }
        public string UserName { get; set; } 
        public string ReportToID {  get; set; } 
        public string ReportToUserName {  get; set; }   

    }
    public class UserandReportToResponse:StatusDTO
    {
        public string UserID { get; set;}
        public string UserName { get; set;}
        public List<RoleIDsAndNames> RoleNamesDetails { get; set; }=new List<RoleIDsAndNames>();
    }
    public class GSSCorporationList
    {
        public string CorporationID { get; set; }
        public string ProfitCenterID { get; set; }

        public string CorporationName { get; set; }

        public string LegalName { get; set; }

        public string PropertyType { get; set; }

        public string Service { get; set; }

        public string Brand { get; set; }

        public string PMS { get; set; }
    }

}
