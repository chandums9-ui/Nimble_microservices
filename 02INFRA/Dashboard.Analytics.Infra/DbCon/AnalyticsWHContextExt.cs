using Common.App.Contracts;
using Common.Domain.DTO.Req;
using Dashboard.Domain.DTO.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Analytics.Infra.DbCon
{
    public partial class AnalyticsWHContext : DbContext, IAppDBContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _config;
        private IOptions<List<ServerAnlyticsGroup>> _serverGroup;
        public AnalyticsWHContext(DbContextOptions<AnalyticsWHContext> options, IConfiguration config, IHttpContextAccessor httpContextAccessor, IOptions<List<ServerAnlyticsGroup>> serverGroup)
        : base(options)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _serverGroup = serverGroup;
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
            string client = Convert.ToString(_httpContextAccessor.HttpContext.Items["ClientName"]).ToLower();
            string clientServer = "";
          
            if(_serverGroup!=null & _serverGroup.Value.Count()>0)
            {
                clientServer = _serverGroup.Value.Where(s=>s.clients.Contains(client)).Select(s=>s.ServerName).FirstOrDefault() ;
              
            }
            return (string.IsNullOrEmpty(clientServer) ? "" : clientServer) + "AnalyticsDBConnection";

        }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
       
    }
}
