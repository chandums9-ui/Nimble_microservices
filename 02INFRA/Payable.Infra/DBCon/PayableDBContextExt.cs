
using Common.App.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataModel.Domain.DataModel;
using static Amazon.S3.Util.S3EventNotification;

namespace Payable.Infra.DBCon
{
    public partial class PayableDBContext : DbContext,IAppDBContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IURLConnection uRLConnection;
        private IConfiguration _config;
        public PayableDBContext(DbContextOptions<PayableDBContext> options, IConfiguration config, IHttpContextAccessor httpContextAccessor,IURLConnection _uRLConnection)
        : base(options)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            uRLConnection = _uRLConnection;
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
            string client = !string.IsNullOrEmpty(uRLConnection.UrlName)? uRLConnection.UrlName: Convert.ToString(_httpContextAccessor.HttpContext.Items["ClientName"]);
            return (string.IsNullOrEmpty(client) ? "sandbox" : client) + "Connection";

        }
        public Task<int> SaveChangesAsync()
        {
           return base.SaveChangesAsync();
        }
       
    }
}
