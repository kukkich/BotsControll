using BotsControll.Core.Models.Base;
using System.Threading.Tasks;

namespace BotsControll.Core.Repositories;

public interface IRepository<TEntity, in TId>
    where TEntity : IHaveId<TId>
{
    public Task<TEntity> GetById(TId id);
    public Task<TEntity> Add(TEntity entity);
    public Task<TEntity> Update(TEntity entity);
    public Task<TEntity> Delete(TEntity entity);
}