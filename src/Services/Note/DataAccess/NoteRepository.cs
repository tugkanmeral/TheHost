using MongoDB.Bson;
using MongoDB.Driver;
using En = Note.Entities;

public class NoteRepository : INoteRepository, IMongoDbRepository<En.Note>
{
    public IMongoClient Client { get; set; }
    public IMongoDatabase Database { get; set; }
    public IMongoCollection<En.Note> Collection { get; set; }

    public NoteRepository(IMongoClient mongoClient)
    {
        Client = mongoClient;
        Database = Client.GetDatabase("OnePassMini");
        Collection = Database.GetCollection<En.Note>("Notes");
    }

    public void Delete(string userId, string id)
    {
        var filter = Builders<En.Note>.Filter.Eq(p => p.Id, id) & Builders<En.Note>.Filter.Eq(p => p.OwnerId, userId);
        Collection.DeleteOne(filter);
    }

    public En.Note Get(string userId, string id)
    {
        var filter = Builders<En.Note>.Filter.Eq(p => p.Id, id) & Builders<En.Note>.Filter.Eq(p => p.OwnerId, userId);
        var note = Collection.Find(filter).FirstOrDefault();
        return note;
    }

    public void Insert(En.Note note)
    {
        Collection.InsertOne(note);
    }

    public List<En.Note> Get(string userId, int skip, int take, string? searchText)
    {
        var findFilter = Builders<En.Note>.Filter.Eq(x => x.OwnerId, userId);

        var projection = Builders<En.Note>
                            .Projection
                            .Include(x => x.Title)
                            .Include(x => x.Text)
                            .Include(x => x.Tags)
                            .Include(x => x.LastUpdateDate)
                            .Include(x => x.CreationDate);

        var sorting = Builders<En.Note>
                        .Sort
                        .Descending(x => x.CreationDate);

        var notes = Collection.Find(findFilter).Project<En.Note>(projection).Sort(sorting).Skip(skip).Limit(take).ToList();
        

        return notes;
    }

    public async Task<List<En.Note>> SearchNote(string userId, int skip, int take, string? searchText, string[]? tags)
    {
        FilterDefinition<En.Note>? filters = Builders<En.Note>.Filter.Empty;

        if (!String.IsNullOrWhiteSpace(searchText))
            filters &= Builders<En.Note>.Filter.Text(searchText);

        if (tags != null && tags.Length > 0)
            filters &= Builders<En.Note>.Filter.All("Tags", tags);

        if (filters == null)
            throw new Exception("Filter cannot be null.");
        
        var projection = Builders<En.Note>
                            .Projection
                            .Include(x => x.Title)
                            .Include(x => x.Text)
                            .Include(x => x.Tags)
                            .Include(x => x.LastUpdateDate)
                            .Include(x => x.CreationDate);

        var sorting = Builders<En.Note>
                        .Sort
                        .Descending(x => x.CreationDate);

        var notes = await Collection.Find(filters).Project<En.Note>(projection).Sort(sorting).Skip(skip).Limit(take).ToListAsync();

        return notes;
    }

    public async Task<long> GetTotalNotesCount(string userId)
    {
        var filter = Builders<En.Note>.Filter.Eq(p => p.OwnerId, userId);
        var count = await Collection.CountDocumentsAsync(filter);
        return count;
    }

    public void Update(En.Note note)
    {
        var filter = Builders<En.Note>.Filter.Eq(p => p.Id, note.Id);
        Collection.ReplaceOne(filter, note);
    }
}