using Common.App.Contracts;
using Common.App.Services;
using Common.Infra.Publishing;
using MassTransit;
using Messages.Domain.Models;
using Messages.Domain.Synch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHSubscription.App.Contract;
using WHSubscription.Infra.Consumers;
using WHSubscription.Infra.DataRepos;
using WHSubscription.Infra.DBCon; 

namespace WHSubscription.Infra
{
    public static class WHSubscriptionInfraDependencyInjection
    {
        public static IServiceCollection AddWebHookInfraServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUnitOfWork, WHUnitOfWork>();
            services.AddDbContext<WHSubscriptionContext>(options => options.UseSqlServer(config.GetConnectionString("WebHookConnection")));
            services.Configure<RabbitMqSettings>(config.GetSection("RabbitMQSettings"));
            services.Configure<WebhookInfo>(config.GetSection("WebhookInfo"));
            services.AddMassTransit(x =>
            {
                // === Consumer Registrations ===
                // Corporation
                x.AddConsumer<CorporationEventConsumer>();
                x.AddConsumer<CorporationReattemptConsumer>();
                x.AddConsumer<CorporationEventErrorConsumer>();

                // PC
                x.AddConsumer<PCEventConsumer>();
                x.AddConsumer<PCReattemptConsumer>();
                x.AddConsumer<PCEventErrorConsumer>();

                // Vendor
                x.AddConsumer<VendorEventConsumer>();
                x.AddConsumer<VendorReattemptConsumer>();
                x.AddConsumer<VendorEventErrorConsumer>();

                // Contract
                x.AddConsumer<ContractEventConsumer>();
                x.AddConsumer<ContractReattemptConsumer>();
                x.AddConsumer<ContractEventErrorConsumer>();

                // Account
                x.AddConsumer<AccountEventConsumer>();
                x.AddConsumer<AccountReattemptConsumer>();
                x.AddConsumer<AccountEventErrorConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var settings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    cfg.Host(new Uri($@"{settings.Host}"), h =>
                    {
                        h.Username(settings.Username);
                        h.Password(settings.Password);
                        h.UseSsl(s =>
                        {
                            s.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                        });
                    });

                    // Break inheritance bindings
                    cfg.Publish<MasterMessage>(x => x.Exclude = true);

                    // ---------------- Corporation ----------------
                    #region Corporation Queues
                    cfg.Message<CorporationMessageEvent>(x =>
                    {
                        x.SetEntityName(settings.corporationSettings.corporationExchanges.Corporation_MainExchange);
                    });

                    cfg.Message<CorporationReattemptEvent>(x =>
                    {
                        x.SetEntityName(settings.corporationSettings.corporationExchanges.Corporation_ReattemptExchange);
                    });

                    cfg.Message<Fault<CorporationReattemptEvent>>(x =>
                    {
                        x.SetEntityName(settings.corporationSettings.corporationExchanges.Corporation_DeadLetterExchange);
                    });

                    cfg.ReceiveEndpoint(settings.corporationSettings.corporationQueues.Corporation_MainQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.corporationSettings.corporationExchanges.Corporation_MainExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<CorporationEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.corporationSettings.corporationQueues.Corporation_ReattemptQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.corporationSettings.corporationExchanges.Corporation_ReattemptExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.UseMessageRetry(r => r.Immediate(2));
                        e.BindDeadLetterQueue(settings.corporationSettings.corporationQueues.Corporation_DeadLetterQueue);
                        e.DiscardFaultedMessages();
                        e.ConfigureConsumer<CorporationReattemptConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.corporationSettings.corporationQueues.Corporation_DeadLetterQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.corporationSettings.corporationExchanges.Corporation_DeadLetterExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<CorporationEventErrorConsumer>(context);
                    });
                    #endregion

                    // ---------------- PC ----------------
                    #region PC Queues
                    cfg.Message<PCMessageEvent>(x =>
                    {
                        x.SetEntityName(settings.pcSettings.pcExchanges.PC_MainExchange);
                    });

                    cfg.Message<PCReattemptEvent>(x =>
                    {
                        x.SetEntityName(settings.pcSettings.pcExchanges.PC_ReattemptExchange);
                    });

                    cfg.Message<Fault<PCReattemptEvent>>(x =>
                    {
                        x.SetEntityName(settings.pcSettings.pcExchanges.PC_DeadLetterExchange);
                    });

                    cfg.ReceiveEndpoint(settings.pcSettings.pcQueues.PC_MainQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.pcSettings.pcExchanges.PC_MainExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<PCEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.pcSettings.pcQueues.PC_ReattemptQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.pcSettings.pcExchanges.PC_ReattemptExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.UseMessageRetry(r => r.Immediate(2));
                        e.BindDeadLetterQueue(settings.pcSettings.pcQueues.PC_DeadLetterQueue);
                        e.DiscardFaultedMessages();
                        e.ConfigureConsumer<PCReattemptConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.pcSettings.pcQueues.PC_DeadLetterQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.pcSettings.pcExchanges.PC_DeadLetterExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<PCEventErrorConsumer>(context);
                    });
                    #endregion

