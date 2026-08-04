using Common.App.Contracts;
using MassTransit;
using Messages.Domain;
using Microsoft.Extensions.Logging;
using Payable.App.Contracts;
using Payable.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Infra.Consumers
{
 
    public class AIUnapprovedBillConsumer : IConsumer<UpdateUnapprovedBillsAIMessage>
    {
        private readonly ILoggerService logger;
        private readonly IConsumerLog log;
        private readonly IBillEntryService billEntryService;
        private readonly IURLConnection uRLConnection;
        public AIUnapprovedBillConsumer(ILoggerService _logger, IConsumerLog _log, IBillEntryService billEntryService,IURLConnection _uRLConnection)
        {
            logger = _logger;
            this.log = _log;
            this.billEntryService = billEntryService;
            uRLConnection = _uRLConnection;
        } 
        public async Task Consume(ConsumeContext<UpdateUnapprovedBillsAIMessage> context)
        {


            UpdateUnapprovedBillsAIMessage message = context.Message; 
            int statusCode = 0;
            string operationName = "AIUnapproved bill";
            try
            {
                UpdateUnapprovedBillsAIRequest req = message.UpdateUnapprovedBillsAIRequest;
                uRLConnection.UrlName = message.ClientName;
                var response = await billEntryService.UpdateUnapprovedBillsAI(req, message.UserID, message.ClientID, message.ClientName);
                await log.InsertSyncLogAsync(message, message.MessageID, operationName, response.StatusCode);
                logger.LogFile (" {Operation} succeed for MessageID : {MessageID} ", operationName, message.MessageID);

            }
            catch (Exception ex)
            {
                string exceptionMsg = ex.Message;
                await log.InsertSyncLogAsync(message, message.MessageID, operationName, 0, exceptionMsg);
                logger.LogFile(" {Operation} failed for MessageID : {MessageID} ", operationName, message.MessageID);
                throw new SyncFailedException(operationName, message.MessageID, 500);
            }

        } 
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

    public class AIUnapprovedBillErrorConsumer : IConsumer<Fault<UpdateUnapprovedBillsAIMessage>>
    {
        private readonly ILogger<AIUnapprovedBillErrorConsumer> _logger;
        private readonly IConsumerLog log;
        public AIUnapprovedBillErrorConsumer(ILogger<AIUnapprovedBillErrorConsumer> logger, IConsumerLog _log)
        {
            _logger = logger;
            log = _log;
        } 
        public async Task Consume(ConsumeContext<Fault<UpdateUnapprovedBillsAIMessage>> context)
        {
            UpdateUnapprovedBillsAIMessage message = context.Message.Message;
            List<string> exceptionInfo = context.Message.Exceptions.Select(s => s.Message).ToList();
            string exceptionMsg = string.Join(",", exceptionInfo);
            await log.InsertSyncLogAsync(message, message.MessageID, "AIUnapproved bill", 0, exceptionMsg);
            await Task.CompletedTask;
        }
    }
}
