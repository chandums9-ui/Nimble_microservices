using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using MassTransit;
using Messages.Domain;
using Messages.Domain.Synch;
using Payable.Domain.DTO.Req;
using System.Net;

namespace Common.Infra.Publishing
{
    public class PublishService : IPublishService
    {
        private readonly IPublishEndpoint publishEndpoint;
        public PublishService(IPublishEndpoint _publishEndpoint) {
            publishEndpoint = _publishEndpoint;
        }
        public async Task<StatusDTO> SynchMessagePublish(SynchMessage message)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                await publishEndpoint.Publish(message);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Status = $"Message Published: {message.MessageID}";
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = $"Failed to publish message: {ex.Message}";
                throw;
            }
            return response;
        }

        public async Task<StatusDTO> CorporationEventPublish(CorporationMessageEvent message)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                message.MessageID = new PFAID(new PFAID().UID).ToString();
                await publishEndpoint.Publish(message);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Status = $"Message Published: {message.MessageID}";
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = $"Failed to publish message: {ex.Message}";
                throw;
            }
            return response;
        }

        public async Task<StatusDTO> PCEventPublish(PCMessageEvent message)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                message.MessageID = new PFAID(new PFAID().UID).ToString();
                await publishEndpoint.Publish(message);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Status = $"Message Published: {message.MessageID}";
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = $"Failed to publish message: {ex.Message}";
                throw;
            }
            return response;
        }

        public async Task<StatusDTO> VendorEventPublish(VendorMessageEvent message)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                message.MessageID = new PFAID(new PFAID().UID).ToString();
                await publishEndpoint.Publish(message);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Status = $"Message Published: {message.MessageID}";
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = $"Failed to publish message: {ex.Message}";
                throw;
            }
            return response;
        }

        public async Task<StatusDTO> ContractEventPublish(ContractMessageEvent message)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                message.MessageID = new PFAID(new PFAID().UID).ToString();
                await publishEndpoint.Publish(message);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Status = $"Message Published: {message.MessageID}";
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = $"Failed to publish message: {ex.Message}";
                throw;
            }
            return response;
        }

        public async Task<StatusDTO> AccountEventPublish(AccountMessageEvent message)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                message.MessageID = new PFAID(new PFAID().UID).ToString();
                await publishEndpoint.Publish(message);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Status = $"Message Published: {message.MessageID}";
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = $"Failed to publish message: {ex.Message}";
                throw;
            }
            return response;
        }

        public async Task<StatusDTO> AIUpdateUnapprovedBillsPublish(UpdateUnapprovedBillsAIRequest message, string userID, string clientID, string clientName)
        {
            StatusDTO response = new StatusDTO();
            try
            {
                UpdateUnapprovedBillsAIMessage aiMessage = new UpdateUnapprovedBillsAIMessage()
                {
                    MessageID = new PFAID(new PFAID().UID).ToString(),
                    UserID = userID,
                    ClientID = clientID,
                    ClientName = clientName,
                    UpdateUnapprovedBillsAIRequest = message
                };
                await publishEndpoint.Publish(aiMessage);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Status = $"Message Published: {aiMessage.MessageID}";
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = $"Failed to publish message: {ex.Message}";
                throw;
            }
            return response;
        }
    }
}
