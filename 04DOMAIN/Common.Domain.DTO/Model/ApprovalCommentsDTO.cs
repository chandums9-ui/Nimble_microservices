using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class ApprovalCommentsDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public string Commentby { get; set; }  // CommentByID
        public DateTime CommentDate { get; set; }
    }

    public class EntryApprovalPolicyUserDetails
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int ApprovalOrder { get; set; }
        public int ApprovalType { get; set; }
        public bool IsCureentUser { get; set; }
    }

    public class UserApprovalDetails
    {
        public string CorporationID { get; set; }
        /// <summary>
        /// User First name & Last name
        /// </summary>
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public int ApprovalOrder { get; set; }
        public int ApprovalType { get; set; }
        public bool IsCurrentUser { get; set; }
    }
    public class UserInfoDetails
    {
        public string UserID { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string MiddleName {  get; set; } 
        public long Phone1 { get; set; }   
        public long Phone2 { get; set; }
        public long Phone3 { get; set; }
        public string Email {  get; set; }  


    }
    public class UserInformationDetail : StatusDTO
    {
        public List<UserInfoDetails> userdetails {  get; set; } =new List<UserInfoDetails>();

    }
}
