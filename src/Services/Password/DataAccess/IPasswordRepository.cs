using En = Password.Entities;
public interface IPasswordRepository
{
    En.Password Get(string id);
    List<En.Password> Get();
    void Insert(En.Password password);
    void Update(En.Password password);
    void Delete(string id);
}