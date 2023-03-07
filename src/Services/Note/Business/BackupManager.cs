using En = Note.Entities;
public class BackupManager : IBackupService
{
    INoteRepository _noteRepository;
    public BackupManager(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task ImportNotes(string userId, List<En.Note> notes)
    {
        notes.ForEach(note => note.OwnerId = userId);
        await _noteRepository.Insert(notes);
    }

    public async Task<IEnumerable<En.Note>> ExportNotes(string userId)
    {
        var notes = await _noteRepository.Get(userId);
        return notes;
    }
}