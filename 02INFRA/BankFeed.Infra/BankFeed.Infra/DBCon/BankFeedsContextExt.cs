using Common.App.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Infra.DBCon
{
    public partial class BankFeedsContext : DbContext,IAppDBContext
    {
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        
    }
}
