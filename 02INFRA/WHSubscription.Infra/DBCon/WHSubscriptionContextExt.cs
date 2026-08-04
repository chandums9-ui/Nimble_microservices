using Common.App.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSubscription.Infra.DBCon
{
    public partial class WHSubscriptionContext : DbContext,IAppDBContext
    {
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
