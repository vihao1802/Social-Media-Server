using System.Net.WebSockets;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IWebSocketService
    {
        Task HandleWebSocketConnectionAsync(WebSocket webSocket);
    }
}
