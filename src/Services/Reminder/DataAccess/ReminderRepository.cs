using MongoDB.Driver;
using En = Reminder.Entities;
public class ReminderRepository : IReminderRepository, IMongoDbRepository<En.Reminder>
{
    public IMongoClient Client { get; set; }
    public IMongoDatabase Database { get; set; }
    public IMongoCollection<En.Reminder> Collection { get; set; }

    public ReminderRepository(IMongoClient mongoClient)
    {
        Client = mongoClient;
        Database = Client.GetDatabase("TheHost");
        Collection = Database.GetCollection<En.Reminder>("Reminders");
    }

    public void SaveReminder(En.Reminder reminder)
    {
        Collection.InsertOne(reminder);
    }
}