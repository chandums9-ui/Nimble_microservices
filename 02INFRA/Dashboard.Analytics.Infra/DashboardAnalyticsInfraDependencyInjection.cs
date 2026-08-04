using Common.App.Contracts;
using Common.Infra.Logger;
using Dashboard.Analytics.Infra.DataRepos;
using Dashboard.Analytics.Infra.DbCon;
using Dashboard.Analytics.Infra.ProviderServices;
using Dashboard.App.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Analytics.Infra
{
    public static class DashboardAnalyticsInfraDependencyInjection
    {
        public static IServiceCollection AddAnalyticsInfraServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddScoped<IUnitOfWorkAnalytics, AnalyticsWHUnitOfWork>();
            services.AddScoped<ISTRExternalAPIService, STRApiCalls>();
            services.AddDbContext<AnalyticsWHContext>();
            //services.AddDbContext<AnalyticsWHContext>(options => options.UseSqlServer(config.GetConnectionString("AnalyticsDBConnection")));
            //services.Configure<DashBoardSettings>(config.GetSection("DashBoardSettings"));
            return services;
        }
    }
}
