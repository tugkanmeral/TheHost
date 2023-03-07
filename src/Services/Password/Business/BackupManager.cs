using En = Password.Entities;

public class BackupManager : IBackupService
{
    IPasswordRepository _passwordRepository;
    public BackupManager(IPasswordRepository passwordRepository)
    {
        _passwordRepository = passwordRepository;
    }

    public async Task<List<En.Password>> ExportPasswords(string userId)
    {
        return await _passwordRepository.GetAsync(userId);
    }
}