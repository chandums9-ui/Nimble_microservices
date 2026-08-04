using DataModel.Domain.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportsAndConfig.APP.Contracts;
using ReportsAndConfig.Infra.DataRepos;
using ReportsAndConfig.Infra.DBCon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAndConfig.Infra
{
    public static class ReportsAndConfigInfraDependencyInjection
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUnitOfWork, ReportAndConfigUnitOfWork>();
            services.AddDbContext<ReportsAndConfigContext>();
            return services;
        }
    }
}
