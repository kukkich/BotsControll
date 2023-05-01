using BotsControll.Core.Models.Identity;
using System.Threading.Tasks;

namespace BotsControll.Core.Repositories;

public interface ITokensRepository : IRepository<Token, string>
{
    public Task<Token?> TryGetToken(string refreshToken);
    public Task<Token?> TryGetByUserId(int id);
    public Task<Token> GetToken(string refreshToken);
}