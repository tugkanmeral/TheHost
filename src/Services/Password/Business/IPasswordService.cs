using En = Password.Entities;
public interface IPasswordService
{
    List<En.Password>GetPasswords();
    En.Password GetPassword(string id, string masterKey);
    void UpdatePassword(En.Password password, string masterKey);
    void InsertPassword(En.Password password, string masterKey);
    void DeletePassword(string id);
    string SaltPassword(string rawPassword);
    string DesaltPassword(string saltedPassword);
}