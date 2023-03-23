using System.Threading.Tasks;

namespace BotsControll.Api.Services.Communication;

public interface IUserMessageService
{
    public Task Send(int userId, string message);
}