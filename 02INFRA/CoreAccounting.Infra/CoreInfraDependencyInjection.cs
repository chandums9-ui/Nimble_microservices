using BankFeed.Domain.DTO.Model;
using Common.App.Contracts;
using Common.Infra.Logger;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Model;
using CoreAccounting.Infra.DataRepos;
using CoreAccounting.Infra.DBCon;
using CoreAccounting.Infra.Services;
using CoreAccounting.Domain.DTO.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using CoreAccounting.Infra.Services.wrkSpotProviderCall;


namespace CoreAccounting.Infra;

public static class CoreInfraDependencyInjection
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration config)
    {
        //LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        // services.AddSingleton<ILoggerService, LoggerService>(); 
      
        services.AddScoped<IUnitOfWork, CoreUnitOfWork>();
        services.AddScoped<IFileService, FileService>();
      
        services.AddScoped<IwrkSpotCall,WrkSpotCall>();
        services.AddDbContext<CoreDBContext>();
        // services.AddDbContext<CoreDBContext>(options => options.UseSqlServer(config.GetConnectionString("fhgConnection")));
        services.Configure<WrkSpotProviderInfo>(config.GetSection("ProviderBaseURLs"));
        services.Configure<WrkSpotKeyInfo>(config.GetSection("WrkSpotKeyInfo"));
        services.Configure<WrkSpotEndPoints>(config.GetSection("WrkSpotEndPoints"));
        return services;
    }
}
