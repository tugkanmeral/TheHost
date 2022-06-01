namespace Models;
public class Note
{
    public string Id { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public string Text { get; set; } = String.Empty;
    public IEnumerable<string> Tags { get; set; } = new List<string>();
    public DateTime CreationDate { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public string OwnerId { get; set; } = String.Empty;
}