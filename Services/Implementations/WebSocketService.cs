using System.Net.WebSockets;
using System.Text;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
public class WebSocketService : IWebSocketService
{
    private static List<WebSocket> _connectedClients = new List<WebSocket>();
    public async Task HandleWebSocketConnectionAsync(WebSocket webSocket)
    {
        _connectedClients.Add(webSocket);
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                
                // Broadcast message to all connected clients
                foreach (var client in _connectedClients)
                {
                    if (client.State == WebSocketState.Open)
                    {
                        var response = Encoding.UTF8.GetBytes(message);
                        await client.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
        }
    }
}
}

/* if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var response = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
            } */
