using BotsControll.Core.Dtos.Login;
using System.Threading.Tasks;

namespace BotsControll.Core.Services.Login;

public interface IUserLoginService
{
    public Task<LoginResultModel> Register(RegistrationModel registrationModel);
    public Task<LoginResultModel> Login(LoginModel loginModel);
    public Task<string> Logout(string refreshToken);
    public Task<LoginResultModel> Refresh(string? refreshToken);
}