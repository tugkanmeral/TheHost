using En = Reminder.Entities;

public class ReminderManager : IReminderService
{
    private readonly IReminderRepository _reminderRepository;
    public ReminderManager(IReminderRepository reminderRepository)
    {
        _reminderRepository = reminderRepository;
    }

    public void SaveReminder(ReminderRequest request, string userId)
    {
        En.Reminder reminder = new()
        {
            Subject = request.Subject,
            Message = request.Message,
            OwnerId = userId,
            CreationDate = DateTime.Now
        };

        _reminderRepository.SaveReminder(reminder);
    }
}