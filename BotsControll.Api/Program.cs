<<<<<<< Updated upstream
=======
using BotsControll.Api.Hubs;
using BotsControll.Api.Middlewares;
using BotsControll.Api.Repositories;
using BotsControll.Api.Services.Communication;
using BotsControll.Api.Services.Connections;
using BotsControll.Api.Services.ModelsServices;
using BotsControll.Api.Services.Tokens;
using BotsControll.Api.Web.Connections;
using BotsControll.Api.Web.Receiving;
using BotsControll.Core.Models;
using BotsControll.Core.Repositories;
using BotsControll.Core.Services.Login;
using BotsControll.Core.Services.Password;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using BotsControll.Api.Services.Login;
using BotsControll.Core.Services.Time;

>>>>>>> Stashed changes
namespace BotsControll.Api;

public class Program
{
<<<<<<< Updated upstream

    public static void ConfigureServices(IServiceCollection services)
=======
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
>>>>>>> Stashed changes
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = TokenConfig.Issuer,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = TokenConfig.Audience,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    IssuerSigningKey = TokenConfig.GetSymmetricSecurityAccessKey(),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/ws/connect"))
                            context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddDbContext<BotsControlContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
            //.UseLoggerFactory(new NullLoggerFactory()) for logging disable
        });

        services.AddControllers().AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.MaxDepth = 0;
        });

        services.AddSignalR();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
<<<<<<< Updated upstream
=======


        services.AddSingleton<ITimeService, CurrentTimeService>();
        services.AddTransient<IBotMessageService, BotMessageService>();
        services.AddTransient<IUserMessageService, UserMessageService>();
        services.AddTransient<IBotConnectionService, BotConnectionService>();
        services.AddTransient<UserConnectionService>();
        services.AddTransient<GroupService>();
        services.AddTransient<BotService>();

        services.AddScoped<IBotsRepository, BotDbRepository>();
        services.AddScoped<ITokensRepository, TokenDbRepository>();
        services.AddScoped<IUserRepository, UserDbRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IJournalRepository, JournalDbRepository>();

        services.AddSingleton<IBotConnectionRepository, BotConnectionRepository>();
        services.AddSingleton<UserConnectionRepository>();

        services.AddScoped<IWebSocketReceiverFactory, BotWebSocketReceiverFactory>();


        services.AddTransient<IUserLoginService, UserLoginService>();
        services.AddTransient<TokenService>();

        services.AddTransient<IPasswordHasher, PasswordHasher>();

        services.AddCors(options =>
        {
            options.AddPolicy(name: "Vue",
                builder =>
                {
                    builder.WithOrigins(
                            "http://localhost:8080",
                            "https://localhost:8080"
                            )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();

                });
        });
>>>>>>> Stashed changes
    }

    public static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("Vue");

        app.UseHttpsRedirection();
<<<<<<< Updated upstream

        app.UseAuthorization();
=======
        
        //app.UseAuthorization();
        app.UseMiddleware<UserMockueAuthenticationMiddleware>();
>>>>>>> Stashed changes

        app.MapControllers();
<<<<<<< Updated upstream
=======
        
        app.Map("/ws/bot", builder =>
        {
            builder.UseMiddleware<BotAuthenticationMiddleware>();
            builder.UseMiddleware<WebSocketMiddleware>();
        });

        app.MapHub<ClientHub>("/ws/connect");


        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetService<BotsControlContext>();
>>>>>>> Stashed changes
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
<<<<<<< Updated upstream
            
        ConfigureServices(builder.Services);
=======

        ConfigureServices(builder.Services, builder.Configuration);
>>>>>>> Stashed changes

        var app = builder.Build();
        Configure(app);

        app.Run();
    }
}