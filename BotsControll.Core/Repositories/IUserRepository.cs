using BotsControll.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotsControll.Core.Repositories;

public interface IUserRepository : IRepository<User, int>
{
    public Task<IEnumerable<User>> GetAll();
    public Task<bool> IsAnyWithLogin(string login);
    public Task<User> GetByLogin(string login);
    public Task<User?> TryGetByLogin(string login);
}