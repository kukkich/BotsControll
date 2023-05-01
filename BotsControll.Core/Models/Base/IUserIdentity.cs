namespace BotsControll.Core.Models.Base;

public interface IUserIdentity : IIdentity<int>
{
    public string Email { get; }
}