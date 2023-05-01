namespace BotsControll.Core.Services.Password;

public interface IPasswordHasher
{
    public string Hash(string password);
}