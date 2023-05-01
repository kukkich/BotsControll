namespace BotsControll.Core.Models.Base;

public interface IHaveId<out T>
{
    public T Id { get; }
}