using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.SignalR;

namespace CoreAccounting.API.Hubs
{
    public interface IPayableSignalR
    {
        Task ReceiveSignalRMessage(SignalResponse res);
        Task UpdateClientsCount(int count);
    }
    public class SignalRHub : Hub<IPayableSignalR>
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
        public async Task SendSignalRMessage(SignalResponse res)
        {
            await Clients.All.ReceiveSignalRMessage(res);
        }
    }
}
