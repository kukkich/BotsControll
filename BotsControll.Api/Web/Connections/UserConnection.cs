using System.Collections.Generic;
using BotsControll.Core.Identity;

namespace BotsControll.Api.Web.Connections;

public class ConnectedUser
{
    public required UserIdentity User { get; init; }
    public HashSet<string> ConnectionIds { get; } = new();
}