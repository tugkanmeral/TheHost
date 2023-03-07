using En = Note.Entities;
public interface INoteRepository : IMongoDbRepository<En.Note>
{
    En.Note Get(string userId, string id);
    Task<IEnumerable<En.Note>> Get(string userId);
    void Insert(En.Note note);
    Task Insert(IEnumerable<En.Note> notes);
    void Update(En.Note note);
    void Delete(string userId, string id);
    IEnumerable<En.Note> Get(string userId, int skip, int take, string? searchText, string[]? tags);
    Task<long> GetTotalNotesCount(string userId);
}