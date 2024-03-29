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

    public IEnumerable<En.Note> GetNotes(string userId, int skip, int take, string? searchText, string[]? tags)
    {
        IEnumerable<En.Note> notes;
        try
        {
            notes = _noteRepository.Get(userId, skip, take, searchText, tags);
        }
        catch (Exception ex)
        {
            throw new Exception("Error while passwords getting", ex);
        }

        return notes;
    }

    public async Task<long> GetTotalNotesCount(string userId)
    {
        return await _noteRepository.GetTotalNotesCount(userId);
    }

    public void DeleteNote(string id, string userId)
    {
        _noteRepository.Delete(userId, id);
    }
    public En.Note GetNote(string id, string userId)
    {
        return _noteRepository.Get(userId, id);
    }
    public void UpdateNote(En.Note note, string userId)
    {
        var existNote = _noteRepository.Get(userId, note.Id);
        if (existNote == null)
            throw new Exception("Note could not find!");

        existNote.Text = note.Text;
        existNote.Title = note.Title;

        var tagList = note.Tags.ToList();
        for (int i = 0; i < tagList.Count; i++)
        {
            tagList[i] = tagList[i].Trim();
        }
        existNote.Tags = tagList;

        existNote.LastUpdateDate = DateTime.Now;

        try
        {
            _noteRepository.Update(existNote);
        }
        catch (Exception)
        {
            throw new Exception("Error while note updating");
        }
    }
}