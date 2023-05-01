using BotsControll.Core.Models;
using BotsControll.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotsControll.Api.Repositories;

public class UserDbRepository : DbRepository, IUserRepository
{

    public UserDbRepository(BotsControlContext context)
        : base(context)
        { }

    public async Task<User> GetById(int id)
    {
        return await Context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.OwnedGroups)
            .FirstAsync(x => x.Id == id);
    }

    public async Task<User> Add(User entity)
    {
        Context.Users.Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<User> Update(User entity)
    {
        Context.Users.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<User> Delete(User entity)
    {
        Context.Users.Remove(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await Context.Users.ToListAsync();
    }

    public async Task<bool> IsAnyWithLogin(string login)
    {
        return await Context.Users.AnyAsync(x => x.Login == login);
    }

    public async Task<User> GetByLogin(string login)
    {
        return await Context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .FirstAsync(x => x.Login == login);
    }

    public async Task<User?> TryGetByLogin(string login)
    {
        return await Context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Login == login);
    }
}