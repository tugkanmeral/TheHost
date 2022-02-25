using En = Password.Entities;
public interface IPasswordService
{
    List<En.Password> GetPasswords(string userId);
    En.Password GetPassword(string id, string userId, string masterKey);
    void UpdatePassword(En.Password password, string userId, string masterKey);
    void InsertPassword(En.Password password, string userId, string masterKey);
    void DeletePassword(string id, string userId);
    string SaltPassword(string rawPassword);
    string DesaltPassword(string saltedPassword);
}