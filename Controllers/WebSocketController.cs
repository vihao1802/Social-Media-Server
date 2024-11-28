using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Controllers
{
    [Route("api/ws")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketService _webSocketService;

        public WebSocketController(IWebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        [HttpGet("messenge")]
        public async Task<IActionResult> Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _webSocketService.HandleWebSocketConnectionAsync(webSocket);
                return new EmptyResult();
            }

            return BadRequest("WebSocket requests only.");
        }
    }
}
