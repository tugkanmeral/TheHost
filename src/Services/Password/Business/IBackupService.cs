using En = Password.Entities;

public interface IBackupService
{
    Task<List<En.Password>> ExportPasswords(string userId);
}