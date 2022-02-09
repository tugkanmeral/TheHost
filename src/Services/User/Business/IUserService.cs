public interface IUserService
{
    void InsertUser(User user);
    User GetUser(string userId);
    IEnumerable<User> GetUsers();
    void Delete(string userId);
}