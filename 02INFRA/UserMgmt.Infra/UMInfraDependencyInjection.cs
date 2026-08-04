using Common.App.Contracts;
using Common.Infra.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using UserMgmt.App.Contracts;
using UserMgmt.Infra.DataRepos;
using UserMgmt.Infra.DBCon;


namespace UserMgmt.Infra
{
    public static class UMInfraDependencyInjection
    {
        public static IServiceCollection AddUMInfraServices(this IServiceCollection services, IConfiguration config)
        {
            //LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            //services.AddSingleton<ILoggerService, LoggerService>();
            services.AddScoped<IUnitOfWork, UMUnitOfWork>();
           
            services.AddDbContext<UMDBContext>(options => options.UseSqlServer(config.GetConnectionString("UMDBConnection")));
            return services;
        }
    }
}
