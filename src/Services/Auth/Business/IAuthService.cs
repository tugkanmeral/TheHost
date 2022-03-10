public interface IAuthService
{
    string? GetToken(string username, string password);
}