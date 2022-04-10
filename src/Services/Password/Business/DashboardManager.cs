public class DashboardManager : IDashboardService
{
    IPasswordRepository _passwordRepository;
    public DashboardManager(IPasswordRepository passwordRepository)
    {
        _passwordRepository = passwordRepository;
    }
    public async Task<long> TotalPasswordCount(string userId)
    {
        return await _passwordRepository.GetUserPasswordsCount(userId);
    }
}