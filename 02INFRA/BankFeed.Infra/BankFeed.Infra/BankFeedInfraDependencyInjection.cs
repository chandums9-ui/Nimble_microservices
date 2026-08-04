using Common.App.Contracts;
using Common.Infra.Logger;
using BankFeed.App.Contracts;
using BankFeed.Infra.DataRepos;
using BankFeed.Infra.DBCon;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Microsoft.EntityFrameworkCore;
using BankFeed.Domain.DTO.Model;
using BankFeed.App.Services;
using BankFeed.Infra.ProviderServices;
using BankFeed.Infra.Services;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BankFeed.Infra;

public static class BankFeedInfraDependencyInjection
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration config)
    {

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Configure<ProviderBaseURLInfo>(config.GetSection("ProviderBaseURLs"));
        services.Configure<YodleeKeyInfo>(config.GetSection("YodleeKeyInfo"));
        services.Configure<YodleeEndPoints>(config.GetSection("YodleeEndPoints"));

        services.Configure<PlaidEndPoints>(config.GetSection("PlaidEndPoints"));
        services.Configure<PlaidKeyInfo>(config.GetSection("PlaidKeyInfo"));

        services.Configure<MeldKeyInfo>(config.GetSection("MeldKeyInfo"));
        services.Configure<MeldEndPoints>(config.GetSection("MeldEndPoints"));

        services.AddScoped<IPlaidProvider, PlaidProviderCalls>();
        services.AddScoped<IYodleeProvider, YodleeProviderCalls>();
        services.AddScoped<IMeldProvider, MeldProviderCalls>();
        services.AddScoped<IAdminServices, AdminService>();
        services.AddDbContext<BankFeedsContext>(options => options.EnableSensitiveDataLogging(true).UseSqlServer(config.GetConnectionString("BankFeedDBConnection")));

        return services;
    }
}
