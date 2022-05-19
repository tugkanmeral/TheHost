using En = Note.Entities;

public interface INoteService
{
    public void InsertNote(En.Note note, string userId);
}