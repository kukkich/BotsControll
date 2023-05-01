using System.Threading.Tasks;
using BotsControll.Core.Models;
using BotsControll.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BotsControll.Api.Repositories;

public class JournalDbRepository : DbRepository, IJournalRepository
{
    public JournalDbRepository(BotsControlContext context)
        : base(context)
        { }

    public async Task<Journal> GetById(string id)
    {
        return await Context.Journal.FirstAsync(x => x.Id == id);
    }

    public Task<Journal> Add(Journal entity)
    {
        throw new System.NotImplementedException();
    }

    public async Task<Journal> Update(Journal entity)
    {
        Context.Journal.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public Task<Journal> Delete(Journal entity)
    {
        throw new System.NotImplementedException();
    }
}