using Microsoft.AspNetCore.SignalR;

namespace Survay_Basket.API.Hubs;

public sealed class ChatHub : Hub<IChatClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.ReceiveMessage( $"{Context.ConnectionId} has joined");
        // {"protocol":"json","version":1}
    }
    public async Task SendMessage(string message)
    {
        await Clients.All.ReceiveMessage( $"{Context.ConnectionId} {message}");
        // {"arguments":["Test message"], "invocationId":"0","target":"SendMessage", "type":1}
    }

}
public interface IChatClient
{
    Task ReceiveMessage(string message);
}