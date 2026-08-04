using Common.App.Contracts;
using Common.Infra.Logger;
using Dashboard.Infra.DataRepos;
using Dashboard.Infra.DBCon;
using Dashboard.App.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dashboard.Domain.DTO.Model;

namespace Dashboard.Infra
{
    public static class DashboardInfraDependencyInjection
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<ILoggerService, LoggerService>();
            //services.AddScoped<IUnitOfWorkAnalytics, AnalyticsUnitOfWork>();
            services.AddScoped<IUnitOfWorkDashboard, UnitOfWork>();

            services.AddDbContext<DashboardCustomizationContext>(options => options.UseSqlServer(config.GetConnectionString("DashboardDBConnection")));
            //services.AddDbContext<DashboardAnalyticsContext>(options => options.UseSqlServer(config.GetConnectionString("AnalyticsDBConnection")));
            services.Configure<DashBoardSettings>(config.GetSection("DashBoardSettings"));
        
            return services;
        }
    }
}
