using En = Password.Entities;
public interface IPasswordRepository
{
    En.Password Get(string userId, string id);
    List<En.Password> Get(string userId, int skip, int take);
    void Insert(En.Password password);
    void Update(En.Password password);
    void Delete(string userId, string id);
    Task<long> GetUserPasswordsCount(string userId);
}