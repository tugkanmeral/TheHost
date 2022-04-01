using En = Password.Entities;
public interface IPasswordRepository
{
    En.Password Get(string userId, string id);
    List<En.Password> Get(string userId);
    void Insert(En.Password password);
    void Update(En.Password password);
    void Delete(string userId, string id);
    Task<long> GetUserPasswordCount(string userId);
}