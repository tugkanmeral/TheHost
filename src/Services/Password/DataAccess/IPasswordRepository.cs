using En = Password.Entities;
public interface IPasswordRepository
{
    Task<List<En.Password>> GetAsync(string userId);
    En.Password Get(string userId, string id);
    List<En.Password> Get(string userId, int skip, int take, string? searchText);
    void Insert(En.Password password);
    void Update(En.Password password);
    void Delete(string userId, string id);
    Task<long> GetUserPasswordsCount(string userId);
}