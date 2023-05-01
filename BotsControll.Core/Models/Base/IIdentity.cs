namespace BotsControll.Core.Models.Base;

public interface IIdentity<out TId> : IHaveId<TId>
{
    public string Name { get; }
}