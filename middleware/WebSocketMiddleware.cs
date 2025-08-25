using ChatAppWebSocket.services;

namespace ChatAppWebSocket.middleware
{
    public class WebSocketMiddleware
    {

        private readonly RequestDelegate _next;

        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context , WebsocketHandler handler)
        {
            if(context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await handler.HandleAsync(webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }

    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketMiddleware>();
        }
    }
}
