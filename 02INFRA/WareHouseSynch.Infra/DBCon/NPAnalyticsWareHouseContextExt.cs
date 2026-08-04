using Common.App.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseSynch.Infra.DBCon
{
    public partial  class NPAnalyticsWareHouseContext : DbContext, IAppDBContext
    {
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
