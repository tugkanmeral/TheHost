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

    public IEnumerable<En.Note> Get(string userId, int skip, int take, string? searchText, string[]? tags)
    {
        BsonArray matchAnd = new();

        if (!String.IsNullOrWhiteSpace(searchText))
            matchAnd.Add(new BsonDocument("$text", new BsonDocument("$search", searchText)));

        matchAnd.Add(new BsonDocument("OwnerId", userId));

        if (tags != null && tags.Length > 0)
        {
            BsonArray tagValues = new();
            foreach (var tag in tags)
            {
                tagValues.Add(tag);
            }
            matchAnd.Add(new BsonDocument("Tags", new BsonDocument("$in", tagValues)));
        }

        var match = new BsonDocument("$match", new BsonDocument("$and", matchAnd));

        var projection = new BsonDocument("$project", new BsonDocument()
        {
            {"Title", 1},
            {"Text", new BsonDocument("$substrBytes", new BsonArray(new BsonValue[]{"$Text", 0, 100}))},
            {"Tags", 1},
            {"CreationDate", 1},
            {"_id", 0}
        });

        var sorting = new BsonDocument("$sort", new BsonDocument("CreationDate", -1));

        var skiping = new BsonDocument("$skip", skip);
        var limit = new BsonDocument("$limit", take);

        var pipeline = new[] { match, projection, sorting, skiping, limit };
        var notes = Collection.Aggregate(PipelineDefinition<En.Note, En.Note>.Create(pipeline)).ToList();

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