public interface IDashboardService
{
    Task<long> TotalPasswordCount(string userId);
}