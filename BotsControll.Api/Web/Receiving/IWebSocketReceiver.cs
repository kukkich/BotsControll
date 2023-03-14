using System.Threading.Tasks;

namespace BotsControll.Api.Web.Receiving;

public interface IWebSocketReceiver
{
    public Task StartReceiveAsync();
}