using En = Reminder.Entities;
public interface IReminderRepository
{
    void SaveReminder(En.Reminder reminder);
}