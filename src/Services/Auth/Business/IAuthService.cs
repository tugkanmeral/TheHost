public interface IAuthService
{
    UserToken GetToken(string username, string password);
}