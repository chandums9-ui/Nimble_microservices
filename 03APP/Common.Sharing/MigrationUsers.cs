using BankFeed.Domain.DataModel;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.App.Contracts;
using UserMgmt.Domain.DataModel;
using UserMgmt.Infra.DataRepos;
using UserMgmt.Infra.DBCon;

namespace Common.Sharing
{
    public class MigrationUsers
    {
        #region Fields
        private readonly string connectionString;

        #endregion

        #region Ctor
        public MigrationUsers(string dataShareingConstr)
        {
            connectionString=dataShareingConstr;
        }
        #endregion

        #region Context

        public IUnitOfWork CreateContext()
        {
            UMDBContext userMgmtContext;
            var services = new ServiceCollection();
            services.AddDbContext<UMDBContext>(options => options.UseSqlServer(connectionString));
            var serviceProvider = services.BuildServiceProvider();
            userMgmtContext = serviceProvider.GetService<UMDBContext>();
            IUnitOfWork unitOfWork = new UMUnitOfWork(userMgmtContext);
            return unitOfWork;
        }

        #endregion

        #region Sharing Migration Users Info Mapping
        public async Task<string> MigrationUsersMapping(List<MigrationUserInfo> MUserslist, long urlID)
        {
            List<DumpUserInformation> migrationUsersList = new List<DumpUserInformation>();
            //string posReqID = Guid.NewGuid().ToString();
            try
            {
                var dbContext = CreateContext();
                foreach (var user in MUserslist)
                {
                    DumpUserInformation migrationUsersInfo = new DumpUserInformation();
                    // migrationUsersInfo. = requestID;
                    migrationUsersInfo.UserName  = user.UserName;
                    migrationUsersInfo.FirstName = user.FirstName;
                    migrationUsersInfo.LastName = user.LastName;
                    migrationUsersInfo.MiddleName = user.MiddleName;
                    migrationUsersInfo.UserId = new PFAID(user.UserID).UID;
                   migrationUsersInfo.ClientId = new PFAID(user.ClientID).UID;
                    migrationUsersInfo.UserLevel =Convert.ToInt32(user.UserLevel);
                    migrationUsersInfo.UrlInfoId = urlID;
                    migrationUsersInfo.Level = user.Level;
                    migrationUsersInfo.Password = user.Password;
                    migrationUsersInfo.ReportTo = !string.IsNullOrEmpty(user.ReportTo)?new PFAID(user.ReportTo).UID : new PFAID(user.UserID).UID;
                    migrationUsersInfo.RoleId = new PFAID(user.RoleID).UID;
                    migrationUsersList.Add(migrationUsersInfo);
                }
                await dbContext.DumpUserInfo.BulkInsert(migrationUsersList);
                int c = await dbContext.SaveAsync();

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                migrationUsersList = null;
            }
        }
        
        #endregion
    }
}
