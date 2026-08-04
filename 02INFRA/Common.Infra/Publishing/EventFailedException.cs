using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.Publishing
{
    public class EventFailedException : Exception
    {
        public int? StatusCode { get; }
        public string OperationName { get; }
        public string MessageId { get; }

        public EventFailedException(string operationName, string messageId, int? statusCode = null)
            : base($" {operationName} failed (Status: {statusCode}) for MessageID: {messageId}")
        { 
            OperationName = operationName;
            MessageId = messageId;
            StatusCode = statusCode;
        }
    }

}
