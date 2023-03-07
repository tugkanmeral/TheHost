using MongoDB.Bson;
using MongoDB.Driver;
using En = Password.Entities;

public class PasswordRepository : IPasswordRepository, IMongoDbRepository<En.Password>
{
    public IMongoClient Client { get; set; }
    public IMongoDatabase Database { get; set; }
    public IMongoCollection<En.Password> Collection { get; set; }

    public PasswordRepository(IMongoClient mongoClient)
    {
        Client = mongoClient;
        Database = Client.GetDatabase("OnePassMini");
        Collection = Database.GetCollection<En.Password>("Passwords");
    }

    public void Delete(string userId, string id)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.Id, id) & Builders<En.Password>.Filter.Eq(p => p.OwnerId, userId);
        Collection.DeleteOne(filter);
    }

    public En.Password Get(string userId, string id)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.Id, id) & Builders<En.Password>.Filter.Eq(p => p.OwnerId, userId);
        var password = Collection.Find(filter).FirstOrDefault();
        return password;
    }

    public List<En.Password> Get(string userId, int skip, int take, string? searchText)
    {
        var findFilter = buildPasswordFilter(userId, searchText);

        var projection = Builders<En.Password>.Projection
        .Include(x => x.Title)
        .Include(x => x.Username)
        .Include(x => x.Detail)
        .Include(x => x.LastUpdateDate)
        .Include(x => x.CreationDate)
        .Include(x => x.OwnerId);
        var passwords = Collection.Find(findFilter).Project<En.Password>(projection).Skip(skip).Limit(take).ToList();

        return passwords;
    }

    public async Task<List<En.Password>> GetAsync(string userId)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.OwnerId, userId);
        var passwords = await Collection.Find(filter).ToListAsync();
        return passwords;
    }

    public void Insert(En.Password password)
    {
        Collection.InsertOne(password);
    }

    public void Update(En.Password password)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.Id, password.Id);
        Collection.ReplaceOne(filter, password);
    }

    public async Task<long> GetUserPasswordsCount(string userId)
    {
        var filter = Builders<En.Password>.Filter.Eq(p => p.OwnerId, userId);
        var count = await Collection.CountDocumentsAsync(filter);
        return count;
    }

    private FilterDefinition<En.Password> buildPasswordFilter(string userId, string? searchText)
    {
        var filterDefinitions = new List<FilterDefinition<En.Password>>();

        filterDefinitions.Add(Builders<En.Password>.Filter.Eq(x => x.OwnerId, userId));

        if (!String.IsNullOrWhiteSpace(searchText))
        {
            filterDefinitions.Add(Builders<En.Password>.Filter.Text(searchText));
        }

        return Builders<En.Password>.Filter.And(filterDefinitions);
    }
}