                    // ---------------- Vendor ----------------
                    #region Vendor Queues
                    cfg.Message<VendorMessageEvent>(x =>
                    {
                        x.SetEntityName(settings.vendorSettings.vendorExchanges.Vendor_MainExchange);
                    });

                    cfg.Message<VendorReattemptEvent>(x =>
                    {
                        x.SetEntityName(settings.vendorSettings.vendorExchanges.Vendor_ReattemptExchange);
                    });

                    cfg.Message<Fault<VendorReattemptEvent>>(x =>
                    {
                        x.SetEntityName(settings.vendorSettings.vendorExchanges.Vendor_DeadLetterExchange);
                    });

                    cfg.ReceiveEndpoint(settings.vendorSettings.vendorQueues.Vendor_MainQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.vendorSettings.vendorExchanges.Vendor_MainExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<VendorEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.vendorSettings.vendorQueues.Vendor_ReattemptQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.vendorSettings.vendorExchanges.Vendor_ReattemptExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.UseMessageRetry(r => r.Immediate(2));
                        e.BindDeadLetterQueue(settings.vendorSettings.vendorQueues.Vendor_DeadLetterQueue);
                        e.DiscardFaultedMessages();
                        e.ConfigureConsumer<VendorReattemptConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.vendorSettings.vendorQueues.Vendor_DeadLetterQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.vendorSettings.vendorExchanges.Vendor_DeadLetterExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<VendorEventErrorConsumer>(context);
                    });
                    #endregion

                    //----------------Contract----------------
                    #region Contract Queues
                    cfg.Message<ContractMessageEvent>(x =>
                    {
                        x.SetEntityName(settings.contractSettings.contractExchanges.Contract_MainExchange);
                    });

                    cfg.Message<ContractReattemptEvent>(x =>
                    {
                        x.SetEntityName(settings.contractSettings.contractExchanges.Contract_ReattemptExchange);
                    });

                    cfg.Message<Fault<ContractReattemptEvent>>(x =>
                    {
                        x.SetEntityName(settings.contractSettings.contractExchanges.Contract_DeadLetterExchange);
                    });

                    cfg.ReceiveEndpoint(settings.contractSettings.contractQueues.Contract_MainQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.contractSettings.contractExchanges.Contract_MainExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<ContractEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.contractSettings.contractQueues.Contract_ReattemptQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.contractSettings.contractExchanges.Contract_ReattemptExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.UseMessageRetry(r => r.Immediate(2));
                        e.BindDeadLetterQueue(settings.contractSettings.contractQueues.Contract_DeadLetterQueue);
                        e.DiscardFaultedMessages();
                        e.ConfigureConsumer<ContractReattemptConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.contractSettings.contractQueues.Contract_DeadLetterQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.contractSettings.contractExchanges.Contract_DeadLetterExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<ContractEventErrorConsumer>(context);
                    });
                    #endregion

                    //----------------Account----------------
                    #region Account Queues
                    cfg.Message<AccountMessageEvent>(x =>
                    {
                        x.SetEntityName(settings.accountSettings.accountExchanges.Account_MainExchange);
                    });

                    cfg.Message<AccountReattemptEvent>(x =>
                    {
                        x.SetEntityName(settings.accountSettings.accountExchanges.Account_ReattemptExchange);
                    });

                    cfg.Message<Fault<AccountReattemptEvent>>(x =>
                    {
                        x.SetEntityName(settings.accountSettings.accountExchanges.Account_DeadLetterExchange);
                    });

                    cfg.ReceiveEndpoint(settings.accountSettings.accountQueues.Account_MainQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.accountSettings.accountExchanges.Account_MainExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<AccountEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.accountSettings.accountQueues.Account_ReattemptQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.accountSettings.accountExchanges.Account_ReattemptExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.UseMessageRetry(r => r.Immediate(2));
                        e.BindDeadLetterQueue(settings.accountSettings.accountQueues.Account_DeadLetterQueue);
                        e.DiscardFaultedMessages();
                        e.ConfigureConsumer<AccountReattemptConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(settings.accountSettings.accountQueues.Account_DeadLetterQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Bind(settings.accountSettings.accountExchanges.Account_DeadLetterExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<AccountEventErrorConsumer>(context);
                    });
                    #endregion
                });
            }); 

            services.AddScoped<IConsumerLog, ConsumerLogService>();
            services.AddScoped<IPublishService, PublishService>();

            return services;
        }
    }
}
