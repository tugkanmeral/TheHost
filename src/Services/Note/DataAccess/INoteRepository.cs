using En = Note.Entities;
public interface INoteRepository : IMongoDbRepository<En.Note>
{
    En.Note Get(string userId, string id);
    void Insert(En.Note note);
    void Update(En.Note note);
    void Delete(string userId, string id);
    IEnumerable<En.Note> Get(string userId, int skip, int take, string? searchText, string[]? tags);
    Task<long> GetTotalNotesCount(string userId);
}