using Common.App.Contracts;
using Common.App.Services;
using Common.Infra.Logger;
using Common.Infra.Publishing;
using MassTransit;
using Messages.Domain;
using Messages.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog;
using Payable.App.Contracts;
using Payable.Domain.DTO.Model;
using Payable.Domain.DTO.Req;
using Payable.Infra.ChatService;
using Payable.Infra.Consumers;
using Payable.Infra.DataRepos;
using Payable.Infra.DBCon;
using Payable.Infra.FIlesService;
using Payable.Infra.OcrTransactions;
using Payable.Infra.PaymentGateWay;
using WebhookInfo = Payable.Domain.DTO.Model.WebhookInfo;


namespace Payable.Infra;

public static class PayableInfraDependencyInjection
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration config)
    {
        //LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        //services.AddSingleton<ILoggerService, LoggerService>(); 
        services.Configure<ProviderURLInfo>(config.GetSection("ProviderBaseURLs"));
        services.Configure<WebhookInfo>(config.GetSection("Webhooks"));
        services.Configure<RePayKeyInfo>(config.GetSection("RePayKeyInfo"));
        services.Configure<RePayEndPoints>(config.GetSection("RePayEndPoints"));
        services.Configure<ChatURLs>(config.GetSection("ChatURLInfo"));
        services.Configure<OCRTransctionsURLs>(config.GetSection("OcrTransactionURLInfo"));
        services.Configure<BillUploadURLs>(config.GetSection("BillUploadURL_Info"));
        services.AddScoped<IUnitOfWork, PayableUnitOfWork>();
        services.AddDbContext<PayableDBContext>();
        services.AddScoped<IPaymentGateway, PaymentGatewayCalls>();
        
        services.AddScoped<IChatService, ChatServiceCalls>();
        services.AddScoped<IOcrTransactionService, OcrTransactionsCalls>();
        services.AddScoped<IBillUploadService,BillUploadServiceCalls>();
        services.Configure<RabbitMqSettings>(config.GetSection("RabbitMQSettings"));
        services.AddMassTransit(x =>
        {
            x.AddConsumer<AIUnapprovedBillConsumer>();
            x.AddConsumer<AIUnapprovedBillErrorConsumer>();

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

                // Custom synch main exchange name
                cfg.Message<SynchMessage>(x =>
                {
                    x.SetEntityName(settings.Queues.MainExchange);
                });

                // Custom Main exchange name
                cfg.Message<UpdateUnapprovedBillsAIMessage>(x =>
                {
                    x.SetEntityName(settings.AIUnapprovedQNames.AIUnapprovedMainExchange);
                });

                // Custom FAULT exchange name
                cfg.Message<Fault<UpdateUnapprovedBillsAIMessage>>(x =>
                {
                    x.SetEntityName(settings.AIUnapprovedQNames.AIUnapprovedDeadLetterExchange);
                });

                // MAIN QUEUE
                cfg.ReceiveEndpoint(settings.AIUnapprovedQNames.AIUnapprovedMainQueue, e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.PrefetchCount = 10;
                    e.ConcurrentMessageLimit = 5;
                    e.Durable = true;
                    e.ExchangeType = "fanout";
                    e.Bind(settings.AIUnapprovedQNames.AIUnapprovedMainExchange, x =>
                    {
                        x.ExchangeType = "fanout";
                    });
                    e.BindDeadLetterQueue(settings.AIUnapprovedQNames.AIUnapprovedDeadLetterQueue);
                    e.DiscardFaultedMessages();
                    e.ConfigureConsumer<AIUnapprovedBillConsumer>(context);
                });

                //// DEAD LETTER QUEUE
                //cfg.ReceiveEndpoint(settings.Queues.DeadLetterQueue, e =>
                //{
                //    e.ConfigureConsumeTopology = false;
                //    e.PrefetchCount = 10;
                //    e.ConcurrentMessageLimit = 5;
                //    e.Durable = true;
                //    e.ExchangeType = "fanout"; e.Bind(settings.Queues.DeadLetterExchange, x =>
                //    {
                //        x.ExchangeType = "fanout";
                //    });
                //    e.ConfigureConsumer<AIUnapprovedBillErrorConsumer>(context);
                //});
            });
        });

        services.AddScoped<IConsumerLog, ConsumerLogService>();
        services.AddScoped<IPublishService, PublishService>();
        return services;
    }
}
