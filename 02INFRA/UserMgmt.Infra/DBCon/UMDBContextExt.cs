
using Common.App.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UserMgmt.Infra.DBCon
{
    public partial class UMDBContext : DbContext, IAppDBContext
    {
        
        public Task<int> SaveChangesAsync()
        {
           return base.SaveChangesAsync();
        }
       


    }
}
