using Azure;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using MassTransit;
using MassTransit.Logging;
using Messages.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseSynch.App.Contracts;
using static System.Data.Entity.Infrastructure.Design.Executor;
namespace WareHouseSynch.Infra.Consumer
{
    public class SynchMessageConsumer : IConsumer<SynchMessage>
    {
        private readonly ISyncEventHandler _syncEventHandler;
        private readonly ILogger<SynchMessageConsumer> _logger;
        private readonly IConsumerLog log;

        public SynchMessageConsumer(ISyncEventHandler syncEventHandler, ILogger<SynchMessageConsumer> logger, IConsumerLog _log)
        {
            _syncEventHandler = syncEventHandler;
            _logger = logger;
            this.log = _log;
        }
        private string GetOperationName(short eventType)
        {
            return Enum.GetName(typeof(SynchEventType), eventType)!;
        }
        public async Task Consume(ConsumeContext<SynchMessage> context)
        {
          
           
            SynchMessage message = context.Message;
            string operationName = GetOperationName(message.EventType);
            int statusCode = 0;
            try
            {
                WareHouseBinReq req = MapRequest(message);
                var response = await _syncEventHandler.Handle((SynchEventType)message.EventType, req, message);
                await log.InsertSyncLogAsync(message, message.MessageID, operationName, response.StatusCode);
                _logger.LogInformation(" {Operation} succeed for MessageID : {MessageID} ", operationName, message.MessageID);

            }
            catch (Exception ex)        
            {
                string exceptionMsg = ex.Message;
                await log.InsertSyncLogAsync(message, message.MessageID, operationName, 0, exceptionMsg);
                _logger.LogError(" {Operation} failed for MessageID : {MessageID} ", operationName, message.MessageID);
                throw new SyncFailedException(operationName, message.MessageID, 500);
            }

        }

        private static WareHouseBinReq MapRequest(SynchMessage message) => new()
        {
            ID = message.ID,
            CorpID = message.CorpID,
            FromDate = message.FromDate,
            ToDate = message.ToDate,
            CloneID = message.CloneID,
            IsUpdatePrevious = message.IsUpdatePrevious
        };
    }
    

    public class SyncFailedException : Exception
    {
        public int? StatusCode { get; }
        public SyncFailedException(string operationName, string messageId, int? statusCode = null)
            : base($"{operationName} failed (Status: {statusCode}) for MessageID: {messageId}")
        {
            StatusCode = statusCode;
        }
    }

    public class SynchMessage_ErrorConsumer : IConsumer<Fault<SynchMessage>>  
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<SynchMessage_ErrorConsumer> _logger;
        private readonly IConsumerLog log;
        public SynchMessage_ErrorConsumer(IPublishEndpoint publishEndpoint, ILogger<SynchMessage_ErrorConsumer> logger, IConsumerLog _log)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            log = _log;
        }
        private string GetOperationName(short eventType)
        {
            return Enum.GetName(typeof(SynchEventType), eventType)!;
        }
        public async Task Consume(ConsumeContext<Fault<SynchMessage>> context)
        {
            SynchMessage message = context.Message.Message;
            List<string> exceptionInfo = context.Message.Exceptions.Select(s=>s.Message).ToList();
            string exceptionMsg=string.Join(",",exceptionInfo);
            await log.InsertSyncLogAsync(message, message.MessageID, GetOperationName(message.EventType), 0,exceptionMsg);
            await Task.CompletedTask;
        }
    }
}
