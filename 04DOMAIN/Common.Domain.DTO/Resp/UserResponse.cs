using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Req;

namespace Common.Domain.DTO.Resp
{
    public class UserPreferencesResponse : UserPreferenceDTO
    {

    }
 

    public class UserMigrationsResp : StatusDTO
    {
       public List<UserInfoRequest> Users { get; set; }
    }
    public class ClientMigrationsResp : StatusDTO
    {
        public List<ClientRegisterRequest> Clients { get; set; }
    }

    public class MigrationUsersRes : StatusDTO
    {
        public List<MigrationUserInfo> MigrationUsers { get; set; }
    }
    public class MigrationRes:StatusDTO
    {
        public string RequestID { get; set; }

    }
}
