using BotsControll.Core.Identity;
using System.Collections.Generic;

namespace BotsControll.Api.Web.Connections;

public class ConnectedUser
{
    public required UserIdentity User { get; init; }
    public HashSet<string> ConnectionIds { get; } = new();
}