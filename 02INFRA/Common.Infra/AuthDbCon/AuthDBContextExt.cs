using Common.App.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.AuthDBCon
{

    public partial class AuthDBContext : DbContext, IAppDBContext
    {

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        public bool IsAlreadyAttchedForUpdate<T>(T obj) where T : class
        {
            return this.Entry(obj).State == EntityState.Modified;
        }
    }
   
}
