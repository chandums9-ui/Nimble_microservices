using BankFeed.Domain.DTO.Resp;
using Microsoft.AspNetCore.SignalR;

namespace BankFeed.API.Hubs
{
    public interface IBankFeedHub
    {
        Task ReceiveSplitSave(FeedTransactionMappingResponse res);
        Task UpdateClientsCount(int count);
    }
    public class FeedHub : Hub<IBankFeedHub>
    {
        static int clientsCount;

        public override async Task OnConnectedAsync()
        {
            clientsCount++;
            await Clients.All.UpdateClientsCount(clientsCount);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            clientsCount--;
            await Clients.All.UpdateClientsCount(clientsCount);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendSplitSave(FeedTransactionMappingResponse res)
        {
            await Clients.All.ReceiveSplitSave(res);
        }
    }
}
