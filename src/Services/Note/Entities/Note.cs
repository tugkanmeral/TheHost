using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Note.Entities;
public class Note
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public string Text { get; set; } = String.Empty;
    public IEnumerable<string> Tags { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public string OwnerId { get; set; } = String.Empty;
}