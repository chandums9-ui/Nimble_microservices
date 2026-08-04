using Common.App.Contracts;
using Common.App.Services;
using Common.Infra.Publishing;
using MassTransit;
using Messages.Domain;
using Messages.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseSynch.App.Contracts;
using WareHouseSynch.Infra.Consumer;
using WareHouseSynch.Infra.DataRepos;
using WareHouseSynch.Infra.DBCon;

namespace WareHouseSynch.Infra
{
    public static class InfraDependencyInjection
    {
        public static IServiceCollection AddSynchInfraServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IWareHouseUnitOfWork, WareHouseUnitOfWork>();
            //services.AddDbContext<NPAnalyticsWareHouseContext>();
            var conn = config.GetConnectionString("S1Connection");
            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("Connection string 'S1Connection' is not configured.");

            services.AddDbContext<NPAnalyticsWareHouseContext>(options =>
                options.UseSqlServer(conn));
            services.Configure<RabbitMqSettings>(config.GetSection("RabbitMQSettings"));

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SynchMessageConsumer>();
                x.AddConsumer<SynchMessage_ErrorConsumer>();

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

                    // Custom Main exchange name
                    cfg.Message<SynchMessage>(x =>
                    {
                        x.SetEntityName(settings.Queues.MainExchange);
                    });

                    // Custom FAULT exchange name
                    cfg.Message<Fault<SynchMessage>>(x =>
                    {
                        x.SetEntityName(settings.Queues.DeadLetterExchange);
                    });

                    // MAIN QUEUE
                    cfg.ReceiveEndpoint(settings.Queues.MainQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Durable = true;
                        e.ExchangeType = "fanout";
                        e.Bind(settings.Queues.MainExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.BindDeadLetterQueue(settings.Queues.DeadLetterQueue);
                        e.DiscardFaultedMessages();
                        e.ConfigureConsumer<SynchMessageConsumer>(context);
                    });

                    // DEAD LETTER QUEUE
                    cfg.ReceiveEndpoint(settings.Queues.DeadLetterQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.PrefetchCount = 10;
                        e.ConcurrentMessageLimit = 5;
                        e.Durable = true;
                        e.ExchangeType = "fanout"; e.Bind(settings.Queues.DeadLetterExchange, x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                        e.ConfigureConsumer<SynchMessage_ErrorConsumer>(context);
                    });
                });
            });

            services.AddScoped<IConsumerLog, ConsumerLogService>();
            services.AddScoped<IPublishService, PublishService>();
            return services;
        }
    }
}
