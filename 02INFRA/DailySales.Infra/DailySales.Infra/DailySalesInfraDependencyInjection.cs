using Common.App.Contracts;
using Common.Infra.Logger;
using CoreAccounting.Infra.Services;
using DailySales.App.Contracts;
using DailySales.App.Services;
using DailySales.Infra.DataRepos;
using DailySales.Infra.DBCon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Infra
{
    public static class DailySalesInfraDependencyInjection
    {

        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration config)
        {
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IAWSFileService, AWSFilesService>();
             services.AddDbContext<DailySalesContext>();
            //services.AddScoped<IDailySaleService, DailySaleService>();

           // services.AddDbContext<DailySalesContext>(options => options.UseSqlServer(config.GetConnectionString("dasConnection")));
            return services;
        }
    }

}
