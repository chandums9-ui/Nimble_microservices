using Common.App.Contracts;
using Common.App.Services;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Resp;
using Common.Infra.AWSS3FileService;
using Common.Infra.Cache;
using Common.Infra.GenericRepos;
using Common.Infra.Logger;
using Common.Infra.Publishing;
using MassTransit;
using Messages.Domain;
using Messages.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra
{
    public static class CommonInfraDependencyInjection
    {
        public static IServiceCollection AddCommonInfraServices(this IServiceCollection services, IConfiguration config, bool enableMassTransit = false)
        {
            services.Configure<AWSSettingsDTO>(config.GetSection("AWSSettings"));
            //LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            //services.AddSingleton<ILoggerService, LoggerService>();
            services.AddScoped<IRepositoryBase, RepositoryBase>();
            services.AddScoped<IAWSFileService, AWSServices>();
            services.Configure<MemoryCacheSettings>(config.GetSection("MemoryCacheSettings"));
            services.Configure<ExpireTokenTime>(config.GetSection("ExpireTokenTime"));
            services.Configure<LoggerSettings>(config.GetSection("LoggerSettings"));
            services.Configure<RedisCacheSettings>(config.GetSection("RedisCacheConfiguration"));
            services.Configure<RedisCacheApprovalDTO>(config.GetSection("RedisCacheApproval"));
            services.Configure<ElastiCacheApprovalDTO>(config.GetSection("ElastiCacheApproval"));
            services.Configure<ElastiCacheSettings>(config.GetSection("ElastiCacheSettings"));
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddSingleton<IElastiCacheService, ElastiCacheService>();
            services.AddScoped<IURLConnection, URLConnection>();
            if (enableMassTransit)
            {
                services.Configure<RabbitMqSettings>(config.GetSection("RabbitMQSettings"));
                services.AddMassTransit(x =>
                {
                    //x.AddConsumer<SynchMessageConsumer>();
                    //x.AddConsumer<SynchMessage_ErrorConsumer>();

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        var settings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
                        
                        // Custom synch main exchange name
                        cfg.Message<SynchMessage>(x =>
                        {
                            x.SetEntityName(settings.Queues.MainExchange);
                        });

                        cfg.Host(new Uri($@"{settings.Host}"), h =>
                        {
                            h.Username(settings.Username);
                            h.Password(settings.Password);
                            h.UseSsl(s =>
                            {
                                s.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                            });
                        });
                    });
                });
                services.AddScoped<IPublishService, PublishService>();
            }

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var elastiCacheSettings = sp.GetRequiredService<IOptions<ElastiCacheSettings>>().Value;

                string primaryEndPoint = elastiCacheSettings.PrimaryEndPoint;

                var configOptions = new ConfigurationOptions
                {
                    EndPoints = { elastiCacheSettings.PrimaryEndPoint },
                    User = elastiCacheSettings.User,
                    Password = elastiCacheSettings.Password,
                    Ssl = elastiCacheSettings.Ssl,
                    AbortOnConnectFail = elastiCacheSettings.AbortOnConnectFail,
                    ConnectTimeout = elastiCacheSettings.ConnectTimeout,
                    SyncTimeout = elastiCacheSettings.SyncTimeout
                };

                return ConnectionMultiplexer.Connect(configOptions);
            });
            return services;
        }
    }
}
