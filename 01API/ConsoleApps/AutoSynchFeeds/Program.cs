using AutoSynchFeeds;
using BankFeed.App.Contracts;
using BankFeed.Infra.DataRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankFeed.Domain.DataModel;
using BankFeed.Domain.DTO.Model;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using BankFeed.Domain.Enums;
using Common.Domain.DTO.Enums;
using Microsoft.Identity.Client;
using System.Net;
using BankFeed.Infra.DBCon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Common.App.Contracts;
using Microsoft.Extensions.Configuration;
using BankFeed.App.Services;
using BankFeed.Infra.ProviderServices;
using Microsoft.Extensions.Options;
using BankFeed.Infra.Services;
using CoreAccounting.Infra.Services;
using BankFeed.App;
using CoreAccounting.App.Service;
using CoreAccounting.App.Contracts;
using Microsoft.AspNetCore.Http;
using CoreAccounting.Infra.DBCon;
using CoreAccounting.Infra.DataRepos;
using Common.Infra.Logger;
using NLog;
using CoreAccounting.App.Services;
using Common.Domain.DTO.Model;

public class Program
{

    static async Task Main()
    {
        try
        {
            //AutoSynchFeeds Implementation for Provider
            BankFeedsContext bankFeedsContext;
            CoreDBContext coreDBdbContext;
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, true);
            IConfiguration configuration = builder.Build();

            var dbConnection = configuration.GetSection("ConnectionStrings:BankFeedDBConnection").Value;
            var coreDBConnection = configuration.GetSection("ConnectionStrings:fhgConnection").Value;

            var services = new ServiceCollection();
            services.AddDbContext<BankFeedsContext>(options =>
            { options.UseSqlServer(dbConnection);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
            ) ;
            services.AddDbContext<CoreDBContext>(options => options.UseSqlServer(coreDBConnection));
            //services.AddScoped<ICoreProperty, CoreProperty>();
            
            services.Configure<ProviderBaseURLInfo>(configuration.GetSection("ProviderBaseURLs"));
            services.Configure<PlaidKeyInfo>(configuration.GetSection("PlaidKeyInfo"));
            services.Configure<PlaidEndPoints>(configuration.GetSection("PlaidEndPoints"));
            services.Configure<YodleeKeyInfo>(configuration.GetSection("YodleeKeyInfo"));
            services.Configure<YodleeEndPoints>(configuration.GetSection("YodleeEndPoints"));

            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            ILoggerService logger=new LoggerService();
            
            var serviceProvider = services.BuildServiceProvider();
            bankFeedsContext = serviceProvider.GetService<BankFeedsContext>();
            coreDBdbContext = serviceProvider.GetService<CoreDBContext>();
            BankFeed.App.Contracts.IUnitOfWork unitOfWork = new UnitOfWork(bankFeedsContext);
            //CoreAccounting.App.Contracts.IUnitOfWork CoreunitOfWork = new CoreUnitOfWork(coreDBdbContext);
            IHttpContextAccessor httpContextAccessor = serviceProvider.GetService<HttpContextAccessor>();
            IWareHouseSynch wareHouseSync=serviceProvider.GetService<IWareHouseSynch>();

            IRedisCacheService redisCacheService = serviceProvider.GetService<IRedisCacheService>();
            IOptions<RedisCacheApprovalDTO> redisCacheApproval = serviceProvider.GetService<IOptions<RedisCacheApprovalDTO>>();
           // ICoreProperty coreProperty = new CoreProperty(CoreunitOfWork, httpContextAccessor, redisCacheService, redisCacheApproval, logger);
            var providerBaseURLs = serviceProvider.GetService<IOptions<ProviderBaseURLInfo>>();
            var PlaidKeyInfo = serviceProvider.GetService<IOptions<PlaidKeyInfo>>();
            var PlaidEndPoints = serviceProvider.GetService<IOptions<PlaidEndPoints>>();
            var YodleeKeyInfo = serviceProvider.GetService<IOptions<YodleeKeyInfo>>();
            var YodleeEndPoints = serviceProvider.GetService<IOptions<YodleeEndPoints>>();

            //ICoreProperty coreProperty = serviceProvider.GetService<ICoreProperty>();
            IYodleeProvider yodleeProvider=new YodleeProviderCalls(providerBaseURLs,YodleeKeyInfo, YodleeEndPoints, logger);
            IPlaidProvider plaidProvider = new PlaidProviderCalls(providerBaseURLs, PlaidKeyInfo, PlaidEndPoints, logger);
            IFeedRuleService feedRuleService = new FeedRuleService(unitOfWork, logger);// serviceProvider.GetService<FeedRuleService>();
           // IProviderInfo providerInfo = new ProviderInfo(unitOfWork,httpContextAccessor, yodleeProvider, plaidProvider, logger); //serviceProvider.GetService<ProviderInfo>();
           // IPlaidService plaidService= new PlaidService(unitOfWork,plaidProvider, providerInfo,logger);
           // IYodleeService yodleeService=new YodleeService(unitOfWork, yodleeProvider, providerInfo, logger);
         

           // AutoSynchFeed autoSynch = new AutoSynchFeed(unitOfWork, new FeedAccountService(unitOfWork, providerInfo,plaidService,yodleeService, logger), new PlaidService(unitOfWork, plaidProvider, providerInfo,logger), new YodleeService(unitOfWork, yodleeProvider, providerInfo, logger), new JournalService(CoreunitOfWork, coreProperty, httpContextAccessor, configuration, logger, wareHouseSync), new FeedTransactionService(unitOfWork, feedRuleService, configuration, logger), new BankFeedPostingService(CoreunitOfWork,coreProperty,httpContextAccessor,configuration, logger, wareHouseSync));
            var env = configuration.GetSection("AutoSynchRunEnv:Environment");
            //env = "1";

            //var res = await autoSynch.AutoSynchFeeds(env.Value);//env.Value
        }
        catch
        {

        }
    }
}
