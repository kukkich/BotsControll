using System.Collections.Generic;
using System.Threading.Tasks;
using BotsControll.Core.Models;

namespace BotsControll.Core.Repositories;

public interface IGroupRepository : IRepository<Group, string>
{
    public Task<List<Group>> GetOwnedByUserId(int id);
}