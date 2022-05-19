using En = Note.Entities;
public class NoteManager : INoteService
{
    INoteRepository _noteRepository;
    public NoteManager(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public void InsertNote(En.Note note, string userId)
    {
        note.CreationDate = DateTime.Now;
        note.OwnerId = userId;

        try
        {
            _noteRepository.Insert(note);
        }
        catch (Exception)
        {
            throw new Exception("Error while note adding");
        }
    }
}