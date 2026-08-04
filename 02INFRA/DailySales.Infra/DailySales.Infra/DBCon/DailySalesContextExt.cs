using Common.App.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Infra.DBCon
{
    public partial class DailySalesContext : DbContext, IAppDBContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _config;
        public DailySalesContext(DbContextOptions<DailySalesContext> options, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        : base(options)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionInfo = CreateConnection();
                string connectionString = _config.GetConnectionString(connectionInfo);
                // dynamic connection string build logic to connect to different databases
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        // dynamic connection string build logic to connect to different databases
        protected string CreateConnection()
        {
            string client = Convert.ToString(_httpContextAccessor.HttpContext.Items["ClientName"]);
            return (string.IsNullOrEmpty(client) ? "sandbox" : client) + "Connection";

        }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
      
    }
}
