public interface IReminderService
{
    void SaveReminder(ReminderRequest request, string userId);
}