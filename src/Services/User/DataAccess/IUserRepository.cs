public interface IUserRepository
{
    User GetByUsername(string username);
    User GetById(string id);
    IEnumerable<User> Get();
    void Insert(User user);
    void Delete(string id);
}