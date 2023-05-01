using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotsControll.Core.Models;
using BotsControll.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BotsControll.Api.Repositories;

public class GroupRepository : DbRepository, IGroupRepository
{
    public GroupRepository(BotsControlContext context)
        : base(context)
        { }

    public async Task<Group> GetById(string id)
    {
        return await Context.Groups
            .Include(x => x.Owner)
            .Include(x => x.UserGroups)
            .Include(x => x.BotGroups)
            .FirstAsync(x => x.Id == id);
    }

    public async Task<Group> Add(Group entity)
    {
        Context.Groups.Add(entity);
        await Context.Entry(entity)
            .Collection(x => x.BotGroups)
            .LoadAsync();
        await Context.Entry(entity)
            .Collection(x => x.UserGroups)
            .LoadAsync();
        await Context.Entry(entity)
            .Reference(x => x.Owner)
            .LoadAsync();

        await Context.SaveChangesAsync();
        return entity;
    }

    public Task<Group> Update(Group entity)
    {
        throw new System.NotImplementedException();
    }

    public async Task<Group> Delete(Group entity)
    {
        Context.Groups.Remove(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<List<Group>> GetOwnedByUserId(int id)
    {
        return await Context.Groups.Where(x => x.OwnerId == id)
            .Include(x => x.Owner)
            .Include(x => x.UserGroups)
            .Include(x => x.BotGroups)
            .ToListAsync();
    }
}