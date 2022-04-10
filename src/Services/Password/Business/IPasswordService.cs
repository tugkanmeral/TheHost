using En = Password.Entities;
public interface IPasswordService
{
    List<En.Password> GetPasswords(string userId, int skip, int take);
    En.Password GetPassword(string id, string userId, string passwordPrivateKey);
    void UpdatePassword(En.Password password, string userId, string passwordPrivateKey);
    void InsertPassword(En.Password password, string userId, string passwordPrivateKey);
    void DeletePassword(string id, string userId);
    string SaltPassword(string rawPassword);
    string DesaltPassword(string saltedPassword);
    Task<long> GetTotalPasswordsCount(string userId);
}