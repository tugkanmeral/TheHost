using En = Note.Entities;

public interface IBackupService
{
    Task ImportNotes(string userId, List<En.Note> notes);
    Task<IEnumerable<En.Note>> ExportNotes(string userId);
}