using BotsControll.Api.Hubs;
using BotsControll.Api.Middlewares;
using BotsControll.Api.Services.Bots;
using BotsControll.Api.Services.Users;
using BotsControll.Api.Web.Connections;
using BotsControll.Api.Web.Receiving;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BotsControll.Api;

public class Program
{

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSingleton<IBotConnectionRepository, BotConnectionRepository>();
        services.AddTransient<IBotConnectionService, BotConnectionService>();
        services.AddTransient<IWebSocketReceiverFactory, BotWebSocketReceiverFactory>();

        services.AddSingleton<UserConnectionRepository>();
        services.AddTransient<UserConnectionService>();
    }

    public static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseDefaultFiles();
        app.UseStaticFiles();
        
        app.UseWebSockets();

        app.MapControllers();
        app.Map("/ws/bot", builder => builder.UseMiddleware<WebSocketMiddleware>());
        app.MapHub<UserHub>("/ws/connect");
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
            
        ConfigureServices(builder.Services);

        var app = builder.Build();
        Configure(app);

        app.Run();
    }
}