public class UserManager : IUserService
{
    IUserRepository _userRepository;
    AppSettings _appSettings;
    public UserManager(IUserRepository userRepository, AppSettings appSettings)
    {
        _userRepository = userRepository;
        _appSettings = appSettings;
    }

    public void Delete(string userId)
    {
        _userRepository.Delete(userId);
    }

    public User GetUser(string userId)
    {
        return _userRepository.GetById(userId);
    }

    public IEnumerable<User> GetUsers()
    {
        return _userRepository.Get();
    }

    public void InsertUser(User user)
    {
        _userRepository.Insert(user);
    }
}