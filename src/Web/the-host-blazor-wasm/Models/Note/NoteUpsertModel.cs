namespace Models;
public class NoteUpsertModel
{
    public string Id { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public string Text { get; set; } = String.Empty;
    public IEnumerable<string> Tags { get; set; } = new List<string>();
}