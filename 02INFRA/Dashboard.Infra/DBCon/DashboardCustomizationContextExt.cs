using Common.App.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Infra.DBCon
{
    public partial class DashboardCustomizationContext : DbContext,IAppDBContext
    {
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
       
    }
}
