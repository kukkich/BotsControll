using BotsControll.Core.Models;

namespace BotsControll.Api.Repositories;

public abstract class DbRepository
{
    protected readonly BotsControlContext Context;

    protected DbRepository(BotsControlContext context)
    {
        Context = context;
    }
}