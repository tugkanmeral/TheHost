using En = Note.Entities;

public interface INoteService
{
    void InsertNote(En.Note note, string userId);
    IEnumerable<En.Note> GetNotes(string userId, int skip, int take, string? searchText, string[]? tags);
    Task<long> GetTotalNotesCount(string userId);
    void DeleteNote(string id, string userId);
    En.Note GetNote(string id, string userId);
    void UpdateNote(En.Note note, string userId);
}