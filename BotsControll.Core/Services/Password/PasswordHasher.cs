namespace BotsControll.Core.Services.Password;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => password;
}