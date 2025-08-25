using System.Net.WebSockets;
using System.Text;

namespace ChatAppWebSocket.services
{
    public class WebsocketHandler
    {

        private static readonly List<WebSocket> _socket = new();

        public async Task HandleAsync(WebSocket webSocket)
        {
            _socket.Add(webSocket);
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while(!result.CloseStatus.HasValue)
            {
                var msg = Encoding.UTF8.GetString(buffer, 0 , result.Count);

                foreach(var socket in _socket.Where(s => s.State == WebSocketState.Open))
                {
                    await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)),
                        result.MessageType,
                        result.EndOfMessage,
                        CancellationToken.None
                        );
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            }

            _socket.Remove(webSocket);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
