using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Infra.Publishing;
using MassTransit;
using Messages.Domain.Models;
using Messages.Domain.Synch;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WHSubscription.App.Contract;
using WHSubscription.Domain.DataModel;

namespace WHSubscription.Infra.Consumers
{
    public class PCEventConsumer : IConsumer<PCMessageEvent>
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<PCEventConsumer> logger;
        private readonly IConsumerLog log;
        private readonly IPublishEndpoint publishEndpoint;
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string externalAPI_SecretKey;
        private readonly string WebHook_CacheKey;
        private readonly IElastiCacheService elastiCacheService;
        #endregion

        #region Constructor
        public PCEventConsumer(IUnitOfWork unitOfWork,
                                     ILogger<PCEventConsumer> logger,
                                     IConsumerLog _log,
                                     IPublishEndpoint _publishEndpoint,
                                     IOptions<WebhookInfo> webhookInfo,
                                     IElastiCacheService elastiCacheService)
        {
            this.unitOfWork = unitOfWork;
            this.publishEndpoint = _publishEndpoint;
            this.logger = logger;
            this.log = _log;
            this.externalAPI_SecretKey = webhookInfo.Value.ExternalAPI_SecretKey;
            this.WebHook_CacheKey = webhookInfo.Value.Webhook_CacheKey;
            this.elastiCacheService = elastiCacheService;
        }
        #endregion

