namespace Models;

public class NoteSearchRequest
{
    public string? SearchText { get; set; }
    public List<string>? Tags { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}