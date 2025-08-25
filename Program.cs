using ChatAppWebSocket.middleware;
using ChatAppWebSocket.services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<WebsocketHandler>();

var app = builder.Build();
app.UseWebSockets();
app.UseWebSocketHandler();

app.UseDefaultFiles();
app.UseStaticFiles();

//app.MapGet("/", () => "Hello World!");

app.Run();
