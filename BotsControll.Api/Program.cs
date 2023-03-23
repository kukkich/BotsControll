using BotsControll.Api.Hubs;
using BotsControll.Api.Middlewares;
using BotsControll.Api.Services.Communication;
using BotsControll.Api.Services.Connections;
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

        services.AddTransient<IBotMessageService, BotMessageService>();
        services.AddTransient<IUserMessageService, UserMessageService>();

        services.AddSingleton<IBotConnectionRepository, BotConnectionRepository>();
        services.AddSingleton<UserConnectionRepository>();

        services.AddTransient<IBotConnectionService, BotConnectionService>();
        services.AddTransient<UserConnectionService>();

        services.AddTransient<IWebSocketReceiverFactory, BotWebSocketReceiverFactory>();

        services.AddCors(options =>
        {
            options.AddPolicy(name: "VueS",
                builder =>
                {
                    builder.WithOrigins("https://localhost:7126")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();

                });
            options.AddPolicy(name: "Vue",
                builder =>
                {
                    builder.WithOrigins("http://localhost:5042")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();

                });
        });
    }

    public static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            //app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseWebSockets();

        app.MapControllers();
        app.Map("/ws/bot", builder =>
        {
            builder.UseMiddleware<BotAuthenticationMiddleware>();
            builder.UseMiddleware<WebSocketMiddleware>();
        });
        app.MapHub<ClientHub>("/ws/connect");
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