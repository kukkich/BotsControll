using BotsControll.Core.Models;
using BotsControll.Core.Models.Identity;
using BotsControll.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BotsControll.Api.Repositories;

public class TokenDbRepository : DbRepository, ITokensRepository
{
    public TokenDbRepository(BotsControlContext context)
        : base(context)
        { }

    public async Task<Token> GetById(string id)
    {
        return await Context.Tokens.FirstAsync(x => x.Id == id);
    }
    public async Task<Token?> TryGetToken(string refreshToken)
    {
        return await Context.Tokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
    }
    public async Task<Token?> TryGetByUserId(int id)
    {
        return await Context.Tokens.FirstOrDefaultAsync(x => x.UserId == id);
    }
    public async Task<Token> GetToken(string refreshToken)
    {
        return await Context.Tokens.FirstAsync(x => x.RefreshToken == refreshToken);
    }

    public async Task<Token> Add(Token entity)
    {
        Context.Tokens.Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<Token> Update(Token entity)
    {
        Context.Tokens.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<Token> Delete(Token entity)
    {
        Context.Tokens.Remove(entity);
        await Context.SaveChangesAsync();
        return entity;
    }
}