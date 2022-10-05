using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Reminder.Entities;

public class Reminder
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;
    public ReminderImportance ReminderPriority { get; set; } = ReminderImportance.Mid;
    public string OwnerId { get; set; } = String.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime? LastUpdateDate { get; set; }
}