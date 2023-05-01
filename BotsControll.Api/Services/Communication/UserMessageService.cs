using BotsControll.Api.Hubs;
using BotsControll.Api.Web.Connections;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.Communication;

public class UserMessageService : IUserMessageService
{
    private readonly UserConnectionRepository _userConnections;
    private readonly IHubContext<ClientHub> _userHub;

    public UserMessageService(IHubContext<ClientHub> userHub, UserConnectionRepository userConnections)
    {
        _userHub = userHub;
        _userConnections = userConnections;
    }

    public async Task Send(int userId, string message)
    {
        _userConnections.TryGetByUserId(userId, out var connectedUser);
        if (connectedUser is null) return;

        await Task.WhenAll(connectedUser.ConnectionIds
            .Select(id =>
            {
                return _userHub.Clients.Client(id).SendCoreAsync(
                    ClientHub.Actions.ReceiveMessage,
                    new object?[] { message }
                );
            })
        );
    }
}