        #region Methods
        public async Task Consume(ConsumeContext<PCMessageEvent> context)
        {
            PCMessageEvent message = context.Message;
            try
            {
                var cacheResponse = await elastiCacheService.GetElastiCacheDataAsync<List<Whsubscription>>(WebHook_CacheKey);
                var subscriptions = cacheResponse.RedisCacheData;

                if (subscriptions == null || !subscriptions.Any())
                {
                    subscriptions = (await unitOfWork.WHSubscriptions
                        .GetAll(x => x.Status == 1)) // fetch all active subs
                        .ToList();

                    await elastiCacheService.SetElastiCacheDataAsync(WebHook_CacheKey, subscriptions);
                }

                subscriptions = subscriptions.Where(x => x.EventType == message.EventType && x.Status == 1).ToList();

                if (!subscriptions.Any())
                {
                    string errorMsg = $"No active subscriptions found for event type {message.EventType}";
                    await log.InsertSyncLogAsync(message, message.MessageID, message.GetType().FullName, 0, errorMsg);
                    logger.LogWarning(errorMsg);
                    return;
                }

                foreach (var subscription in subscriptions)
                    {
                        bool failed = false;
                        var payload = JsonSerializer.Serialize(message.PCEventPayload);
                        var signature = SignatureHelper.GenerateSignature(payload, externalAPI_SecretKey);

                        // attach signature to request
                        httpClient.DefaultRequestHeaders.Remove("X-Signature");
                        httpClient.DefaultRequestHeaders.Add("X-Signature", signature);

                        var urlName = subscription.CallBackUrl + (subscription.UrlName == null ? "" : subscription.UrlName);

                        try
                        {
                            var response = await httpClient.PostAsJsonAsync(urlName, message.PCEventPayload);
                            if (response.StatusCode != System.Net.HttpStatusCode.NoContent && response.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                failed = true;
                                string exMsg = (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.Unauthorized) ? "Invalid signature" : "";
                                logger.LogWarning("Webhook failed for message {Id} - Status {Code}",
                                    message.MessageID, (int)response.StatusCode);
                                await log.InsertSyncLogAsync(message, message.MessageID, message.GetType().FullName, (int)response.StatusCode, exMsg, subscription.Id);

                            }
                            else
                            {
                                failed = false;
                                logger.LogInformation("Webhook succeed for message {Id}", message.MessageID);
                                await log.InsertSyncLogAsync(message, message.MessageID, message.GetType().FullName, (int)response.StatusCode, null, subscription.Id);
                            }
                        }
                        catch (Exception ex)
                        { 
                            logger.LogError(ex, "Error calling webhook for message {Id}", message.MessageID);
                            await log.InsertSyncLogAsync(message, message.MessageID, message.GetType().FullName, 0, ex.Message, subscription.Id);

                            // If there were failures → send to Reattempt Queue
                            PCReattemptEvent request = MapRequest(message, urlName, subscription.Id);
                            await publishEndpoint.Publish(request);
                        }
                        // If there were failures → send to Reattempt Queue
                        if (failed)
                        {
                            PCReattemptEvent request = MapRequest(message, urlName, subscription.Id);
                            await publishEndpoint.Publish(request);
                        }
                    }
                 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing PCEvent");
                await log.InsertSyncLogAsync(message, message.MessageID, message.GetType().FullName, 0, ex.Message);
            }
        }
        private static PCReattemptEvent MapRequest(PCMessageEvent message, string URLName, long subscriptionID) => new()
        {
            PCEventPayload = message.PCEventPayload,
            EventType = message.EventType,
            UrlName = URLName,
            MessageID = message.MessageID,
            SubscriptionID = subscriptionID
        };
        #endregion
    }

    public class PCReattemptConsumer : IConsumer<PCReattemptEvent>
    {
        #region Fields
        private readonly ILogger<PCReattemptConsumer> logger;
        private readonly IConsumerLog consumerLog;
        private readonly string externalAPI_SecretKey;
        #endregion

        #region Constructor
        public PCReattemptConsumer( ILogger<PCReattemptConsumer> _logger,
                                    IConsumerLog _consumerLog, 
                                    IOptions<WebhookInfo> webhookInfo)
        { 
            this.logger = _logger;
            this.consumerLog = _consumerLog;
            this.externalAPI_SecretKey = webhookInfo.Value.ExternalAPI_SecretKey;
        }

        #endregion

        #region Methods
        public async Task Consume(ConsumeContext<PCReattemptEvent> context)
        {
            PCReattemptEvent message = context.Message;
            if (message != null && message.UrlName != null)
            {
                var httpClient = new HttpClient();
                try
                {
                    var payload = JsonSerializer.Serialize(message.PCEventPayload);
                    var signature = SignatureHelper.GenerateSignature(payload, externalAPI_SecretKey);

                    // attach signature to request
                    httpClient.DefaultRequestHeaders.Remove("X-Signature");
                    httpClient.DefaultRequestHeaders.Add("X-Signature", signature);

                    var response = await httpClient.PostAsJsonAsync(message.UrlName, message.PCEventPayload);
                    if (response.StatusCode != System.Net.HttpStatusCode.NoContent && response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        string exMsg = (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.Unauthorized) ? "Invalid signature" : "";
                        logger.LogWarning("Webhook failed for message {Id} - Status {Code}", message.MessageID, (int)response.StatusCode);
                        await consumerLog.InsertSyncLogAsync(message, message.MessageID, message.GetType().FullName, (int)response.StatusCode, exMsg, message.SubscriptionID);
                        throw new EventFailedException(message.GetType().FullName, message.MessageID, 500);
                    }
                    else
                    {
                        logger.LogInformation("Webhook succeed for message {Id}", message.MessageID);
                        await consumerLog.InsertSyncLogAsync(message, message.MessageID, message.GetType().FullName, (int)response.StatusCode, null, message.SubscriptionID);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Webhook failed for message {Id} - Status {Code}", message.MessageID, 0);
                    await consumerLog.InsertSyncLogAsync(message, message.MessageID, message.GetType().FullName, 0, ex.Message, message.SubscriptionID);
                    throw new EventFailedException(message.GetType().FullName, message.MessageID, 500);
                }

            }
            else
            {
                logger.LogWarning("Received null message in PCReattemptConsumer");
            }
        }

        #endregion
    }

    public class PCEventErrorConsumer : IConsumer<Fault<PCReattemptEvent>>
    {
        private readonly IConsumerLog log;
        public PCEventErrorConsumer(IConsumerLog log)
        {
            this.log = log;
        }
        public async Task Consume(ConsumeContext<Fault<PCReattemptEvent>> context)
        {
            PCReattemptEvent message = context.Message.Message;
            List<string> exceptionInfo = context.Message.Exceptions.Select(s => s.Message).ToList();
            string exceptionMsg = string.Join(",", exceptionInfo);
            await log.InsertSyncLogAsync(message, message.MessageID, message.GetType().FullName, 0, exceptionMsg, message.SubscriptionID);
        }
    }
     
}
