using BotsControll.Core.Models;
using BotsControll.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotsControll.Api.Repositories;

public class BotDbRepository : DbRepository, IBotsRepository
{
    public BotDbRepository(BotsControlContext context)
        : base(context)
        { }

    public async Task<Bot> GetById(string id)
    {
        return await Context.Bots
            .Include(x => x.Owner)
            .Include(x => x.BotGroups)
            .FirstAsync(x => x.Id == id);
    }

    public async Task<Bot> Add(Bot entity)
    {
        Context.Bots.Add(entity);
        await Context.Entry(entity).Reference(bot => bot.Owner).LoadAsync();
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<Bot> Update(Bot entity)
    {
        Context.Bots.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<Bot> Delete(Bot entity)
    {
        Context.Bots.Remove(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<List<Bot>> GetOwnedByUserId(int userId)
    {
        return await Context.Bots
            .Include(x => x.Owner)
            .Include(x => x.BotGroups)
            .Where(x => x.OwnerId == userId)
            .ToListAsync();
    }

    public Task<List<Bot>> GetAvailableByUserId(int userId)
    {
        throw new System.NotImplementedException();
    }
}