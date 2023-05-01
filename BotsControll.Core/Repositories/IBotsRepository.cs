using BotsControll.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotsControll.Core.Repositories;

public interface IBotsRepository : IRepository<Bot, string>
{
    public Task<List<Bot>> GetOwnedByUserId(int userId);
    public Task<List<Bot>> GetAvailableByUserId(int userId);
}